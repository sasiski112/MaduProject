using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    static int laius = 50;
    static int kõrgus = 20;
    static int skoor = 0;
    static int mänguKiirus = 120;
    const string FailiNimi = "leaders.txt"; // Файл для хранения рекордов

    static void Main()
    {
        Console.CursorVisible = false;
        Console.Title = "Madu / Ussimäng Pro";

        while (true)
        {
            // Главное интерактивное меню
            int valik = NäitaInteraktiivneMenüü();

            if (valik == 0) // Старт игры
            {
                Mängi();
            }
            else if (valik == 1) // Таблица лидеров
            {
                NäitaEdetabel();
            }
            else if (valik == 2) // Выход
            {
                break;
            }
        }
    }

    // ИНТЕРАКТИВНОЕ МЕНЮ НА СТРЕЛОЧКАХ
    static int NäitaInteraktiivneMenüü()
    {
        int valitudIndeks = 0;
        string[] menüüPunktid = { "1. Alusta mängu (Старт)", "2. Edetabel (Таблица лидеров)", "3. Välju (Выход)" };

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================");
            Console.WriteLine("         MÄNG \"USS / MADU\" PRO          ");
            Console.WriteLine("=========================================");
            Console.ResetColor();
            Console.WriteLine("  Kasuta nooli [↑ / ↓] ja vajuta [Enter]:\n");

            for (int i = 0; i < menüüPunktid.Length; i++)
            {
                if (i == valitudIndeks)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"  > {menüüPunktid[i]} <  ");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"    {menüüPunktid[i]}    ");
                }
            }

            ConsoleKeyInfo klahv = Console.ReadKey(true);
            if (klahv.Key == ConsoleKey.UpArrow)
            {
                valitudIndeks = (valitudIndeks == 0) ? menüüPunktid.Length - 1 : valitudIndeks - 1;
            }
            else if (klahv.Key == ConsoleKey.DownArrow)
            {
                valitudIndeks = (valitudIndeks == menüüPunktid.Length - 1) ? 0 : valitudIndeks + 1;
            }
            else if (klahv.Key == ConsoleKey.Enter)
            {
                return valitudIndeks;
            }
        }
    }

    // ВЫБОР СЛОЖНОСТИ НА СТРЕЛОЧКАХ
    static void ValiRaskusaste()
    {
        int valitudIndeks = 1; // По умолчанию "Normaalne"
        string[] raskusastmed = { "Lihtne (Aeglane)", "Normaalne", "Raske (Kiire)" };

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================");
            Console.WriteLine("            VALI RASKUSASTE              ");
            Console.WriteLine("=========================================");
            Console.ResetColor();
            Console.WriteLine("  Vali kiirus [↑ / ↓] ja vajuta [Enter]:\n");

            for (int i = 0; i < raskusastmed.Length; i++)
            {
                if (i == valitudIndeks)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"  > {raskusastmed[i]} <  ");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"    {raskusastmed[i]}    ");
                }
            }

            ConsoleKeyInfo klahv = Console.ReadKey(true);
            if (klahv.Key == ConsoleKey.UpArrow)
            {
                valitudIndeks = (valitudIndeks == 0) ? raskusastmed.Length - 1 : valitudIndeks - 1;
            }
            else if (klahv.Key == ConsoleKey.DownArrow)
            {
                valitudIndeks = (valitudIndeks == raskusastmed.Length - 1) ? 0 : valitudIndeks + 1;
            }
            else if (klahv.Key == ConsoleKey.Enter)
            {
                switch (valitudIndeks)
                {
                    case 0: mänguKiirus = 200; break;
                    case 1: mänguKiirus = 120; break;
                    case 2: mänguKiirus = 50; break;
                }
                return;
            }
        }
    }

    // ОСНОВНОЙ ИГРОВОЙ ПРОЦЕСС
    static void Mängi()
    {
        ValiRaskusaste(); // Сначала выбираем сложность

        Console.Clear();
        skoor = 0;
        JoonistaRaam();

        Uss uss = new Uss(10, 10, 3);
        Toit toit = new Toit(laius, kõrgus);

        while (true)
        {
            Console.SetCursorPosition(0, kõrgus);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Skoor: {skoor}  ");
            Console.ResetColor();

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo klahv = Console.ReadKey(true);
                if (klahv.Key == ConsoleKey.UpArrow && uss.PraeguneSuund != Suund.Alla)
                    uss.PraeguneSuund = Suund.Üles;
                else if (klahv.Key == ConsoleKey.DownArrow && uss.PraeguneSuund != Suund.Üles)
                    uss.PraeguneSuund = Suund.Alla;
                else if (klahv.Key == ConsoleKey.LeftArrow && uss.PraeguneSuund != Suund.Paremale)
                    uss.PraeguneSuund = Suund.Vasakule;
                else if (klahv.Key == ConsoleKey.RightArrow && uss.PraeguneSuund != Suund.Vasakule)
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

            // Врезался в стену или себя — конец игры
            if (pea.X <= 0 || pea.X >= laius - 1 || pea.Y <= 0 || pea.Y >= kõrgus - 1 || uss.HammustasEnnast())
            {
                MängLäbi();
                break;
            }

            Thread.Sleep(mänguKiirus);
        }
    }

    static void JoonistaRaam()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        for (int i = 0; i < laius; i++)
        {
            Console.SetCursorPosition(i, 0); Console.Write("#");
            Console.SetCursorPosition(i, kõrgus - 1); Console.Write("#");
        }
        for (int i = 0; i < kõrgus; i++)
        {
            Console.SetCursorPosition(0, i); Console.Write("#");
            Console.SetCursorPosition(laius - 1, i); Console.Write("#");
        }
        Console.ResetColor();
    }

    // ЭКРАН СМЕРТИ И ЗАПИСЬ РЕКОРДА
    static void MängLäbi()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("=========================================");
        Console.WriteLine("               MÄNG LÄBI!                ");
        Console.WriteLine("=========================================");
        Console.ResetColor();
        Console.WriteLine($"Sinu lõplik skoor: {skoor}\n");

        Console.Write("Sisesta oma nimi edetabeli jaoks: ");
        string nimi = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(nimi)) nimi = "Anonüümne";

        SalvestaSkoor(nimi, skoor);
    }

    // РАБОТА С ФАЙЛОМ РЕКОРДОВ
    static void SalvestaSkoor(string nimi, int tulemus)
    {
        try
        {
            // Формат записи: Имя;Очки
            using (StreamWriter sw = File.AppendText(FailiNimi))
            {
                sw.WriteLine($"{nimi};{tulemus}");
            }
        }
        catch { }
    }

    static void NäitaEdetabel()
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

            // Сортируем по убыванию очков и берем ТОП-5
            var top5 = edetabel.OrderByDescending(x => x.Value).Take(5).ToList();

            for (int i = 0; i < top5.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {top5[i].Key} — {top5[i].Value} punkti");
            }
        }
        else
        {
            Console.WriteLine("Edetabel on veel tühi (Таблица пока пуста).");
        }

        Console.WriteLine("\nTagasi menüüsse pääsemiseks vajuta mis tahes klahvi...");
        Console.ReadKey(true);
    }
}
