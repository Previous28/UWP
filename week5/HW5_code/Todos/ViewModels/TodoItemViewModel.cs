using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Todos.ViewModels
{
    class TodoItemViewModel
    {
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }

        private Models.TodoItem selectedItem = default(Models.TodoItem);
        public Models.TodoItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        public TodoItemViewModel()
        {
            this.selectedItem = null;
            // 加入三个用来测试的item
            this.allItems.Add(new Models.TodoItem(DateTime.Today, null, "Item1", "description1"));
            this.allItems.Add(new Models.TodoItem(DateTime.Today, null, "Item2", "description2"));
            this.allItems.Add(new Models.TodoItem(DateTime.Today, null, "好好学习", "天天向上"));
        }

        public void AddTodoItem(DateTime date, ImageSource img, string title, string description)
        {
            this.allItems.Add(new Models.TodoItem(date, img, title, description));
        }

        public void UpdateTodoItem(DateTime date, ImageSource img, string title, string description)
        {
            this.selectedItem.date = date;
            this.selectedItem.img = img;
            this.selectedItem.title = title;
            this.selectedItem.description = description;

            // set selectedItem to null after update
            this.selectedItem = null;
        }

    }
}
