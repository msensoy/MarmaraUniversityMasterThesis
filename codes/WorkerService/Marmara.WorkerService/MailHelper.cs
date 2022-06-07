using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Marmara.WorkerService
{
    public static class MailHelper
    {
        public static string Subject { get; set; } = "Information";
        //public static string Sender => "marmarauni2020@gmail.com";
        public static string Sender => "marmara.wot@gmail.com";
        public static string Username => "Smart Building WoT";
        public static string Password => "marmara2020";
        public static string Receiver => "mehmet.sensoy.96@gmail.com";
        public static string ReceiverName => "Mehmet Sensoy";
        public static void Send(List<string> receivers)
        {
            MailMessage msg = new MailMessage(); //Mesaj gövdesini tanımlıyoruz...
            msg.Subject = Subject;
            msg.From = new MailAddress(Sender, Username);
            msg.To.Add(new MailAddress(Receiver, ReceiverName)); // çoğaltılabilir *******************************************************************************************
            foreach (var receiver in receivers)
            {
                msg.To.Add(new MailAddress(receiver, receiver));
            }

            //Mesaj içeriğinde HTML karakterler yer alıyor ise aşağıdaki alan TRUE olarak gönderilmeli ki HTML olarak yorumlansın. Yoksa düz yazı olarak gönderilir...
            msg.IsBodyHtml = true;
            msg.Body = string.Format(@"<img src='https://dbdzm869oupei.cloudfront.net/img/sticker/preview/8625.png'  width='100' height='100'>
                         <p>Warning!</p>
                         <p>Flame gas was detected inside the building.</p>
                         <p><br></p> 
                         <p>{0}</p> ", DateTime.Now.ToString());

            //Mesaj önceliği (BELİRTMEK ZORUNLU DEĞİL!)
            msg.Priority = MailPriority.High;

            //SMTP/Gönderici bilgilerinin yer aldığı erişim/doğrulama bilgileri
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587); //Bu alanda gönderim yapacak hizmetin smtp adresini ve size verilen portu girmelisiniz.
            NetworkCredential AccountInfo = new NetworkCredential(Sender, Password);
            //smtp.UseDefaultCredentials = false; //Standart doğrulama kullanılsın mı? -> Yalnızca gönderici özellikle istiyor ise TRUE işaretlenir.
            smtp.Credentials = AccountInfo;
            smtp.EnableSsl = true; //SSL kullanılarak mı gönderilsin...

            try
            {
                smtp.Send(msg);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Gönderim Hatası: {0}", ex.Message));
            }

        }
    }
}
