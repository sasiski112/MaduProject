using System;

class Program
{
    static void Main()
    {
        Console.CursorVisible = false;
        Console.Title = "Madu / Ussimäng Pro";

        Mäng mänguhaldur = new Mäng();

        while (true)
        {
            int valik = mänguhaldur.NäitaInteraktiivneMenüü();

            if (valik == 0)
            {
                mänguhaldur.Mängi();
            }
            else if (valik == 1)
            {
                mänguhaldur.Edetabel();
            }
            else if (valik == 2)
            {
                break;
            }
        }
    }
}