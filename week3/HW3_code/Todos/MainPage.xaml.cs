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
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Todos
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            this.ViewModel = new ViewModels.TodoItemViewModel();
        }

        ViewModels.TodoItemViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }

            if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel))
            {
                this.ViewModel = (ViewModels.TodoItemViewModel)(e.Parameter);
            }
        }

        private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (Models.TodoItem)(e.ClickedItem);
            //如果是窄屏则跳转到新页面，否则，在右边部分同步所选Item的相关信息
            if (InlineToDoItemViewGrid.Visibility.ToString() == "Collapsed")
            {
                Frame.Navigate(typeof(NewPage), ViewModel);
            }
            else
            {
                createOrUpdateButton.Content = "Update";
                Title.Text = ViewModel.SelectedItem.title;
                myImage.Source = ViewModel.SelectedItem.img;
                Details.Text = ViewModel.SelectedItem.description;
                datepicker.Date = ViewModel.SelectedItem.date;
            }
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //只有在窄面显示的时候点击MainPage底部的“＋”按钮，才会跳转到NewPage
            if (InlineToDoItemViewGrid.Visibility.ToString() == "Collapsed")
            {
                ViewModel.SelectedItem = null;
                Frame.Navigate(typeof(NewPage), ViewModel);
            }
        }
        //选中后显示横线
        private void check(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as DependencyObject);
            Line line = VisualTreeHelper.GetChild(parent, 3) as Line;
            line.Opacity = 1;
        }
        //取消选中横线消失
        private void uncheck(object sender, RoutedEventArgs e)
        {
            var parent = VisualTreeHelper.GetParent(sender as DependencyObject);
            Line line = VisualTreeHelper.GetChild(parent, 3) as Line;
            line.Opacity = 0;
        }

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
            {   //根据按钮的文本内容确定要调用的函数
                if (createOrUpdateButton.Content.ToString() == "Create")
                {
                    ViewModel.AddTodoItem(datepicker.Date.DateTime, myImage.Source, Title.Text, Details.Text);
                    await new MessageDialog("Create successfully!").ShowAsync();
                    ViewModel.SelectedItem = null;
                    //Frame.Navigate(typeof(MainPage), ViewModel);
                }
                else
                {
                    ViewModel.UpdateTodoItem(datepicker.Date.DateTime, myImage.Source, Title.Text, Details.Text);
                    await new MessageDialog("Update successfully!").ShowAsync();
                    ViewModel.SelectedItem = null;
                    //Frame.Navigate(typeof(MainPage), ViewModel);
                }
            }
        }

        //点击Cancel按钮时，检查Title、Description置为空，DueDate置为当前日期,重置图片
        private void cancelClick(object sender, RoutedEventArgs e)
        {
            Title.Text = Details.Text = "";
            datepicker.Date = DateTime.Now.Date;
            myImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/background.jpeg"));
        }

    }
}
