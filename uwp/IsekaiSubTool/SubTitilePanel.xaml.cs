using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Web;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Core.Preview;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
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
    public sealed partial class SubtitlePanel : UserControl
    {
        public SubtitlePanel()
        {
            this.InitializeComponent();

        }
        public void ShowSpinner()
        {
            MainProgress.IsActive = true;
        }
        public ScrollViewer DlWrapper()
        {
            return DlogsWrapper;
        }
        public StackPanel Dl()
        {
            return DLogs;
        }
        public Grid BP()
        {
            return BottomPanel;
        }
        public ProgressRing Progress()
        {
            return MainProgress;
        }
        public Button returnButton()
        {
            return ReturnButton;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int s = 0;

            foreach (DialogListItem ditem in DLogs.Children)
            {
                string file_id = ditem.Header.Split(" (")[0];
                foreach (Dialog dialog in DialogsLink.lastDialogs)
                {
                    if (file_id == dialog.file_id)
                    {
                        if (dialog.text != ditem.Text)
                        {
                            i++;
                            DialogsLink.lastDialogs[DialogsLink.lastDialogs.IndexOf(dialog)].text = ditem.Text;
                            string result = DialogsLink.SaveUpdate(file_id, HttpUtility.UrlEncode(ditem.Text));
                            if (result == "success") s++;
                        }
                    }
                }
            }
            if (s == i && i > 0)
            {
                Task.Factory.StartNew(async () =>
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        MessageDialog d = new MessageDialog("Все изменения успешно сохранены!", "Данные на сервере обновлены");
                        await d.ShowAsync();
                    });
                });
            }
            else if (s > 0)
            {
                Task.Factory.StartNew(async () =>
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        MessageDialog d = new MessageDialog("Не все изменения были сохранены... Попробуйте ещё раз", "Ой..");
                        await d.ShowAsync();
                    });
                });
            }
            else if (s == 0 && i > 0)
            {
                Task.Factory.StartNew(async () =>
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        MessageDialog d = new MessageDialog("При загрузке изменений произошла ошибка. Проверьте ваше подключение к Интернету.", "Ошибка!");
                        await d.ShowAsync();
                    });
                });
            }
            else
            {
                Task.Factory.StartNew(async () =>
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        MessageDialog d = new MessageDialog("Изменения не обнаружены");
                        await d.ShowAsync();
                    });
                });
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {

        }

        
        private async void ChangeSubViewText(string t, string h)
        {
            if(nCoreView != null)
            {
                try
                {
                    await nCoreView.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                () =>
                {
                    SubView subView = Window.Current.Content as SubView;

                    subView.ChangeText(t, h);
                });
                }
                catch
                {

                }
            }
            
            
        }
        private CoreApplicationView nCoreView = null;

        private async void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nCoreView == null)
                {
                    nCoreView = CoreApplication.CreateNewView();

                    ApplicationView newAppView = null;
                    int mainViewId = ApplicationView.GetApplicationViewIdForWindow(
                      CoreApplication.MainView.CoreWindow);

                    await nCoreView.Dispatcher.RunAsync(
                      CoreDispatcherPriority.Normal,
                      () =>
                      {
                          newAppView = ApplicationView.GetForCurrentView();
                          Window.Current.Content = new SubView();
                          newAppView.Consolidated += async (a, b) =>
                          {
                            await Dispatcher.RunAsync(
                            CoreDispatcherPriority.Normal, () =>
                            {
                                CloseSubWindow();
                            });
                          };
                          Window.Current.Activate();

                      });

                    await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                      newAppView.Id,
                      ViewSizePreference.UseHalf,
                      mainViewId,
                      ViewSizePreference.UseHalf);

                    foreach (DialogListItem dialogListItem in DLogs.Children)
                    {
                        dialogListItem.GotFocus += (x, y) =>
                        {
                            DialogListItem listItem = x as DialogListItem;
                            string text = listItem.Text;
                            string header = listItem.Header;
                            ChangeSubViewText(text, header);
                        };
                        dialogListItem.ValueChanged += (k, s) =>
                        {
                            DialogListItem listItem = k as DialogListItem;
                            string header = listItem.Header;
                            ChangeSubViewText(s.newValue, header);
                        };
                        //dialogListItem.
                    };
                }
            }
            catch
            {

            }
            
        }
        public async void CloseSubWindow()
        {
            try
            {
                if(nCoreView != null)
                {
                    SubButton.IsChecked = false;
                    await nCoreView.Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () =>
                    {
                        Window.Current.Close();
                    });
                    nCoreView = null;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            CloseSubWindow();
        }
    }
}
