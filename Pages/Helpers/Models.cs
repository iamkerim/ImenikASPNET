namespace Imenik.Pages.Helpers
{
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
