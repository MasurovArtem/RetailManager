using System.Configuration;

namespace RMDataManager.Library
{
    public class ConfigHelper
    {
        public static decimal GetTaxRate()
        {
            string rateText = ConfigurationManager.AppSettings["taxRate"];

            bool isValidTextRate = decimal.TryParse(rateText, out decimal output);

            if (isValidTextRate == false)
            {
                throw new ConfigurationErrorsException("The tax rate is not set up properly");
            }

            return output;
        }
    }
}