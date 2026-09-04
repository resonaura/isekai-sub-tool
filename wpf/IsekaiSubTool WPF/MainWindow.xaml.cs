using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace IsekaiSubTool_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> audio_fnames = new List<string>();
        public MainWindow()
        {
            InitializeComponent();

            MLText.Text = "Загрузка аудиофайлов...";
            UpdateAudioFnames();
            SP1.ReturnButton.Click += (x, y) =>
            {
                RestorePages();
            };
            SP2.ReturnButton.Click += (x, y) =>
            {
                RestorePages();
            };
        }
        public void RestorePages()
        {
            SP1.DLogs.Children.Clear();
            SP1.DLogs.BeginAnimation(OpacityProperty, new DoubleAnimation(0, TimeSpan.FromMilliseconds(300)));
            SP1.Visibility = Visibility.Collapsed;
            SP2.DLogs.Children.Clear();
            SP2.DLogs.BeginAnimation(OpacityProperty, new DoubleAnimation(0, TimeSpan.FromMilliseconds(300)));
            SP2.Visibility = Visibility.Collapsed;
            ChFlip.Visibility = Visibility.Visible;
            PersWrapper.Visibility = Visibility.Visible;

            if (SP1.subView != null)
            {
                SP1.CloseSubWindow();
            }
            if (SP2.subView != null)
            {
                SP2.CloseSubWindow();
            }

            var anim = new DoubleAnimation(1, TimeSpan.FromMilliseconds(300));
            /*anim.Completed += (x, y) =>
            {
                
            };*/
            ChFlip.BeginAnimation(SubTitlePanel.OpacityProperty, anim);
        }
        public void UpdateAudioFnames()
        {
            Task.Factory.StartNew(() =>
            {
                string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\data";
                string path = appPath + "\\audio";

                var storageFiles = Directory.GetFiles(path);

                foreach (string storageFile in storageFiles)
                {
                    audio_fnames.Add(Path.GetFileNameWithoutExtension(storageFile));
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MLSpinner.IsActive = false;
                    DoubleAnimation aset = new DoubleAnimation(0, TimeSpan.FromMilliseconds(300));
                    aset.Completed += (x, y) =>
                    {
                        MainLoading.Visibility = Visibility.Collapsed;
                    };
                    MainLoading.BeginAnimation(OpacityProperty, aset);
                });
                
            });
            
        }

        private void ChButton_Click(object sender, RoutedEventArgs e)
        {
            MainTab.SelectedIndex = 0;
            RestorePages();
        }

        private void PersButton_Click(object sender, RoutedEventArgs e)
        {
            MainTab.SelectedIndex = 1;
            RestorePages();
        }

        private void Chapter_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SP1.Visibility = Visibility.Visible;
            SP1.ShowSpinner();

            var anim = new DoubleAnimation(0, TimeSpan.FromMilliseconds(300));
            anim.Completed += (x, y) =>
            {
                ChFlip.Visibility = Visibility.Collapsed;

                try
                {
                    DialogsLink.LoadDialogs((ChapterEvents)ChFlip.SelectedIndex, SP1.DLogs, SP1.Progress(), audio_fnames);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            };
            ChFlip.BeginAnimation(SubTitlePanel.OpacityProperty, anim);
        }

        private void PersListItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            SP2.Visibility = Visibility.Visible;
            SP2.ShowSpinner();

            var anim = new DoubleAnimation(0, TimeSpan.FromMilliseconds(300));
            anim.Completed += (x, y) =>
            {
                PersWrapper.Visibility = Visibility.Collapsed;

                try
                {
                    DialogsLink.LoadDialogsByPers((sender as PersListItem).Persen, SP2.DLogs, SP2.Progress(), audio_fnames);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            };
            PersWrapper.BeginAnimation(SubTitlePanel.OpacityProperty, anim);
        }
    }
}
