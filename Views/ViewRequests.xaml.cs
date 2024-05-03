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

            Repair3Context repairContext = new Repair3Context();
            Rolelabel.Content = repairContext.Roles.FirstOrDefault(r => r.RoleId == User.ActiveUser.RoleId).Title;
            FilterComboBox.ItemsSource =
                repairContext.Statuses.Select(status => status.Title).ToList();

            if (User.ActiveUser.RoleId == 2) // исполнитель
            {
                CreateProductButton.Visibility = Visibility.Collapsed;

                requests = repairContext.Requests
                    .Where(r => r.Executor.UserId == User.ActiveUser.UserId)
                    .Include(r => r.Status)
                    .Include(r => r.Executor)
                    .ToList();
            }
            else // администратор
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
                    || (request.StatusId != FilterComboBox.SelectedIndex + 1 && FilterComboBox.SelectedIndex != -1))  // Фильтрация, поиск
                {
                    continue;
                }
                viewRequests.Add(request);
            }

            if (SortComboBox.SelectedIndex == 1) // сортировка
            {
                viewRequests = viewRequests.OrderByDescending(request => request.CreationDate).ToList();
            }
            else
            {
                viewRequests = viewRequests.OrderBy(request => request.CreationDate).ToList();
            }

            IssuedLabel.Content = viewRequests.Count(); // число выданных
            IssuedFromLabel.Content = requests.Count(); // общее число

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