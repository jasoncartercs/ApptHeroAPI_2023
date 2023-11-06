using System;

namespace ApptHeroAPI.Services.Implementation.Util
{
    public class PaymentUtil
    {
        public static int NumberOfPenniesInDollar = 100;
        public static string US_DOLLARS = "USD";


        public static string ConvertTotalIntToString(object totalObj)
        {
            string total = string.Empty;
            if (totalObj != null)
            {
                if (int.TryParse(totalObj.ToString(), out int totalInt))
                {
                    decimal price = PaymentUtil.ConvertIntToPrice(totalInt);
                    total = price.ToString("N2");
                }
            }
            return total;
        }

        public static int ConvertPriceToInt(decimal price)
        {
            return Convert.ToInt32(Convert.ToDouble(price) * NumberOfPenniesInDollar);
        }

        public static decimal ConvertIntToPrice(int price)
        {
            return Convert.ToDecimal(Convert.ToDouble(price) / NumberOfPenniesInDollar);
        }

        public static int ConvertToIntMoney(int price)
        {
            return price * NumberOfPenniesInDollar;
        }
    }
}