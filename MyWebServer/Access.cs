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

        public void addTemp(Temperature temp)
        {
            string addQuery = "INSERT INTO temp(date, celsius) VALUES (@date,@celsius);";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(addQuery, connection))
                {
                    
                    command.Parameters.AddWithValue("@date", temp.Date);
                    command.Parameters.AddWithValue("@celsius", temp.Celsius);
                    command.Prepare();

                    command.ExecuteNonQuery();
                   
                }
            }
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
                            var id = (int)reader["ID"];
                            var date = (DateTime)reader["date"];
                            var celsius = (double)reader["celsius"]; ;
                            Temperature temp = new Temperature(id, date, celsius);
                            result.Add(temp);
                        }
                    }
                }
            }
            return result;
        }

        public List<Temperature> getTemperature(DateTime from, DateTime until)
        {
            string getQuery = "SELECT * FROM temp WHERE date >= @from AND date <= @until ORDER BY date;";

            List<Temperature> result = new List<Temperature>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(getQuery, connection))
                {
                    command.Parameters.AddWithValue("@from", from);
                    command.Parameters.AddWithValue("@until", until);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = (int)reader["ID"];
                            var date = (DateTime)reader["date"];
                            var celsius = (double)reader["celsius"]; ;
                            Temperature temp = new Temperature(id,date,celsius);
                            
                            result.Add(temp);
                        }
                    }
                }
            }
            return result;
        }
    }
}
