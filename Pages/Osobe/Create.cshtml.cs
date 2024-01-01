using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Imenik.Pages.Helpers;

namespace Imenik.Pages.Osobe
{
    public class CreateModel : PageModel
    {
        public OsobaInfo osobaInfo = new OsobaInfo();
        Database database = new Database();
        ValidationHelpers validation = new ValidationHelpers();
        public List<Country> countries = new List<Country>();
        List<City> cities = new List<City>();
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

            String validationResult = validation.ValidatePerson(osobaInfo);
            if(validationResult == "Valid")
            {
                osobaInfo.age = "" + validation.CalculateAge(osobaInfo.dob);
                String databaseResult = database.InsertIntoDatabase(osobaInfo);
                if (databaseResult != "Success")
                {
                    errorMessage = databaseResult;
                } else
                {
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
            } else
            {
                errorMessage = validationResult;
            }
        }

        public void getCountries()
        {
            countries = database.GetCountriesFromDB();
            if(countries.Count == 0 || countries == null) 
            {
                this.errorMessage = "Database connection problem!";
            }
        }

        public IActionResult OnGetCitiesForCountry(int countryId)
        {
            var cities = GetCitiesForCountry(countryId);
            if (cities.Count == 0 || cities == null)
            {
                this.errorMessage = "Database connection problem!";
                return null;
            } else
            {
                return new JsonResult(cities);
            }
        }

        private List<City> GetCitiesForCountry(int countryId)
        {
            cities = database.GetCitiesFromDB(countryId);
            return cities;
        }
    }
}