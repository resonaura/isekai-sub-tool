using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IsekaiSubTool_WPF
{
    /// <summary>
    /// Логика взаимодействия для PersListItem.xaml
    /// </summary>
    public sealed partial class PersListItem : UserControl
    {
        public static readonly DependencyProperty IHeaderProperty =
        DependencyProperty.Register(
            "IHeader", typeof(string),
            typeof(DialogListItem), null
        );
        public static readonly DependencyProperty ITextProperty =
        DependencyProperty.Register(
            "IText", typeof(string),
            typeof(DialogListItem), null
        );
        public static readonly DependencyProperty PersenProperty =
       DependencyProperty.Register(
           "Persen", typeof(string),
           typeof(DialogListItem), null
       );
        public static readonly DependencyProperty ISourceProperty =
        DependencyProperty.Register(
            "ISource", typeof(ImageSource),
            typeof(DialogListItem), null
        );
        public string IHeader
        {
            get { return (string)GetValue(IHeaderProperty); }
            set { SetValue(IHeaderProperty, value); DHeader.Text = value; }
        }
        public string IText
        {
            get { return (string)GetValue(ITextProperty); }
            set { SetValue(ITextProperty, value); DBox.Text = value; }
        }
        public string Persen
        {
            get { return (string)GetValue(PersenProperty); }
            set { SetValue(PersenProperty, value); }
        }
        public ImageSource ISource
        {
            get { return (ImageSource)GetValue(ISourceProperty); }
            set { SetValue(ISourceProperty, value); Poster.Source = value; }
        }
        public PersListItem()
        {
            this.InitializeComponent();
            DataContext = this;
        }
    }
}
