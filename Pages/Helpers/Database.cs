using System.Data.SqlClient;

namespace Imenik.Pages.Helpers
{
    public class Database
    {

        List<Country> countries = new List<Country>();
        List<City> cities = new List<City>();
        List<OsobaInfo> osobe = new List<OsobaInfo>();
        OsobaInfo osobaInfo = new OsobaInfo();

        public String InsertIntoDatabase(OsobaInfo osobaInfo)
        {
            String connectionString = getConnectionString();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Person (Name, Surname, Phone_number, Gender, Email, City, Country, DOB, Age)" +
                        "VALUES(@name, @surname, @phone, @gender, @email, @city, @country, @dob, @age);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", osobaInfo.name);
                        command.Parameters.AddWithValue("@surname", osobaInfo.surname);
                        command.Parameters.AddWithValue("@phone", osobaInfo.phone);
                        command.Parameters.AddWithValue("@gender", osobaInfo.gender);
                        command.Parameters.AddWithValue("@email", osobaInfo.email);
                        command.Parameters.AddWithValue("@city", osobaInfo.city);
                        command.Parameters.AddWithValue("@country", osobaInfo.country);
                        command.Parameters.AddWithValue("@dob", osobaInfo.dob);
                        command.Parameters.AddWithValue("@age", osobaInfo.age);
                        command.ExecuteNonQuery();
                    }
                }
                return "Success";
            } catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public List<OsobaInfo> getPeopleFromDB()
        {
            String connectionString = getConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String sql = "SELECT * FROM Person LEFT JOIN Cities ON Person.City=Cities.CityId LEFT JOIN Countries ON Person.Country=Countries.CountryId";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OsobaInfo osobaInfo = new OsobaInfo();
                            osobaInfo.id = "" + reader.GetInt32(0);
                            osobaInfo.name = reader.GetString(1);
                            osobaInfo.surname = reader.GetString(2);
                            osobaInfo.phone = reader.GetString(3);
                            osobaInfo.gender = reader.GetString(4);
                            osobaInfo.email = reader.GetString(5);
                            osobaInfo.city = reader.GetString(11);
                            osobaInfo.country = reader.GetString(14);
                            osobaInfo.dob = reader.GetString(8);
                            osobaInfo.age = "" + reader.GetInt32(9);
                            osobe.Add(osobaInfo);
                        }
                    }
                }
            }
            return osobe;
        }

        public List<Country> GetCountriesFromDB()
        {
            String connectionString = getConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String sql = "SELECT * FROM Countries;";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Country country = new Country();
                            country.id = reader.GetInt32(0);
                            country.name = reader.GetString(1);
                            countries.Add(country);
                        }
                    }

                }
            }
            return countries;
        }

        public List<City> GetCitiesFromDB(int countryId)
        {
            String connectionString = getConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String sql = "SELECT * FROM Cities WHERE CountryId=@countryId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@countryId", countryId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            City city = new City();
                            city.Id = reader.GetInt32(0);
                            city.Name = reader.GetString(1);
                            city.CountryId = reader.GetInt32(2);
                            cities.Add(city);
                        }

                    }

                }
            }
            return cities;

        }

        public String deletePersonFromDB(String id)
        {
            try
            {
                String connectionString = getConnectionString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "DELETE FROM Person WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public OsobaInfo getPersonFromDB(String id)
        {

            String connectionString = getConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String sql = "SELECT * FROM Person LEFT JOIN Cities ON Person.City=Cities.CityId LEFT JOIN Countries ON Person.Country=Countries.CountryId WHERE id=@id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            osobaInfo.id = "" + reader.GetInt32(0);
                            osobaInfo.name = reader.GetString(1);
                            osobaInfo.surname = reader.GetString(2);
                            osobaInfo.phone = reader.GetString(3);
                            osobaInfo.gender = reader.GetString(4);
                            osobaInfo.email = reader.GetString(5);
                            osobaInfo.city = reader.GetString(11);
                            osobaInfo.country = reader.GetString(14);
                            osobaInfo.dob = reader.GetString(8);
                            osobaInfo.age = "" + reader.GetInt32(9);
                            osobaInfo.countryId = reader.GetInt32(7);
                        }
                    }

                }
            }

            return osobaInfo;
        }

        public String UpdatePersonInDB(OsobaInfo osobaInfo)
        {
            try
            {
                String connectionString = getConnectionString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Person " +
                        "SET name=@name, surname=@surname, phone_number=@phone, gender=@gender, email=@email, " +
                        "city=@city, country=@country, dob=@dob, age=@age " +
                        "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", osobaInfo.name);
                        command.Parameters.AddWithValue("@surname", osobaInfo.surname);
                        command.Parameters.AddWithValue("@phone", osobaInfo.phone);
                        command.Parameters.AddWithValue("@gender", osobaInfo.gender);
                        command.Parameters.AddWithValue("@email", osobaInfo.email);
                        command.Parameters.AddWithValue("@city", osobaInfo.city);
                        command.Parameters.AddWithValue("@country", osobaInfo.country);
                        command.Parameters.AddWithValue("@dob", osobaInfo.dob);
                        command.Parameters.AddWithValue("@age", osobaInfo.age);
                        command.Parameters.AddWithValue("@id", osobaInfo.id);
                        command.ExecuteNonQuery();
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public String getConnectionString()
        {
            return "Data Source=.\\sqlexpress;Initial Catalog=Imenik;Integrated Security=True;TrustServerCertificate=True;";
        }
    }
}
