﻿using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System;
using Avalonia.Rendering;
using Avalonia.Input;

namespace FluentAvalonia.UI.Controls.Primitives
{
	public class MinMaxCloseControl : TemplatedControl
	{
		public MinMaxCloseControl()
		{
			KeyboardNavigation.SetIsTabStop(this, false);
		}

		protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
		{
			if (_minimizeButton != null)
				_minimizeButton.Click -= OnButtonClick;

			if (_maximizeButton != null)
				_maximizeButton.Click -= OnButtonClick;

			if (_closeButton != null)
				_closeButton.Click -= OnButtonClick;

			base.OnApplyTemplate(e);

			_minimizeButton = e.NameScope.Find<Avalonia.Controls.Button>("MinimizeButton");
			if (_minimizeButton != null)
				_minimizeButton.Click += OnButtonClick;

			_maximizeButton = e.NameScope.Find<Avalonia.Controls.Button>("MaxRestoreButton");
			if (_maximizeButton != null)
				_maximizeButton.Click += OnButtonClick;

			_closeButton = e.NameScope.Find<Avalonia.Controls.Button>("CloseButton");
			if (_closeButton != null)
				_closeButton.Click += OnButtonClick;
		}

		internal Avalonia.Controls.Button MinimizeButton => _minimizeButton;

		internal Avalonia.Controls.Button MaximizeButton => _maximizeButton;

		internal Avalonia.Controls.Button CloseButton => _closeButton;

		protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
		{
			base.OnAttachedToVisualTree(e);

			_owner = this.VisualRoot as CoreWindow;

			if (_owner != null)
			{
				_windowStateObservable = _owner.GetObservable(Window.WindowStateProperty)
					.Subscribe(OnWindowStateChanged);
			}
		}

		protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
		{
			base.OnDetachedFromVisualTree(e);

			_windowStateObservable?.Dispose();
			_windowStateObservable = null;
		}

		private void OnButtonClick(object sender, RoutedEventArgs e)
		{
			if (_owner == null)
				return;

			if (sender == _minimizeButton)
			{
				_owner.WindowState = WindowState.Minimized;
			}
			else if (sender == _maximizeButton)
			{
				if (_owner.WindowState == WindowState.Maximized)
				{
					_owner.WindowState = WindowState.Normal;
				}
				else if (_owner.WindowState == WindowState.Normal)
				{
					_owner.WindowState = WindowState.Maximized;
				}
			}
			else if (sender == _closeButton)
			{
				_owner.Close();
			}
		}

		private void OnWindowStateChanged(WindowState state)
		{
			PseudoClasses.Set(":maximized", state == WindowState.Maximized);
		}

		internal bool HitTestMaxButton(Point pos)
		{
			if (_maximizeButton != null)
				return _maximizeButton.HitTestCustom(pos);

			return false;
		}

		internal void FakeMaximizeHover(bool hover)
		{
			if (_maximizeButton != null)
			{
				// We can't set the IsPointerOver property b/c it's readonly and that make things angry
				// so we'll just force set the Pseudoclass
				((IPseudoClasses)_maximizeButton.Classes).Set(":pointerover", hover);
				//_maximizeButton.SetValue(InputElement.IsPointerOverProperty, hover);
			}
		}

		internal void FakeMaximizePressed(bool pressed)
		{
			if (_maximizeButton != null)
			{
				_maximizeButton.SetValue(Button.IsPressedProperty, pressed);
			}
		}

		internal void FakeMaximizeClick()
		{
			OnButtonClick(_maximizeButton, null);
		}
				
		private IDisposable _windowStateObservable;
		private CoreWindow _owner;
		private Avalonia.Controls.Button _minimizeButton;
		private Avalonia.Controls.Button _maximizeButton;
		private Avalonia.Controls.Button _closeButton;
	}
}
