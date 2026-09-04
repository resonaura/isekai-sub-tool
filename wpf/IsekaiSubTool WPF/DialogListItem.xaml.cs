using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Логика взаимодействия для DialogListItem.xaml
    /// </summary>
    public sealed partial class DialogListItem : UserControl
    {
        // Declare a delegate
        public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

        // Declare an event
        [Category("Action")]
        [Description("Fires when the value is changed")]
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

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
        public DialogListItem()
        {
            this.InitializeComponent();
            DataContext = this;
        }
        /*
        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string filename = (string)GetValue(HeaderProperty);
                filename = filename.Split(" (")[0] + ".mp3";
                string filenameogg = filename.Split(".mp3")[0] + ".ogg";
                if (filename != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            MediaPlayer mediaPlayer = new MediaPlayer();
                            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///audio/" + filename));
                            mediaPlayer.Play();
                            mediaPlayer.MediaEnded += (a, b) =>
                            {
                                mediaPlayer.Dispose();
                            };
                            mediaPlayer.MediaFailed += (c, d) =>
                            {
                                mediaPlayer.Dispose();
                                MediaPlayer mediaPlayer2 = new MediaPlayer();
                                mediaPlayer2.Source = MediaSource.CreateFromUri(new Uri("ms-appx:///audio/" + filenameogg));
                                mediaPlayer2.Play();
                                mediaPlayer2.MediaEnded += (m, v) =>
                                {
                                    mediaPlayer2.Dispose();
                                };
                                mediaPlayer2.MediaFailed += (y, z) =>
                                {
                                    Task.Factory.StartNew(async () =>
                                    {
                                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                                        {
                                            MessageDialog dialog = new MessageDialog(d.ExtendedErrorCode.ToString(), "Ошибка воспроизведения!");
                                            await dialog.ShowAsync();
                                        });
                                    });
                                };



                            };
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.ToString());
                        }
                    });
                }
            }
        }
        */
        private void DBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                SetValue(TextProperty, DBox.Text);
                ValueChanged(this, new ValueChangedEventArgs(DBox.Text));
            }
            catch
            {

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\data";
            string path = appPath + "\\audio";
            string fname = (string)GetValue(HeaderProperty);
            string filename = fname.Split(' ')[0] + ".mp3";
            string filenameogg = fname.Split(' ')[0] + ".ogg";
            if (filename != null)
            {
                Task.Factory.StartNew(() =>
                {
                    MediaPlayer mediaPlayer = new MediaPlayer();
                    mediaPlayer.Open(new Uri(path + "\\" + filename));
                    mediaPlayer.Play();
                    mediaPlayer.MediaEnded += (a, b) =>
                    {
                    };
                    mediaPlayer.MediaFailed += (c, d) =>
                    {
                        MediaPlayer mediaPlayer2 = new MediaPlayer();
                        mediaPlayer2.Open(new Uri(path + "\\" + filenameogg));
                        mediaPlayer2.Play();
                        mediaPlayer2.MediaEnded += (m, v) =>
                        {
                            //mediaPlayer2.Dispose();
                        };
                        mediaPlayer2.MediaFailed += (y, z) =>
                        {
                            MessageBox.Show(z.ErrorException.ToString(), "Ошибка воспроизведения!", MessageBoxButton.OK, MessageBoxImage.Error);
                        };



                    };
                });
            }
        }
        private static void DoDragDropOrClipboardSetDataObject(MouseButton button, DependencyObject dragSource,
    VirtualFileDataObject virtualFileDataObject, DragDropEffects allowedEffects)
        {
            try
            {
                if (button == MouseButton.Left)
                {
                    // Left button is used to start a drag/drop operation
                    VirtualFileDataObject.DoDragDrop(dragSource, virtualFileDataObject, allowedEffects);
                }
                else if (button == MouseButton.Right)
                {
                    // Right button is used to copy to the clipboard
                    // Communicate the preferred behavior to the destination
                    virtualFileDataObject.PreferredDropEffect = allowedEffects;
                    Clipboard.SetDataObject(virtualFileDataObject);
                }
            }
            catch (COMException)
            {
                // Failure; no way to recover
            }
        }
        private void PackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\data";
            string path = appPath + "\\audio";

            string fname = (string)GetValue(HeaderProperty);
            string filename = fname.Split(' ')[0] + ".mp3";
            string filenameogg = fname.Split(' ')[0] + ".ogg";
            
            try
            {
                var dataObject = new DataObject(DataFormats.FileDrop, new string[] { path + "\\" + filename });
                dataObject.SetData(DataFormats.StringFormat, dataObject);
                DragDrop.DoDragDrop((sender as Grid), dataObject, DragDropEffects.Copy);
            }
            catch
            {
                try
                {
                    var dataObject = new DataObject(DataFormats.FileDrop, new string[] { path + "\\" + filenameogg });
                    dataObject.SetData(DataFormats.StringFormat, dataObject);
                    DragDrop.DoDragDrop((sender as Grid), dataObject, DragDropEffects.Copy);
                }
                catch
                {

                }
            }
            
        }
    }

    public class ValueChangedEventArgs
    {
        public string newValue;

        public ValueChangedEventArgs(string newValue)
        {
            this.newValue = newValue;
        }
    }
}
