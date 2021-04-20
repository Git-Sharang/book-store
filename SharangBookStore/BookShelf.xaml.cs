using Microsoft.Build.Framework.XamlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        This page is the Book Shelf of a user, where he/she can add a book, 
        add a snaphot, and view the list of books. 
        Also, user can filter the shelf to look for his recently read/added book.
        </summary> **/
    public partial class BookShelf : Window
    {
        public DDBOperation dynamoDbOperation;
        public string username;
        public BookShelf(string currentUsername)
        {
            InitializeComponent();
            this.username = currentUsername;
            dynamoDbOperation = new DDBOperation();
            string userFirstName = dynamoDbOperation.GetFirstName(username);
            Firstlbl.Content = userFirstName + "!";

            //Loading the Book Shelf of the User
            BooksIntoDataGrid();

            //Adding date and time inside the ReadDateTime textbox
            datetimeTxt.Text = DateTime.Now.ToString();
        }

        // For logging out of the SharangBookShelf
        private void Button_Logout(object sender, RoutedEventArgs e)
        {
            this.Close();
            Owner.Show();
        }

        //For adding the book in the User's Book Shelf
        private void Button_AddBook(object sender, RoutedEventArgs e)
        {
            if (!(String.IsNullOrEmpty(isbnTxt.Text) || String.IsNullOrEmpty(bookTitleTxt.Text)
                    || String.IsNullOrEmpty(authorTxt.Text)))
            {
                dynamoDbOperation.AddBook(username, isbnTxt.Text, bookTitleTxt.Text, authorTxt.Text);
                isbnTxt.Text = "";
                bookTitleTxt.Text = "";
                authorTxt.Text = "";
                BooksIntoDataGrid();
            }
            else
            {
                MessageBox.Show("Enter the book information", "Error !!!!");
            }
        }

        //For adding a snaphot inside the selected book
        private void Button_AddSnapshot(object sender, RoutedEventArgs e)
        {
            var selectedBook = bookShelfDataGrid.SelectedItem;
            UserBook selectedItem = selectedBook as UserBook;
            string selectedIsbn;
            string lastPageNumber = lastPageTxt.Text;
            try
            {
                if (!(selectedItem == null || lastPageNumber == null))
                {
                    selectedIsbn = selectedItem.Isbn;
                    dynamoDbOperation.AddSnaphot(username, selectedIsbn, int.Parse(lastPageNumber));
                    lastPageTxt.Text = "";
                    BooksIntoDataGrid();
                }
                else
                {
                    MessageBox.Show("Select a Book and enter the Last Read Page Number", "Error !!!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error !!!!");
            }
        }

        //For the recently read book
        private void Button_RecentFilter(object sender, RoutedEventArgs e)
        {
            bookShelfDataGrid.Items.SortDescriptions.Clear();
            bookShelfDataGrid.Items.SortDescriptions.Add(new SortDescription("ReadDateTime", ListSortDirection.Descending));
            bookShelfDataGrid.Items.Refresh();
        }

        //Populating the DataGrid with the Books in the UserBook Class
        private void BooksIntoDataGrid()
        {
            var _itemSourceList = new CollectionViewSource() { Source = dynamoDbOperation.GetBooks(username) };
            ICollectionView Itemlist = _itemSourceList.View;
            bookShelfDataGrid.ItemsSource = Itemlist;
        }
    }
}
