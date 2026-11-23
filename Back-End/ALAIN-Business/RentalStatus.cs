using ALAIN_DataAccess;

namespace ALAIN_Business
{
    public class RentalStatus
    {
        public static List<string> GetActiveStatus()
        {
            return RentalStatusData.GetActiveStatus();
        }
    }
}