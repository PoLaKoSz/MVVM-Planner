﻿using Planner.Utilty;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Planner
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DataService dataService = new DataService();

            MainViewModel vm = new MainViewModel(dataService);
            MainWindow view = new MainWindow(vm);
            view.Show();
        }
    }
}
