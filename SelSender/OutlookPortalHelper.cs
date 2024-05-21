using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelSender
{
    internal class OutlookProgramHelper
    {
        internal static void LogInToOutlook(string mail, string messageSubject, string messageBody)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://login.microsoftonline.com/common/oauth2/authorize?client_id=00000002-0000-0ff1-ce00-000000000000&redirect_uri=https%3a%2f%2foutlook.office.com%2fowa%2f&resource=00000002-0000-0ff1-ce00-000000000000&response_mode=form_post&response_type=code+id_token&scope=openid&msafed=1&msaredir=1&client-request-id=0ffa3394-8d6c-5c56-8025-2a0b70e222f8&protectedtoken=true&claims=%7b%22id_token%22%3a%7b%22xms_cc%22%3a%7b%22values%22%3a%5b%22CP1%22%5d%7d%7d%7d&nonce=638508775565220352.2add7b71-fef5-47d3-bf47-00238e550cac&state=Dcu7DoJAEEDRRf_FbmWYZZilIBYaQ6ENmmjo9kUikWCAYPx7tzi3u4kQYhttogRiBBdKE2hmooIQQRHu0XjPljPZhY5kzl5J2-UsAVDpQATOuCS-p3T8mvQwL2YJVbabgn9NwS33sTJ1A66-FpdfufpnM1ssp8tQDu3w7tsb9RZhtY_zxx71Hw&sso_reload=true");
            Thread.Sleep(3000);
            var email = driver.FindElement(By.Id("i0116"));
            email.SendKeys("pzx110299@student.wsb.poznan.pl");
            var nextButton = driver.FindElement(By.CssSelector("input#idSIButton9.win-button.button_primary.button.ext-button.primary.ext-primary"));
            nextButton.Click();
            Thread.Sleep(5000);
            var wsbLogin = driver.FindElement(By.CssSelector("input#username"));
            wsbLogin.SendKeys("pzx110299");
            var wsbPass = driver.FindElement(By.CssSelector("input#password"));
            wsbPass.SendKeys("Mzkwcim181099!");
            var login = driver.FindElement(By.Id("submitButton"));
            login.Click();
            Thread.Sleep(3000);
            var btnBlack = driver.FindElement(By.Id("idBtn_Back"));
            btnBlack.Click();
            Thread.Sleep(7000);
            IWebElement newMessageButton = driver.FindElement(By.XPath("//*[text()='Nowa wiadomość']"));
            newMessageButton.Click();
            Thread.Sleep(3000);
            var sendTo = driver.FindElement(By.CssSelector("[aria-label='Do']"));
            sendTo.SendKeys(mail);
            IWebElement subject = driver.FindElement(By.CssSelector("[placeholder='Dodaj temat']"));

            subject.SendKeys(messageSubject);
            var messageTextBox = driver.FindElement(By.CssSelector("div[aria-label='Treść wiadomości, naciśnij klawisze Alt+F10, aby zakończyć']"));
            messageTextBox.SendKeys(messageBody);

            var expandArrow = driver.FindElement(By.CssSelector("[data-icon-name='ChevronDown']"));
            expandArrow.Click();
            
            IWebElement sendButton = driver.FindElement(By.XPath("//button[@aria-label='Wyślij']"));

            sendButton.Click();
            driver.Close();
        }
    }
}
