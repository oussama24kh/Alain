using ALAIN_DataAccess;

namespace ALAIN_Business
{
    public class Country
    {
        public static List<string> GetAllCountries()
        {
            return CountryData.GetAllCountries();
        }
        public static List<string> GetActiveCountries()
        {
            return CountryData.GetActiveCountries();
        }
    }
}