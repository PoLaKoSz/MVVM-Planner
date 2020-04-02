﻿using Planner.Utilty;
using System;

namespace Planner.Models
{
    public class Task:ObservableObject
    {
        private string _toDo;
        private bool _done;
        public Task(string newtask)
        {
            ToDo = newtask;
        }
        public string ToDo 
        {
            get 
            {
                return _toDo; 
            } 
            set 
            { 
                if (value == _toDo) 
                    return;
                _toDo = value; OnPropertyChanged(nameof(ToDo));
            } 
        }
        public bool Done 
        {
            get 
            {
                return _done;
            }
            set 
            {
                if (value == _done)
                    return;
                _done = value; OnPropertyChanged(nameof(Done));
            }
        }
        public string Date { get; set; } = DateTime.Now.ToShortTimeString();
    }
}
