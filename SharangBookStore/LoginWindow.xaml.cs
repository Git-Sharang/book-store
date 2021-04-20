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

namespace SharangBookStore
{
    /** <summary>
        This page is the main window, it is used for logging into the User's BookShelf
        </summary> **/
    public partial class MainWindow : Window
    {
        DDBOperation ddb;
        public MainWindow()
        {
            InitializeComponent();
            ddb = new DDBOperation();
        }

        //For signing into an account
        private void Button_SignIn(object sender, RoutedEventArgs e)
        {
            if (!(String.IsNullOrEmpty(userNameTxt.Text) || String.IsNullOrEmpty(passwordtxt.Password)))
            {
                bool userexists = ddb.CheckUsernameExists(userNameTxt.Text);
                if (userexists == true)
                {
                    string username = ddb.GetUsername(userNameTxt.Text);
                    string password = ddb.GetPassword(userNameTxt.Text);
                    if (username == userNameTxt.Text && passwordtxt.Password == password)
                    {
                        BookShelf bookShelf = new BookShelf(userNameTxt.Text);
                        bookShelf.Show();
                        bookShelf.Owner = this;
                        this.Hide();
                        userNameTxt.Text = "";
                        passwordtxt.Password = "";
                    }
                    else
                    {
                        MessageBox.Show("Check your Credentials", "Error !!!!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Fill in your credentials", "Error !!!!");
            }
        }

        //For going to the Registration page
        private void Button_SignUp(object sender, RoutedEventArgs e)
        {
            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.Show();
            signUpWindow.Owner = this;
            this.Hide();
        }
    }
}
