using System;
using System.Diagnostics;

namespace TestQuadTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            
            Square caja = new Square(new Point(0, 0),3,1.2);
            QuadTree Tree = new QuadTree(caja);
            Cell[] punto = new Cell[4];
            punto[0] = new Cell(-1, -1);
            punto[1] = new Cell(-2, -1);
            punto[2] = new Cell(-1, -2);
            punto[3] = new Cell(-1, -2);

            sw.Start();
            for (int i=0; i<punto.Length;i++)
            {
                Tree.AddCell(punto[i]);
            }
            sw.Stop();
            Console.WriteLine("Tiempo en llenar una celda: " + sw.ElapsedTicks);

            Cell Try = new Cell(-2, -2);
            //Console.WriteLine(Tree.IsFilled(Try));

           
            Tree.IsFilled(Try);
            
        }
    }
}
