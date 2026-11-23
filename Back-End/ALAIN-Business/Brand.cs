using ALAIN_DataAccess;

namespace ALAIN_Business
{
    public class Brand
    {
        public static List<string> GetAllBrands()
        {
            return BrandData.GetAllBrands();
        }
    }
}