using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OneViewer.Observable
{
    /// <summary>
	/// Class implements a sorted observable collection.
	/// 
	/// Source: http://elegantcode.com/2009/05/14/write-a-sortable-observablecollection-for-wpf/
	/// </summary>
    public class SortableObservableCollection<T> : ObservableCollection<T>
    {
        #region constructor
        /// <summary>
        /// Standard class constructor
        /// </summary>
        public SortableObservableCollection()
          : base()
        {
        }

        /// <summary>
        /// Class constructor from IList interface. />
        /// </summary>
        /// <param name="list"></param>
        public SortableObservableCollection(List<T> list)
          : base(list)
        {
        }

        /// <summary>
        /// Class constructor from IEnumerable.
        /// </summary>
        /// <param name="collection"></param>
        public SortableObservableCollection(IEnumerable<T> collection)
          : base(collection)
        {
        }
        #endregion constructor

        #region methods
        /// <summary>
        /// Sort in descending or ascending order via lamda expression:
        /// 'MySortableList.Sort(x => x.Name, ListSortDirection.Ascending);'
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <param name="direction"></param>
        public void Sort<TKey>(Func<T, TKey> keySelector, ListSortDirection direction)
        {
            switch (direction)
            {
                case ListSortDirection.Ascending:
                    {
                        this.ApplySort(Items.OrderBy(keySelector));
                        break;
                    }

                case ListSortDirection.Descending:
                    {
                        this.ApplySort(Items.OrderByDescending(keySelector));
                        break;
                    }
            }
        }

        /// <summary>
        /// Sort in descending or ascending order^via keySelector.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <param name="comparer"></param>
        public void Sort<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer)
        {
            this.ApplySort(Items.OrderBy(keySelector, comparer));
        }

        /// <summary>
        /// Sort in descending or ascending order.
        /// </summary>
        /// <param name="comparer"></param>
        public void Sort(IComparer<T> comparer)
        {
            this.ApplySort(Items.OrderBy(i => i, comparer));
        }

        private void ApplySort(IEnumerable<T> sortedItems)
        {
            var sortedItemsList = sortedItems.ToList();

            foreach (var item in sortedItemsList)
            {
                this.Move(this.IndexOf(item), sortedItemsList.IndexOf(item));
            }
        }
        #endregion methods
    }
}
