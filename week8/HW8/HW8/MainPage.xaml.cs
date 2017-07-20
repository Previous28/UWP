using System;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace HW8
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        double currentWidth;
        double currentHeight;
        double fullWidth;
        double fullHeight;

        public MainPage()
        {
            this.InitializeComponent();
        }

        //play the media
        public void OnMouseDownPlayMedia(Object sender, RoutedEventArgs e)
        {
            myMediaElement.Play();
            InitializePropertyValues();
        }

        //pause the media
        void OnMouseDownPauseMedia(Object sender, RoutedEventArgs e)
        {
            myMediaElement.Pause();
        }

        //stop the media
        void OnMouseDownStopMedia(Object sender, RoutedEventArgs e)
        {
            myMediaElement.Stop();
        }


        private void ElementMediaOpened(object sender, RoutedEventArgs e)
        {
            var ts = myMediaElement.NaturalDuration.TimeSpan;
            total.Text = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            timelineSlider.Maximum = ts.TotalMilliseconds;
        }

        private void ElementMediaEnded(object sender, RoutedEventArgs e)
        {
            myMediaElement.Stop();
        }

        private void SeekMediaPosition(object sender, RangeBaseValueChangedEventArgs e)
        {
            int SliderValue = (int)timelineSlider.Value;

            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            myMediaElement.Position = ts;
        }


        void InitializePropertyValues()
        {
            myMediaElement.Volume = (double)volumeSlider.Value;
        }

        //change the volume of the media
        private void ChangeMediaVolume(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            myMediaElement.Volume = (double)volumeSlider.Value;
        }

        //选择文件
        private async void OpenFileClick(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            //选择视图模式  
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            //openPicker.ViewMode = PickerViewMode.List;  
            //初始位置  
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            //添加文件类型  
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".avi");
            openPicker.FileTypeFilter.Add(".mp3");
            openPicker.FileTypeFilter.Add(".asf");
            openPicker.FileTypeFilter.Add(".wma");


            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                    myMediaElement.SetSource(stream, file.ContentType);

            }
        }

        private void fullScreenClick(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();

            bool isInFullScreenMode = view.IsFullScreenMode;

            if (isInFullScreenMode)
            {
                view.ExitFullScreenMode();
                fullWidth = myMediaElement.Width;
                fullHeight = myMediaElement.Height;
                myMediaElement.Width = currentWidth;
                myMediaElement.Height = currentHeight;
            }
            else
            {
                view.TryEnterFullScreenMode();
                fullWidth = 1280;
                fullHeight = 720;


                currentWidth = myMediaElement.Width;
                currentHeight = myMediaElement.Height;
                myMediaElement.Width = fullWidth;
                myMediaElement.Height = fullHeight;
            }
        }

        private void myMediaElementLoaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += (ss, ee) =>
            {
                //显示当前视频进度
                var ts = myMediaElement.Position;
                current.Text = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                timelineSlider.Value = ts.TotalMilliseconds;
            };
            timer.Start();
        }
    }
}
