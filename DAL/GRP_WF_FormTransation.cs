using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class GRP_WF_FormTransation
    {

        public static string Transation(List<string> myStringLists, string DBType)
        {

            var serverName = "10.10.10.101";
            var uidName = "root";
            var pwdName = "1234";
            var databaseName = "BDS";
            string connStr = String.Format("server={0};uid={1};pwd={2};database={3}",
            serverName, uidName, pwdName, databaseName);



            string MsgString = "";
            switch (DBType)
            {
                case "9":
                    using (MySqlConnection connection = new MySqlConnection(connStr))
                    {
                        connection.Open();
                        //DbTransaction transaction = connection.BeginTransaction();
                        try
                        {
                            foreach (string myStringList in myStringLists)
                            {
                                //DbCommand Command = db9.GetSqlStringCommand(myStringList);
                                //db8.ExecuteNonQuery(Command, transaction);

                                MySqlCommand cmd = new MySqlCommand(myStringList);
                                cmd.ExecuteNonQuery();
                            }
                            //transaction.Commit();
                            MsgString = " Transfer Successful!";
                        }
                        catch (Exception e)
                        {

                            //transaction.Rollback();
                            MsgString = "  Transfer Error! Message:  " + e.ToString();
                        }
                        connection.Close();
                    }
                    break;

            }
            return MsgString;
        }

    }
}
