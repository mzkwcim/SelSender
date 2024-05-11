using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using SelSender;
using System.Globalization;
using System.Xml.Linq;
using System.Linq;
using ExcelToMail;
internal class Program
{
    public static List<string> cieloAthletes = ["Zakens Gabriela", "POLODY ESTERA", "SIEPKA ZOFIA", "JAGŁOWSKI DAWID", "OLEJNICZAK MATEUSZ", 
        "SZMIDCHEN ALAN", "BERG MARIA", "LECHOWICZ MIKOŁAJ", "HEYMANN PATRYK", "SMYKAJ ANTONINA", "BARTOSZEWSKA MARTA", "KRZEŚNIAK JAKUB", "DOPIERAŁA PIOTR", "DROST STANISŁAW", 
        "MADELSKA NATALIA", "MAKOWSKA BLANKA", "Sumisławska Aleksandra", "SEWIŁO MARTYNA", "MROCZEK DOMINIK", "HOROWSKA ZUZANNA", "KUBIAK ZUZANNA", "KUREK ANTONI", "MALICKA MAJA", 
        "Moros Bruno","NOGALSKA IGA"];
    public static List<string> MarcinsAthletes = [""];
    private static void Main(string[] args)
    {
        Dictionary<string, string> expirationDates = SelPortalHelper.LogInToSEL();
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

    internal static bool CheckWhetherMedicalsAreExpiredOrWillExpireIn14Days(string dateString)
    {
        //this function is made to chceck whether the medicals of athlete are up to date
        DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        DateTime newDate = date.AddDays(-14);
        DateTime today = DateTime.Today;
        return DateTime.Compare(newDate, today) < 0;
    }
    internal static string WhenMedicalsAreExpiring(string dateString)
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
