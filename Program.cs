using System;

namespace TestQuadTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Square caja = new Square(new Point(0, 0),3,1.2);
            //Hcer privado el miembro fill
            QuadTree Tree = new QuadTree(caja,1.2);//Quirar Lead
            Cell punto = new Cell(-2,-2);
            Tree.AddCell(punto);
            Cell Try = new Cell(-2, -2);
            //Tree.IsFilled(Try);
            Console.WriteLine(Tree.IsFilled(Try));
        }
    }



}
