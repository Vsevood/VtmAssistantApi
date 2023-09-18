using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Auth
{
    public class Connection
    {
        //static public void ConnectToMySQL(string request)
        static public IDataReader ConnectToMySQL(string request)
        {
            MySqlConnection conn = null;
            MySqlDataReader reader = null;
            var dt = new DataTable();
            var sb = new MySqlConnectionStringBuilder
            {
                Server = "127.0.0.1",
                UserID = "root",
                Password = "Sql90MyProt!An",
                Port = 3306,
                Database = "DataBase1"
            };
            try
            {
                conn = new MySqlConnection(sb.ConnectionString);
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = request;
                reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                dt.Load(reader);
                //work
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
                
            }
           
           
            return dt.CreateDataReader();
            //return reader;
        }
    }
}
