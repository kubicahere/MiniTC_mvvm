using MiniTC_mvvm.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace MiniTC_mvvm.ViewModel
{
    class DriverPanel : BaseViewModel
    {
        private string _driveSelected;
        private string _listSelected;
        private string _path;
        private ObservableCollection<string> _data;
        private string _diskPathFill = null;

        public DriverPanel()
        {
            diskList = Directory.GetLogicalDrives();
            _path = diskList[0];
            _driveSelected = _path;
            _data = LoadDriveContent(_driveSelected);
        }
        #region getters & setters
        public string[] diskList { get; set; }

        public string driveSelected
        {
            get
            {
                return _driveSelected;
            }
            set
            {
                _driveSelected = value;
                OnPropertyChanged(nameof(driveSelected));
            }
        }
        public string listSelected
        {
            get
            {
                return _listSelected;
            }
            set
            {
                _listSelected = value;
                OnPropertyChanged(nameof(listSelected));
            }
        }
        public string path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                OnPropertyChanged(nameof(path));
            }
        }

        public ObservableCollection<string> data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                OnPropertyChanged(nameof(data));
            }
        }
        public string diskPathFill
        {
            get
            {
                return _diskPathFill;
            }
            set
            {
                _diskPathFill = value;
            }
        }
        #endregion

        #region Methods
        public ObservableCollection<string> LoadDriveContent(string path)
        {
            try
            {
                ObservableCollection<string> listOfElements = new ObservableCollection<string>();
                string[] directories = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);
                string relativePath = string.Empty;
                if(!diskList.Contains(path))
                {
                    listOfElements.Add("..");
                }
                for(int i = 0; i < directories.Length; i++)
                {
                    if(directories[i].Contains(path))
                    {
                        relativePath = directories[i].Replace(path, "");
                        if (relativePath.ElementAt(0).Equals(Convert.ToChar("\\")))
                        {
                            relativePath = relativePath.Substring(1);
                        }
                    }
                    string element = "<" + path[0] + ">" + relativePath;
                    listOfElements.Add(element);
                }
                for(int i = 0; i < files.Length; i++)
                {
                    if (files[i].Contains(path))
                    {
                        relativePath = files[i].Replace(path, "");
                        if (relativePath.ElementAt(0).Equals(Convert.ToChar("\\")))
                        {
                            relativePath = relativePath.Substring(1);
                        }
                    }
                    string element = relativePath;
                    listOfElements.Add(element);
                }
                return listOfElements;
            }
            catch(UnauthorizedAccessException)
            {
                defaultPath(new object());
                return data;
            }
        }

        public void defaultPath(object sender)
        {
            path = driveSelected;
            data = LoadDriveContent(driveSelected);
        }

        public void refreshPath(string path)
        {
            this.path = path;
            data = LoadDriveContent(path);
        }

        public void updateData(object sender)
        {
            FileAttributes temp = File.GetAttributes(path);
            DirectoryInfo parent = Directory.GetParent(path);
            string fillPath = string.Empty;
            if (fillPath == string.Empty)
            {
                fillPath += path;
                if(File.Exists(fillPath))  // sprawdzanie czy plik zaznaczony -> zeby uniknac nadpisania sciezki 
                {
                    fillPath = parent.FullName;
                }
            }

            if(listSelected != null)
            {
                if (listSelected == "..")
                {
                    if(temp.HasFlag(FileAttributes.Directory))
                    {
                        path = parent.FullName;
                        data = LoadDriveContent(path);
                    }
                    else
                    {
                        parent = Directory.GetParent(parent.FullName);
                        path = parent.FullName;
                        data = LoadDriveContent(path);
                    }
                    listSelected = null;
                }
                else if(listSelected[0] == '<')
                {
                    string slash = "\\";
                    if(fillPath != (path[0] + ":\\"))
                    {
                        listSelected = listSelected.Substring(3);
                        fillPath += slash + listSelected;
                    }
                    else
                    {
                        listSelected = listSelected.Substring(3);
                        fillPath += listSelected;
                    }
                    path = fillPath;
                    data = LoadDriveContent(fillPath);
                }
                else //obsluga plikow
                {
                    if (fillPath != (path[0] + ":\\"))
                    {
                        fillPath += "\\" + listSelected;
                    }
                    else
                    {
                        fillPath += listSelected;
                    }
                    path = fillPath;
                }
            }
        }
        #endregion
    }
}
