﻿using Foundation;
using MapNotepad.Controls;
using MapNotepad.iOS.Renderers.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomNoBorderEntry), typeof(CustomNoBorderEntryRenderer))]
namespace MapNotepad.iOS.Renderers.Controls
{
    class CustomNoBorderEntryRenderer : EntryRenderer
    {

        #region --- Ovverides ---

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
            }
        }

        #endregion

    }
}