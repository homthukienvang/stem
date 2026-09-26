using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Compare 2 strings is equal
        /// </summary>
        /// <param name="self"></param>
        /// <param name="destString"></param>
        /// <returns></returns>
        public static bool Similar(this string self, string destString)
        {
            return self.ToLower() == destString.ToLower();
        }

        /// <summary>
        /// To lower the first character of string: ConcreteString -> concreteString
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ToLowerFirst(this string self)
        {
            if (string.IsNullOrEmpty(self)) return self;
            return self.Substring(0, 1).ToLower() + self.Substring(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string MD5Hash(this string self)
        {
            if (string.IsNullOrEmpty(self)) return self;

            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(Encoding.ASCII.GetBytes(self));

            //get hash result after compute it
            byte[] result = md5.Hash;

            var strBuilder = new StringBuilder();
            foreach (byte t in result)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(t.ToString("x2"));
            }
            return strBuilder.ToString();
        }
        public static List<E> MixList<E>(List<E> inputList)
        {
            if (inputList == null || inputList.Count == 0) return new List<E>();
            if (inputList.Count == 1) return inputList;

            List<E> randomList = new List<E>();

            Random r = new Random();
            int randomIndex = 0;
            if (inputList.Count == 2)
            {
                randomList.Add(inputList[1]);
                randomList.Add(inputList[0]);
            }
            else
            {
                while (inputList.Count > 0)
                {
                    randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                    randomList.Add(inputList[randomIndex]); //add it to the new, random list
                    inputList.RemoveAt(randomIndex); //remove to avoid duplicates
                }
            }

            return randomList; //return the new random list
        }

        public static object GetValue(this Dictionary<string, object> value, string key)
        {
            return value.ContainsKey(key) ? value[key] : "";
        }

        public static DateTime GetFirstDayOfWeek(this DateTime sourceDateTime)
        {
            var daysAhead = (DayOfWeek.Sunday - (int)sourceDateTime.DayOfWeek);

            sourceDateTime = sourceDateTime.AddDays((int)daysAhead);

            return sourceDateTime;
        }

        public static DateTime GetLastDayOfWeek(this DateTime sourceDateTime)
        {
            var daysAhead = DayOfWeek.Saturday - (int)sourceDateTime.DayOfWeek;

            sourceDateTime = sourceDateTime.AddDays((int)daysAhead);

            return sourceDateTime;
        }

        public static string Formatting(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            string[] strDestination = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string pStr in strDestination)
            {
                value += pStr + " ";
            }
            return value.Trim();
        }
    }
}
