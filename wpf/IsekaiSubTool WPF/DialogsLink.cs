using MahApps.Metro.Controls;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace IsekaiSubTool_WPF
{
    public class Dialog
    {
        public string file_id = "";
        public string pers = "";
        public string text = "";
    }
    public enum ChapterEvents
    {
        Chapter1_CopCar,
        Chapter1_Forest,
        Chapter2_ClementineYard, 
        Chapter3_ClementineHouse,
        Chapter4_HershelsFarm,
        Chapter4_HershelsFarmShawnAndDuck,
        Chapter5_Drugstore,
        Chapter5_Clem,
        Chapter5_MotorInnMission,
        Chapter5_GR,
        Chapter6,
        Chapter7,
        Chapter8
    }
    static class DialogsLink
    {
        private static Dictionary<ChapterEvents, string> env_fnames = new Dictionary<ChapterEvents, string>
        {
            [ChapterEvents.Chapter1_CopCar] = "env_copcar_english.txt",
            [ChapterEvents.Chapter1_Forest] = "env_forest_english.txt",
            [ChapterEvents.Chapter2_ClementineYard] = "env_clementineyard_english.txt",
            [ChapterEvents.Chapter3_ClementineHouse] = "env_clementinehouse_english.txt",
            [ChapterEvents.Chapter4_HershelsFarm] = "env_hershelsfarm_english.txt",
            [ChapterEvents.Chapter4_HershelsFarmShawnAndDuck] = "env_hershelsfarm_english.txt",
            [ChapterEvents.Chapter5_Drugstore] = "env_maconstreet_english.txt",
            [ChapterEvents.Chapter5_Clem] = "env_drugstore_english.txt",
            [ChapterEvents.Chapter5_MotorInnMission] = "env_motorinnmission_english.txt",
            [ChapterEvents.Chapter5_GR] = "env_motorinnmission_english.txt",
            [ChapterEvents.Chapter6] = "env_maconstreet_english.txt",
            [ChapterEvents.Chapter7] = "env_drugstoreprologue_english.txt",
            [ChapterEvents.Chapter8] = "env_motorinn_english.txt"
        };
        private static Dictionary<ChapterEvents, string> env_link = new Dictionary<ChapterEvents, string>
        {
            [ChapterEvents.Chapter1_CopCar] = "dlogs\\env_copcar_english.txt.json",
            [ChapterEvents.Chapter1_Forest] = "dlogs\\env_forest_english.txt.json",
            [ChapterEvents.Chapter2_ClementineYard] = "dlogs\\env_clementineyard_english.txt.json",
            [ChapterEvents.Chapter3_ClementineHouse] = "dlogs\\env_clementinehouse_english.txt.json",
            [ChapterEvents.Chapter4_HershelsFarm] = "dlogs\\env_hershelsfarm_english.txt.json",
            [ChapterEvents.Chapter4_HershelsFarmShawnAndDuck] = "dlogs\\env_hershelsfarm_english.txt.json",
            [ChapterEvents.Chapter5_Drugstore] = "dlogs\\env_maconstreet_english.txt.json",
            [ChapterEvents.Chapter5_Clem] = "dlogs\\env_drugstore_english.txt.json",
            [ChapterEvents.Chapter5_MotorInnMission] = "dlogs\\env_motorinnmission_english.txt.json",
            [ChapterEvents.Chapter5_GR] = "dlogs\\env_motorinnmission_english.txt.json",
            [ChapterEvents.Chapter6] = "dlogs\\env_maconstreet_english.txt.json",
            [ChapterEvents.Chapter7] = "dlogs\\env_drugstoreprologue_english.txt.json",
            [ChapterEvents.Chapter8] = "dlogs\\env_motorinn_english.txt.json"
        };
        public static string GetCloudJSON(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch
            {
                return null;
            }
        }
        public static string SaveUpdate(string file_id, string text)
        {
            return GetCloudJSON("http://anidark.ru/twd/update?file_id="+file_id+"&text="+text);
        }
        public static List<Dialog> lastDialogs = new List<Dialog>();
        public static void AddDItem(UIElementCollection uIElements, string header, string text)
        {
            DialogListItem dialogListItem = new DialogListItem { Header = header, Text = text };
            uIElements.Add(dialogListItem);
        }
        public static void LoadDialogs(ChapterEvents env, StackPanel DLogs, ProgressRing ring, List<string> fnames)
        {
            DLogs.Children.Clear();
            Task.Factory.StartNew(async () =>
            {
                
                if (!env_link.ContainsKey(env)) return;

                string text = GetCloudJSON("http://anidark.ru/twd/getJSON?chf=" + env_fnames[env]);
                if (text == null)
                {
                    string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\data";
                    string jsonPath = appPath + "\\" + env_link[env];
                    using (var reader = File.OpenText(jsonPath))
                    {
                        text = await reader.ReadToEndAsync();
                        // Do something with fileText...
                    }
                }
                Dialog[] JSONObj = null;
                try
                {
                    JSONObj = new JavaScriptSerializer().Deserialize<Dialog[]>(text);
                }
                catch
                {
                    MessageBox.Show("Сервер вернул некорректный ответ. Возможно вы используете публичное подключение к сети", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    ring.IsActive = false;
                    return;
                }
                

                lastDialogs.Clear();
                foreach (Dialog d in JSONObj)
                {
                    if (d != null)
                    {
                        lastDialogs.Add(d);
                        if (d.text == null || d.pers == null || d.file_id == null) continue;
                        string pers = " (" + d.pers.Replace("\r", "") + ")";
                        string header = d.file_id.Replace("\r", "").Trim();

                        if (fnames.Contains(header))
                        {
                            if (pers != " ()") header += pers;
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                AddDItem(DLogs.Children, header, d.text);
                            });
                            
                        }

                    }

                }

                
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DLogs.BeginAnimation(MainWindow.OpacityProperty, new DoubleAnimation(1, TimeSpan.FromMilliseconds(300)));
                    ring.IsActive = false;
                });
            });
            

        }

        public static void LoadDialogsByPers(string p, StackPanel DLogs, ProgressRing ring, List<string> fnames)
        {
            try
            {
                DLogs.Children.Clear();

                string text = GetCloudJSON("http://anidark.ru/twd/getJSON?pers=" + p);
                
                if (text == null)
                {
                    MessageBox.Show("Проверьте Ваше подключение к Интернету", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    ring.IsActive = false;
                    return;
                }
                if (text == "[null]")
                {
                    MessageBox.Show("Персонаж не найден", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    ring.IsActive = false;
                    return;
                }

                Dialog[] JSONObj = null;
                try
                {
                    JSONObj = new JavaScriptSerializer().Deserialize<Dialog[]>(text);
                }
                catch
                {
                    MessageBox.Show("Сервер вернул некорректный ответ. Возможно вы используете публичное подключение к сети", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    ring.IsActive = false;
                    return;
                }

                lastDialogs.Clear();
                foreach (Dialog d in JSONObj)
                {
                    if (d != null)
                    {
                        lastDialogs.Add(d);
                        if (d.text == null || d.pers == null || d.file_id == null) continue;
                        string pers = " (" + d.pers.Replace("\r", "") + ")";
                        string header = d.file_id.Replace("\r", "").Trim();

                        if (fnames.Contains(header))
                        {
                            if (pers != " ()") header += pers;
                            AddDItem(DLogs.Children, header, d.text);
                        }

                    }

                }

                DLogs.BeginAnimation(MainWindow.OpacityProperty, new DoubleAnimation(1, TimeSpan.FromMilliseconds(300)));
                ring.IsActive = false;



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
