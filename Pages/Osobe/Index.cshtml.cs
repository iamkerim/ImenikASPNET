using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Imenik.Pages.Osobe
{
    public class IndexModel : PageModel
    {
        public List<OsobaInfo> osobe = new List<OsobaInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Imenik;Integrated Security=True;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Person LEFT JOIN Cities ON Person.City=Cities.CityId LEFT JOIN Countries ON Person.Country=Countries.CountryId";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);  
            }
        }
    }
    public class OsobaInfo
    {
        public String? id;
        public String? name;
        public String? surname;
        public String? phone;
        public String? gender;
        public String? email;
        public String? city;
        public String? country;
        public String? dob;
        public String? age;
        public int? countryId;
    }
}
