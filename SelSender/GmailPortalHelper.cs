using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelSender
{
    internal class GmailPortalHelper
    {
        private IWebDriver driver;
        public GmailPortalHelper()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            driver = new ChromeDriver(options);
        }

        public void LogIn(string username, string password)
        {
            driver.Navigate().GoToUrl("https://accounts.google.com/v3/signin/identifier?continue=https%3A%2F%2Fmail.google.com%2Fmail%2F&ifkv=AaSxoQyMXNrunkp2JxlSbgqxoJKWveTkOCrsy1NdIBQVdue_09SFukuyAtZNn-T-7c519Z-APsJvQA&rip=1&sacu=1&service=mail&flowName=GlifWebSignIn&flowEntry=ServiceLogin&dsh=S-1409136707%3A1715547571531990&ddm=0");
            Thread.Sleep(500);
            var email = driver.FindElement(By.XPath("//input[@aria-label='Email or phone']"));
            email.SendKeys(username);
            var buttonNext = driver.FindElement(By.XPath("//span[text()='Next']"));
            buttonNext.Click();
            Thread.Sleep(4000);
            var pass = driver.FindElement(By.XPath("//input[@aria-label='Enter your password']"));
            pass.SendKeys(password);
            var buttonNextPass = driver.FindElement(By.XPath("//span[text()='Next']"));
            buttonNextPass.Click();
        }

        public void SendMessage(string recipient, string subject, string message)
        {
            Thread.Sleep(5000);
            var composeEmail = driver.FindElement(By.XPath("//div[contains(text(),'Compose')]"));
            composeEmail.Click();
            Thread.Sleep(1000);
            var recepientTextBox = driver.FindElement(By.XPath("//input[@aria-label='To recipients']"));
            recepientTextBox.SendKeys(recipient);
            Thread.Sleep(1000);
            var subjectTextBox = driver.FindElement(By.XPath("//input[@aria-label='Subject']"));
            subjectTextBox.SendKeys(subject);
            Thread.Sleep(1000);
            var messageTextBox = driver.FindElement(By.XPath("//div[@aria-label='Message Body']"));
            messageTextBox.SendKeys(message);
            Thread.Sleep(1000);
            var sendMessage = driver.FindElement(By.Id(":6z"));
            sendMessage.Click();
        }
        public void Close()
        {
            driver.Quit();
        }
    }
}
