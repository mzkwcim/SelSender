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
        LibrusPortalHelper portalHelper = new LibrusPortalHelper();
        portalHelper.LogIn("xxx", "xxx");
        string subject = "Książeczka zdrowia";
        foreach (var (key, value) in expirationDates)
        {
            string athletename = ToTitleString(key);
            string message = $"Dzień Dobry\n\n" +
                             $"przypominamy że badania {NameDecantation(athletename)} {value}\n" +
                             $"prosilibyśmy o ich jak najszybsze wykonanie i dostarczenie nam zdjęć ważnej książeczki sportowej\n" +
                             $"Z poważaniem,\n" +
                             $"trenerzy\n" +
                             $"Maciej Waliński\n" +
                             $"Waldek Krakowiak\n";
            portalHelper.SendMessage(athletename, subject, message);
        }
    }

    internal static bool CheckWhetherMedicalsAreExpiredOrWillExpireIn14Days(string dateString)
    {
        DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        DateTime newDate = date.AddDays(-14);
        DateTime today = DateTime.Today;
        return DateTime.Compare(newDate, today) < 0;
    }
    internal static string WhenMedicalsAreExpiring(string dateString)
    {
        DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        DateTime today = DateTime.Today;
        var comparation = DateTime.Compare(date, today);
        string output = date.ToString().Split(" ")[0];
        return comparation < 0 ? $"wygasły {output}" : $"wygasają {output}";
    }
    public static string ToTitleString(string fullname)
    {
        string[] words = fullname.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < words.Length; i++)
        {
            words[i] = char.ToUpper(words[i][0]) + words[i][1..].ToLower();
        }
        return string.Join(" ", words).Replace(",", "");
    }
    internal static string NameDecantation(string fullname)
    {
        var firstname = fullname.Split(" ")[1];
        Dictionary<string, string> firstToSecondDecantation = new Dictionary<string, string>{
            {"Marta", "Marty" },
            {"Maria", "Marii" },
            {"Piotr", "Piotra" },
            {"Stanisław", "Stasia" },
            {"Patryk", "Patryka" },
            {"Dawid", "Dawida" },
            {"Jakub", "Kuby" },
            {"Mikołaj", "Mikołaja" },
            {"Natalia", "Natalii" },
            {"Blanka", "Blanki" },
            {"Bruno", "Bruna" },
            {"Mateusz", "Mateusza" },
            {"Estera", "Estery" },
            {"Zofia", "Zosi" },
            {"Antonina", "Tosi" },
            {"Alan", "Alana" },
            {"Gabriela", "Gabrysi" },
            {"Zuzanna", "Zuzy" },
            {"Antoni", "Antka" },
            {"Dominik", "Dominika" },
            {"Iga", "Igi" },
            {"Martyna", "Martyny" },
            {"Maja", "Mai" },
            {"Aleksandra", "Oli" }
        };
        return firstToSecondDecantation[firstname];
    }
}