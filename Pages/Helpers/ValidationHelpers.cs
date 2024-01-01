using System.Globalization;

namespace Imenik.Pages.Helpers
{
    public class ValidationHelpers
    {
        public String ValidatePerson(OsobaInfo osobaInfo)
        {
            if (osobaInfo.name.Length == 0 || osobaInfo.surname.Length == 0 ||
                osobaInfo.phone.Length == 0 || osobaInfo.gender.Length == null ||
                osobaInfo.email.Length == 0 || osobaInfo.city.Length == 0 ||
                osobaInfo.country.Length == 0 || osobaInfo.dob.Length == 0)
            {
                return "Obavezna su sva polja!";
            }
            else if (!IsValidPhone(osobaInfo.phone))
            {
                return "Netacan format telefona(XXX/XXX-XXX)!";
            }
            else if (!IsValidEmail(osobaInfo.email))
            {
                return "Netacan format emaila(example@example.com)!";
            }
            else if (!IsValidDOB(osobaInfo.dob))
            {
                return "Netacan format godine rodjenja(dd.mm.yyyy)!";
            }
            else
            {
                return "Valid";
            }

        }

        private static bool IsValidPhone(string value)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^\d{3}\/\d{3}-\d{3}$"); // XXX/XXX-XXX format
            return regex.IsMatch(value);
        }

        private static bool IsValidDOB(string value)
        {
            if (DateTime.TryParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return true;
            }
            return false;
        }

        private static bool IsValidEmail(string value)
        {
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(value);

        }
        public int? CalculateAge(string Dob)
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
    }
}
