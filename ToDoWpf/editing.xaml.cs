﻿using System;
using System.Windows;
using System.Windows.Input;

namespace ToDoWpf
{

    public partial class editing : Window
    {
        private readonly int index = new int();
        private readonly string StartText;
        public editing(int SelectedItemIndex)
        {
            InitializeComponent();
            DateLabel.Content = Class1.ToDo[SelectedItemIndex].Date;
            TextInput.Text = Class1.ToDo[SelectedItemIndex].ToDo;
            TextInput.SelectionStart = TextInput.Text.Length;
            TextInput.Focus();
            index = SelectedItemIndex;
            StartText = TextInput.Text;
        }

        private void ExitBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void TopBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void EditButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (TextInput.Text.Length == 0)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to delete this task?", "Empty task", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes: Class1.ToDo.RemoveAt(index); DialogResult = true; break;
                    case MessageBoxResult.No:
                        TextInput.Text = StartText; TextInput.SelectionStart = TextInput.Text.Length;
                        TextInput.Focus(); break;
                }
            }
            else
            {
                Class1.ToDo[index].ToDo = TextInput.Text;
                DialogResult = true;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TextInput.Text.Length == 0)
                {
                    MessageBoxResult result = MessageBox.Show("Do you want to delete this task?", "Empty task", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    switch (result)
                    {
                        case MessageBoxResult.Yes: Class1.ToDo.RemoveAt(index); DialogResult = true; break;
                        case MessageBoxResult.No:
                            TextInput.Text = StartText; TextInput.SelectionStart = TextInput.Text.Length;
                            TextInput.Focus(); break;
                    }
                }
                else
                {
                    Class1.ToDo[index].ToDo = TextInput.Text;
                    DialogResult = true;
                }
            }
        }

        private void DeleteTask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Class1.ToDo.RemoveAt(index); DialogResult = true;
        }

        private void UpdateTime_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Class1.ToDo[index].Time = DateTime.Now.ToShortTimeString();
            Class1.ToDo[index].Date = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
            DateLabel.Content = Class1.ToDo[index].Date;
        }
    }
}
