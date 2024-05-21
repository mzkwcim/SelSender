using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using SelSender;
using System.Globalization;
using System.Xml.Linq;
using System.Linq;
using ExcelToMail;
using System.Diagnostics.Metrics;
internal class Program
{
    public static List<string> cieloAthletes = ["Zakens Gabriela", "POLODY ESTERA", "SIEPKA ZOFIA", "JAGŁOWSKI DAWID", "OLEJNICZAK MATEUSZ", 
        "SZMIDCHEN ALAN", "BERG MARIA", "LECHOWICZ MIKOŁAJ", "HEYMANN PATRYK", "SMYKAJ ANTONINA", "BARTOSZEWSKA MARTA", "KRZEŚNIAK JAKUB", "DOPIERAŁA PIOTR", "DROST STANISŁAW", 
        "MADELSKA NATALIA", "MAKOWSKA BLANKA", "Sumisławska Aleksandra", "SEWIŁO MARTYNA", "MROCZEK DOMINIK", "HOROWSKA ZUZANNA", "KUBIAK ZUZANNA", "KUREK ANTONI", "MALICKA MAJA", 
        "Moros Bruno","NOGALSKA IGA"];
    public static List<string> marcinsAthletes = ["CHLIAHTENKO TIMOFEY", "CUDZIŁO MICHAŁ", "CZŁAPA BLANKA", "JUSZKIEWICZ-ZAPATA PIOTR", "KRYŚCIAK WOJCIECH", "KUREK FRANCISZEK",
                                                  "LASKOWSKI FILIP", "LATANOWICZ NATALIA", "MALOVYCHKO FILIP", "MARKIEWICZ IGNACY", "MAZURKIEWICZ MARTYNA", "MIKOŁAJCZAK HELENA",
                                                  "OCZUJDA DANIEL", "PRACHARCZYK MAKS", "RÓG ALEKSANDRA", "SMYKAJ KATARZYNA", "STEFANIAK KAROLINA", "TERTOŃ SZYMON", "Wiśniewski Leszek",
                                                  "WOJCIECHOWSKI ANTONI", "WÓJCIK MARTA"];
    public static List<string> elasAthletes = ["SHVETS MYKHAILO", "Brazhnyk Dmytro", "Jedwabny Maciej", "Zygnarowska Michalina", "Wrzeszczyńska Marta", "NOGAJ ALICJA",
                                               "KRUCKI KAJETAN", "KOLAŃCZYK WIKTORIA", "Nowicka Weronika"];
    private static void Main(string[] args)
    {
        List<Dictionary<string, string>> coaches = SelPortalHelper.LogInToSEL();
        Thread.Sleep(3000);
        int counter = 0;
        foreach(var coach in  coaches)
        {
            Thread.Sleep(3000);
            Console.WriteLine(counter == 0 ? "Maciej i Młody" : counter == 1 ? "Marcin" : "Ela");
            if (counter == 0 && coach.Count > 0)
            {
                LibrusPortalHelper portalHelper = new LibrusPortalHelper();
                portalHelper.LogIn("10620900", "Krakus1998!");
                string subject = "Książeczka zdrowia";
                foreach (var (key, value) in coach)
                {
                    Console.WriteLine(key + " " + value);
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
                portalHelper.Close();
            }
            else if (counter == 1 && coach.Count > 0)
            {
                OutlookProgramHelper outlook = new OutlookProgramHelper();
                string subjectForMarcin = "Badania Sportowe";
                string messageForMarcin = "Trenerze\n\nWysyłam trenerowi listę zawodników Trenera, którzy mają nieważne karty sportowca, lub ich ważność wygasa w ciągu 14 dni:";
                foreach (var (key, value) in coach)
                {
                    messageForMarcin += $"\n{ToTitleString(key)} badania {value}";
                }
                messageForMarcin += "\nPozdrawiam,\nWaldek Krakowiak";
                string receiver = "marcinchojnackitrener@gmail.com";
                OutlookProgramHelper.LogInToOutlook(receiver, subjectForMarcin, messageForMarcin);
            }
            else if (counter == 2 && coach.Count > 0)
            {
                GmailPortalHelper gmail = new GmailPortalHelper();
                string subjectForEla = "Badania Sportowe";
                string messageForEla = "Cześć,\n\nWysyłam Ci listę twoich zawodników, którzy mają nieważne karty sportowca, lub ich ważność wygasa w ciągu 14 dni:";
                foreach (var (key, value) in coach)
                {
                    messageForEla += $"\n{ToTitleString(key)} badania {value}";
                }
                messageForEla += "\nPozdrawiam,\nWaldek Krakowiak";
                string receiver = "krakowiak98@interia.pl";
                OutlookProgramHelper.LogInToOutlook(receiver, subjectForEla, messageForEla);
            }
            
            counter++;
        }

        

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
