using DMOptions;
using System.Data.SqlClient;
using Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace DAL
{
    public class DataAccessLayer
    {
        private readonly string connectionString;
        private readonly SqlConnection sqlConnection;

        public DataAccessLayer(DataManagerOptions options = null)
        {
            options ??= new DataManagerOptions();
            connectionString = options.DataBaseOptions.ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
        }

        public List<Employee> GetEmployees()
        {
            string sqlCommandName = "GetEmployees";
            var employees = new List<Employee>();

            try
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(sqlCommandName, sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader sqlReader = command.ExecuteReader();

                if (sqlReader.HasRows)
                {
                    while (sqlReader.Read())
                    {
                        Employee obj = new Employee();
                        obj.JobTitle = sqlReader.GetString(0);
                        obj.LoginID = sqlReader.GetString(1);
                        obj.Gender = sqlReader.GetString(2);
                        obj.BirthDate = sqlReader.GetDateTime(3);
                        obj.MaritalStatus = sqlReader.GetString(4);
                        employees.Add(obj);
                    }
                }
                sqlReader.Close();
            }
            catch(Exception ex)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.txt");
                using StreamWriter sr = new StreamWriter(path, true);
                sr.Write(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            return employees;
        }
    }
}