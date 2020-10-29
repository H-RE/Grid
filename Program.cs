using System;
using System.Diagnostics;

namespace TestQuadTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Square caja = new Square(new Point(0, 0),3,1.2);
            QuadTree Tree = new QuadTree(caja);
            Cell[] punto = new Cell[4];
            punto[0] = new Cell(-1, -1);
            punto[1] = new Cell(-2, -1);
            punto[2] = new Cell(-2, -2);
            punto[3] = new Cell(-1, -2);
            Console.WriteLine("====== PROCESO DE INSERCION =========");
            for (int i=0; i<punto.Length;i++)
            {
                Console.WriteLine("\n\n Add cell: "+punto[i].X+"\t"+ punto[i].Y);
                Tree.AddCell(punto[i]);
            }
            
            Cell Try = new Cell(-2, -2);
            Console.WriteLine("\n\n====== PROCESO DE BUSQUEDA =========");
            var sw = new Stopwatch();
            sw.Start();
            Console.WriteLine(Tree.IsFilled(Try));
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            
        }
    }



}
