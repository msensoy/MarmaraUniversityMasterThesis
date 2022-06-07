using System.IO;

namespace Marmara.Common
{
    public static class Const
    {
        public const string admin = "admin";
        public const string RoleAdmin = "Admin";
        public const string RoleUser = "User";

        public static string Safety = "Safety";
        public static string HVAC = "HVAC";
        public static string Lighting = "Lighting";
        public static string properties = "properties";
        public static string actions = "actions";
        public static string events = "events";

        public static string HUBURLS = "HUBURLS";
        public static string MAILSENDTIME = "MAILSENDTIME";
        public static string MAILFREQUENCY = "MAILFREQUENCY";
        public static string SAVEDATAFREQUENCY = "SAVEDATAFREQUENCY";
        public static string RULEFREQUENCY = "RULEFREQUENCY";
        public static string SCHEDULEFREQUENCY = "SCHEDULEFREQUENCY";
  

        public static string Sensor = "Sensor";
        public static string Actuator = "Actuator";


        public static string MQ2 = "MQ2";
        public static string MQ135 = "MQ135";
        public static string CO = "CO";
        public static string LPG = "LPG";
        public static string SMOKE = "SMOKE";
        public static string TEMPERATURE = "TEMPERATURE";
        public static string HUMIDITY = "HUMIDITY";

        public static string DHT11 = "DHT11";
        public static string LDR = "LDR";

        public static string DataSensorOld = "DataSensorOld";
        public static string DataSensor = "DataSensor";
        public static string DataMeta = "DataMeta";
        public static string DataActuator = "DataActuator";


        public static string LUX = "LUX";



        public static string CO2 = "CO2";
        public static string PPM = "PPM";

        public static string ON = "ON";
        public static string OFF = "OFF";

        public static string CFL = "CFL";
        public static string LED = "LED";
        public static string FAN = "FAN";
        public static string ALARM = "ALARM";
        public static string ALARMSENSOR = "ALARMSENSOR";
        public static string FLAME = "FLAME";
        public static string PIR = "PIR";
        public static string SOUND = "SOUND";


        public static string Token_Login = "token_Login";
        public static string Token_UserRole = "token_Role";
        public static string Token_Username = "token_Username";

        public static string API_URL = @"https://192.168.43.36:5001/";
        public static string API_URL_LOGIN = @"https://192.168.43.36:5001/api/authenticate/login";

        public static string GetUrl(string path) => API_URL + path;


        //public static string TxtFilesPath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "TxtFiles");

        public static string TxtDataFolderPath(string folderName)
        {

            var paths = "/home/pi/Marmara/DataFiles/" + folderName ;
#if DEBUG
            paths = @"C:\Users\MONSTER\source\repos\MarmaraMaster\HardWare\DataFiles\" + folderName;
#endif
            return paths;

        }


        public static string connStr = @"Server=tcp:marmara.database.windows.net,1433;Initial Catalog=MarmaraDb;Persist Security Info=False;User ID=mehmet;Password=Ericsson2021;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    }
}