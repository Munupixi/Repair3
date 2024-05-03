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
    /// <summary>
    /// Логика взаимодействия для ManipulationRequest.xaml
    /// </summary>
    public partial class ManipulationRequest : Page
    {
        public ManipulationRequest()
        {
            InitializeComponent();
            Repair3Context repairContext = new Repair3Context();
            if (repairContext.Requests.Count() == 0)
            {
                RequestIdTextBox.Text = "1";
            }
            else
            {
                RequestIdTextBox.Text = (repairContext.Requests.Max(
                request => request.RequestId) + 1).ToString();
            }
            

            StatusComboBox.ItemsSource =
                repairContext.Statuses.Select(status => status.Title).ToList();
            ExecutorComboBox.ItemsSource = repairContext.Users.Where(user => user.RoleId == 2).Select(user => user.Name).ToList();

        }
        public ManipulationRequest(Request request)
        {
            InitializeComponent();
            Repair3Context repairContext = new Repair3Context();
            StatusComboBox.ItemsSource =
                repairContext.Statuses.Select(status => status.Title).ToList();
            ExecutorComboBox.ItemsSource = repairContext.Users.Where(user => user.RoleId == 2).Select(user => user.Name).ToList();
            RequestIdTextBox.Text = request.RequestId.ToString();
            CreationDateTextBox.Text = request.CreationDate.ToString();
            ExecutorCommentRichTextBox.Document =
                new FlowDocument(new Paragraph(
                new Run(request.ExecutorComment?.ToString())));
            StatusComboBox.SelectedIndex = request.StatusId;
            ExecutorComboBox.SelectedIndex = request.ExecutorId ?? -1;
            FaultTypeTextBox.Text = request.FaultType?.ToString();
            ServiceTypeTextBox.Text = request.ServiceType?.ToString();
            CompleteNameTextBox.Text = request.CompleteName?.ToString();
        }
        private void DeleteRequest()
        {
            Repair3Context repairContext = new Repair3Context();
            var requestToRemove = repairContext.Requests.FirstOrDefault(
               request => request.RequestId ==
               Convert.ToInt32(RequestIdTextBox.Text));

            if (requestToRemove != null)
            {
                repairContext.Requests.Remove(requestToRemove);
                repairContext.SaveChanges();
            }
        }
        private void CreateRequest()
        {
            Repair3Context repairContext = new Repair3Context();
            Request request = new Request(
            Convert.ToInt32(RequestIdTextBox.Text),
            DateOnly.Parse(CreationDateTextBox.Text),
            new TextRange(ExecutorCommentRichTextBox.Document.ContentStart,
            ExecutorCommentRichTextBox.Document.ContentEnd).Text,
            StatusComboBox.SelectedIndex + 1,
            ServiceTypeTextBox.Text,
            FaultTypeTextBox.Text);
            repairContext.Requests.Add(request);
            repairContext.SaveChanges();
        }
        private bool IsValid()
        {
            if (!DateOnly.TryParse(CreationDateTextBox.Text, out _))
            {
                MessageBox.Show("Некорректный формат даты!",
                    "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            if (FaultTypeTextBox.Text.Length > 50)
            {
                MessageBox.Show("Тип ошибки не должен быть длинее 50 симыволов!",
                    "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }
            if (ServiceTypeTextBox.Text.Length > 50)
            {
                MessageBox.Show("Тип оборудования не должен быть длинее 50 симыволов!",
                    "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            if (StatusComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Необходимо выбрать статус!",
                    "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValid()) return;
            DeleteRequest();
            CreateRequest();
            MainWindow.Frame.Content = new ViewRequests();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteRequest();
            MainWindow.Frame.Content = new ViewRequests();
        }
    }
}
