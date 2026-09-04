using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace IsekaiSubTool
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SubView : Page
    {
        public SubView()
        {
            this.InitializeComponent();
            Windows.UI.ViewManagement.ApplicationViewTitleBar uwpTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;

            uwpTitleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;

            uwpTitleBar.BackgroundColor = Windows.UI.Colors.Transparent;
            uwpTitleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;
            Windows.ApplicationModel.Core.CoreApplicationViewTitleBar coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;

            coreTitleBar.ExtendViewIntoTitleBar = true;
        }
        public void ChangeText(string text, string header)
        {
            MainText.Text = HttpUtility.HtmlDecode(text);
            MainHeader.Text = header;
        }
    }
}
