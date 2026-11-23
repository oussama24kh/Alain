using ALAIN_DataAccess;

namespace ALAIN_Business
{
    public class BodyType
    {
        public static List<string> GetAllBodyTypes()
        {
            return BodyType_Data.GetAllBodyTypes();
        }
    }
}