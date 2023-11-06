using System;
using PhoneNumbers;
using System.IO;
using System.Linq;

namespace ApptHeroAPI.Common.Utilities
{
    public class PhoneNumberUtility
    {
        public static bool IsValidNumber(string aNumber)
        {
            bool result = false;

            aNumber = aNumber.Trim();

            if (aNumber.StartsWith("00"))
            {
                // Replace 00 at beginning with +
                aNumber = "+" + aNumber.Remove(0, 2);
            }

            try
            {

                var phoneNumber = PhoneNumberUtil.GetInstance().Parse(aNumber, "IT");
                result = PhoneNumberUtil.GetInstance().IsValidNumber(phoneNumber);
            }
            catch (Exception ex)
            {
                // Exception means is no valid number
                using (StreamWriter w = File.AppendText("myFile.txt"))
                {
                    w.WriteLine("Exception: " + ex.Message);
                }
            }

            return result;
        }

        public static string FormatE164PhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.Replace("+1", "");
            return FormatPhoneNumber(phoneNumber);
        }

        public static string ChangeToE164PhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.Replace(" ", "");
            phoneNumber = "+1" + phoneNumber;
            return phoneNumber;
        }


        public static string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return string.Empty;

            if (phoneNumber.Length > 20)
                return phoneNumber;
            phoneNumber = new System.Text.RegularExpressions.Regex(@"\D")
                .Replace(phoneNumber, string.Empty);
            phoneNumber = phoneNumber.TrimStart('1');
            if (phoneNumber.Length == 7)
                return Convert.ToInt64(phoneNumber).ToString("### ####");
            if (phoneNumber.Length == 9)
                return Convert.ToInt64(phoneNumber).ToString("(###) ### ###");
            if (phoneNumber.Length == 10)
                return Convert.ToInt64(phoneNumber).ToString("(###) ###-####");
            if (phoneNumber.Length > 10)
                return String.Format("{0:(###) ###-####}", phoneNumber);

            return phoneNumber;
        }

        public static bool IsPhoneNumber(string phoneNumber)
        {
            bool isPhoneNumber = false;
            if (!phoneNumber.Contains("@"))
            {
                //if the phone number has digits
                if (phoneNumber.Any(char.IsDigit))
                {
                    isPhoneNumber = true;
                }
            }


            return isPhoneNumber;
        }

        public static string RemovePhoneNumberFormat(string phoneNumber)
        {

            if (string.IsNullOrEmpty(phoneNumber))
                return string.Empty;

            char[] arr = phoneNumber.ToCharArray();
            arr = Array.FindAll<char>(arr, (c => (char.IsLetterOrDigit(c))));
            phoneNumber = string.Join("", arr);
            return phoneNumber;
        }
    }
}
