using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveMenu.Classes
{
    public static class CustomMethods
    {
        public static string RandomString(int num)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, num)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}