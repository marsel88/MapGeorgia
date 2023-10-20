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
using System.Windows.Shapes;

namespace MapGeorgia.Windows
{
    /// <summary>
    /// Логика взаимодействия для AlertWindow.xaml
    /// </summary>
    public partial class AlertWindow : Window
    {
        public AlertWindow()
        {
            InitializeComponent();

            CancelBtn.Visibility = Visibility.Collapsed;
        }

        public AlertWindow(string message)
        {
            InitializeComponent();

            MsgTB.Text = message;
            CancelBtn.Visibility = Visibility.Collapsed;
        }

        public AlertWindow(string message, bool value)
        {
            InitializeComponent();

            MsgTB.Text = message;
            CancelBtn.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow.IsDelete = true;
            this.Close();
        }
    }
}
