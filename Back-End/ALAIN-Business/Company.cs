using ALAIN_DataAccess;
using ALAIN_DataAccess.DTO;

namespace ALAIN_Business
{
    public class Company
    {
        public static ALAIN_DataAccess.Model.Company? GetCompanyInfo()
        {
            return CompanyData.GetCompanyInfo();
        }
        public static bool SaveCompanySettings(ALAIN_DataAccess.Model.Company company)
        {
            return CompanyData.SaveCompanySettings(company);
        }
    }
}