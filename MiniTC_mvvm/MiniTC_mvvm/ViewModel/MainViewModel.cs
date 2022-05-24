using MiniTC_mvvm.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MiniTC_mvvm.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        public DriverPanel leftPanel {get; set;}
        public DriverPanel rightPanel { get; set; }
        public MainViewModel()
        {
            leftPanel = new DriverPanel();
            rightPanel = new DriverPanel();
        }

        public void copyItem(object sender)
        {
            string destination = rightPanel.path;
            string[] leftPanelPath = leftPanel.path.Split("\\");
            string updateData = destination;
            destination += "\\";
            destination += leftPanelPath[leftPanelPath.Length - 1];
            try
            {
                if(File.Exists(rightPanel.path))
                {
                    rightPanel.refreshPath(Directory.GetParent(updateData).ToString());
                    return;
                }

                if(!File.Exists(destination) && File.Exists(leftPanel.path))
                {
                    File.Copy(leftPanel.path, destination);
                    rightPanel.refreshPath(updateData);
                }
                else
                {
                    //
                }
            }
            catch(UnauthorizedAccessException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #region Commands

        private ICommand _leftSelectionChanged = null;
        public ICommand leftSelectionChanged
        {
            get
            {
                if(_leftSelectionChanged == null)
                {
                    _leftSelectionChanged = new RelayCommand(leftPanel.updateData, arg => true);
                }
                return _leftSelectionChanged;
            }
        }

        private ICommand _leftDriverChanged = null;
        public ICommand leftDriverChanged
        {
            get
            {
                if (_leftDriverChanged == null)
                {
                    _leftDriverChanged = new RelayCommand(leftPanel.defaultPath, arg => true);
                }
                return _leftDriverChanged;
            }
        }

        private ICommand _rightSelectionChanged = null;
        public ICommand rightSelectionChanged
        {
            get
            {
                if(_rightSelectionChanged == null)
                {
                    _rightSelectionChanged = new RelayCommand(rightPanel.updateData, arg => true);
                }
                return _rightSelectionChanged;
            }
        }

        private ICommand _rightDriverChanged = null;
        public ICommand rightDriverChanged
        {
            get
            {
                if(_rightDriverChanged == null)
                {
                    _rightDriverChanged = new RelayCommand(rightPanel.defaultPath, arg => true);
                }
                return _rightDriverChanged;
            }
        }

        private ICommand _copyClick = null;
        public ICommand copyClick
        {
            get
            {
                if(_copyClick == null)
                {
                    _copyClick = new RelayCommand(copyItem, arg => File.Exists(leftPanel.path) && Directory.Exists(rightPanel.path)); 
                }
                return _copyClick;
            }
        }

        #endregion
    }
}
