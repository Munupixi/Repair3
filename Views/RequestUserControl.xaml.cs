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
    /// Логика взаимодействия для RequestUserControl.xaml
    /// </summary>
    public partial class RequestUserControl : UserControl
    {
        private Request request;
        public RequestUserControl(Request request)
        {
            InitializeComponent();
            this.request = request;

            Repair3Context repairContext = new Repair3Context();

            RequestIdTextBlock.Text = request.RequestId.ToString();
            CreationDateTextBlock.Text = request.CreationDate.ToString();
            StatusTextBlock.Text = request.StatusId.ToString();
            StatusTextBlock.Text = repairContext.Statuses.FirstOrDefault(s => s.StatusId == request.StatusId).Title.ToString() ?? "null";
            ExecutorTextBlock.Text = request.ExecutorComment?.ToString();
            ServiceTypeTextBlock.Text = request.ServiceType?.ToString();
            FaultTypeTextBlock.Text = request.FaultType?.ToString();
            ExecutorCommentRichTextBox.Document =
                new FlowDocument(new Paragraph(
                new Run(request.ExecutorComment?.ToString())));
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (User.ActiveUser.RoleId == 1)
            {
                MainWindow.Frame.Content = new ManipulationRequest(request);
            }
        }
    }
}
