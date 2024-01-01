using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Imenik.Pages.Helpers;

namespace Imenik.Pages.Osobe
{
    public class IndexModel : PageModel
    {
        public List<OsobaInfo> osobe = new List<OsobaInfo>();
        Database database = new Database();
        public String errorMessage = "";
        public void OnGet()
        {
            osobe = database.getPeopleFromDB();
            if(osobe == null || osobe.Count  == 0)
            {
                errorMessage = "Database connection problem";
            }
        }
    }
}
