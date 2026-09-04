using MahApps.Metro.Controls;
using Nancy.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для SubTitlePanel.xaml
    /// </summary>
    public partial class SubTitlePanel : UserControl
    {
        public SubTitlePanel()
        {
            InitializeComponent();
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
                string file_id = ditem.Header.Split(' ')[0];
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
                MessageBox.Show("Все изменения успешно сохранены!", "Данные на сервере обновлены", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (s > 0)
            {
                MessageBox.Show("Не все изменения были сохранены... Попробуйте ещё раз", "Ой..", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (s == 0 && i > 0)
            {
                MessageBox.Show("При загрузке изменений произошла ошибка. Проверьте ваше подключение к Интернету.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Изменения не обнаружены", "Изменения не обнаружены", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            CloseSubWindow();
        }

        public SubView subView;
        private void ChangeSubViewText(string t, string h)
        {
            if(subView != null)
            {
                try
                {
                    subView.ChangeText(t, h);
                }
                catch
                {

                }
            }
            /*
            if (nCoreView != null)
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

            */
        }
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if(subView == null)
                {
                    subView = new SubView();
                    subView.Show();
                    subView.Activate();
                    subView.Closed += (a, b) =>
                    {
                        CloseSubWindow();
                    };

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
                /*
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
                }*/
            }
            catch
            {

            }

        }
        public void CloseSubWindow()
        {
            
            try
            {
                if (subView != null)
                {
                    SubButton.IsChecked = false;
                    subView.Close();
                    subView = null;
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
