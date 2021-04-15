using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace DAL
{
    public class GetStocksData
    {
        private static MySqlCommand cmd;

        public static int RunSQLCommand(string SQLCommand)
        {
            //MySqlDataReader reader = null;

            var serverName = "10.10.10.234";
            var uidName = "root";
            var pwdName = "1234";
            var databaseName = "BDS";
            string connStr = String.Format("server={0};uid={1};pwd={2};database={3}",
            serverName, uidName, pwdName, databaseName);
            MySqlConnection conn = null;

            try
            {
                conn = new MySqlConnection(connStr);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(SQLCommand, conn);
                return cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                return 0;
            }
            finally
            {
                if (conn != null) conn.Close();
            }
        }

        public static DataView  GetStocksCode()
        {
            MySqlDataReader reader = null;

            var serverName = "10.10.10.234";
            var uidName = "root";
            var pwdName = "1234";
            var databaseName = "BDS";
            string connStr = String.Format("server={0};uid={1};pwd={2};database={3}",
            serverName, uidName, pwdName, databaseName);
            MySqlConnection conn = null;

            try
            {



                conn = new MySqlConnection(connStr);
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM STOCKSCODE", conn);

                reader = cmd.ExecuteReader();
                
                DataView myDataView = PopulateDataView(reader, "myTable");
                return myDataView;
                //while (reader.Read())
                //{
                //    listBox2.Items.Add(reader.GetString(0));
                //}
            }
            catch (MySqlException ex)
            {
                return null;
                // MessageBox.Show("Failed to populate database list: " + ex.Message);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        private static DataView PopulateDataView(IDataReader dataReader, string tableName)
        {
            DataTable dataReaderTable = new DataTable(tableName);

            try
            {
                for (int count = 0; count < dataReader.FieldCount; count++)
                {
                    DataColumn tempCol = new DataColumn(dataReader.GetName(count), dataReader.GetFieldType(count));

                    dataReaderTable.Columns.Add(tempCol);
                }

                while (dataReader.Read())
                {
                    DataRow dr = dataReaderTable.NewRow();

                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        dr[i] = dataReader.GetValue(dataReader.GetOrdinal(dataReader.GetName(i)));
                    }

                    dataReaderTable.Rows.Add(dr);
                }

                return dataReaderTable.DefaultView;
            }
            catch
            {
                return null;
            }
        }
    }
}