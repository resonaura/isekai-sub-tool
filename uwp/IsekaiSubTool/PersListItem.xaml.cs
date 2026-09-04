using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace IsekaiSubTool
{
    public sealed partial class PersListItem : UserControl
    {
        public static readonly DependencyProperty HeaderProperty =
        DependencyProperty.Register(
            "Header", typeof(string),
            typeof(DialogListItem), null
        );
        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            "Text", typeof(string),
            typeof(DialogListItem), null
        );
        public static readonly DependencyProperty PersenProperty =
       DependencyProperty.Register(
           "Persen", typeof(string),
           typeof(DialogListItem), null
       );
        public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(
            "Source", typeof(ImageSource),
            typeof(DialogListItem), null
        );
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); DHeader.Text = value; }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); DBox.Text = value; }
        }
        public string Persen
        {
            get { return (string)GetValue(PersenProperty); }
            set { SetValue(PersenProperty, value); }
        }
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); Poster.Source = value;}
        }
        public PersListItem()
        {
            this.InitializeComponent();
        }
    }
}
