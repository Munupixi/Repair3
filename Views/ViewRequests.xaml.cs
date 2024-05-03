using Microsoft.EntityFrameworkCore;
using Repair3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Repair3.Views
{
    public partial class ViewRequests : Page
    {
        private List<Request> requests;
        private List<Request> viewRequests = new List<Request>();

        public ViewRequests()
        {
            InitializeComponent();
            if (User.ActiveUser.RoleId == 1)
            {
                CreateProductButton.Visibility = Visibility.Collapsed;
            }
            Repair3Context repairContext = new Repair3Context();
            FilterComboBox.ItemsSource =
                repairContext.Statuses.Select(status => status.Title).ToList();

            if (User.ActiveUser.RoleId == 2)
            {
                requests = repairContext.Requests
                    .Where(r => r.Executor.UserId == User.ActiveUser.UserId)
                    .Include(r => r.Status)
                    .Include(r => r.Executor)
                    .ToList();
            }
            else
            {
                requests = repairContext.Requests
                    .Include(r => r.Status)
                    .Include(r => r.Executor)
                    .ToList();
            }
            UpdateView();
        }

        private void UpdateView()
        {
            viewRequests.Clear();
            foreach (Request request in requests)
            {
                if ((!request.ServiceType.ToLower().Contains(SearchTextBox.Text.ToLower()) &&
                    !request.FaultType.ToLower().Contains(SearchTextBox.Text.ToLower()))
                    || (request.StatusId != FilterComboBox.SelectedIndex + 1 && FilterComboBox.SelectedIndex != -1))
                {
                    continue;
                }
                viewRequests.Add(request);
            }

            if (SortComboBox.SelectedIndex == 1)
            {
                viewRequests = viewRequests.OrderByDescending(request => request.CreationDate).ToList();
            }
            else
            {
                viewRequests = viewRequests.OrderBy(request => request.CreationDate).ToList();
            }

            IssuedLabel.Content = viewRequests.Count();
            IssuedFromLabel.Content = requests.Count();

            RequestsDataGrid.ItemsSource = viewRequests;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) => UpdateView();

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateView();

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateView();

        private void CreateProductButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame.Content = new ManipulationRequest();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            FilterComboBox.SelectedIndex = -1;
            SortComboBox.SelectedIndex = -1;
            SearchTextBox.Text = String.Empty;
            UpdateView();
        }

        private void ManipulationButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Frame.Content = new ManipulationRequest(
                (sender as FrameworkElement).DataContext as Request);
        }
    }
}