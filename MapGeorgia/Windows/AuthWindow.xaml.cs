using MapGeorgia.Model;
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
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private MapEntitiesEntities mapEntities = new MapEntitiesEntities();

        public AuthWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (EmailTB.Text == String.Empty || PasswordPB.Password == String.Empty)
            {
                new AlertWindow("All lines wasnt be Empty!").ShowDialog();
                return;
            }

            var user = mapEntities.User.FirstOrDefault(x=>x.Email == EmailTB.Text && x.Password == PasswordPB.Password);

            if (user == null) 
            {
                new AlertWindow("This user does not exist!").ShowDialog();
                return;
            }

            if(user.RoleID == 1)
            {
                new AdminWindow().Show();
                this.Close();
                return;
            }
            else
            {
                new MainWindow().Show();
                this.Close();
                return;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void NextRegBtn_Click(object sender, RoutedEventArgs e)
        {
            if (EmailRegTB.Text == String.Empty || PasswordRegPB.Password == String.Empty || PasswordReg1PB.Password == String.Empty)
            {
                new AlertWindow("All lines wasnt be Empty!").ShowDialog();
                return;
            }

            if (PasswordRegPB.Password != PasswordReg1PB.Password)
            {
                new AlertWindow("Passwords do not match!").ShowDialog();
                return;
            }

            try
            {
                User user = new User()
                {
                    ID = Convert.ToInt32(Guid.NewGuid()),
                    Email = EmailTB.Text,
                    Password = PasswordPB.Password,
                    RoleID = 2
                };

                mapEntities.User.Add(user);
                mapEntities.SaveChanges();
                new MainWindow().Show();
                this.Close();
            }
            catch
            {
                new AlertWindow("Invalid output value!").ShowDialog();
            }
        }
    }
}
