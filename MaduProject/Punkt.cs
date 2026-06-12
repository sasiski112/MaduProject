using System;

public enum Suund
{
    Üles,
    Alla,
    Vasakule,
    Paremale
}

public class Punkt
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Sümbol { get; set; }

    public Punkt(int x, int y, char sümbol)
    {
        X = x;
        Y = y;
        Sümbol = sümbol;
    }

    public void Joonista()
    {
        // Проверка границ, чтобы избежать ошибок выхода за пределы буфера консоли
        if (X >= 0 && X < Console.BufferWidth && Y >= 0 && Y < Console.BufferHeight)
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(Sümbol);
        }
    }

    public void Kustuta()
    {
        if (X >= 0 && X < Console.BufferWidth && Y >= 0 && Y < Console.BufferHeight)
        {
            Console.SetCursorPosition(X, Y);
            Console.Write(' ');
        }
    }
}
