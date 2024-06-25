using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using System.Net.Mail;

namespace SelSender
{
    internal class GmailPortalHelper
    {
        static string ApplicationName = "Gmail API .NET Quickstart";
        private static string googleClientId = "xxx";
        private static string googleClientSecret = "xxx";
        private static UserCredential Login(string googleClientId, string googleClientSecret, string[] scopes)
        {
            ClientSecrets secrets = new ClientSecrets()
            {
                ClientId = googleClientId,
                ClientSecret = googleClientSecret
            };
            return GoogleWebAuthorizationBroker.AuthorizeAsync(secrets, scopes, "user", CancellationToken.None, new FileDataStore("token.json", true)).Result;
        }
        internal void SendEmail(string subject, string body, string receiver)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string[] scopes = new string[]
            {
                Google.Apis.Gmail.v1.GmailService.Scope.GmailSend
            };
            UserCredential credential = Login(googleClientId, googleClientSecret, scopes);
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var emailMessage = new AE.Net.Mail.MailMessage()
            {
                Subject = subject,
                Body = body,
                From = new MailAddress("wkrak98@gmail.com"),
            };
            emailMessage.To.Add(new MailAddress(receiver));
            emailMessage.ReplyTo.Add(emailMessage.From);

            var msgStr = new StringWriter();
            emailMessage.Save(msgStr);

            var result = service.Users.Messages.Send(new Message
            {
                Raw = Base64UrlEncode(msgStr.ToString())
            }, "me").Execute();

            Console.WriteLine("Message ID {0} sent.", result.Id);
        }
        private static string Base64UrlEncode(string message)
        {
            var inputBytes = Encoding.GetEncoding("utf-8").GetBytes(message);
            return Convert.ToBase64String(inputBytes).Replace('+', '-').Replace('/', '_').Replace("=", "");
        }
    }
}
