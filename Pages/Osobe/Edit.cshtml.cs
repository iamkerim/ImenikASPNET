using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Globalization;

namespace Imenik.Pages.Osobe
{
    public class EditModel : PageModel
    {
        public OsobaInfo osobaInfo = new OsobaInfo();
        public List<Country> countries = new List<Country>();

        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            getCountries();
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Imenik;Integrated Security=True;TrustServerCertificate=True;";
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
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            osobaInfo.name = Request.Form["name"];
            osobaInfo.id = Request.Form["id"];
            osobaInfo.surname = Request.Form["surname"];
            osobaInfo.phone = Request.Form["phone"];
            osobaInfo.gender = Request.Form["gender"];
            osobaInfo.email = Request.Form["email"];
            osobaInfo.city = Request.Form["selectedValueCity"];
            osobaInfo.country = Request.Form["country"];
            osobaInfo.dob = Request.Form["dob"];

            if (osobaInfo.name.Length == 0 || osobaInfo.surname.Length == 0 ||
                osobaInfo.phone.Length == 0 || osobaInfo.gender.Length == null ||
                osobaInfo.email.Length == 0 || osobaInfo.city.Length == 0 ||
                osobaInfo.country.Length == 0 || osobaInfo.dob.Length == 0)
            {
                errorMessage = "Obavezna su sva polja!";
                return;
            }
            else if (!IsValidPhone(osobaInfo.phone))
            {
                errorMessage = "Netacan format telefona(XXX/XXX-XXX)!";
                return;
            }
            else if (!IsValidDOB(osobaInfo.dob))
            {
                errorMessage = "Netacan format godine rodjenja(dd.mm.yyyy)!";
                return;
            }
            else
            {
                osobaInfo.age = "" + CalculateAge(osobaInfo.dob);
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Imenik;Integrated Security=True;TrustServerCertificate=True;";
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
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Osobe/Index");
        }
        private bool IsValidPhone(string value)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{3}\/\d{3}-\d{3}$"); // XXX/XXX-XXX format
            return regex.IsMatch(value);
        }

        private bool IsValidDOB(string value)
        {
            if (DateTime.TryParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return true;
            }
            return false;
        }
        private int? CalculateAge(string Dob)
        {
            if (DateTime.TryParseExact(Dob, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var birthDate))
            {
                // Calculate age based on the current date
                var today = DateTime.Today;
                var age = today.Year - birthDate.Year;

                // Adjust age if birthday hasn't occurred yet this year
                if (today < birthDate.AddYears(age))
                {
                    age--;
                }

                return age;
            }

            return null; // Return null if date parsing fails
        }

        public void getCountries()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Imenik;Integrated Security=True;TrustServerCertificate=True;";
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
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public class Country
        {
            public int? id;
            public String? name;
        }
    }
}
