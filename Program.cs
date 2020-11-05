using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace TestQuadTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            
            Square caja = new Square(new Point(0, 0),14,1);
            QuadTree Tree = new QuadTree(caja);
            List<Point> punto = new List<Point>();
            for (int i=0; i<1000; i++)
            {
                for(int j=0; j<1000;j++)
                {
                    punto.Add(new Point(i-49.3, j-49.3));
                }
            }
            Console.WriteLine(punto.Count.ToString());
            sw.Start();
            for (int i=0; i<punto.Count;i++)
            {
                Tree.AddCell(punto[i]);
            }
            sw.Stop();
            Console.WriteLine("Tiempo en llenar una celda: " + sw.ElapsedMilliseconds);

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
            Console.WriteLine(lis.Count.ToString());
            Console.ReadKey();
        }
    }
}
