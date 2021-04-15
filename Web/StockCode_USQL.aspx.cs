using Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using MongoDB.Driver.Builders;

namespace Web
{
    public partial class LStockCode_USQL : System.Web.UI.Page
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

        protected void btnUpdateByID_Click(object sender, EventArgs e)
        {
            List<MongoStockCode> StockCodes = new List<MongoStockCode>();
            MongoCollection<MongoStockCode> _StockCodes = myDB.GetCollection<MongoStockCode>("StockCode");

            var _product = _StockCodes.FindOne(Query.EQ("Code", txtCode.Text));
            if (_product != null)
            {

                _product.Name = txtName.Text;
                _StockCodes.Save(_product);
            }
            foreach (MongoStockCode product in _StockCodes.FindAll())
            {

                StockCodes.Add(product);
            }
            gvStockCodes.DataSource = StockCodes;
            gvStockCodes.DataBind();
        }
    }
}
