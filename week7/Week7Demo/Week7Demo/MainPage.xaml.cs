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
using System.Net.Http;
using Windows.Data.Xml.Dom;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Week7Demo
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            location.Text = "";
            runner.Text = "";
            queryAsync(number.Text);
        }

        async void queryAsync(string tel)
        {
            string url = "http://api.k780.com:88/?app=phone.get&phone=" + tel + "&appkey=24439&sign=6dad02605b0d960c0b029e455ab454a8&format=xml";
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(url);
            XmlDocument document = new XmlDocument();
            document.LoadXml(result);
            XmlNodeList list = document.GetElementsByTagName("att");
            IXmlNode node = list.Item(0);
            location.Text = node.InnerText;
            list = document.GetElementsByTagName("operators");
            node = list.Item(0);
            runner.Text = node.InnerText;
        }
    }
}
