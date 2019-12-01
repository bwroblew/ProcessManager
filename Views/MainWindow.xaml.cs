using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProcessManager
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            InitializePriorityComboBox();
            InitializeSortComboBox();
        }

        private void InitializePriorityComboBox()
        {
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(ProcessPriorityClass)).Cast<ProcessPriorityClass>();
            priorityComboBox.SelectedIndex = 0;
        }

        private void InitializeSortComboBox()
        {
            sortComboBox.ItemsSource = Enum.GetNames(typeof(ViewModel.SortKeys));
            sortComboBox.SelectedIndex = 0;
        }

        private void ProcessesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;

            if (listBox.SelectedItems.Count > 0)
            {
                var viewModel = (ViewModel)DataContext;
                viewModel.SelectedProcess = ((SingleProcess)listBox.SelectedItems[0]).Process;
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ViewModel)DataContext;
            viewModel.ChangeFilter(filterTextBox.Text);
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            var sortKey = (ViewModel.SortKeys)Enum.Parse(typeof(ViewModel.SortKeys), (string)sortComboBox.SelectionBoxItem);
            var viewModel = (ViewModel)DataContext;
            viewModel.ChangeSortKey(sortKey);
        }

        private void NewProcessButton_Click(object sender, RoutedEventArgs e)
        {
            var name = newProcessTextBox.Text;
            var time = Convert.ToInt32(newProcessTimeTextBox.Text);
            var viewModel = (ViewModel)DataContext;
            viewModel.CreateProcess(name, time);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ViewModel)DataContext;
            viewModel.UpdateProcessesList(sender, e);
        }

        private void PiorityButton_Click(object sender, RoutedEventArgs e)
        {
            var priority = (ProcessPriorityClass)priorityComboBox.SelectionBoxItem;
            var viewModel = (ViewModel)DataContext;
            viewModel.ChangePriority(priority);
        }

        private void KillButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (ViewModel)DataContext;
            viewModel.KillSelectedProcess();
        }
    }
}
