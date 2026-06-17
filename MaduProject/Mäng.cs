using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

public class Mäng
{
    private int laius = 50;
    private int kõrgus = 20;
    private int skoor = 0;
    private int mänguKiirus = 120;
    private const string FailiNimi = "leaders.txt";

    public int NäitaInteraktiivneMenüü()
    {
        int valitudIndeks = 0;
        string[] menüüPunktid = { "1. Alusta mängu", "2. Edetabel", "3. Välju" };

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================");
            Console.WriteLine("             USSAMÄNG / MADU             ");
            Console.WriteLine("=========================================");
            Console.ResetColor();

            for (int i = 0; i < menüüPunktid.Length; i++)
            {
                if (i == valitudIndeks)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($" > {menüüPunktid[i]} ");
                }
                else
                {
                    Console.WriteLine($"   {menüüPunktid[i]}");
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo võti = Console.ReadKey(true);

            if (võti.Key == ConsoleKey.UpArrow)
            {
                valitudIndeks = (valitudIndeks == 0) ? menüüPunktid.Length - 1 : valitudIndeks - 1;
            }
            else if (võti.Key == ConsoleKey.DownArrow)
            {
                valitudIndeks = (valitudIndeks == menüüPunktid.Length - 1) ? 0 : valitudIndeks + 1;
            }
            else if (võti.Key == ConsoleKey.Enter)
            {
                return valitudIndeks;
            }
        }
    }

    public void ValiRaskusaste()
    {
        int valitudIndeks = 0;
        string[] raskusastmed = { "Lihtne", "Keskmine", "Raske" };
        int[] kiirused = { 180, 110, 60 };

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Vali raskusaste:");
            Console.ResetColor();

            for (int i = 0; i < raskusastmed.Length; i++)
            {
                if (i == valitudIndeks)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($" > {raskusastmed[i]} ");
                }
                else
                {
                    Console.WriteLine($"   {raskusastmed[i]}");
                }
                Console.ResetColor();
            }

            ConsoleKeyInfo võti = Console.ReadKey(true);

            if (võti.Key == ConsoleKey.UpArrow)
            {
                valitudIndeks = (valitudIndeks == 0) ? raskusastmed.Length - 1 : valitudIndeks - 1;
            }
            else if (võti.Key == ConsoleKey.DownArrow)
            {
                valitudIndeks = (valitudIndeks == raskusastmed.Length - 1) ? 0 : valitudIndeks + 1;
            }
            else if (võti.Key == ConsoleKey.Enter)
            {
                mänguKiirus = kiirused[valitudIndeks];
                break;
            }
        }
    }

    public void Mängi()
    {
        Console.Clear();
        skoor = 0;

        ValiRaskusaste();

        Console.Clear();
        JoonistaRaam();

        Uss uss = new Uss(laius / 2, kõrgus / 2, 3);
        Toit toit = new Toit(laius, kõrgus);

        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo võti = Console.ReadKey(true);

                if (võti.Key == ConsoleKey.UpArrow && uss.PraeguneSuund != Suund.Alla)
                    uss.PraeguneSuund = Suund.Üles;
                else if (võti.Key == ConsoleKey.DownArrow && uss.PraeguneSuund != Suund.Üles)
                    uss.PraeguneSuund = Suund.Alla;
                else if (võti.Key == ConsoleKey.LeftArrow && uss.PraeguneSuund != Suund.Paremale)
                    uss.PraeguneSuund = Suund.Vasakule;
                else if (võti.Key == ConsoleKey.RightArrow && uss.PraeguneSuund != Suund.Vasakule)
                    uss.PraeguneSuund = Suund.Paremale;
            }

            uss.Liigu();

            Punkt pea = uss.HangiPea();
            if (pea.X == toit.Asukoht.X && pea.Y == toit.Asukoht.Y)
            {
                uss.Kasva();
                skoor += 10;
                toit.LooUusToit();
            }

            if (pea.X <= 0 || pea.X >= laius - 1 || pea.Y <= 0 || pea.Y >= kõrgus - 1 || uss.HammustasEnnast())
            {
                MängLäbi();
                break;
            }

            Console.SetCursorPosition(0, kõrgus + 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Skoor: {skoor}");
            Console.ResetColor();

            Thread.Sleep(mänguKiirus);
        }
    }

    public void JoonistaRaam()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        for (int x = 0; x < laius; x++)
        {
            new Punkt(x, 0, '#').Joonista();
            new Punkt(x, kõrgus - 1, '#').Joonista();
        }
        for (int y = 0; y < kõrgus; y++)
        {
            new Punkt(0, y, '#').Joonista();
            new Punkt(laius - 1, y, '#').Joonista();
        }
        Console.ResetColor();
    }

    public void MängLäbi()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("=========================================");
        Console.WriteLine("               MÄNG LÄBI!                ");
        Console.WriteLine($"               Skoor: {skoor}            ");
        Console.WriteLine("=========================================");
        Console.ResetColor();

        Console.Write("Sisesta oma nimi: ");
        string nimi = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(nimi)) nimi = "Anonüümne";

        SalvestaSkoor(nimi, skoor);
    }

    public void SalvestaSkoor(string nimi, int tulemus)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(FailiNimi, true))
            {
                sw.WriteLine($"{nimi};{tulemus}");
            }
        }
        catch { }
    }

    public void Edetabel()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("=========================================");
        Console.WriteLine("            TOP 5 MÄNGIJAT               ");
        Console.WriteLine("=========================================");
        Console.ResetColor();

        if (File.Exists(FailiNimi))
        {
            var ridad = File.ReadAllLines(FailiNimi);
            var edetabel = new List<KeyValuePair<string, int>>();

            foreach (var rida in ridad)
            {
                var osad = rida.Split(';');
                if (osad.Length == 2 && int.TryParse(osad[1], out int ojad))
                {
                    edetabel.Add(new KeyValuePair<string, int>(osad[0], ojad));
                }
            }

            var top5 = edetabel.OrderByDescending(x => x.Value).Take(5).ToList();

            for (int i = 0; i < top5.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {top5[i].Key} — {top5[i].Value} punkti");
            }
        }
        else
        {
            Console.WriteLine("Edetabel on veel tühi.");
        }

        Console.WriteLine("\nTagasi menüüsse...");
        Console.ReadKey(true);
    }
}