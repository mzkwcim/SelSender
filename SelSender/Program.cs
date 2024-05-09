using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using SelSender;
using System.Globalization;
using System.Xml.Linq;
internal class Program
{
    private static void Main(string[] args)
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
        wsbLogin.SendKeys("xyz");
        var wsbPass = driver.FindElement(By.CssSelector("input#password"));
        wsbPass.SendKeys("xyz");
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
        sendTo.SendKeys("xyz");
        IWebElement subject = driver.FindElement(By.CssSelector("[placeholder='Dodaj temat']"));

        // Kliknij na ten element
        subject.SendKeys("Test wysyłania wiadomości przez outlooka");
        var messageTextBox = driver.FindElement(By.CssSelector("div[aria-label='Treść wiadomości, naciśnij klawisze Alt+F10, aby zakończyć']"));

        messageTextBox.SendKeys("No to siema\nmam nadzieję że wszystko zadziałało");
        IWebElement sendButton = driver.FindElement(By.XPath("//button[@aria-label='Wyślij']"));

        // Kliknij na ten element
        sendButton.Click();


    }

    internal static bool Subtractor(string dateString)
    {
        DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        DateTime newDate = date.AddDays(-14);
        DateTime today = DateTime.Today;
        return DateTime.Compare(newDate, today) < 0;
    }

    internal static void LogInToSEL()
    {
        IWebDriver driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://l2.polswim.pl/user");
        var insertEmail = driver.FindElement(By.CssSelector("input#edit-name.form-control.form-text.required"));
        insertEmail.SendKeys("xyz");
        var insertPassword = driver.FindElement(By.CssSelector("input#edit-pass.form-control.form-text.required"));
        insertPassword.SendKeys("xyz");
        var loginButton = driver.FindElement(By.CssSelector("button#edit-submit.btn.btn-default.form-submit"));
        loginButton.Click();
        Thread.Sleep(1000);
        driver.Navigate().GoToUrl("https://l2.polswim.pl/my_club/zawodnicy");
        var athletesNames = driver.FindElements(By.CssSelector("td.views-field.views-field-title"));
        var medical = driver.FindElements(By.CssSelector("td.views-field.views-field-field-competitor-medical"));

        for (int i = 0; i < athletesNames.Count; i++)
        {
            var medicalDate = medical[i];
            var medicalDateText = medicalDate.FindElement(By.CssSelector("span.date-display-single")).Text.Trim();
            if (Subtractor(medicalDateText))
            {
                var athleteName = athletesNames[i];
                var athleteLink = athleteName.FindElement(By.CssSelector("a"));
                Console.WriteLine($"Athlete {i + 1}: {athleteLink.Text.Trim()}\tExpiration Date: {medicalDateText}");
            }
        }

        driver.Close();
    }
}