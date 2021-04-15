using Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using MongoDB.Driver.Builders;

namespace Web
{
    public partial class LStockCode_R : System.Web.UI.Page
    {
        private MongoDatabase myDB;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<MongoStockCode> StockCodes = new List<MongoStockCode>();
            // 產生 MongoClient 物件
            string Link = "mongodb://10.10.10.234:27017";
            MongoClient _client = new MongoClient(Link);
            //_client.GetServer().Connect();

            // 取得 MongoServer 物件
            #pragma warning disable CS0618

            MongoServer server = _client.GetServer();
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

        protected void btnQueryByID_Click(object sender, EventArgs e)
        {
            List<MongoStockCode> StockCodes = new List<MongoStockCode>();
            MongoCollection<MongoStockCode> _StockCodes = myDB.GetCollection<MongoStockCode>("StockCode");

            var _stockCode = _StockCodes.FindOne(Query.EQ("Code", txtCode.Text));

            StockCodes.Add(_stockCode);
            txtCode.Text = string.Empty;
            gvStockCodes.DataSource = StockCodes;
            gvStockCodes.DataBind();
        }
    }
}
