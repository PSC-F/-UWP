using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using System.Threading.Tasks;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace 壁纸UWP
{
    public class Global
    {
        public static int Index_Start = 0;//全局变量控制图片的开始序号
        public static List<string> Cache_Uri;
    }
    public class Rdata
    {

    }

    public class Data
    {
        public string id { get; set; }
        public string class_id { get; set; }
        public string resolution { get; set; }
        public string url_mobile { get; set; }
        public string url { get; set; }
        public string url_thumb { get; set; }
        public string url_mid { get; set; }
        public string download_times { get; set; }
        public string imgcut { get; set; }
        public string tag { get; set; }
        public string create_time { get; set; }
        public string update_time { get; set; }
        public string ad_id { get; set; }
        public string ad_img { get; set; }
        public string ad_pos { get; set; }
        public string ad_url { get; set; }
        public string ext_1 { get; set; }
        public string ext_2 { get; set; }
        public string utag { get; set; }
        public string tempdata { get; set; }
        public List<Rdata> rdata { get; set; }
        public string img_1600_900 { get; set; }
        public string img_1440_900 { get; set; }
        public string img_1366_768 { get; set; }
        public string img_1280_800 { get; set; }
        public string img_1280_1024 { get; set; }
        public string img_1024_768 { get; set; }
    }

    public class RootObject
    {
        public string errno { get; set; }
        public string errmsg { get; set; }
        public string consume { get; set; }
        public string total { get; set; }
        public List<Data> data { get; set; }
    }
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class IndexPage : Page
    {
        public IndexPage()
        {
            this.InitializeComponent();

            //   this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;//启用缓存，保护现场
        }
        /// <summary>
        /// 内存中实例化图片，添加至List列表中
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static List<Image> Get_ImageToList(string uri)
        {
            List<Image> imagelist = new List<Image>();
            Image image = new Image
            {
                Source = new BitmapImage(new Uri(uri))
            };
            imagelist.Add(image);
            image = null;
            return imagelist;
        }
        public static List<String> Get_ImageResource(string uri)
        {
            List<string> result_list = new List<string>();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync(uri).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            RootObject rb = JsonConvert.DeserializeObject<RootObject>(result);
            string test = rb.data.ToString();
            foreach (Data data in rb.data)
            {
                result_list.Add(data.url_mobile);//选择分辨率
            }
            return result_list;
        } /// <summary>
          /// 显示图片
          /// </summary>
        public void Show(int Start)
        {
            List<Image> imagelist = new List<Image>();
            List<string> save_imglist;
            string uri_id = "http://wallpaper.apc.360.cn/index.php?c=WallPaper&a=getAppsByCategory&cid=36&start=" + Start.ToString() + "&count=15&from=360chrome";
            //*   string uri =@"https://source.unsplash.com/1600x900/?nature,water";*/
            save_imglist = Get_ImageResource(uri_id);//得到15张图片地址 字符串列表类型
            save_imglist = save_imglist.Where(x => x != null).ToList();
            Global.Cache_Uri = save_imglist;//将图片地址列表缓存
            for (int i = 0; i < save_imglist.Count; i++)
            {
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(save_imglist[i]))
                };            
                imagelist.Add(image);
                image = null;
            }        
            IndexListView1.ItemsSource= imagelist.GetRange(0, 5);
            IndexListView2.ItemsSource = imagelist.GetRange(5, 5);
            IndexListView3.ItemsSource = imagelist.GetRange(10, save_imglist.Count/3);
            Global.Index_Start = Global.Index_Start + 8;//调整每次图片加载张数
        }
        public static async Task SaveImage(string imageName, string imageUri)
        {
            BackgroundDownloader backgroundDownload = new BackgroundDownloader();//实例化下载器
            StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("图影重重", CreationCollisionOption.OpenIfExists);//新建文件夹
            StorageFile newFile = await folder.CreateFileAsync(imageName, CreationCollisionOption.OpenIfExists);//创建文件
            Uri uri = new Uri(imageUri);
            DownloadOperation download = backgroundDownload.CreateDownload(uri, newFile);//创建下载器
            await download.StartAsync();//创建下载任务
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Global.Index_Start = 0;//初始化图片第一张序号
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
                string File_Name = Global.Cache_Uri[index].Substring(Global.Cache_Uri[index].Length-20,20);//获取将保存的文件名
                SaveImage(File_Name, Global.Cache_Uri[index]);
                NotifyPopup notifyPopup = new NotifyPopup("保存成功");
                notifyPopup.Show();
            }
        }

        private void IndexListView2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IndexListView2.SelectedIndex != -1)
            {
                int index = (int)IndexListView2.SelectedIndex;//保存选中的索引值
                string File_Name = Global.Cache_Uri[index+5].Substring(Global.Cache_Uri[index+5].Length - 20, 20);//获取将保存的文件名
                SaveImage(File_Name, Global.Cache_Uri[index+5]);//+5的偏移为ListView2的选中序列
                NotifyPopup notifyPopup = new NotifyPopup("保存成功");
                notifyPopup.Show();
            }
        }

        private void IndexListView3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IndexListView3.SelectedIndex != -1)
            {
                int index = (int)IndexListView3.SelectedIndex;//保存选中的索引值
                string File_Name = Global.Cache_Uri[index + 10].Substring(Global.Cache_Uri[index + 10].Length - 20, 20);//获取将保存的文件名
                SaveImage(File_Name, Global.Cache_Uri[index + 10]);//+5的偏移为ListView2的选中序列
                NotifyPopup notifyPopup = new NotifyPopup("保存成功");
                notifyPopup.Show();
            }
        }

      
    }

}
