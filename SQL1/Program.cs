using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SQL1
{
    class Program
    {
        static string connectionString = "Server=.;Database=Minions;Trusted_Connection=True";

        static void Main(string[] args)
        {
            SelectTask2();
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
                connection.Close();
            }

            static void SelectTask2()
            {
                static object GetVillainName(int id)
                {
                    string selectionCommandString =
                        $"SELECT Name FROM Villains v WHERE v.Id = " + id;
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    SqlCommand command = new SqlCommand(selectionCommandString, connection);
                    using (connection)
                    {
                        if (!(command.ExecuteScalar() is System.DBNull))
                        {
                            var result = command.ExecuteScalar();
                            connection.Close();
                            return result;
                        }
                        else { connection.Close(); return null; }
                    }
                }

                static bool VillainHaveMinions(int id)
                {
                    string selectionCommandString =
                        $"SELECT VillainsId " +
                        $"FROM MinionsVillains m " +
                        $"WHERE m.VillainsId=" + id;
                    SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    SqlCommand command = new SqlCommand(selectionCommandString, connection);
                    if (!(command.ExecuteScalar() is System.DBNull)) { connection.Close(); return false; }
                    else { connection.Close(); return true; }
                }

                int id = Int32.Parse(Console.ReadLine());
                while (GetVillainName(id) == null) { Console.WriteLine("No villain with ID " + id + "exists in the database"); id = Int32.Parse(Console.ReadLine()); };
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
                        Console.WriteLine(GetVillainName(id));
                        if (!VillainHaveMinions(id)) { Console.WriteLine("(no minions)"); }
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
                connection.Close();
            }




        }
    }
}
