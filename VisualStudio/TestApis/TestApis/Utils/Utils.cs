using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Authenticators;
using Moq;
using Newtonsoft.Json;
using RestSharp;


namespace TestApis.Utils
{
    public class Utils
    {

        public static T DeserializeResponse<T>(string jsonResponse)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            catch (Exception ex)
            {
                // Manejo de errores en caso de fallo de deserialización
                throw new Exception("Error deserializando la respuesta JSON: " + ex.Message);
            }
        }

        public static string GenerateRandomText(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var rndText = new StringBuilder();
            
            for (int i = 0; i < length; i++)
            {
                rndText.Append(chars[random.Next(chars.Length)]);
            }

            return rndText.ToString();
        }

        public static int GenerateRandomNumber(int min, int max)
        {
            Random random = new Random();
            int rndNumber = random.Next(min, max); 

            return rndNumber;
        }

    }
}
