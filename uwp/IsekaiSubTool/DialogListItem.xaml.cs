using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
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
        }
        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(sender is Button button)
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

        private async void FileCopyDrag(UIElement sender, DragStartingEventArgs args)
        {
            try {
                List<IStorageItem> files = new List<IStorageItem>();

                string filename = (string)GetValue(HeaderProperty);
                filename = filename.Split(" (")[0] + ".mp3";


                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///audio/" + filename));
                files.Add(file);
                args.DragUI.SetContentFromDataPackage();
                args.Data.RequestedOperation = DataPackageOperation.Copy;
                args.Data.SetStorageItems(files);
            }
            catch
            {
                try
                {
                    List<IStorageItem> files = new List<IStorageItem>();

                    string filename = (string)GetValue(HeaderProperty);
                    filename = filename.Split(" (")[0] + ".oggт";


                    StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///audio/" + filename));
                    files.Add(file);
                    args.DragUI.SetContentFromDataPackage();
                    args.Data.RequestedOperation = DataPackageOperation.Copy;
                    args.Data.SetStorageItems(files);
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
