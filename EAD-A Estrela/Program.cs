using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        char[,] mapa =
        {
            { 'S','0','X','0','0' },
            { '0','X','0','0','0' },
            { '0','0','0','X','0' },
            { '0','0','0','X','F' }
        };

        List<No> aberta = new List<No>();
        List<No> fechada = new List<No>();

        No inicio = null, fim = null;

        for (int i = 0; i < mapa.GetLength(0); i++)
            for (int j = 0; j < mapa.GetLength(1); j++)
            {
                if (mapa[i, j] == 'S') inicio = new No(i, j);
                if (mapa[i, j] == 'F') fim = new No(i, j);
            }

        aberta.Add(inicio);

        while (aberta.Count > 0)
        {
            No atual = aberta.OrderBy(n => n.F).First();

            if (atual.X == fim.X && atual.Y == fim.Y)
            {
                while (atual.Pai != null)
                {
                    if (mapa[atual.X, atual.Y] == '0')
                        mapa[atual.X, atual.Y] = '-';
                    atual = atual.Pai;
                }
                break;
            }

            aberta.Remove(atual);
            fechada.Add(atual);

            int[,] d = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };

            for (int i = 0; i < 4; i++)
            {
                int x = atual.X + d[i, 0];
                int y = atual.Y + d[i, 1];

                if (x < 0 || y < 0 || x >= mapa.GetLength(0) || y >= mapa.GetLength(1))
                    continue;

                if (mapa[x, y] == 'X' || fechada.Any(n => n.X == x && n.Y == y))
                    continue;

                int g = atual.G + 1;
                No viz = aberta.FirstOrDefault(n => n.X == x && n.Y == y);

                if (viz == null)
                {
                    viz = new No(x, y);
                    viz.G = g;
                    viz.H = Math.Abs(x - fim.X) + Math.Abs(y - fim.Y);
                    viz.F = viz.G + viz.H;
                    viz.Pai = atual;
                    aberta.Add(viz);
                }
            }
        }

        for (int i = 0; i < mapa.GetLength(0); i++)
        {
            for (int j = 0; j < mapa.GetLength(1); j++)
                Console.Write(mapa[i, j] + " ");
            Console.WriteLine();
        }

        Console.ReadKey();
    }
}

public class No
{
    public int X, Y, G, H, F;
    public No Pai;

    public No(int x, int y)
    {
        X = x;
        Y = y;
    }
}

