using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Todos
{
    public sealed partial class NewPage : Page
    {
        private Task srcImage;

        public object MyImage { get; private set; }

        public NewPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            //如果页面可以回退，则显示回退按钮，使得点击NewPage顶部的“<-”按钮，跳转回MainPage
            if (rootFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }

            var i = new MessageDialog("Welcome!").ShowAsync();
        }

        //点击create按钮时，检查Title、Description是否为空，DueDate是否正确
        private void createItem(object sender, RoutedEventArgs e)
        {
            //获取DatePicker的时间
            string str1 = datepicker.Date.ToString("yyyy-MM-dd");
            //获取当前时间
            string str2 = DateTime.Now.Date.ToString("yyyy-MM-dd");
            var dateCompareResult = string.Compare(str1, str2);
            //将报错信息细化，共有2x2x2-1=7种错误
            if (Title.Text == "" && Details.Text == "" && dateCompareResult == -1)
            {
                var i = new MessageDialog("Title and some descriptions are requested!\nThe date is illegal!").ShowAsync();
            }
            else if (Title.Text == "" && Details.Text == "" && (dateCompareResult == 1 || dateCompareResult == 0))
            {
                var i = new MessageDialog("Title and some descriptions are requested!").ShowAsync();
            }
            else if (Title.Text == "" && Details.Text != "" && dateCompareResult == -1)
            {
                var i = new MessageDialog("Title is requested!\nThe date is illegal!").ShowAsync();
            }
            else if (Title.Text != "" && Details.Text == "" && dateCompareResult == -1)
            {
                var i = new MessageDialog("Some descriptions is requested!\nThe date is illegal!").ShowAsync();
            }
            else if (Title.Text != "" && Details.Text == "" && (dateCompareResult == 1 || dateCompareResult == 0))
            {
                var i = new MessageDialog("Some descriptions is requested!").ShowAsync();
            }
            else if (Title.Text == "" && Details.Text != "" && (dateCompareResult == 1 || dateCompareResult == 0))
            {
                var i = new MessageDialog("Title is requested!").ShowAsync();
            }
            else if (Title.Text != "" && Details.Text != "" && dateCompareResult == -1)
            {
                var i = new MessageDialog("The date is illegal!").ShowAsync();
            }
        }

        //点击Cancel按钮时，检查Title、Description置为空，DueDate置为当前日期
        private void cancelClick(object sender, RoutedEventArgs e)
        {
            Title.Text = Details.Text = "";
            datepicker.Date = DateTime.Now.Date;
        }

        //点击Select按钮，能够在本地选择图片并显示
        private async void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
            //文件选择器  
            FileOpenPicker openPicker = new FileOpenPicker();
            //选择视图模式  
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            //openPicker.ViewMode = PickerViewMode.List;  
            //初始位置  
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //添加文件类型  
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");


            StorageFile file = await openPicker.PickSingleFileAsync();
            BitmapImage srcImage = new BitmapImage();

            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    //修改Image标签的文件源
                    await srcImage.SetSourceAsync(stream);
                    myImage.Source = srcImage;
                }
            }
            else
            {
                var i = new MessageDialog("Operation cancelled!").ShowAsync();
            }
        }
    }
}
