using System.Collections.Generic;
using System.Linq;

public class Uss
{
    private List<Punkt> keha = new List<Punkt>();
    public Suund PraeguneSuund { get; set; }
    private bool peabKasvama = false;

    public Uss(int algX, int algY, int pikkus)
    {
        PraeguneSuund = Suund.Paremale;

        for (int i = 0; i < pikkus; i++)
        {
            Punkt p = new Punkt(algX - i, algY, '*');
            keha.Add(p);
            p.Joonista();
        }
    }

    public void Liigu()
    {
        Punkt pea = keha.First();
        Punkt uusPea = new Punkt(pea.X, pea.Y, '*');

        switch (PraeguneSuund)
        {
            case Suund.Paremale: uusPea.X++; break;
            case Suund.Vasakule: uusPea.X--; break;
            case Suund.Alla: uusPea.Y++; break;
            case Suund.Üles: uusPea.Y--; break;
        }

        keha.Insert(0, uusPea);
        uusPea.Joonista();

        // Оптимизация роста
        if (peabKasvama)
        {
            peabKasvama = false;
        }
        else
        {
            Punkt saba = keha.Last();
            saba.Kustuta();
            keha.Remove(saba);
        }
    }

    public Punkt HangiPea()
    {
        return keha.First();
    }

    public void Kasva()
    {
        peabKasvama = true;
    }

    //самопересечениe
    public bool HammustasEnnast()
    {
        Punkt pea = HangiPea();
        // Проверяем все элементы тела, кроме самой головы (индекс 0)
        for (int i = 1; i < keha.Count; i++)
        {
            if (pea.X == keha[i].X && pea.Y == keha[i].Y)
            {
                return true;
            }
        }
        return false;
    }
}
