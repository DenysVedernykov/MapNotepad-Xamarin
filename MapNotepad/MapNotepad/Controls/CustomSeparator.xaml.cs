﻿using Xamarin.Forms;

namespace MapNotepad.Controls
{
    public partial class CustomSeparator : ContentView
    {
        public CustomSeparator()
        {
            InitializeComponent();
        }

        #region -- Public properties --

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            propertyName: nameof(TextColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomSeparator),
            defaultValue: Color.Silver,
            defaultBindingMode: BindingMode.TwoWay);

        public Color TextColor
        {
            set => SetValue(TextColorProperty, value);
            get => (Color)GetValue(TextColorProperty);
        }

        public static readonly BindableProperty LineColorProperty = BindableProperty.Create(
            propertyName: nameof(LineColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomSeparator),
            defaultValue: Color.Silver,
            defaultBindingMode: BindingMode.TwoWay);

        public Color LineColor
        {
            set => SetValue(LineColorProperty, value);
            get => (Color)GetValue(LineColorProperty);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: nameof(Text),
            returnType: typeof(string),
            declaringType: typeof(CustomSeparator),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string Text
        {
            set => SetValue(TextProperty, value);
            get => (string)GetValue(TextProperty);
        }

        #endregion

    }
}