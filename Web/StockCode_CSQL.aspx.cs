using Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Web
{
    public partial class LStockCode_CSQL : System.Web.UI.Page
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

        protected void btnNewProduct_Click(object sender, EventArgs e)
        {

            MongoCollection<MongoStockCode> _StockCodes = myDB.GetCollection<MongoStockCode>("StockCode");
            var newStockCode = new MongoStockCode();
            newStockCode.Code = txtCode.Text;
            newStockCode.Name = txtName.Text;

            _StockCodes.Insert(newStockCode);

            MongoCollection<MongoStockCode> _NewStockCodes = myDB.GetCollection<MongoStockCode>("StockCode");
            List<MongoStockCode> StockCodes = new List<MongoStockCode>();
            foreach (MongoStockCode stockCode in _NewStockCodes.FindAll())
            {

                StockCodes.Add(stockCode);
            }
            txtCode.Text = "";
            txtName.Text = "";
            gvStockCodes.DataSource = StockCodes;
            gvStockCodes.DataBind();

        }
    }
}