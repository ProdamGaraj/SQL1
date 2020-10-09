using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SQL1
{
    class Program
    {
        static string connectionString = "Server=.;Database=Minions;Trusted_Connection=True";
        static bool IsVillainHasMinions(int id)
        {
            string selectionCommandString =
                $"SELECT COUNT(*) " +
                $"FROM MinionsVillains " +
                $"WHERE VillainsId=" + id;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(selectionCommandString, connection);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        static string GetVillainName(int id)
        {
            string selectionCommandString =
                $"SELECT Name FROM Villains v WHERE v.Id = " + id;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(selectionCommandString, connection);
            using (connection)
            {
                var result = command.ExecuteScalar();
                try
                {
                    return (string)result;
                }
                catch
                {
                    return string.Empty;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        static void SelectTask1()
        {
            string selectionCommandString =
                $"SELECT v.Name , COUNT(MinionId) " +
                $"FROM MinionsVillains m " +
                $"RIGHT JOIN Villains v ON m.VillainsId = v.Id " +
                $"GROUP BY m.VillainsId,v.Name " +
                $"HAVING COUNT(MinionId) > 2 " +
                $"ORDER BY COUNT(MinionId) DESC ";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(selectionCommandString, connection);
            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader[i]} ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        static void SelectTask2()
        {
            int id = Int32.Parse(Console.ReadLine());
            while (GetVillainName(id) == string.Empty) { Console.WriteLine("No villain with ID " + id + " exists in the database"); id = Int32.Parse(Console.ReadLine()); };
            Console.WriteLine(GetVillainName(id));
            if (!IsVillainHasMinions(id))
            {
                Console.WriteLine("(no minions)");
                return;
            }
            string selectionCommandString =
                $"SELECT Name, Age FROM Minions m " +
                $"JOIN MinionsVillains v ON m.Id = v.MinionId " +
                $"WHERE v.VillainsId = " + id;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(selectionCommandString, connection);
            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    int j = 1;
                    while (reader.Read())
                    {
                        Console.WriteLine(j++ + ". " + $"{reader["Name"]} {reader["Age"]}");
                    }
                }
            }
        }

        static void SelectTask3()
        {

        }
        static void Main(string[] args)
        {
            //SelectTask1();
            //SelectTask2();
            //SelectTask3();
        }
    }
}