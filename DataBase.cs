using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity;

namespace Task1
{
   class DataBase
    {
        //SQL Connection string must be changed!
        SqlConnection newSqlConnection = new SqlConnection(@"Data Source = MainMachine; Initial Catalog=RigsDB; Integrated Security = True");

        public void openConnection()
        {
            if (newSqlConnection.State == System.Data.ConnectionState.Closed)
            {
                try
                {
                    newSqlConnection.Open();
                }
                catch
                {
                    Console.WriteLine("Can't connect do DB");
                }
                
            }
        }

        public void closeConnection()
        {
            if (newSqlConnection.State == System.Data.ConnectionState.Open)
            {
                newSqlConnection.Close();
            }
        }

        public SqlConnection getConnection()
        {
            return newSqlConnection;
        }

        public List<Rigs> createGetQuery (string query)
        {
            SqlCommand command = new SqlCommand(query, getConnection());
            SqlDataReader reader = null;
            List<Rigs> listRigs = new List<Rigs>(); 

            try
            {
               reader = command.ExecuteReader();
            }
            catch
            {
                Console.WriteLine("Something went wrong with DB connection. Please, check your SqlConnection string (Data Source, Initial Catalogue)");
                return null;
            }

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //Adding (ID, Name, lng, lat)
                    listRigs.Add(new Rigs(reader.GetInt32(0), reader.GetString(1), reader.GetDouble(2),reader.GetDouble(3)));
                }
            }
            return listRigs;

        }

        public void createSetQuery (int id, double lng, double lat)
        {
            openConnection();

            string query = @"UPDATE Rigs SET Lng=" + lng.ToString().Replace(',', '.') + ", Lat=" + lat.ToString().Replace(',', '.') + " WHERE Id=" + id.ToString(); 
            SqlCommand command = new SqlCommand(query, getConnection());

            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                Console.WriteLine("Something went wrong with DB connection. Please, check your SqlConnection string (Data Source, Initial Catalogue)");
            }
            closeConnection();
        }
    }
}
