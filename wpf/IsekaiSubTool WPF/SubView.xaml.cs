using Nancy.Helpers;
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
using System.Windows.Shapes;

namespace IsekaiSubTool_WPF
{
    /// <summary>
    /// Логика взаимодействия для SubView.xaml
    /// </summary>
    public partial class SubView : Window
    {
        public SubView()
        {
            this.InitializeComponent();
        }
        public void ChangeText(string text, string header)
        {
            MainText.Text = HttpUtility.HtmlDecode(text);
            MainHeader.Text = header;
        }
    }
}
