using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todos.Models;
using Windows.UI.Xaml.Media;

namespace Todos.ViewModels
{
    class TodoItemViewModel
    {
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return this.allItems; } }

        private Models.TodoItem selectedItem = default(Models.TodoItem);
        public Models.TodoItem SelectedItem { get { return selectedItem; } set { this.selectedItem = value; } }

        public Models.TodoItem getItem(long Id)
        {
            foreach (TodoItem i in allItems)
            {
                if (i.id == Id)
                    return i;
            }
            return null;
        }

        public TodoItemViewModel()
        {
            this.selectedItem = null;
            var conn = App.conn;
            var sql = "SELECT * FROM TodoItem";
            try
            {
                using (var statement = conn.Prepare(sql))
                {
                    while (SQLiteResult.ROW == statement.Step())
                    {
                        string dateStr = (string)statement[3];
                        dateStr = dateStr.Substring(0, dateStr.IndexOf(' '));
                        DateTime date = new DateTime(int.Parse(dateStr.Split('/')[0]), int.Parse(dateStr.Split('/')[1]), int.Parse(dateStr.Split('/')[2]));
                        this.AddTodoItem((long)statement[0], date, null,
                            (string)statement[1], (string)statement[2]);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddTodoItem(long id, DateTime date, ImageSource img, string title, string description)
        {
            this.allItems.Add(new Models.TodoItem(id, date, img, title, description));
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
