using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Todos.Models
{
    public class TodoItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long id { get; set; }

        private string _title;
        public string title
        {
            set
            {
                _title = value;
                NotifyPropertyChanged("title");
            }
            get
            {
                return _title;
            }
        }

        private ImageSource _img;
        public ImageSource img
        {
            set
            {
                _img = value;
                NotifyPropertyChanged("img");
            }
            get
            {
                return _img;
            }
        }

        public string description { get; set; }

        public bool completed { get; set; }

        public DateTime date { get; set; }

        private void NotifyPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        public TodoItem(long id, DateTime date, ImageSource img, string title = "", string description = "")
        {
            this.id = id;
            this.title = title;
            this.img = (img == null ? new BitmapImage(new Uri("ms-appx:///Assets/background.jpeg")) : img);
            this.description = description;
            this.date = date;
            this.completed = false; //默认为未完成
        }
    }
}
