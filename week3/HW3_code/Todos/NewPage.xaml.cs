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

        private ViewModels.TodoItemViewModel ViewModel;

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

            ViewModel = ((ViewModels.TodoItemViewModel)e.Parameter);
            if (ViewModel.SelectedItem == null)
            {
                createOrUpdateButton.Content = "Create";
                //var i = new MessageDialog("Welcome!").ShowAsync();
            }
            else
            {
                createOrUpdateButton.Content = "Update";
                Title.Text = ViewModel.SelectedItem.title;
                myImage.Source = ViewModel.SelectedItem.img;
                Details.Text = ViewModel.SelectedItem.description;
                datepicker.Date = ViewModel.SelectedItem.date;
                // ...
            }
        }

        //点击create按钮时，检查Title、Description是否为空，DueDate是否正确
        private async void createOrUpdateItem(object sender, RoutedEventArgs e)
        {
            //获取DatePicker的时间
            string str1 = datepicker.Date.ToString("yyyy-MM-dd");
            //获取当前时间
            string str2 = DateTime.Now.Date.ToString("yyyy-MM-dd");
            var dateCompareResult = string.Compare(str1, str2);
            string alert = "";
            if (Title.Text == "")
                alert += "Title can't be empty!\n";
            if (Details.Text == "")
                alert += "Detail can't be empty!\n";
            if (dateCompareResult < 0)
                alert += "The date is illegal!\n";
            if (alert != "")
            {
                var i = new MessageDialog(alert).ShowAsync();
            }
            else
            {
                if (createOrUpdateButton.Content.ToString() == "Create")
                {
                    ViewModel.AddTodoItem(datepicker.Date.DateTime, myImage.Source, Title.Text, Details.Text);
                    await new MessageDialog("Create successfully!").ShowAsync();
                    ViewModel.SelectedItem = null;
                    Frame.Navigate(typeof(MainPage), ViewModel);
                }
                else
                {
                    ViewModel.UpdateTodoItem(datepicker.Date.DateTime, myImage.Source, Title.Text, Details.Text);
                    await new MessageDialog("Update successfully!").ShowAsync();
                    ViewModel.SelectedItem = null;
                    Frame.Navigate(typeof(MainPage), ViewModel);
                }
            }
        }

        //点击Cancel按钮时，检查Title、Description置为空，DueDate置为当前日期，图片重置
        private void cancelClick(object sender, RoutedEventArgs e)
        {
            Title.Text = Details.Text = "";
            datepicker.Date = DateTime.Now.Date;
            myImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/background.jpeg"));
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
        }

        private async void DeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem != null)
            {
                ViewModel.AllItems.Remove(ViewModel.SelectedItem);//调用Remove接口
                ViewModel.SelectedItem = null;
                await new MessageDialog("Delete successfully!").ShowAsync();
                Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }
    }
}
