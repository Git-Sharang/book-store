using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace SharangBookStore
{
    /** <summary>
        This page is used as a signup form for registrating the user in User DynamoDB table
        </summary> **/
    public partial class SignUpWindow : Window
    {
        DDBOperation op;
        public SignUpWindow()
        {
            InitializeComponent();
            op = new DDBOperation();
        }

        //For creating an User Account
        private void Button_CreateAccount(object sender, RoutedEventArgs e)
        {
            string userName = usernameTxt.Text;
            string password = passwordtxt.Password;
            string firstName = firstNametxt.Text;
            string lastName = lastNameTxt.Text;
            if (!(String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password)
                || String.IsNullOrEmpty(firstName) || String.IsNullOrEmpty(lastName)))
            {
                op.InsertUser(userName, password, firstName, lastName);
                this.Hide();
                Owner.Show();
            }
            else
            {
                MessageBox.Show("Enter the values inside the field", "ERROR !!!!");
            }

        }

        //For going back to the Main page, which is the login page
        private void Button_Back(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Owner.Show();
        }
    }
}
