using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using SQLite.Net;
using static System.Diagnostics.Debug;
using 壁纸UWP.Models;
using Microsoft.Advertising.WinRT.UI;
// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace 壁纸UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            AdControl.ApplicationId= "9nc4zpfn97sc";//广告
            AdControl.AdUnitId = "1100039452";
           AdControl.ErrorOccurred += (s, e) =>
            {
             Adstaus.Text = e.ErrorMessage;

            };
            
            Window.Current.SetTitleBar(TitleTextBlock);//设置标题栏为TitleTextBlock
            BackButton.Visibility = Visibility.Collapsed;
            MyFrame.Navigate(typeof(IndexPage));
            TitleTextBlock.Text = "4K专区";
           Four_k.IsSelected = true;
            dbname = "MyDB.db";//数据库名
            string fdlocal = ApplicationData.Current.LocalFolder.Path;
            string filename = dbname;
            string dbfullpath = Path.Combine(fdlocal, filename);
            ISQLitePlatform platform = new SQLitePlatformWinRT();
            // 连接对象       
            SQLiteConnection conn = new SQLiteConnection(platform, dbfullpath);
            WriteLine("db pathe: " + conn.DatabasePath);
            // 创建表      
            int rn = conn.CreateTable<UserItem>(CreateFlags.None);
            WriteLine("create table res = {0}", rn);
            conn.Dispose();
        }
        public static string dbname;//数据库名
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyFrame.CanGoBack)
            {
                MyFrame.GoBack();
                Four_k.IsSelected = true;
            }
        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Four_k.IsSelected)
            {
                BackButton.Visibility = Visibility.Collapsed;
                MyFrame.Navigate(typeof(IndexPage));
                TitleTextBlock.Text = "4K专区";
            }
            else if (View.IsSelected)
            {
                BackButton.Visibility = Visibility.Visible;
                MyFrame.Navigate(typeof(ViewPage));
                TitleTextBlock.Text = "风景大片";
            }
            else if(Female.IsSelected)
            {
                BackButton.Visibility = Visibility.Visible;
                MyFrame.Navigate(typeof(FemalePage));
                TitleTextBlock.Text = "美女模特";
            }
            else if (Love.IsSelected)
            {
                BackButton.Visibility = Visibility.Visible;
                MyFrame.Navigate(typeof(LovePage));
                TitleTextBlock.Text = "爱情美图";
            }
            else if (Green.IsSelected)
            {
                BackButton.Visibility = Visibility.Visible;
                MyFrame.Navigate(typeof(GreenPage));
                TitleTextBlock.Text = "小清新";
            }
            else if (Bilibili.IsSelected)
            {
                BackButton.Visibility = Visibility.Visible;
                MyFrame.Navigate(typeof(BilibiliPage));
                TitleTextBlock.Text = "动漫卡通";
            }    else if (Game.IsSelected)
            {
                BackButton.Visibility = Visibility.Visible;
                MyFrame.Navigate(typeof(GamePage));
                TitleTextBlock.Text = "游戏壁纸";
            }
            else if (Setting.IsSelected)
            {
                BackButton.Visibility = Visibility.Visible;
                MyFrame.Navigate(typeof(ConfPage));
                TitleTextBlock.Text = "设置";
            }
        }

        private void HumburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
    }
    
}
