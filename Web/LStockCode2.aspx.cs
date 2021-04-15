using Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Web
{
    public partial class LStockCode2 : System.Web.UI.Page
    {
        private MongoDatabase myDB;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<MongoStockCode> StockCodes = new List<MongoStockCode>();
            // 產生 MongoClient 物件
            string Link = "mongodb://10.10.10.234:27017";
            MongoClient _client = new MongoClient(Link);
            //_client.GetServer().Connect();
            // "Server=10.10.10.234:27017"
            // 取得 MongoServer 物件
            //#pragma warning disable CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // 類型或成員已經過時
            MongoServer server = _client.GetServer();
#pragma warning restore CS0618 // 類型或成員已經過時
            // 取得 MongoDatabase 物件
            myDB = server.GetDatabase("dbs");
            // 取得 Collection
            MongoCollection<MongoStockCode> _StockCodes = myDB.GetCollection<MongoStockCode>("StockCode");

            foreach (MongoStockCode StockCode in _StockCodes.FindAll())
            {
                StockCodes.Add(StockCode);
            }

            gvStockCodes.DataSource = StockCodes;
            gvStockCodes.DataBind();
        }
    }
}