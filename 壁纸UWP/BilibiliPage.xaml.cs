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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 壁纸UWP
{
   
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BilibiliPage : Page
    {
        public BilibiliPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// 内存中实例化图片，添加至List列表中
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public void Show(int Start)
        {
            List<Image> imagelist = new List<Image>();
            List<string> save_imglist;
            string uri_id = "http://wallpaper.apc.360.cn/index.php?c=WallPaper&a=getAppsByCategory&cid=26&start=" + Start.ToString() + "&count=15&from=360chrome";
            //*   string uri =@"https://source.unsplash.com/1600x900/?nature,water";*/
            save_imglist =IndexPage.Get_ImageResource(uri_id);//得到15张图片地址 字符串列表类型
            save_imglist = save_imglist.Where(x => x != null).ToList();
            Global.Cache_Uri = save_imglist;//将图片地址列表缓存
            for (int i = 0; i < save_imglist.Count; i++)
            {
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(save_imglist[i]))
                };
                imagelist.Add(image);
            }
            IndexListView1.ItemsSource = imagelist.GetRange(0, 5);
            IndexListView2.ItemsSource = imagelist.GetRange(5, 5);
            IndexListView3.ItemsSource = imagelist.GetRange(10, save_imglist.Count/3);
            Global.Index_Start = Global.Index_Start + 8;//调整每次图片加载张数
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Global.Index_Start = 0;
            Show(Global.Index_Start);
        }
        private void MyScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (MyScrollViewer.ScrollableHeight == MyScrollViewer.VerticalOffset)
            {
                Show(Global.Index_Start); //显示图片
                MyScrollViewer.ScrollToVerticalOffset(0);//滚动条复位
            }
        }
        private void IndexListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IndexListView1.SelectedIndex != -1)
            {
                int index = (int)IndexListView1.SelectedIndex;//保存选中的索引值
                string File_Name = Global.Cache_Uri[index].Substring(Global.Cache_Uri[index].Length - 20, 20);//获取将保存的文件名
              IndexPage.SaveImage(File_Name, Global.Cache_Uri[index]);
                NotifyPopup notifyPopup = new NotifyPopup("保存成功,至图库查看");
                notifyPopup.Show();
            }
        }

        private void IndexListView2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IndexListView2.SelectedIndex != -1)
            {
                int index = (int)IndexListView2.SelectedIndex;//保存选中的索引值
                string File_Name = Global.Cache_Uri[index + 5].Substring(Global.Cache_Uri[index + 5].Length - 20, 20);//获取将保存的文件名
                IndexPage.SaveImage(File_Name, Global.Cache_Uri[index + 5]);//+5的偏移为ListView2的选中序列
                NotifyPopup notifyPopup = new NotifyPopup("保存成功,至图库查看");
                notifyPopup.Show();
            }
        }

        private void IndexListView3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IndexListView3.SelectedIndex != -1)
            {
                int index = (int)IndexListView3.SelectedIndex;//保存选中的索引值
                string File_Name = Global.Cache_Uri[index + 10].Substring(Global.Cache_Uri[index + 10].Length - 20, 20);//获取将保存的文件名
                IndexPage.SaveImage(File_Name, Global.Cache_Uri[index + 10]);//+5的偏移为ListView2的选中序列
                NotifyPopup notifyPopup = new NotifyPopup("保存成功");
                notifyPopup.Show();
            }
        }


    }

   
}
