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

namespace Animal
{
    public sealed partial class MainPage : Page
    {
        private delegate string AnimalSaying(object sender, myEventArgs e);//声明一个委托
        private event AnimalSaying Say;//委托声明一个事件

        public MainPage()
        {
            this.InitializeComponent();
        }

        interface Animal
        {
            //方法
            string saying(object sender, myEventArgs e);
        }

        class cat : Animal
        {
            TextBlock word;

            public cat(TextBlock w)
            {
                this.word = w;
            }

            public string saying(object sender, myEventArgs e)
            {
                this.word.Text += "Cat: I am a cat.\n";
                return "";
            }

        }

        class dog : Animal
        {
            TextBlock word;

            public dog(TextBlock w)
            {
                this.word = w;
            }

            public string saying(object sender, myEventArgs e)
            {
                this.word.Text += "Dog: I am a dog.\n";
                return "";
            }

        }

        class pig : Animal
        {
            TextBlock word;

            public pig(TextBlock w)
            {
                this.word = w;
            }

            public string saying(object sender, myEventArgs e)
            {
                this.word.Text += "Pig: I am a pig.\n";
                return "";
            }

        }

        private cat c;
        private dog d;
        private pig p;

        private void speakButton_Click(object sender, RoutedEventArgs e)
        {
            words.Text = "";
            c = new cat(words);
            d = new dog(words);
            p = new pig(words);
            //获取随机数
            Random r = new Random();
            int RandKey = r.Next(0, 30);
            //根据随机数来注销事件和注册事件
            if (RandKey >= 0 && RandKey < 10)
            {
                Say -= new AnimalSaying(d.saying);
                Say -= new AnimalSaying(p.saying);
                Say += new AnimalSaying(c.saying);
            }
            else if (RandKey >= 10 && RandKey < 20)
            {
                Say -= new AnimalSaying(c.saying);
                Say -= new AnimalSaying(p.saying);
                Say += new AnimalSaying(d.saying);
            }
            else
            {
                Say -= new AnimalSaying(c.saying);
                Say -= new AnimalSaying(d.saying);
                Say += new AnimalSaying(p.saying);
            }
            //执行事件
            Say(this, new myEventArgs());
        }

        //自定义一个Eventargs传递事件参数
        class myEventArgs : EventArgs
        {
            public myEventArgs() { }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            words.Text = "";
            c = new cat(words);
            d = new dog(words);
            p = new pig(words);
            //获取输入
            string inputAnimal = this.textBox.Text;
            if (inputAnimal == "cat")
            {
                Say -= new AnimalSaying(d.saying);
                Say -= new AnimalSaying(p.saying);
                Say += new AnimalSaying(c.saying);
            }
            else if (inputAnimal == "dog")
            {
                Say -= new AnimalSaying(c.saying);
                Say -= new AnimalSaying(p.saying);
                Say += new AnimalSaying(d.saying);
            }
            else if (inputAnimal == "pig")
            {
                Say -= new AnimalSaying(c.saying);
                Say -= new AnimalSaying(d.saying);
                Say += new AnimalSaying(p.saying);
            }
            this.textBox.Text = "";
            //执行事件
            Say(this, new myEventArgs());
        }
    }
}
