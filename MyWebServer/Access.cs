using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MyWebServer
{
    class Access
    {
        string connectionString = "SERVER=127.0.0.1;DATABASE=temperature;UID=root;PASSWORD=;Convert Zero Datetime=True;";

        public void Insert(Temperature temp)
        {

        }

        public List<Temperature> getTemperature()
        {
            string getQuery = "SELECT * FROM temp;";

            List<Temperature> result = new List<Temperature>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(getQuery, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Temperature temp = new Temperature();
                            temp.ID = (int)reader["ID"];
                            temp.Date = (DateTime)reader["date"];
                            temp.Celsius = (double)reader["celsius"];
                            result.Add(temp);
                        }
                    }
                }
            }
            return result;
        }
    }
}
