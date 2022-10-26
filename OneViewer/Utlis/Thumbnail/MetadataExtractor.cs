// ImageListView - A listview control for image files
// Copyright (C) 2009 Ozgur Ozcitak
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Ozgur Ozcitak (ozcitak@yahoo.com)
//
// WIC support coded by Jens

using System;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace OneViewer.Utlis
{
    /// <summary>
    /// Read metadata.
    /// Only EXIF data when using .NET 2.0 methods.
    /// Prioritized EXIF/XMP/ICC/etc. data when using WIC/WPF methods.
    /// </summary>
    public partial class Extractor : GDIExtractor
    {
        #region Properties
        /// <summary>
        /// Gets the name of this extractor.
        /// </summary>
        public override string Name => "WPF/WIC Thumbnail Extractor";
        #endregion

        #region WIC Metadata Paths
        private static readonly string[] WICPathImageDescription = new string[] { "/app1/ifd/{ushort=40095}", "/app1/ifd/{ushort=270}" };
        private static readonly string[] WICPathCopyright = new string[] { "/app1/ifd/{ushort=33432}", "/app13/irb/8bimiptc/iptc/copyright notice", "/xmp/<xmpalt>dc:rights", "/xmp/dc:rights" };
        private static readonly string[] WICPathComment = new string[] { "/app1/ifd/{ushort=40092}", "/app1/ifd/{ushort=37510}", "/xmp/<xmpalt>exif:UserComment" };
        private static readonly string[] WICPathSoftware = new string[] { "/app1/ifd/{ushort=305}", "/xmp/xmp:CreatorTool", "/xmp/xmp:creatortool", "/xmp/tiff:Software", "/xmp/tiff:software", "/app13/irb/8bimiptc/iptc/Originating Program" };
        private static readonly string[] WICPathSimpleRating = new string[] { "/app1/ifd/{ushort=18246}", "/xmp/xmp:Rating" };
        private static readonly string[] WICPathRating = new string[] { "/app1/ifd/{ushort=18249}", "/xmp/MicrosoftPhoto:Rating" };
        private static readonly string[] WICPathArtist = new string[] { "/app1/ifd/{ushort=315}", "/app13/irb/8bimiptc/iptc/by-line", "/app1/ifd/{ushort=40093}", "/xmp/tiff:artist" };
        private static readonly string[] WICPathEquipmentManufacturer = new string[] { "/app1/ifd/{ushort=271}", "/xmp/tiff:Make", "/xmp/tiff:make" };
        private static readonly string[] WICPathEquipmentModel = new string[] { "/app1/ifd/{ushort=272}", "/xmp/tiff:Model", "/xmp/tiff:model" };
        private static readonly string[] WICPathDateTaken = new string[] { "/app1/ifd/exif/{ushort=36867}", "/app13/irb/8bimiptc/iptc/date created", "/xmp/xmp:CreateDate", "/app1/ifd/exif/{ushort=36868}", "/app13/irb/8bimiptc/iptc/date created", "/xmp/exif:DateTimeOriginal" };
        private static readonly string[] WICPathExposureTime = new string[] { "/app1/ifd/exif/{ushort=33434}", "/xmp/exif:ExposureTime" };
        private static readonly string[] WICPathFNumber = new string[] { "/app1/ifd/exif/{ushort=33437}", "/xmp/exif:FNumber" };
        private static readonly string[] WICPathISOSpeed = new string[] { "/app1/ifd/exif/{ushort=34855}", "/xmp/<xmpseq>exif:ISOSpeedRatings", "/xmp/exif:ISOSpeed" };
        private static readonly string[] WICPathFocalLength = new string[] { "/app1/ifd/exif/{ushort=37386}", "/xmp/exif:FocalLength" };
        #endregion

        #region Helper Methods
        /// <summary>
        /// Returns the metadata for the given query.
        /// </summary>
        /// <param name="metadata">The image metadata.</param>
        /// <param name="query">A list of query strings.</param>
        /// <returns>Metadata object or null if the metadata is not found.</returns>
        private static object GetMetadataObject(BitmapMetadata metadata, params string[] query)
        {
            foreach (string q in query)
            {
                object val = metadata.GetQuery(q);
                if (val != null)
                    return val;
            }
            return null;
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the WPFExtractor class.
        /// </summary>
        public Extractor()
            : base()
        {
            
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Inits metadata via WIC/WPF (.NET 3.0).
        /// If WIC lacks a metadata reader for this image type then fall back to .NET 2.0 method. 
        /// </summary>
        /// <param name="path">Filepath of image</param>
        public override Metadata GetMetadata(string path)
        {
            Metadata m = new Metadata();
            try
            {
                using (FileStream streamWpf = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BitmapFrame frameWpf = BitmapFrame.Create
                            (streamWpf,
                             BitmapCreateOptions.IgnoreColorProfile,
                             BitmapCacheOption.None);

                    m.Width = frameWpf.PixelWidth;
                    m.Height = frameWpf.PixelHeight;
                    m.DPIX = frameWpf.DpiX;
                    m.DPIY = frameWpf.DpiY;

                    BitmapMetadata data = frameWpf.Metadata as BitmapMetadata;
                    if (data != null)
                    {
                        object val;

                        // Subject
                        val = GetMetadataObject(data, WICPathImageDescription);
                        if (val != null)
                            m.ImageDescription = val as string;
                        // Copyright
                        val = GetMetadataObject(data, WICPathCopyright);
                        if (val != null)
                            m.Copyright = val as string;
                        // Comment
                        val = GetMetadataObject(data, WICPathComment);
                        if (val != null)
                            m.Comment = val as string;
                        // Software
                        val = GetMetadataObject(data, WICPathSoftware);
                        if (val != null)
                            m.Software = val as string;
                        // Simple rating
                        val = GetMetadataObject(data, WICPathSimpleRating);
                        if (val != null)
                        {
                            ushort simpleRating = Convert.ToUInt16(val);

                            if (simpleRating == 1)
                                m.Rating = 1;
                            else if (simpleRating == 2)
                                m.Rating = 25;
                            else if (simpleRating == 3)
                                m.Rating = 50;
                            else if (simpleRating == 4)
                                m.Rating = 75;
                            else if (simpleRating == 5)
                                m.Rating = 99;
                        }
                        // Rating
                        val = GetMetadataObject(data, WICPathRating);
                        if (val != null)
                            m.Rating = (int)Convert.ToUInt16(val);
                        // Authors
                        val = GetMetadataObject(data, WICPathArtist);
                        if (val != null)
                        {
                            if (val is string)
                                m.Artist = (string)val;
                            else if (val is System.Collections.Generic.IEnumerable<string>)
                            {
                                int i = 0;
                                StringBuilder authors = new StringBuilder();
                                foreach (string author in (System.Collections.Generic.IEnumerable<string>)val)
                                {
                                    if (i != 0)
                                        authors.Append(";");
                                    authors.Append(authors);
                                    i++;
                                }
                                m.Artist = authors.ToString();
                            }
                        }

                        // Camera manufacturer
                        val = GetMetadataObject(data, WICPathEquipmentManufacturer);
                        if (val != null)
                            m.EquipmentManufacturer = val as string;
                        // Camera model
                        val = GetMetadataObject(data, WICPathEquipmentModel);
                        if (val != null)
                            m.EquipmentModel = val as string;

                        // Date taken
                        val = GetMetadataObject(data, WICPathDateTaken);
                        if (val != null)
                            m.DateTaken = ExifDateTime((string)val);
                        // Exposure time
                        val = GetMetadataObject(data, WICPathExposureTime);
                        if (val != null)
                            m.ExposureTime = ExifDouble(BitConverter.GetBytes((ulong)val));
                        // FNumber
                        val = GetMetadataObject(data, WICPathFNumber);
                        if (val != null)
                            m.FNumber = ExifDouble(BitConverter.GetBytes((ulong)val));
                        // ISOSpeed
                        val = GetMetadataObject(data, WICPathISOSpeed);
                        if (val != null)
                            m.ISOSpeed = Convert.ToUInt16(val);
                        // FocalLength
                        val = GetMetadataObject(data, WICPathFocalLength);
                        if (val != null)
                            m.FocalLength = ExifDouble(BitConverter.GetBytes((ulong)val));
                    }
                }
            }
            catch (Exception eWpf)
            {
                m.Error = eWpf;
            }
            return m;
        }
        #endregion
    }
}
