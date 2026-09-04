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
    /// Логика взаимодействия для Chapter.xaml
    /// </summary>
    public partial class Chapter : UserControl
    {


        public BitmapSource Img
        {
            get { return (BitmapSource)GetValue(ImgProperty); }
            set { SetValue(ImgProperty, value); }
        }
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeightProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ImageProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImgProperty =
            DependencyProperty.Register("Img", typeof(BitmapSource), typeof(Chapter));
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(Chapter));

        public Chapter()
        {
            DataContext = this;
            InitializeComponent();
        }
    }
}
