﻿using Planner.Models;
using Planner.Utilty;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Planner
{
    public class AppViewModel : ObservableObject
    {
        private string _inputTaskText;
        private string _inputFolderText;
        private bool _isFolderInputVisible;
        private bool _isFolderInputFocused;
        private double _lineWidth;
        private readonly DataService service = new DataService("FolderData.txt");

        public RelayCommand ClosingWindowCommand { get; private set; }
        public RelayCommand MinimizeWindowCommand { get; private set; }
        public RelayCommand AddFolderCommand { get; private set; }
        public RelayCommand SelectFolderCommand { get; private set; }
        public RelayCommand AddTaskCommand { get; private set; }
        public RelayCommand MakeTaskDoneCommand { get; private set; }
        public RelayCommand DeleteTaskCommand { get; private set; }


        public ObservableCollection<Folder> Folders { get; set; }

        public double LineWidth 
        {
            get
            {
                int index = new int();
                for (int i = 0; i < Folders.Count; i++)
                {
                    if (Folders[i].Selected)
                    {
                        index = i;
                    }
                }
                if (Folders[index].Tasks.Count == 0) 
                {
                    return 0;
                }
                double percentOfDone = Convert.ToDouble(Folders[index].NumberOfDoneTasks) / Convert.ToDouble(Folders[index].Tasks.Count);
                _lineWidth = 800 * percentOfDone;
                return _lineWidth;
            }
            set
            {
                if (value == _lineWidth)
                    return;
                _lineWidth = value;
                OnPropertyChanged(nameof(LineWidth));
            }
        }
        public string InputTaskText
        {
            get
            {
                return _inputTaskText;
            }
            set
            {
                if (value == _inputTaskText)
                    return;
                _inputTaskText = value;
                OnPropertyChanged(nameof(InputTaskText));
            }
        }
        public bool IsFolderInputFocused
        {
            get
            {
                return _isFolderInputFocused;
            }
            set
            {
                if (value == _isFolderInputFocused)
                    return;
                _isFolderInputFocused = value;
                OnPropertyChanged(nameof(IsFolderInputFocused));
            }
        }
        public string InputFolderText
        {
            get
            {
                return _inputFolderText;
            }
            set
            {
                if (value == _inputFolderText)
                    return;
                _inputFolderText = value;
                OnPropertyChanged(nameof(InputFolderText));
            }
        }
        public bool IsFolderInputVisible
        {
            get
            {
                return _isFolderInputVisible;
            }
            set
            {
                if (value == _isFolderInputVisible)
                    return;
                _isFolderInputVisible = value;
                OnPropertyChanged(nameof(IsFolderInputVisible));
            }
        }
        private void MinimizeWindow()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void MakeTaskDone(object parameter)
        {
            if (parameter != null)
            {
                (parameter as Task).Done = !(parameter as Task).Done;

                if ((parameter as Task).Done)
                {
                    for (int i = 0; i < Folders.Count; i++)
                    {
                        if (Folders[i].Selected)
                        {
                            Folders[i].NumberOfDoneTasks++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Folders.Count; i++)
                    {
                        if (Folders[i].Selected)
                        {
                            Folders[i].NumberOfDoneTasks--;
                        }
                    }
                }
                OnPropertyChanged(nameof(LineWidth));
            }
        }
        private void AddFolder(object parameter)
        {
            if (CanAddText(parameter.ToString()) && IsFolderInputVisible)
            {
                Folders.Add(new Folder(parameter as string));
                IsFolderInputVisible = !IsFolderInputVisible;
                return;
            }
            else
            {
                InputFolderText = "";
                IsFolderInputVisible = !IsFolderInputVisible;
            }
        }
        private bool CanAddText(string InputText)
        {
            return !string.IsNullOrWhiteSpace(InputText);
        }

        private void DeleteTask(object parameter)
        {
            if (parameter != null)
            {
                if ((parameter as Task).Done)
                {
                    for (int i = 0; i < Folders.Count; i++)
                        if (Folders[i].Selected)
                        {
                            Folders[i].Tasks.Remove(parameter as Task);
                            Folders[i].NumberOfDoneTasks--;
                        }
                }
                else
                {
                    for (int g = 0; g < Folders.Count; g++)
                        if (Folders[g].Selected)
                            Folders[g].Tasks.Remove(parameter as Task);
                }
                OnPropertyChanged(nameof(LineWidth));
            }
        }
        private void SelectFolder(object parameter)
        {
            if (parameter != null)
            {
                foreach (Folder i in Folders)
                {
                    i.Selected = false;
                }
            (parameter as Folder).Selected = true;
            }
            OnPropertyChanged(nameof(LineWidth));
        }
        private void ClosingWindow()
        {
            Application.Current.Shutdown();
            try
            {
                service.SaveData(Folders);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Oops, there was an error : "+ex.ToString());
            }
        }
        private void AddTask()
        {
            if (CanAddText(InputTaskText))
                for (int i = 0; i < Folders.Count; i++)
                    if (Folders[i].Selected)
                        Folders[i].Tasks.Add(new Task(InputTaskText));
            InputTaskText = "";
            OnPropertyChanged(nameof(LineWidth));
        }
        public AppViewModel()
        {
            try
            {
                Folders = service.LoadData();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Oops, there was an error : "+ex.ToString());
            }
            ClosingWindowCommand = new RelayCommand(p =>ClosingWindow(), p => true);
            MinimizeWindowCommand = new RelayCommand(p => MinimizeWindow(), p => true);
            AddFolderCommand = new RelayCommand(p => AddFolder(p), p => true);
            SelectFolderCommand = new RelayCommand(p => SelectFolder(p), p => true);
            AddTaskCommand = new RelayCommand(p => AddTask(), p => true);
            MakeTaskDoneCommand = new RelayCommand(p => MakeTaskDone(p), p => true);
            DeleteTaskCommand = new RelayCommand(p => DeleteTask(p), p => true);
            IsFolderInputFocused = true;
        }
    }
}
