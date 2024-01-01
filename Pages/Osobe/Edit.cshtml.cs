using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Globalization;
using Imenik.Pages.Helpers;

namespace Imenik.Pages.Osobe
{
    public class EditModel : PageModel
    {
        public OsobaInfo osobaInfo = new OsobaInfo();
        public List<Country> countries = new List<Country>();
        Database database = new Database();
        ValidationHelpers validation = new ValidationHelpers();
        public String errorMessage = "";
        public String successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            getCountries();
            osobaInfo = database.getPersonFromDB(id);
            if(osobaInfo == null) {
                errorMessage = "Database connection problem!";
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

            String validationResult = validation.ValidatePerson(osobaInfo);
            if (validationResult == "Valid")
            {
                osobaInfo.age = "" + validation.CalculateAge(osobaInfo.dob);
                String databaseResult = database.UpdatePersonInDB(osobaInfo);
                if (databaseResult != "Success")
                {
                    errorMessage = databaseResult;
                }
                else
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
            }
            else
            {
                errorMessage = validationResult;
            }
        }
        
        public void getCountries()
        {
            countries = database.GetCountriesFromDB();
            if (countries.Count == 0 || countries == null)
            {
                this.errorMessage = "Database connection problem!";
            }
        }
    }
}
