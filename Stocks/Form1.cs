using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HtmlAgilityPack;
using System.IO;
using System.Net;
using MySql.Data.MySqlClient;
using DAL;
using System.Net.Mail;

using System.Xml;
using System.Xml.Linq;
using Model;

namespace Stocks
{
	public partial class Form1 : Form
	{
		/// <summary>
		/// 介紹用法
		/// http://msdn.microsoft.com/zh-tw/evalcenter/ee787055.aspx
		/// </summary>

		private MySqlConnection conn;
		private MySqlCommand cmd;
		private int nextTime;
		private IAsyncResult asyncResult;
		private DateTime start;


		public Form1()
		{
			InitializeComponent();

			listBox2.Items.Add("2330");
			listBox2.Items.Add("2303");

		}

		private void button1_Click(object sender, EventArgs e)
		{
			GetData();
		}
		public void GetData()
		{
			//指定來源網頁
			WebClient url = new WebClient();
			MemoryStream ms = new MemoryStream();

			for (int j = 0; j < listBox2.Items.Count; j++)
			{
				//將網頁來源資料暫存到記憶體內
				ms = new MemoryStream(url.DownloadData("http://tw.stock.yahoo.com/q/q?s=" + listBox2.Items[j].ToString()));
				//以奇摩股市為例http://tw.stock.yahoo.com
				//2330 表示為股票代碼

				// 使用預設編碼讀入 HTML 
				HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
				doc.Load(ms, Encoding.Default);

				// 裝載第一層查詢結果 
				HtmlAgilityPack.HtmlDocument hdc = new HtmlAgilityPack.HtmlDocument();

				//XPath 來解讀它 /html[1]/body[1]/center[1]/table[2]/tr[1]/td[1]/table[1] 
				hdc.LoadHtml(doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/center[1]/table[2]/tr[1]/td[1]/table[1]").InnerHtml);

				// 取得個股標頭 
				HtmlNodeCollection htnode = hdc.DocumentNode.SelectNodes("./tr[1]/th");
				// 取得個股數值 
				string[] txt = hdc.DocumentNode.SelectSingleNode("./tr[2]").InnerText.Trim().Split('\n');
				int i = 0;

				// SQL Command 建立 
				var SQLCommand = "INSERT INTO STOCKS (Date, No, Time, Price, Buy, Sell, Fluctuation, Number, ClosePrice, ";
				SQLCommand += "OpenPrice, High, Low, StocksData ) VALUE ('" + DateTime.Now.ToString("yyyy/MM/dd") + "'";

				// 輸出資料 
				foreach (HtmlNode nodeHeader in htnode)
				{
					listBox1.Items.Add(nodeHeader.InnerText + ":" + txt[i].Trim().Replace("加到投資組合", "") + "");
					// SQL Command 
					//if (i != 0)
					//{
					//    SQLCommand += "','";
					//}
					SQLCommand += ", '" +txt[i].Trim().Replace("加到投資組合", "") + "'"  ;
					//將 "加到投資組合" 這個字串過濾掉
					//Response.Write(nodeHeader.InnerText + ":" + txt[i].Trim().Replace("加到投資組合", "") + "");
					i++;
				}
				SQLCommand += ")";



				//---------------------------------------------------------------------------------------------------
				var serverName = "10.10.10.234";
				var uidName = "root";
				var pwdName = "1234";
				var databaseName = "BDS";
				string connStr = String.Format("server={0};uid={1};pwd={2};database={3}",
				serverName, uidName, pwdName, databaseName);
				conn = new MySqlConnection(connStr);
				try
				{
					conn.Open();

					cmd = new MySqlCommand(SQLCommand, conn);
					cmd.ExecuteNonQuery();


					asyncResult = cmd.BeginExecuteNonQuery();
					nextTime = 5;
					//timer1.Enabled = true;
					start = DateTime.Now;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Exception: " + ex.Message);
				}
				//j += 1;

			}
			//清除資料
			//doc = null;
			//hdc = null;
			url = null;
			ms = null;
			//ms.Close();

		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			var strTime = DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00");

			switch (strTime)
			{
				case "133100":
					GetData();
					break;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			//---------------------------------------------------------------------------------------------------
			// 伺服器名稱
			var serverName = "10.10.10.101";
			// 帳號
			var uidName = "root";
			// 密碼
			var pwdName = "1234";
			// 資料庫
			var databaseName = "BDS";
			// 連線字串
			string connStr = String.Format("server={0};uid={1};pwd={2};database={3}",
			serverName, uidName, pwdName, databaseName);

			conn = new MySqlConnection(connStr);
			try
			{
				// 開啟連線
				conn.Open();

				// SQL Command
				string sql = "SELECT * FROM STOCKSCODE ";
				cmd = new MySqlCommand(sql, conn);
				// 執行SQL 
				cmd.ExecuteNonQuery();

				asyncResult = cmd.BeginExecuteNonQuery();
				nextTime = 5;
				start = DateTime.Now;
			}
			catch (Exception ex)
			{
				// 錯誤訊息丟出 
				MessageBox.Show("Exception: " + ex.Message);
			}

			// 如果連線還沒關閉, 關閉它 
			if (conn != null)
				conn.Close();

			DataView myDataView = DAL.GetStocksData.GetStocksCode();

			

			foreach (DataRowView myDRV in myDataView)
			{
				listBox2.Items.Add(myDRV["code"].ToString());
			}
		}

		//private void LoadStocksCode()
		//{
		//    var serverName = "10.10.10.231";
		//    var uidName = "root";
		//    var pwdName = "80324831";
		//    var databaseName = "BDS";

		//    string connStr = String.Format("server={0};uid={1};pwd={2};database={3}",
		//    serverName, uidName, pwdName, databaseName);

		//    try
		//    {
		//        conn = new MySqlConnection(connStr);
		//        conn.Open();

		//        GetDatabases();
		//    }
		//    catch (MySqlException ex)
		//    {
		//        MessageBox.Show("Error connecting to the server: " + ex.Message);
		//    }


		//}

		//private void GetDatabases()
		//{
		//    MySqlDataReader reader = null;

		//    MySqlCommand cmd = new MySqlCommand("SELECT * FROM STOCKSCODE", conn);
		//    try
		//    {
		//        reader = cmd.ExecuteReader();
		//        listBox2.Items.Clear();
		//        while (reader.Read())
		//        {
		//            listBox2.Items.Add(reader.GetString(0));
		//        }
		//    }
		//    catch (MySqlException ex)
		//    {
		//        MessageBox.Show("Failed to populate database list: " + ex.Message);
		//    }
		//    finally
		//    {
		//        if (reader != null) reader.Close();
		//    }
		//}



		private void button3_Click(object sender, EventArgs e)
		{
			send_email("測試內容", "測試主旨標題", "thomastseng@csstw.com.tw");//呼叫send_email函式測試
		}
		
		
		public void send_email(string msg, string mysubject, string address)
		{
			MailMessage message = new MailMessage("thomastseng@csstw.com.tw", address);//MailMessage(寄信者, 收信者)
			message.IsBodyHtml = true;
			message.BodyEncoding = System.Text.Encoding.UTF8;//E-mail編碼
			message.Subject = mysubject;//E-mail主旨
			message.Body = msg;//E-mail內容
	
			SmtpClient smtpClient = new SmtpClient("10.10.10.18", 25);//設定E-mail Server和port
			smtpClient.Send(message);
		}

		private void button4_Click(object sender, EventArgs e)
		{

			var Message = "";
			var DBType = "9"; // 9 Taroko 1. LPI ERP
			List<string> myStringLists = new List<string>();


			//var HttpUrl = "http://opendata.cwb.gov.tw/opendata/MFC/F-D0047-093.zip";
			var HttpUrl = "http://opendata.cwb.gov.tw/opendata/MFC/F-C0032-005.xml";

			//var HttpUrl = "http://data.moi.gov.tw/MoiOD/System/DownloadFile.aspx?DATA=16DB45A1-37BD-478E-BEE1-E44088B54CF5";
			//var HttpUrl = "http://data.fda.gov.tw/opendata/exportDataList.do?method=ExportData&InfoId=7&logType=2";
			var NewUrl = "";
			var ToDate = "";
			var WorkType = 1;
			if (WorkType == 1)
			{
				ToDate = DateTime.Now.ToString("yyyyMMdd");
				//HttpUrl += ToDate + ".zip";
				WebClient wc = new WebClient();
				wc.DownloadFile(HttpUrl, "c:\\web\\" + ToDate + ".xml");
				NewUrl = "C:\\web\\" + ToDate + ".xml";

				//string path = @"c:\\web\\" + ToDate + ".zip";//壓縮檔案路徑
				//UnZipFiles(path, string.Empty);

				//NewUrl = "C:\\web\\" + ToDate + "\\" + ToDate + ".ZIP";
			}
			else
			{
				ToDate = "20140719";
				NewUrl = "C:\\web" + ToDate + ".ZIP";
			}



			//取得根節點內的子節點
			XmlDocument doc = new XmlDocument();
			doc.Load(NewUrl);
			//選擇節點
			XmlNode main = doc.SelectSingleNode("fifowml");
			
			
			
			if (main == null)
				return;

			//取得節點內的欄位
			XmlElement element = (XmlElement)main;


			//取得節點內的"部門名稱"內容
			string data = element.GetAttribute("category");

			//取得節點內的"部門名稱"的屬性
			XmlAttribute attribute = element.GetAttributeNode("start");


			//列舉節點內的屬性
			XmlAttributeCollection attributes = element.Attributes;
			string content = "";
			foreach (XmlAttribute item in attributes)
			{
				content += item.Name + "," + item.Value + Environment.NewLine;
				if (item.Name == "部門名稱")
					item.Value = "胎哥部門";
				if (item.Name == "部門負責人")
					item.Value = "胎哥郎";
			}



			XmlNode data2 = main.SelectSingleNode("data");
			XmlElement element2 = (XmlElement)data2;

			XmlNodeList data5 = data2.SelectNodes("location");



			//取得節點內的"部門名稱"內容
			string data3 = element.GetAttribute("category");
			if (data2 == null)
				return;

			//取得節點內的欄位
			XmlElement element21 = (XmlElement)data2;

			//取得節點內的"部門名稱"的屬性
			XmlAttribute attribute2 = element2.GetAttributeNode("Name");

			//列舉節點內的屬性
			XmlAttributeCollection attributes2 = element2.Attributes;

			string content2 = "";
			foreach (XmlAttribute item2 in attributes2)
			{
				content2 += item2.Name + "," + item2.Value + Environment.NewLine;
				if (item2.Name == "部門名稱")
					item2.Value = "胎哥部門";
				if (item2.Name == "部門負責人")
					item2.Value = "胎哥郎";
			}


			foreach (XmlNode grandsonNode in data5)
			{
				XmlNode data51 = grandsonNode.SelectSingleNode("name");
				// "臺北市"

				XmlNode data52 = grandsonNode.SelectSingleNode("weather-elements");



				XmlNode data521 = data52.SelectSingleNode("Wx");
				XmlNode data522 = data52.SelectSingleNode("MaxT");
				XmlNode data523 = data52.SelectSingleNode("MinT");

				XmlNode data5211 = data521.SelectSingleNode("time");
				XmlNode data5221 = data522.SelectSingleNode("time");
				XmlNode data5231 = data523.SelectSingleNode("time");
				

				if (grandsonNode.Attributes["type"] != null)
				{
					string value = grandsonNode.Attributes["type"].Value;
					string value2 = grandsonNode.Attributes["weather-elements"].Value;
					string value3 = grandsonNode.Attributes["Wx"].Value;


					//String StrAttrName = grandsonNode.Attributes.Name;
					String StrAttrValue = grandsonNode.Attributes["MyAttr1"].Value;
					String StrAttrValue2 = grandsonNode.InnerText;

				}

				

				Console.WriteLine(grandsonNode.InnerText);
				Console.WriteLine(grandsonNode.InnerXml);
			}

			foreach (XmlElement childElement in element21)
			{
				Console.WriteLine(childElement.GetAttribute("name"));
			}





		}

		private void button5_Click(object sender, EventArgs e)
		{

            //XmlDocument MyDoc = new XmlDocument(); //建立一XmlDocument 名為 MyDoc

            ////載入Yahoo Weather RSS
            //MyDoc.Load("http://xml.weather.yahoo.com/forecastrss?p=TWXX0022&u=c");

            ////建立一XmlNamespaceManager
            //XmlNamespaceManager MyXmlNamespaceManager = new XmlNamespaceManager(MyDoc.NameTable);

            ////加入NameSpace
            //MyXmlNamespaceManager.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");
            ////查出 yweather:condition 節點
            //XmlNode MyNode = MyDoc.SelectSingleNode("/rss/channel/item/yweather:condition", MyXmlNamespaceManager);

            ////設定顯示的天氣圖示, 日夜有分
            //var WeatherID = string.Format("{0}{1}", MyNode.Attributes["code"].Value,
            // DateTime.Now.Hour > 18 || DateTime.Now.Hour < 6 ? "n" : "d");

            ////設定目前溫度
            ////Label3.Text = string.Format("{0}&#176;", MyNode.Attributes["temp"].Value);

            ////查出 yweather:forecast 節點
            //MyNode = MyDoc.SelectSingleNode("/rss/channel/item/yweather:forecast", MyXmlNamespaceManager);

            ////設定高低溫
            ////Label4.Text = string.Format("High: {0}&#176; Low: {1}&#176;",
            // //MyNode.Attributes["high"].Value,
            // //MyNode.Attributes["low"].Value);

            ////最後宣告一 保護層級的變數WeatherID 即可
            ////protected string WeatherID = "29d"; 





		}



		static List<string> getMonthData(string stock_id, int year, int month)
		{
			//ref: http://msdn.microsoft.com/zh-tw/library/system.net.webrequest.aspx

			// http://www.twse.com.tw/ch/trading/exchange/STOCK_DAY_AVG/STOCK_DAY_AVG2.php?STK_NO=2330&myear=2014&mmon=10&type=csv

			// Create a request for the URL. 		
			WebRequest request = WebRequest.Create("http://www.twse.com.tw/ch/trading/exchange/STOCK_DAY_AVG/STOCK_DAY_AVG2.php?STK_NO=" + stock_id + "&myear=" + year + "&mmon=" + month.ToString("0#") + "&type=csv");

			// If required by the server, set the credentials.
			request.Credentials = CredentialCache.DefaultCredentials;

			// Get the response.
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();

			// Get the stream containing content returned by the server.
			Stream dataStream = response.GetResponseStream();

			// Open the stream using a StreamReader for easy access.
			StreamReader reader = new StreamReader(dataStream, Encoding.GetEncoding(950));

			// Read the content.

			List<string> data = new List<string>();

			string title1 = reader.ReadLine();  //98年12月 2002 中鋼 日收盤價及月平均收盤價(元)

			if (title1 == null) //表示沒資料
			{

			}
			else
			{

				reader.ReadLine();  //日期	收盤價

				while (!reader.EndOfStream)
				{
					data.Add(stock_id.Trim() + "," + reader.ReadLine().Trim());
				}

				data.RemoveAt(data.Count - 1);  //說明：以上成交資料採市場交易時間之資料計算。
				data.RemoveAt(data.Count - 1);  //月平均收盤價	31.18
			}

			// Cleanup the streams and the response.
			reader.Close();
			dataStream.Close();
			response.Close();


			return data;
		}

		private void button6_Click(object sender, EventArgs e)
		{
			string stock_id = "";

            label1.Text = "0";
            
            // 讀取所有股票列表
			DataView myDataView = DAL.GetStocksData.GetStocksCode();

            // 如果沒有 C:/Stocks/ 目錄, 則建立
			if (!Directory.Exists("C:/Stocks/")) Directory.CreateDirectory("C:/Stocks/");

            // 所有股票列表
            foreach (DataRowView myDRV in myDataView)
			{
				listBox2.Items.Add(myDRV["code"].ToString());

				stock_id = myDRV["code"].ToString();

				FileStream fs = new FileStream("C:/Stocks/" + stock_id + ".csv", FileMode.Create);
				StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding(950));

				sw.WriteLine("股市代號,日期,收盤價");
                var SQLCommand = string.Empty;
                // 自1999年到2014年
				for (int y = 1999; y <= 2014; y++)
				{
                    // 自1到12月
					for (int m = 1; m <= 12; m++)
					{
                        // 讀取 getMonthData
						List<string> month_data = getMonthData(stock_id, y, m);
                        for (int i = 0; i < month_data.Count; i++)
						{
                            // 產生 SQLCommand 
                            SQLCommand = "INSERT INTO stockshistory (StocksCode,Date,Price) VALUES('";
                            SQLCommand += month_data[i].Replace(",", "','");
                            SQLCommand += "')";

                            int intStatus = GetStocksData.RunSQLCommand(SQLCommand);
                            label1.Text = (Convert.ToDouble(label1.Text) + 1).ToString();
                            // 寫入資料
							sw.WriteLine(month_data[i]);
						}
                        // 秀在Console
						Console.WriteLine(y + "/" + m + (month_data.Count == 0 ? " no data." : " download."));
					}
				}

				sw.Close();
				fs.Close();
			}
		}




	}
}
