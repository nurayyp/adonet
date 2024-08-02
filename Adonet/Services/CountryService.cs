using Adonet.Constants;
using Adonet.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adonet.Services
{
    public static class CountryService
    {
        public static void AddCountry()
        {
            Messages.InputMessage("Country Name");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                using (SqlConnection connection = new SqlConnection(ConnectionStrings.Default))
                {
                    connection.Open();
                    var selectCommand = new SqlCommand("SELECT * FROM Countries WHERE Name = @name", connection);
                    selectCommand.Parameters.AddWithValue("name", name);
                    try
                    {
                        int Id = Convert.ToInt32(selectCommand.ExecuteScalar());
                        if (Id > 0)
                            Messages.AlreadyExistsMessage("Country", name);
                        else
                        {
                            Messages.InputMessage("Country area");
                            string areaInput = Console.ReadLine();
                            decimal area;
                            bool isSucceeded = decimal.TryParse(areaInput, out area);
                            if (isSucceeded)
                            {
                                var command = new SqlCommand("INSERT INTO Countries VALUES(@name,@area)", connection);
                                command.Parameters.AddWithValue("@name", name);
                                command.Parameters.AddWithValue("@area", area);
                                var affectedRows = command.ExecuteNonQuery();
                                if (affectedRows > 0)
                                    Messages.SuccessAddMessage("Country", name);
                                else
                                    Messages.ErrorOccuredMessage();
                            }
                            else
                                Messages.InvalidInputMessage("Country Area");
                        }
                    }
                    catch (Exception)
                    {
                        Messages.ErrorOccuredMessage();
                    }
                }
            }
            else
                Messages.InvalidInputMessage("Country Name");
        }
        public static void GetAllCountries()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStrings.Default))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Countries", connection);
                using ( var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        string name = Convert.ToString(reader["Name"]);
                        decimal area = Convert.ToDecimal(reader["Area"]);
                        Messages.PrintMessage("Name", name);
                        Messages.PrintMessage("Area", area.ToString());
                    }
                }
            }
        }
        public static void UpdateCountry()
        {
            GetAllCountries();
            Messages.InputMessage("Country name");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                using (SqlConnection connection = new SqlConnection(ConnectionStrings.Default))
                {
                    var command = new SqlCommand("SELECT * FROM Countries WHERE Name = @name", connection);
                    command.Parameters.AddWithValue("@name", name);
                    try
                    {
                        int id = Convert.ToInt32(command.ExecuteScalar());
                        if (id > 0)
                        {
                        NameChangeInput: Messages.PrintWantToChangeMessage("name");
                            var choiceForName = Console.ReadLine();
                            char choice;
                            bool isSuccceeded = char.TryParse(choiceForName, out choice);
                            if (isSuccceeded && choice.isValidChoice())
                            {
                                string newName = string.Empty;
                                if (choice.Equals('y'))
                                {
                                NewNameInput: Messages.InputMessage("new name");
                                    newName = Console.ReadLine();
                                    if (!string.IsNullOrWhiteSpace(newName))
                                    {
                                        var alreadyExistsCommand = new SqlCommand("SELECT * FROM Countries WHERE Name = @name AND Id != @id", connection);
                                        alreadyExistsCommand.Parameters.AddWithValue("@name", newName);
                                        alreadyExistsCommand.Parameters.AddWithValue("@id", id);
                                        int existId = Convert.ToInt32(alreadyExistsCommand.ExecuteScalar());
                                        if (existId > 0)
                                            Messages.AlreadyExistsMessage("Country", newName);
                                        goto NameChangeInput;
                                    }
                                    else
                                        Messages.InvalidInputMessage("new name");
                                    goto NewNameInput;
                                }
                                AreaChangeInput: Messages.PrintWantToChangeMessage("area");
                                var choiceForArea = Console.ReadLine();
                                isSuccceeded = char.TryParse(choiceForArea, out choice);
                                decimal newArea = default;
                                if (!isSuccceeded && choice.isValidChoice())
                                {
                                    if (choice.Equals('y'))
                                    {
                                        Messages.InputMessage("new area");
                                        InputNewArea: var newAreaInput = Console.ReadLine();
                                        isSuccceeded = decimal.TryParse(newAreaInput, out newArea);
                                        if (!isSuccceeded)
                                        {
                                            Messages.InvalidInputMessage("new area");
                                            goto InputNewArea;
                                        }
                                    }
                                }
                                else
                                {
                                    Messages.InvalidInputMessage("Choice for Area");
                                    goto AreaChangeInput;
                                }
                                var updateCommand = new SqlCommand("UPDATE Countries SET", connection);
                                if (newName != string.Empty || newArea != default)
                                {
                                    if (newName != string.Empty)
                                    {
                                        updateCommand.CommandText = updateCommand.CommandText.Concat("Name=@name,").ToString();
                                        updateCommand.Parameters.AddWithValue("@name", newName);
                                    }
                                    if (newArea != default)
                                    {
                                        updateCommand.CommandText = updateCommand.CommandText.Concat("Area=@area").ToString();
                                        updateCommand.Parameters.AddWithValue("@area", newArea);
                                    }
                                    updateCommand.CommandText = updateCommand.CommandText.Concat("WHERE id=@id").ToString();
                                    int affectedRows = Convert.ToInt32(updateCommand.ExecuteNonQuery());
                                    if (affectedRows > 0)
                                    {
                                        Messages.SuccessUpdateMessage("Country", newName);
                                    }
                                    else
                                        Messages.ErrorOccuredMessage();
                                }
                            }
                            else
                                Messages.InvalidInputMessage("Choice for Name");
                        }
                        else
                            Messages.NotFoundMessage("Country", name);
                    }
                    catch (Exception)
                    {
                        Messages.ErrorOccuredMessage();
                    }
                }   
            }
            else
                Messages.InvalidInputMessage("Country name");
        }
        public static void DeleteCountry()
        {
            GetAllCountries();
            Messages.InputMessage("country name");
            string countryName = Console.ReadLine();
            if (!string.IsNullOrEmpty(countryName))
            {
                using (SqlConnection connection = new SqlConnection(ConnectionStrings.Default))
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM Countires WHERE Name=@name",connection);
                    command.Parameters.AddWithValue("@name", countryName);
                    try
                    {
                        int id = Convert.ToInt32(command.ExecuteScalar());
                        if (id > 0)
                        {
                            SqlCommand deleteCommand = new SqlCommand("DELETE Countries WHERE Id=@id", connection);
                            deleteCommand.Parameters.AddWithValue("@id", id);
                            int affectedRows = deleteCommand.ExecuteNonQuery();
                            if (affectedRows > 0)
                            {
                                Messages.SuccessDeleteMessage("country", countryName);
                            }
                            else
                                Messages.ErrorOccuredMessage();
                        }
                        else
                            Messages.NotFoundMessage("country", countryName);
                    }
                    catch (Exception)
                    {
                        Messages.ErrorOccuredMessage();
                    }
                }
            }
            else
                Messages.InvalidInputMessage("country name");
        }
        public static void GetDetailsOfCountry()
        {
            GetAllCountries();
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                using (SqlConnection connection = new SqlConnection(ConnectionStrings.Default))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Countries WHERE Name=@name", connection);
                    using(var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Messages.PrintMessage("Id", Convert.ToString(reader["Id"]));
                            Messages.PrintMessage("Name", Convert.ToString(reader["Name"]));
                            Messages.PrintMessage("Area", Convert.ToString(reader["Area"]));
                        }

                    }
                }
            }
            else
                Messages.InvalidInputMessage("country name");
        }
    }
}
