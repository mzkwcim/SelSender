using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace SelSender
{
    internal class SelPortalHelper
    {
        internal static List<Dictionary<string, string>> LogInToSEL()
        {
            List<Dictionary<string, string>> coaches = new List<Dictionary<string, string>>();
            Dictionary<string, string> cieloExpirationDates = new Dictionary<string, string>();
            Dictionary<string, string> marcinsExpirationDates = new Dictionary<string, string>();
            Dictionary<string, string> elasExpirationDates = new Dictionary<string, string>();
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://l2.polswim.pl/user");
            var insertEmail = driver.FindElement(By.CssSelector("input#edit-name.form-control.form-text.required"));
            insertEmail.SendKeys("slawek.plonka@onet.pl");
            var insertPassword = driver.FindElement(By.CssSelector("input#edit-pass.form-control.form-text.required"));
            insertPassword.SendKeys("Zosia2004");
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
                        cieloExpirationDates.Add(athleteLink.Text.Trim(), Program.WhenMedicalsAreExpiring(medicalDateText));
                    }
                    else if (Program.marcinsAthletes.Contains(athleteLink.Text.Trim()))
                    {
                        marcinsExpirationDates.Add(athleteLink.Text.Trim(), Program.WhenMedicalsAreExpiring(medicalDateText));
                    }
                    else if (Program.elasAthletes.Contains(athleteLink.Text.Trim()))
                    {
                        elasExpirationDates.Add(athleteLink.Text.Trim(), Program.WhenMedicalsAreExpiring(medicalDateText));
                    }
                }
            }
            coaches.Add(cieloExpirationDates);
            coaches.Add(marcinsExpirationDates);
            coaches.Add(elasExpirationDates);

            driver.Close();
            return coaches;
        }
    }
}
