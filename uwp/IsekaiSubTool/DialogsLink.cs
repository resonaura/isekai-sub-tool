using Microsoft.Toolkit.Uwp.UI.Animations;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace IsekaiSubTool
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
        private static Dictionary<ChapterEvents, Uri> env_link = new Dictionary<ChapterEvents, Uri>
        {
            [ChapterEvents.Chapter1_CopCar] = new Uri("ms-appx:///dlogs/env_copcar_english.txt.json"),
            [ChapterEvents.Chapter1_Forest] = new Uri("ms-appx:///dlogs/env_forest_english.txt.json"),
            [ChapterEvents.Chapter2_ClementineYard] = new Uri("ms-appx:///dlogs/env_clementineyard_english.txt.json"),
            [ChapterEvents.Chapter3_ClementineHouse] = new Uri("ms-appx:///dlogs/env_clementinehouse_english.txt.json"),
            [ChapterEvents.Chapter4_HershelsFarm] = new Uri("ms-appx:///dlogs/env_hershelsfarm_english.txt.json"),
            [ChapterEvents.Chapter4_HershelsFarmShawnAndDuck] = new Uri("ms-appx:///dlogs/env_hershelsfarm_english.txt.json"),
            [ChapterEvents.Chapter5_Drugstore] = new Uri("ms-appx:///dlogs/env_maconstreet_english.txt.json"),
            [ChapterEvents.Chapter5_Clem] = new Uri("ms-appx:///dlogs/env_drugstore_english.txt.json"),
            [ChapterEvents.Chapter5_MotorInnMission] = new Uri("ms-appx:///dlogs/env_motorinnmission_english.txt.json"),
            [ChapterEvents.Chapter5_GR] = new Uri("ms-appx:///dlogs/env_motorinnmission_english.txt.json"),
            [ChapterEvents.Chapter6] = new Uri("ms-appx:///dlogs/env_maconstreet_english.txt.json"),
            [ChapterEvents.Chapter7] = new Uri("ms-appx:///dlogs/env_drugstoreprologue_english.txt.json"),
            [ChapterEvents.Chapter8] = new Uri("ms-appx:///dlogs/env_motorinn_english.txt.json")
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
        public async static void LoadDialogs(ChapterEvents env, StackPanel DLogs, ProgressRing ring, List<string> fnames)
        {
            try
            {
                DLogs.Children.Clear();
                if (!env_link.ContainsKey(env)) return;

                string text = GetCloudJSON("http://anidark.ru/twd/getJSON?chf=" + env_fnames[env]);
                if (text == null)
                {
                    StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(env_link[env]);
                    using (TextReader textReader = new StreamReader(await storageFile.OpenStreamForReadAsync()))
                    {
                        text = textReader.ReadToEnd();
                    }
                }
                
                var JSONObj = new JavaScriptSerializer().Deserialize<Dialog[]>(text);

                lastDialogs.Clear();
                foreach (Dialog d in JSONObj)
                {
                    if (d != null)
                    {
                        lastDialogs.Add(d);
                        if (d.text == null || d.pers == null || d.file_id == null) continue;
                        string pers = " (" + d.pers.Replace("\r", "") + ")";
                        string header = d.file_id.Replace("\r", "").Trim();

                        if(fnames.Contains(header))
                        {
                            if (pers != " ()") header += pers;
                            AddDItem(DLogs.Children, header, d.text);
                        }

                    }

                }

                DLogs.Fade(1, 300).Start();
                ring.IsActive = false;
                


            }
            catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
            }
            

        }

        public static void LoadDialogsByPers(string p, StackPanel DLogs, ProgressRing ring, CoreDispatcher Dispatcher, List<string> fnames)
        {
            try
            {
                DLogs.Children.Clear();

                string text = GetCloudJSON("http://anidark.ru/twd/getJSON?pers=" + p);
                
                if (text == null)
                {
                    Task.Factory.StartNew(async () =>
                    {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            MessageDialog d = new MessageDialog("Проверьте Ваше подключение к Интернету", "Ошибка!");
                            await d.ShowAsync();
                        });
                    });
                    return;
                }
                if (text == "[null]")
                {
                    Task.Factory.StartNew(async () =>
                    {
                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                        {
                            MessageDialog d = new MessageDialog("Персонаж не найден", "Ошибка!");
                            await d.ShowAsync();
                        });
                    });
                    return;
                }

                var JSONObj = new JavaScriptSerializer().Deserialize<Dialog[]>(text);

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

                DLogs.Fade(1, 300).Start();
                ring.IsActive = false;



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
