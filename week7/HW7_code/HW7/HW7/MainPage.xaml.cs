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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using System.Text.RegularExpressions;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace HW7
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

        private void queryIpAddressClick(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ipNumber.Text = "";
            location.Text = "";
            runner.Text = "";
            queryIpAddressAsync(queryIpAddress.Text.Trim());
        }

        private void queryWeatherClick(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            weatherPicture.Source = null;
            cityName.Text = "";
            date.Text = "";
            details.Text = "";
            temperature.Text = "";
            queryWeatherAsync(queryWeather.Text.Trim());
        }

        private void choseIp(object sender, RoutedEventArgs e)
        {
            weatherPicture.Source = null;
            cityName.Text = "";
            date.Text = "";
            details.Text = "";
            temperature.Text = "";
            //显示已选功能模块，隐藏未选功能模块
            queryIpAddress.Visibility = Visibility.Visible;
            queryWeather.Visibility = Visibility.Collapsed;
            phoneDetails.Visibility = Visibility.Visible;
            weatherDetails.Visibility = Visibility.Collapsed;
        }

        private void choseWeather(object sender, RoutedEventArgs e)
        {
            ipNumber.Text = "";
            location.Text = "";
            runner.Text = "";
            //显示已选功能模块，隐藏未选功能模块
            queryIpAddress.Visibility = Visibility.Collapsed;
            queryWeather.Visibility = Visibility.Visible;
            phoneDetails.Visibility = Visibility.Collapsed;
            weatherDetails.Visibility = Visibility.Visible;
        }

        bool ipIsValid(string ip)
        {
            string rStr = "^(1\\d{2}|2[0-4]\\d|25[0-5]|[1-9]\\d|[1-9])\\."
                        + "(1\\d{2}|2[0-4]\\d|25[0-5]|[1-9]\\d|\\d)\\."
                        + "(1\\d{2}|2[0-4]\\d|25[0-5]|[1-9]\\d|\\d)\\."
                        + "(1\\d{2}|2[0-4]\\d|25[0-5]|[1-9]\\d|\\d)$";
            Regex r = new Regex(rStr);
            Match m = r.Match(ip);
            if (m.Success) return true;
            else return false;
        }

        async void queryIpAddressAsync(string ip)
        {
            if (ipIsValid(ip))
            {
                //testIp=183.129.210.50
                string url = "http://api.k780.com:88/?app=ip.get&ip=" + ip + "&appkey=24517&sign=2860e6e910408f6afa120f801b5577da&format=xml";
                Uri uri = new Uri(url);
                HttpClient client = new HttpClient();
                string result = await client.GetStringAsync(uri);
                //下面开始处理返回来的xml格式的字符串
                XmlDocument document = new XmlDocument();
                document.LoadXml(result);
                //获取att标签的内容，也就是归属地
                XmlNodeList list = document.GetElementsByTagName("att");
                IXmlNode node = list.Item(0);
                location.Text = node.InnerText;
                //获取operators标签的内容，也就是运营商
                list = document.GetElementsByTagName("operators");
                node = list.Item(0);
                runner.Text = node.InnerText;
                ipNumber.Text = ip;
            }
            else
            {
                await new MessageDialog("IP地址非法！").ShowAsync();
            }
            queryIpAddress.Text = "";
        }

        async void queryWeatherAsync(string city)
        {
            string url = "https://api.seniverse.com/v3/weather/now.json?key=gtm2sbajinm6suvt&location=" + city + "&language=zh-Hans&unit=c";
            Uri uri = new Uri(url);
            HttpClient client = new HttpClient();
            try
            {
                string result = await client.GetStringAsync(uri);
                //下面开始处理得到的JSON格式的返回值
                JObject jo = JObject.Parse(result);
                string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();
                JArray message = (JArray)JsonConvert.DeserializeObject(values[0]);

                string text = message[0]["now"]["text"].ToString();
                string code = message[0]["now"]["code"].ToString();
                string temper = message[0]["now"]["temperature"].ToString();
                weatherPicture.Source = new BitmapImage(new Uri("ms-appx:///Assets/weather/" + code + ".png"));
                cityName.Text = message[0]["location"]["name"].ToString();
                date.Text = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月" + DateTime.Now.Day.ToString() + "日";
                details.Text = text;
                temperature.Text = temper + "°C";
                
                queryWeather.Text = "";
            }
            catch (Exception)
            {
                string errorMsg = "输入非法！\n合法的输入格式:\n"
                        + "城市ID 例如：WX4FBXXFKE4F\n"
                        + "城市中文名 例如：北京\n"
                        + "省市名称组合 例如：广东广州\n"
                        + "城市拼音/英文名 例如：beijing（如拼音相同城市，可在之前加省份和空格，例：shanxi yulin\n"
                        + "经纬度 例如：39.93:116.40（纬度前经度在后，冒号分隔\n"
                        + "IP地址 例如：220.181.111.86（某些IP地址可能无法定位到城市）\n"
                        + "“ip”两个字母 自动识别请求IP地址，例如：location=ip\n";
                await new MessageDialog(errorMsg).ShowAsync();
            }
        }
    }
}
