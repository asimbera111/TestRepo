using System;

namespace WebServiceAutomation.Helpers.Authentication
{
    public class Base64StringConverter
    {
        public static string GetBase64String(String username, string password)
        {
            string auth = username + ":" + password;
            byte[] inArray = System.Text.UTF8Encoding.UTF8.GetBytes(auth);
            return System.Convert.ToBase64String(inArray);
        }
    }
}
