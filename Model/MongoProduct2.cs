using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace Model
{
    public class MongoStockCode
    {
        public ObjectId _id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}