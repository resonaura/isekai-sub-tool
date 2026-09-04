using Microsoft.Toolkit.Uwp.UI.Animations;
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
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace IsekaiSubTool
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public SubtitlePanel SP1 = new SubtitlePanel();
        public SubtitlePanel SP2 = new SubtitlePanel();
        public List<PersListItem> PERS = new List<PersListItem> {
            new PersListItem { Header = "Ли", Text = "Личность Ли полностью зависит от принятых игроком решений на протяжении всей игры. Он может либо вселять в людей надежду и немного идеалистического образа, часто даёт людям благо сомнения и сомнение в необходимости выполнения нравственно сомнительных действий, либо будет достаточно прагматичным человеком, который показывает признаки агрессии или насилия, если необходимо, в соответствии с напряжёнными ситуациями.Наиболее очевидная черта Ли, отображённая на протяжении всей серии, является способность быть заботливым и любящим.", Source=new BitmapImage(new Uri("ms-appx:///pers/LEE.jpg")), Persen = "LEE"},
            new PersListItem { Header = "Клементина", Text="\"Эта маленькая девочка - загадка\"", Source=new BitmapImage(new Uri("ms-appx:///pers/CLEMENTINE.jpg")), Persen="CLEMENTINE"},
            new PersListItem { Header = "Шон", Text="Шон является старшим сыном Хершела Грина, он добрый, жизнерадостный парень, который знает, что отец его тоже любит. Шон зачастую уверен, что именно он должен делать любую работу на ферме, так как он самый старший.", Source = new BitmapImage(new Uri("ms-appx:///pers/SHAWN.jpg")), Persen="SHAWN" },
            new PersListItem { Header = "Митчел", Text = "Ничего не известно о жизни Андре до апокалипсиса, кроме того, что он был офицером полиции.", Source=new BitmapImage(new Uri("ms-appx:///pers/MITCHELL.jpg")), Persen="MITCHELL"},
            new PersListItem { Header = "Марк", Text = "Марк родился в Мейконе, был пилотом, служил на авиабазе Робинс до того, как она была захвачена зомби. Группа встретила его в промежутке между 1-м и 2-м эпизодами. Так как у него был доступ к большим запасам еды и оружия на базе, Лилли позволила ему присоединиться к их группе.", Source=new BitmapImage(new Uri("ms-appx:///pers/MARK.jpg")), Persen="MARK"},
            new PersListItem { Header = "Лилли", Text = "Лилли - властная женщина, имеющая способность контролировать группу таким образом, каким она считает нужным. Интересно отметить, что когда Ларри начинает действовать в приступе ярости, она действует как пацифист и пытается его успокоить, но всё проходит безрезультатно. Это показывает, что, в отличие от её отца, у неё нет похожих горечи и негодования, которые имеет Ларри ко всем остальным, хотя у неё есть характер, с которым не будут считаться.", Source=new BitmapImage(new Uri("ms-appx:///pers/LILLY.jpg")), Persen="LILLY"},
            new PersListItem { Header = "Ларри", Text = "Личность Ларри не такая уж плохая, как можно увидеть во время игры. В глубине души Ларри сильно переживает за свою последнюю надежду – Лилли. И, зная, что Ли Эверетт - убийца, не подпускает её к нему. Он старается, чтобы Лилли была счастлива, но при попадании в трудную эмоциональную ситуацию у него может случиться сердечный приступ.", Source=new BitmapImage(new Uri("ms-appx:///pers/LARRY.jpg")), Persen="LARRY"},
            new PersListItem { Header = "Кэнни", Text = "Симпатичный, но резкий, работящий парень, который любит, чтобы всё было так, как решит он, невзирая на мнение остальных членов группы. Кенни в силу основы своего характера просто довольно резок в отношении к людям, что вмешиваются в дела его семьи и близких друзей.", Source=new BitmapImage(new Uri("ms-appx:///pers/KENNY.jpg")), Persen="KENNY"},
            new PersListItem { Header = "Катя", Text = "Жена Кенни и матерь Дака,  заботливая женщина и не боится сказать то, что думает. Упорная и настойчивая, Катя является очень сильной опорой и голосом разума для Кенни, когда возникают сложные вопросы. Как мать и домохозяйка средних лет в группе, Катя была одной из персонажей игры, чья личность переживает радикальные изменения в связи с событиями, которые происходят.", Source=new BitmapImage(new Uri("ms-appx:///pers/KATJAA.jpg")), Persen="KATJAA"},
            new PersListItem { Header = "Хершель", Text = "Мало известно о Хершеле до вспышки инфекции. Известно лишь, что он бывший ветеринар. Когда-то он был семейным человеком, который владел фермой. Его старший сын Шон также появлялся в видеоигре.", Source=new BitmapImage(new Uri("ms-appx:///pers/HERSHEL.jpg")), Persen="HERSHEL"},
            new PersListItem { Header = "Гленн", Text = "Почти ничего не известно о жизни Гленна до начала нашествия зомби, кроме того, что он работал развозчиком пиццы", Source=new BitmapImage(new Uri("ms-appx:///pers/GLENN.jpg")), Persen="GLENN"},
            new PersListItem { Header = "Дак", Text = "Характер Дака (в отличие от Клементины) ещё не сформирован. Он капризен и пытлив. Создаётся ощущение, что он не до конца понимает всю тяжесть ситуации.", Source=new BitmapImage(new Uri("ms-appx:///pers/DUCK.jpg")), Persen="DUCK"},
            new PersListItem { Header = "Даг", Text = "Даг был программистом. За два месяца до апокалипсиса он приехал в Мейкон и жил со своим дядей. Даг спас Карли, когда её и её начальницу атаковали ходячие.", Source=new BitmapImage(new Uri("ms-appx:///pers/DOUG.jpg")), Persen="DOUG"},
            new PersListItem { Header = "Дэвид", Text = "Дэвид Паркер - преподаватель в школе, где учились Бен Пол и Трэвис. Ещё он является руководителем школьного оркестра, в котором состоят Бен и Трэвис.", Source=new BitmapImage(new Uri("ms-appx:///pers/DAVID.jpg")), Persen="DAVID"},
            new PersListItem { Header = "Коп", Text = "Просто коп", Source=new BitmapImage(new Uri("ms-appx:///pers/COP.jpg")), Persen="COP"},
            new PersListItem { Header = "Чет", Text = "До вспышки эпидемии Чет жил рядом с фермой семьи Хершела вместе со своей матерью и дружил с Шоном Грином.", Source=new BitmapImage(new Uri("ms-appx:///pers/CHET.jpg")), Persen="CHET"},
            new PersListItem { Header = "Карли", Text = "Карли впервые появляется как серьёзный журналист, который вместе с Гленном решает спасти Ли и семью Кенни по их прибытию в Мейкон. Проведя время в районах боевых действий в качестве корреспондента, она, по-видимому, приобретает основные знания об огнестрельном оружии и тактике выживания, что позволило ей выжить во время заражения. Её точность при обращении с оружием часто замечается Кенни как \"dead eye accuracy\".", Source=new BitmapImage(new Uri("ms-appx:///pers/CARLEY.jpg")), Persen="CARLEY"},
            new PersListItem { Header = "Энди", Text = "", Source=new BitmapImage(new Uri("ms-appx:///pers/ANDY.jpg")), Persen="ANDY"},
    };

        public MainPage()
        {
            this.InitializeComponent();
            Windows.UI.ViewManagement.ApplicationViewTitleBar uwpTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;

            uwpTitleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;

            uwpTitleBar.BackgroundColor = Windows.UI.Colors.Transparent;
            uwpTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;


            //using Windows.ApplicationModel.Core

            Windows.ApplicationModel.Core.CoreApplicationViewTitleBar coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;

            coreTitleBar.ExtendViewIntoTitleBar = true;

            ChName.Text = chapters[0];
            xGrid.Children.Add(SP1);
            xPGRID.Children.Add(SP2);

            SP1.Visibility = Visibility.Collapsed;
            SP2.Visibility = Visibility.Collapsed;
            SP1.returnButton().Tapped += RB_Tapped;
            SP2.returnButton().Tapped += RB_Tapped;

            foreach(PersListItem listItem in PERS)
            {
                listItem.Tapped += ListItem_Tapped;
                PersWrapper.Children.Add(listItem);
            }
            MLText.Text = "Загрузка аудиофайлов...";
            UpdateAudioFnames();


        }
        public async void UpdateAudioFnames()
        {
            string root = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            string path = root + @"\audio";

            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path);
            var storageFiles = await folder.GetFilesAsync();

            foreach (StorageFile storageFile in storageFiles)
            {
                audio_fnames.Add(Path.GetFileNameWithoutExtension(storageFile.Name));
            }

            MLSpinner.IsActive = false;
            AnimationSet aset = MainLoading.Fade(0);
            aset.Completed += (x, y) =>
            {
                MainLoading.Visibility = Visibility.Collapsed;
            };
            aset.Start();
        }
        public List<string> audio_fnames = new List<string>();
        private void RB_Tapped(object sender, TappedRoutedEventArgs e)
        {
            restorePages();
        }

        private void ListItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(sender is PersListItem listItem)
            {
                SP2.Visibility = Visibility.Visible;
                SP2.ShowSpinner();

                var anim = PersScroll.Fade(0, 300);
                anim.Completed += (x, y) =>
                {
                    PersScroll.Visibility = Visibility.Collapsed;
                    SP2.DlWrapper().Visibility = Visibility.Visible;

                    try
                    {
                        SP2.DlWrapper().Fade(1, 300).Start();
                        DialogsLink.LoadDialogsByPers(listItem.Persen, SP2.Dl(), SP2.Progress(), Dispatcher, audio_fnames);
                        SP2.BP().Fade(1, 300).Start();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                };
                anim.Start();
            }
        }

        private string[] chapters = new string[]
        {
            "Глава 1: Долгая поездка домой",
            "Глава 1: Лес",
            "Глава 2: Во дворе",
            "Глава 3: Встреча Клементины",
            "Глава 4: Ферма Хершела",
            "Глава 4: Шон или Дак",
            "Глава 5: Ну вот и дома",
            "Глава 5: Нападение на Клем",
            "Глава 5: Motor Inn",
            "Глава 5: The Girl in Room 9",
            "Глава 6: Брат",
            "Глава 7: Даг или Карли",
            "Глава 8: Спасены... Почти"
        };
        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ChName.Text = chapters[ChFlip.SelectedIndex];
            }
            catch
            {

            }
            
        }
        private void FlipViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SP1.Visibility = Visibility.Visible;
            SP1.ShowSpinner();

            ChName.Fade(0, 300).Start();
            var anim = ChFlip.Fade(0, 300);
            anim.Completed += (x, y) =>
            {
                ChFlip.Visibility = Visibility.Collapsed;
                ChName.Visibility = Visibility.Collapsed;
                SP1.DlWrapper().Visibility = Visibility.Visible;

                try
                {
                    SP1.DlWrapper().Fade(1, 300).Start();
                    DialogsLink.LoadDialogs((ChapterEvents)ChFlip.SelectedIndex, SP1.Dl(), SP1.Progress(), audio_fnames);
                    SP1.BP().Fade(1, 300).Start();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            };
            anim.Start();
            


        }
        private void restorePages()
        {
            ChFlip.Visibility = Visibility.Visible;
            ChName.Visibility = Visibility.Visible;
            PersScroll.Visibility = Visibility.Visible;
            
            SP1.CloseSubWindow();
            SP2.CloseSubWindow();
            ChFlip.Fade(1, 300).Start();
            ChName.Fade(1, 300).Start();
            PersScroll.Fade(1, 300).Start();
            SP1.Dl().Children.Clear();
            SP2.Dl().Children.Clear();
            SP1.BP().Fade(0, 300).Start();
            SP2.BP().Fade(0, 300).Start();
            SP1.DlWrapper().Fade(0, 300).Start();
            SP2.DlWrapper().Fade(0, 300).Start();
            SP2.Dl().Fade(0, 300).Start();
            var anim = SP1.Dl().Fade(0, 300);
            anim.Completed += (x, y) =>
            {
                SP1.DlWrapper().Visibility = Visibility.Collapsed;
                SP2.DlWrapper().Visibility = Visibility.Collapsed;
                SP1.Visibility = Visibility.Collapsed;
                SP2.Visibility = Visibility.Collapsed;
            };
            anim.Start();
        }
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            restorePages();
        }
    }
}
