using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Globalization;

namespace Imenik.Pages.Osobe
{
    public class CreateModel : PageModel
    {
        public OsobaInfo osobaInfo = new OsobaInfo();
        public List<Country> countries = new List<Country>();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            getCountries();
        }

        public void OnPost()
        {
            osobaInfo.name = Request.Form["name"];
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
                osobaInfo.country.Length == 0 || osobaInfo.dob.Length == 0 )
            {
                errorMessage = "Obavezna su sva polja!";
                return;
            } else if(!IsValidPhone(osobaInfo.phone))
            {
                errorMessage = "Netacan format telefona(XXX/XXX-XXX)!";
                return;
            } else if(!IsValidDOB(osobaInfo.dob))
            {
                errorMessage = "Netacan format godine rodjenja(dd.mm.yyyy)!";
                return;
            } else
            {
                osobaInfo.age = "" + CalculateAge(osobaInfo.dob);
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Imenik;Integrated Security=True;TrustServerCertificate=True;";
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
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            osobaInfo.name = "";
            osobaInfo.surname = "";
            osobaInfo.phone = "";
            osobaInfo.gender = "";
            osobaInfo.email = "";
            osobaInfo.dob = "";
            osobaInfo.age = null;
            successMessage = "Nova osoba uspjesno dodana u imenik!";
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
                var today = DateTime.Today;
                var age = today.Year - birthDate.Year;

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

        public IActionResult OnGetCitiesForCountry(int countryId)
        {
            var cities = GetCitiesFromDatabase(countryId);
            return new JsonResult(cities);
        }

        private List<City> GetCitiesFromDatabase(int countryId)
        {
            List<City> cities = new List<City>();
            
            try
            {

                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Imenik;Integrated Security=True;TrustServerCertificate=True;";
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
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class Country
    {
        public int? id;
        public String? name;
    }

    public class City
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
    }
}