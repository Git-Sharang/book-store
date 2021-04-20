using Amazon.DynamoDBv2.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharangBookStore
{
    [DynamoDBTable ("User")]
    class User
    {
        [DynamoDBHashKey("Username")]
        public string Username
        {
            get; set;
        }
        public string Password
        {
            get; set;
        }
        [DynamoDBProperty]
        public string Firstname
        {
            get; set;
        }
        [DynamoDBProperty]
        public string Lastname
        {
            get; set;
        }
        [DynamoDBProperty]
        public List<UserBook> Books { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
