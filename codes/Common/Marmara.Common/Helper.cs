using Marmara.Common.Model;
using Marmara.Common.ThingClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Marmara.Common
{
    public static class Helper
    {
        public static class ReadAndWriteFileHelper
        {

            public static string ReadFile(string folderName, string fileName)
            {
                var fullPath = GetTxtFileFullPath(folderName, fileName);
                return File.ReadAllText(fullPath);
            }
            public static string[] ReadAllLines(string folderName, string fileName)
            {
                var fullPath = GetTxtFileFullPath(folderName, fileName);
                return File.ReadAllLines(fullPath);
            }

            public static string ReadFileData(string folderName, string fileName, string type = null)
            {
                try
                {
                    if (type == null)
                    {
                        type = fileName;
                    }
                    var text = ReadFile(folderName, fileName);
                    if (!string.IsNullOrEmpty(text))
                    {
                        string[] datas = text.Trim().Split(';');
                        var value = datas.FirstOrDefault(x => x.Contains(type)).Split(':')[1];
                
#if DEBUG
                        value = value.Replace('.', ',');
#endif
                        return value.Trim();
                    }
                    else
                    {
                        return "0";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return "0.00";

            }
            public static string ReadFileHubUrl(string type)
            {
                var text = ReadAllLines(Const.DataMeta, Const.HUBURLS);
                var value = text.FirstOrDefault(x => x.Contains(type)).Split('=')[1];
                return value;
            }

            public static bool ReadFileDataStatus(string folderName, string fileName, string type)
            {
                var value = ReadFileData(folderName, fileName, type);
                return value == Const.ON ? true : false;
            }

            public static void WriteFile(string folderName, string fileName, string value)
            {
                var fullPath = GetTxtFileFullPath(folderName, fileName);
                File.WriteAllText(fullPath, value);
            }
            public static void WriteFileData(string folderName, string fileName, string value, string type = null)
            {
                var fullPath = GetTxtFileFullPath(folderName, fileName);
                string text = string.Empty;
                if (type == null)
                {
                    text = $"{fileName}:{value}";
                }
                else
                {
                    text = $"{type}:{value}";
                }

                File.WriteAllText(fullPath, text);


                if (fileName == Const.LED)
                {
                    value = value != "0" ? ((int)Math.Floor(Convert.ToDouble(value) * 0.3 + 50)).ToString() : value; // Regression Equation of x on y - Formula
                    text = $"DimPercent:{value}";
                    var LEDdimpath = fullPath.Replace("LED", "LEDdim");
                    File.WriteAllText(LEDdimpath, text);
                }
            }


            public static string GetAbstractPath(string fileName)
            {
                var basefolder = Directory.GetCurrentDirectory();
                var path = String.Concat(basefolder, fileName);
                return path;
            }
            public static string GetTxtFileFullPath(string folderName, string fileName)
            {
                fileName = fileName.Contains(".txt") ? fileName : String.Concat(fileName, ".txt");
                var fullpath = Path.Combine(Const.TxtDataFolderPath(folderName), fileName);
                return fullpath;
            }
     

            public static DateTime GetLastSendMailTime()
            {
                string path = Helper.ReadAndWriteFileHelper.GetTxtFileFullPath(Const.DataMeta, Const.MAILSENDTIME);
                string text = File.ReadAllText(path);
                DateTime value;
                if (string.IsNullOrEmpty(text))
                {
                    text = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                    File.WriteAllText(path, text);
                }
                CultureInfo culture = new CultureInfo("tr-TR");
                value = Convert.ToDateTime(text, culture);
                return value;
            }

            public static double ConvertToDouble(string Value)
            {
                if (Value == null)
                {
                    return 0;
                }
                else
                {
                    double.TryParse(Value, out double OutVal);

                    if (double.IsNaN(OutVal) || double.IsInfinity(OutVal))
                    {
                        return 0;
                    }
                    return Math.Round(OutVal, 2);
                }
            }

        }



        public static class RandomHelper
        {
            private static readonly Random _random = new Random();
            public static bool RandomBoolean()
            {
                var isExist = _random.Next(0, 10);
                return isExist % 2 == 1 ? true : false;
            }

            public static double RandomDouble(int min, int max)
            {
                var value = Math.Round((_random.NextDouble() * (max - min) + min), 2);
                return value;
            }

            public static int RandomInteger(int min, int max)
            {
                return _random.Next(0, 100);
            }

            public static double[] RandomMQ2(double min, double max)
            {
                var values = new double[3];
                var co = Math.Round((_random.NextDouble() * (max - min) + min), 2);
                var lpg = co * _random.Next(1, 2);
                var smoke = lpg * _random.Next(1, 3);
                values[0] = co;
                values[1] = lpg;
                values[2] = smoke;
                return values;
            }
        }

        public static class RequestHelper
        {
            public class ResponseModel
            {
                public string Token { get; set; }
                public string Expiration { get; set; }
            }

      

            public static string PostRequestAPIMethod(string url, string value)
            {
                var token = GetTokenOnLoginUrl();
                if (!string.IsNullOrEmpty(token))
                {
                    string html = string.Empty;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                    var request = (HttpWebRequest)WebRequest.Create(url);

                    request.ContentType = "application/json";
                    request.Method = "POST";
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    request.PreAuthenticate = true;
                    request.Headers.Add("Authorization", "Bearer " + token);

                    using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(value.ToLower());
                    }
                    HttpWebResponse httpWebResponse = (HttpWebResponse)request.GetResponse();
                    string result;
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    return result;
                }
                return null;
            }

            public static List<Thing> GetRequestAPIMethod(string url)
            {
                var token = GetTokenOnLoginUrl();
                if (!string.IsNullOrEmpty(token))
                {
                    string html = string.Empty;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    request.PreAuthenticate = true;
                    request.Headers.Add("Authorization", "Bearer " + token);
                    request.Accept = "application/json";
                    //string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(UserName + ":" + UserPassword));
                    //request.Headers.Add("Authorization", "Basic " + encoded);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }
                    //var thingsDescription = JsonConvert.DeserializeObject<List<Thing>>(html);
                    var thingsDescription = JsonConvert.DeserializeObject<List<Thing>>(JsonConvert.DeserializeObject<string>(html));

                    return thingsDescription;
                }
                return new List<Thing>();
            }
            public static string GetRequestAPIMethodString(string url)
            {
                var token = GetTokenOnLoginUrl();
                if (!string.IsNullOrEmpty(token))
                {
                    string html = string.Empty;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    request.PreAuthenticate = true;
                    request.Headers.Add("Authorization", "Bearer " + token);
                    request.Accept = "application/json";
    
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }
                    return html;
                  
                }
                return null;
            }
            public static string GetRequestForData(string url)
            {
                var token = GetTokenOnLoginUrl();
                if (!string.IsNullOrEmpty(token))
                {
                    string html = string.Empty;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    request.PreAuthenticate = true;
                    request.Headers.Add("Authorization", "Bearer " + token);
                    request.Accept = "application/text";

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }
                    return html;

                }
                return null;
            }

            public static void CallHubService(string hubUrl)
            {
                var token = GetTokenOnLoginUrl();
                if (!string.IsNullOrEmpty(token))
                {
                    string html = string.Empty;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(hubUrl);
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    request.PreAuthenticate = true;
                    request.Headers.Add("Authorization", "Bearer " + token);
                    request.Accept = "application/json";
                    using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    using Stream stream = response.GetResponseStream();
                    using StreamReader reader = new StreamReader(stream);
                    html = reader.ReadToEnd();
                }
            }

            public static string GetTokenOnLoginUrl(LoginModel model =null)
            {
                string result = string.Empty;
                var loginModel = new LoginModel();
                if (model == null)
                {
                    loginModel.Username = "admin";
                    loginModel.Password = "Admin@1";
                }
                else
                {
                    loginModel = model;
                }
                string requestJson = JsonConvert.SerializeObject(loginModel);

                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Const.API_URL_LOGIN);
                request.ContentType = "application/json";
                request.Method = "POST";

                using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestJson);
                }
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message == "The remote server returned an error: (401) Unauthorized.")
                    {
                        return null;
                    }
                }
              

                var responseModel = JsonConvert.DeserializeObject<ResponseModel>(result);
                return responseModel.Token;
            }

        }

        public static class ThingModelHelper
        {
            public static class ReflectiveEnumerator
            {
                static ReflectiveEnumerator()
                {
                }

                public static List<T> GetEnumerableOfType<T>()
                {
                    List<T> objects = new List<T>();
                    foreach (Type type in
                        Assembly.GetAssembly(typeof(T)).GetTypes()
                        .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
                    {
                        objects.Add((T)Activator.CreateInstance(type));
                    }
                    return objects;
                }
            }

            public static string GetThingJsonModel(object thing)
            {
                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                };

                string model = JsonConvert.SerializeObject(thing, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented
                });

                return model;
            }

            public static string GetThingsJsonModel(IEnumerable<object> things)
            {
                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                };

                string model = JsonConvert.SerializeObject(things, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented
                });

                return model;
            }

            //public static Thing GetThingModel(Thing type, string s)
            //{
            //    DefaultContractResolver contractResolver = new DefaultContractResolver
            //    {
            //        NamingStrategy = new CamelCaseNamingStrategy(),
            //    };
            //    Type type1 = Type.GetType(s);
            //    Object o = (Activator.CreateInstance(type1));
            //    Thing model = JsonConvert.DeserializeObject<o>(s);

            //    return model;
            //}
        }




    }

 
}