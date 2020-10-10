using System;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

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
                $"SELECT Name FROM Villains WHERE Id = " + id;
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
            }
        }

        static int GetTownId(string name)
        {
            string selectionCommandString =
                $"SELECT Id FROM Towns WHERE Name = '" + name + "'";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(selectionCommandString, connection);
            using (connection)
            {
                var result = command.ExecuteScalar();
                if (result != null) { return Convert.ToInt32(result); }
                else { return -1; }
            }
        }
        static int GetVillainId(string name)
        {
            string selectionCommandString =
                $"SELECT Id FROM Villains WHERE Name = '" + name + "' ";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(selectionCommandString, connection);
            using (connection)
            {
                var result = command.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else { return -1; }
            }
        }
        static int AddMinion(int Id, string name, int age, int townId)
        {
            string sql =
                   $"INSERT INTO Minions(Id ,Name, Age, TownId) " +
                   $"VALUES(" + Id + ", '" + name + "', " + age + " ," + townId + ")";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                var result = Convert.ToInt32(command.ExecuteScalar());
                return result;
            }
        }
        static int GetLastMinionId()
        {
            string selectionCommandString =
                $"Select MAX(Id) From Minions";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand(selectionCommandString, connection);
            using (connection)
            {
                var result = command.ExecuteScalar();
                try
                {
                    return Convert.ToInt32(result);
                }
                catch
                {
                    return -1;
                }
            }
        }
        static int AddMinionsVillains(int minionsId, int villainId)
        {
            string sql =
                $"INSERT INTO MinionsVillains(MinionId, VillainsId) VALUES( " + minionsId + " , " + villainId + " )";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                var result = Convert.ToInt32(command.ExecuteScalar());
                return result;
            }
        }

        static int AddTown(string name, int countryId)
        {
            string sql =
                   $"INSERT INTO Towns(Name, CountryId) " +
                   $"VALUES('" + name + "', " + countryId + "); SELECT SCOPE_IDENTITY()";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                SqlCommand command = new SqlCommand(sql, connection);
                var result = Convert.ToInt32(command.ExecuteScalar());
                return result;
            }
        }
        static int AddVillain(string name)
        {
            string sql =
                   $"INSERT INTO Villains(Name, EvilnessFactorId) VALUES('" + name + "', " + 1 + ")";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                SqlCommand command = new SqlCommand(sql, connection);
                var result = Convert.ToInt32(command.ExecuteScalar());
                return result;
            }
        }

        static int DeleteVillain(int id)
        {
            string sql = $"DELETE FROM Villains WHERE id=" + id;
            SqlConnection connection = new SqlConnection(connectionString);
            using (connection)
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                return command.ExecuteNonQuery();
            }

        }

        static int FreeMinions(int id)
        {
            string sql =
                $"DELETE FROM MinionsVillains " +
                $"WHERE VillainsId=" + id;
            SqlConnection connection = new SqlConnection(connectionString);
            using (connection)
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                return command.ExecuteNonQuery();
            }
        }

        static int IncrementMinionsAge(List<int> idList)
        {
            string sql =
                $"UPDATE Minions " +
                $"SET Age = Age + 1" +
                $"WHERE Id IN ({string.Join(", ", idList)})";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                return command.ExecuteNonQuery();
            }
        }
        static void Task2()
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

        static void Task3()
        {
            int id = Int32.Parse(Console.ReadLine());
            while (GetVillainName(id) == null) { Console.WriteLine("No villain with ID " + id + " exists in the database"); id = Int32.Parse(Console.ReadLine()); };
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
                    int i = 1;
                    while (reader.Read())
                    {
                        Console.WriteLine(i++ + ". " + $"{reader["Name"]} {reader["Age"]}");
                    }
                }
            }
        }

        static void Task4()
        {
            string[] minion = new string[3];
            minion = Console.ReadLine().Split(' ');
            string villain = Console.ReadLine();
            if (GetTownId(minion[2]) == -1)
            {
                AddTown(minion[2], 1);
                Console.WriteLine("Город " + minion[2] + " был добавлен в базу данных");
            }
            if (GetVillainId(villain) == -1)
            {
                AddVillain(villain); Console.WriteLine("Злодей " + villain + " был добавлен в базу данных");
            }
            {
                AddMinion(GetLastMinionId() + 1, minion[0], Int32.Parse(minion[1]), GetTownId(minion[2]));
                AddMinionsVillains(GetLastMinionId(), GetVillainId(villain));
                Console.WriteLine("Успешно добавлен " + minion[0] + " чтобы быть миньном " + villain);
            }

        }

        static void Task5()
        {
            int villainId = Int32.Parse(Console.ReadLine());
            string villainName = GetVillainName(villainId);
            while (villainName == null) { Console.WriteLine("Такой злодей не найден"); }
            var freedMibionsCount = FreeMinions(villainId);
            DeleteVillain(villainId);
            Console.WriteLine(villainName + " был удалён\n" + freedMibionsCount + " миньонов было освобождено");
        }

        static void Task6()
        {
            string[] idStrMass = Console.ReadLine().Split(' ');
            List<int> idList = new List<int>();
            foreach (string i in idStrMass) { idList.Add(Int32.Parse(i)); }
            IncrementMinionsAge(idList);
            string sql =
                $"SELECT Minions.Name n, Minions.Age a " +
                $"FROM Minions ";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["n"]}, {reader["a"]} ");
                    }
                }
            }

        }

        static void Main(string[] args)
        {
            //Task2();
            //Task3();
            //Task4();
            Task5();
            //Task6();
        }
    }
}