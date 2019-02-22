using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using 壁纸UWP.Models;
using static System.Diagnostics.Debug;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 壁纸UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ConfPage : Page
    {
        public ConfPage()
        {
            this.InitializeComponent();
        }
        public static string Temp_Filelocal;
        /// <summary>
        /// 修改存储路径
        /// </summary>
        private async Task Folder_ClickAsync()//修改存储路径
        {

            FolderPicker picker = new FolderPicker();
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");
            StorageFolder folder = await picker.PickSingleFolderAsync();
            if (folder != null)
            {

                Temp_Filelocal = folder.Path;
            }
            if (Temp_Filelocal != null)
            {
                string localFolder = ApplicationData.Current.LocalFolder.Path;
                string dbFullPath = Path.Combine(localFolder, MainPage.dbname);
                // 建立连接      
                using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), dbFullPath))
                {
                    //conn.DeleteAll<Person>();        
                    // 插入数据             
                    UserItem[] stus =     {
                new UserItem {FilePath=Temp_Filelocal}   };
                    int n = conn.InsertAll(stus);
                    WriteLine($"已插入 {n} 条数据。");
                    //读取数据

                    // 获取列表               
                    TableQuery<UserItem> t = conn.Table<UserItem>();
                    var Result = t.LastOrDefault();
                    FileLocal.Text = Result.FilePath;  // 绑定                     
                }
            }

        }
        private void Local_button_Click(object sender, RoutedEventArgs e)
        {
            Folder_ClickAsync();//修改路径   
        }

        private void Rec_localButton_Click(object sender, RoutedEventArgs e)
        {
            string localFolder = ApplicationData.Current.LocalFolder.Path;
            string dbFullPath = Path.Combine(localFolder, MainPage.dbname);
            // 建立连接        
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), dbFullPath))
            {
                conn.DeleteAll<UserItem>();//删除数据恢复默认设置

            }
        }
    }
}

