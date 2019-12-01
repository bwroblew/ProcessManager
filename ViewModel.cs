using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using ProcessManager.Properties;

namespace ProcessManager
{
    public class ViewModel : INotifyPropertyChanged
    {
        private int _progressStatus;
        private Process _selectedProcess;
        public string Filter;
        public ObservableCollection<SingleProcess> _processesList = new ObservableCollection<SingleProcess>();
        public SortKeys CurrentSortKey;
        public float timeout;
        public float elapsedTime;

        public enum SortKeys
        {
            Name,
            Id,
            Threads
        }

        public ViewModel()
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            timer.Tick += UpdateProcessesList;
            timer.Start();
            Filter = "";
            CurrentSortKey = SortKeys.Name;
        }

        public int ProgressStatus
        {
            get => _progressStatus;
            set
            {
                _progressStatus = value;
                OnPropertyChanged();
            }
        }

        public Process SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SingleProcess> ProcessesList
        {
            get { return _processesList; }
            set
            {
                _processesList = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateProcessesList(object sender, EventArgs e)
        {
            var newProcessList = new ObservableCollection<SingleProcess>();
            foreach (var process in Process.GetProcesses())
            {
                if (process.ProcessName.ToLower().Contains(Filter.ToLower()) || Filter == "")
                {
                    newProcessList.Add(new SingleProcess(process));
                }
            }
            ProcessesList = new ObservableCollection<SingleProcess>(newProcessList);
            SortProcess();
        }

        public void ChangePriority(ProcessPriorityClass priority)
        {
            if (SelectedProcess == null) return;
            SelectedProcess.PriorityClass = priority;
        }

        public void ChangeFilter(string filter)
        {
            Filter = filter;
        }

        public void ChangeSortKey(SortKeys sort)
        {
            CurrentSortKey = sort;
            SortProcess();
        }

        public void SortProcess()
        {
            ObservableCollection<SingleProcess> sortedProcessesList = null;
            switch (CurrentSortKey)
            {
                case SortKeys.Name:
                default:
                    sortedProcessesList = new ObservableCollection<SingleProcess>(ProcessesList.OrderBy(p => p.Process.ProcessName));
                    break;
                case SortKeys.Id:
                    sortedProcessesList = new ObservableCollection<SingleProcess>(ProcessesList.OrderBy(p => p.Process.Id));
                    break;
                case SortKeys.Threads:
                    sortedProcessesList = new ObservableCollection<SingleProcess>(ProcessesList.OrderByDescending(p => p.Process.Threads.Count));
                    break;
            }
            ProcessesList = new ObservableCollection<SingleProcess>(sortedProcessesList);
        }

        public void CreateProcess(string name, int timeout)
        {
            if (this.timeout > 0)
            {
                MessageBox.Show("You have to wait until previous process finishes!");
            }
            this.timeout = timeout;
            elapsedTime = 0f;
            var process = new Process();
            process.StartInfo.FileName = name;
            process.StartInfo.ErrorDialog = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();

            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (sender, e) => timeController(sender, e, process);
            timer.Start();

        }

        private void timeController(object sender, EventArgs e, Process process)
        {
            elapsedTime += 1f;
            try
            {
                ProgressStatus = (int) (elapsedTime / timeout * 100);
            }
            catch (DivideByZeroException)
            {
                ClearProgress();
            }

            if (elapsedTime >= timeout)
            {
                var timer = (DispatcherTimer)sender;
                timer.Stop();
                process.Kill();
                ClearProgress();
                MessageBox.Show("Time of the created process has ended!");
            }
            else if (process.HasExited)
            {
                var timer = (DispatcherTimer)sender;
                timer.Stop();
                ClearProgress();
                MessageBox.Show("Time if the created process has ended but process has been killed before!");
            }
        }

        public void ClearProgress()
        {
            elapsedTime = 0f;
            timeout = 0f;
            ProgressStatus = 0;
        }

        public void KillSelectedProcess()
        {
            if (SelectedProcess == null) return;
            SelectedProcess.Kill();
            SelectedProcess = null;
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}