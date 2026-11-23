namespace RC_API.Global_Classes
{
    public class Format
    {
        public static string DateToShort(DateTime Dt1)
        {
            return Dt1.ToString("dd/MMM/yyyy");
        }
    }
}