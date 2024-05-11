using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace ExcelToMail
{
    internal class LibrusPortalHelper
    {
        private IWebDriver driver;
        public LibrusPortalHelper()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            driver = new ChromeDriver(options);
        }
        public void LogIn(string username, string password)
        {
            driver.Navigate().GoToUrl("https://portal.librus.pl/szkola/synergia/loguj");
            Thread.Sleep(500);
            try
            {
                driver.FindElement(By.CssSelector("button.modal-button__primary[data-modal-submit-all='']")).Click();
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Cookie files Acceptance button hasn't been found");
            }
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("a.btn-synergia-top.btn-navbar.d-none.d-lg-block")).Click();
            Thread.Sleep(500);
            driver.SwitchTo().Frame(driver.FindElement(By.Id("caLoginIframe")));
            driver.FindElement(By.CssSelector("input#Login.form-control")).SendKeys(username);
            driver.FindElement(By.Id("Pass")).SendKeys(password);
            driver.FindElement(By.Id("LoginBtn")).Click();
            Thread.Sleep(500);
            driver.SwitchTo().DefaultContent();
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id=\"centrumPowiadomien\"]/div[2]/div[1]/span[2]")).Click();
        }

        public void SendMessage(string recipient, string subject, string message)
        {
            Thread.Sleep(500);
            driver.Navigate().GoToUrl(driver.FindElement(By.CssSelector("a#icon-wiadomosci")).GetAttribute("href"));
            Thread.Sleep(500);
            driver.Navigate().GoToUrl(driver.FindElement(By.CssSelector("a#wiadomosci-napisz.button.left.blue")).GetAttribute("href"));
            driver.FindElement(By.Id("radio_rodzic_klasami")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//*[@id=\"adresaci\"]/table/tbody/tr[45]/td[3]/input")).Click();
            driver.FindElement(By.XPath("//*[@id=\"adresaci\"]/table/tbody/tr[49]/td[3]/input")).Click();
            Thread.Sleep(500);
            IList<IWebElement> labelElements = driver.FindElements(By.XPath("//table//label"));
            foreach (IWebElement labelElement in labelElements)
            {
                if (labelElement.Text.Contains(recipient))
                {
                    string forValue = labelElement.GetAttribute("for");
                    IWebElement checkboxElement = driver.FindElement(By.Id(forValue));
                    if (checkboxElement != null && checkboxElement.Enabled)
                    {
                        checkboxElement.Click();
                        break;
                    }
                }
            }
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("input#temat.stretch")).SendKeys(subject);
            driver.FindElement(By.CssSelector("textarea#tresc_wiadomosci.stretch")).SendKeys(message);
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("input#sendButton.medium.ui-button.ui-widget.ui-state-default.ui-corner-all")).Click();
        }
        public void Close()
        {
            driver.Quit();
        }
    }
}