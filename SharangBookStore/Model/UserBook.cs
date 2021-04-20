using Amazon.DynamoDBv2.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharangBookStore
{
    [DynamoDBTable("UserBook")]
    public class UserBook
    {
            [DynamoDBHashKey("Isbn")]
            public string Isbn
            {
                get; set;
            }
            [DynamoDBProperty]
            public string Title
            {
                get; set;
            }
            [DynamoDBProperty]
            public string Author
            {
                get; set;
            }
            [DynamoDBProperty]
            public int LastPageRead
            {
                get; set;
            }
            [DynamoDBProperty]
            public string ReadDateTime
            {
                get; set;
            }
            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
}
