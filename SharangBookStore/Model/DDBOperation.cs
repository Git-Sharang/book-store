using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SharangBookStore
{
    public class DDBOperation
    {
        AmazonDynamoDBClient client;
        DynamoDBContext context;
        Amazon.Runtime.BasicAWSCredentials credentials;
        public DDBOperation()
        {
            credentials = new Amazon.Runtime.BasicAWSCredentials(
                ConfigurationManager.AppSettings["accessId"],
                ConfigurationManager.AppSettings["secretKey"]);
            client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.USEast1);
            context = new DynamoDBContext(client);

            // Check the existence of the User database and creating one if it does not exist
            try
            {
                var res = client.DescribeTable(new DescribeTableRequest { TableName = "User" });
            }
            catch (Exception)
            {
                CreateTable();
            }
        }

        //Creating the User table in DynamoDb
        public void CreateTable()
        {
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = "User",
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName="Username",
                        AttributeType="S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName="Username",
                        KeyType="HASH"
                    }
                },
                BillingMode = BillingMode.PROVISIONED,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 2,
                    WriteCapacityUnits = 1
                }
            };
            try
            {
                var response = client.CreateTable(request);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        //Inserting the user in the User table in DynamoDb
        public void InsertUser(string username, string password, string firstname, string lastname)
        {
            try
            {
                List<UserBook> books = new List<UserBook>();
                User user = new User
                {
                    Username = username,
                    Password = password,
                    Firstname = firstname,
                    Lastname = lastname,
                    Books = books
                };
                context.Save(user);
                MessageBox.Show("Welcome " + firstname + "! you can login with username: " + username, "Creation Successful");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error !!!!");
            }
        }


        //Adding the book for a specific user in the UserBook table
        public void AddBook(string username, string isbn, string bookTitle, string author)
        {
            try
            {
                    User retrieveUser = context.Load<User>(username);
                    retrieveUser.Books.Add(new UserBook { Isbn = isbn, Title = bookTitle, Author = author, LastPageRead = 0, ReadDateTime = DateTime.Today.ToString() });
                    context.Save(retrieveUser);
                    MessageBox.Show("Your book, " + bookTitle + " is in the Shelf", "Successful");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error !!!!");
            }
        }


        //Adding the snapshot for a specific book in the UserBook table for a specific book
        public void AddSnaphot(string username, string isbn, int lastPageRead)
        {
            try
            {
                User retrieveUser = context.Load<User>(username);
                retrieveUser.Books.Find(item => item.Isbn == isbn).LastPageRead = lastPageRead;
                retrieveUser.Books.Find(item => item.Isbn == isbn).ReadDateTime = DateTime.Now.ToString();
                context.Save(retrieveUser);
                MessageBox.Show("Added the snapshot to your book", "Successful");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error !!!!");
            }
        }


        //Checking the existence of a username
        public bool CheckUsernameExists(string username)
        {
            User user = context.Load<User>(username, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });
            try
            {
                if (!(user == null))
                {
                    user.Username.ToString();
                }
                else
                {
                    MessageBox.Show("User Doesnot Exist, please sign up", "ERROR !!!!");
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ERROR !!!!");
            }
            return true;
        }


        //Getting the username from the DynamoDb based on the user's input
        public string GetUsername(string username)
        {
            User user = context.Load<User>(username, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });
            return user.Username.ToString();
        }


        //Getting the First Name from the DynamoDb based on the username
        public string GetFirstName(string username)
        {
            User user = context.Load<User>(username, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });
            return user.Firstname.ToString();
        }


        //Getting the password from the DynamoDb based on the username
        public string GetPassword(string username)
        {
            User user = context.Load<User>(username, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });
            return user.Password.ToString();
        }


        //Getting the list of books from the DynamoDb UserBook based on the username
        public List<UserBook> GetBooks(string username)
        {
            User user = context.Load<User>(username, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });
            return user.Books;
        }
    }
}
