using System;

public class Toit
{
    private Random rnd = new Random();
    private int ekraaniLaius;
    private int ekraaniKõrgus;
    public Punkt Asukoht { get; private set; }

    public Toit(int laius, int kõrgus)
    {
        ekraaniLaius = laius;
        ekraaniKõrgus = kõrgus;
        LooUusToit();
    }

    public void LooUusToit()
    {
        int x = rnd.Next(1, ekraaniLaius - 1);
        int y = rnd.Next(1, ekraaniKõrgus - 1);
        Asukoht = new Punkt(x, y, '@');
        Asukoht.Joonista();
    }
}
