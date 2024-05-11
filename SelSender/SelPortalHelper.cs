using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelSender
{
    internal class SelPortalHelper
    {
        internal static Dictionary<string, string> LogInToSEL()
        {
            Dictionary<string, string> expirationDates = new Dictionary<string, string>();
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://l2.polswim.pl/user");
            var insertEmail = driver.FindElement(By.CssSelector("input#edit-name.form-control.form-text.required"));
            insertEmail.SendKeys("xxx");
            var insertPassword = driver.FindElement(By.CssSelector("input#edit-pass.form-control.form-text.required"));
            insertPassword.SendKeys("xxx");
            var loginButton = driver.FindElement(By.CssSelector("button#edit-submit.btn.btn-default.form-submit"));
            loginButton.Click();
            Thread.Sleep(1000);
            driver.Navigate().GoToUrl("https://l2.polswim.pl/my_club/zawodnicy");
            Thread.Sleep(1000);
            var athletesNames = driver.FindElements(By.CssSelector("td.views-field.views-field-title"));
            var medical = driver.FindElements(By.CssSelector("td.views-field.views-field-field-competitor-medical"));

            for (int i = 0; i < athletesNames.Count; i++)
            {
                var medicalDate = medical[i];
                var medicalDateText = medicalDate.FindElement(By.CssSelector("span.date-display-single")).Text.Trim();
                if (Program.CheckWhetherMedicalsAreExpiredOrWillExpireIn14Days(medicalDateText))
                {
                    var athleteName = athletesNames[i];
                    var athleteLink = athleteName.FindElement(By.CssSelector("a"));
                    if (Program.cieloAthletes.Contains(athleteLink.Text.Trim()))
                    {
                        Console.WriteLine($"Athlete {i + 1}: {athleteLink.Text.Trim()}\tExpiration Date: {medicalDateText}");
                        expirationDates.Add(athleteLink.Text.Trim(), Program.WhenMedicalsAreExpiring(medicalDateText));
                    }
                }
            }

            driver.Close();
            return expirationDates;
        }
    }
}
