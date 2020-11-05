using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestQuadTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            
            Square caja = new Square(new Point(0, 0),5,1.2);
            QuadTree Tree = new QuadTree(caja);
            Point[] punto = new Point[7];
            punto[0] = new Point(-1, -1);
            punto[1] = new Point(-2, -1);
            punto[2] = new Point(-1, -2);
            punto[3] = new Point(-2, -2);
            punto[4] = new Point(-0.1, -5);
            punto[5] = new Point(-4, -2);
            punto[6] = new Point(-4.2, -5);

            sw.Start();
            for (int i=0; i<punto.Length;i++)
            {
                Tree.AddCell(punto[i]);
            }
            sw.Stop();
            Console.WriteLine("Tiempo en llenar una celda: " + sw.ElapsedTicks);

            Point Try = new Point(-2, -2);
            sw.Reset();
            sw.Start();
            Tree.IsFilled(Try);
            sw.Stop();
            Console.WriteLine("Tiempo en checar una celda: " + sw.ElapsedTicks);

            var lis = Tree.FindXCol(-1);
            foreach(var p in lis)
            {
                Console.WriteLine(p.X.ToString() + ',' + p.Y.ToString());
            }
            Console.ReadKey();
        }
    }
}
