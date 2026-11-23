using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace project_03.Global_Classes
{
    public class Format
    {
        public static string DateToShort(DateTime Dt)
        {
            return Dt.ToString("dd/MMM/yyyy", new CultureInfo("fr-FR"));
        }
        public static string DateToLong(DateTime Dt)
        {
            return Dt.ToString("dd MMMM yyyy", new CultureInfo("fr-FR"));
        }
        public static string FullDate(DateTime Dt)
        {
            return Dt.ToString("D", new CultureInfo("fr-FR"));
        }
    }
}