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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            bool suspending = ((App)App.Current).isSuspending;
            if (suspending)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                composite["box1isChecked"] = checkBox1.IsChecked.ToString();
                composite["box2isChecked"] = checkBox2.IsChecked.ToString();
                ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] = composite;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TheWorkInProgress"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] as ApplicationDataCompositeValue;
                    if ((string)composite["box1isChecked"] == "True") {
                        checkBox1.IsChecked = true;
                        line1.Visibility = Visibility.Visible;
                    } else {
                        checkBox1.IsChecked = false;
                        line1.Visibility = Visibility.Collapsed;
                    }
                    if ((string)composite["box2isChecked"] == "True") {
                        checkBox2.IsChecked = true;
                        line2.Visibility = Visibility.Visible;
                    } else {
                        checkBox2.IsChecked = false;
                        line2.Visibility = Visibility.Collapsed;
                    }

                    ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
                }
            }
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewPage), "");
        }

        //只有当CheckBox被勾选了，横线Line才出现
        private void CheckBox1_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox1.IsChecked == true)
            {
                line1.Visibility = Visibility.Visible;
            }
            else if (checkBox1.IsChecked == false)
            {
                line1.Visibility = Visibility.Collapsed;
            }
        }

        private void CheckBox2_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox2.IsChecked == true)
            {
                line2.Visibility = Visibility.Visible;
            }
            else if (checkBox2.IsChecked == false)
            {
                line2.Visibility = Visibility.Collapsed;
            }
        }
    }
}
