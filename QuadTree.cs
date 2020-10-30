using System;
using System.Windows;


namespace TestQuadTree
{
    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point(double X,double Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Point() { X = 0;Y = 0; }
    }
    class QuadTree
    {
        //Hacer que las celdas sean moviles para que se desplazen rápido
        QuadTree []child;
        Square Dimensions;
        private Fill fill;

        public QuadTree(Square Region)
        {
            child = new QuadTree[4];
            Dimensions = Region;
            child[0] = null;
            child[1] = null;
            child[2] = null;
            child[3] = null;
            fill = Fill.White;
        }
        public bool AddCell(Cell cell)
        {
            if (Dimensions.InRange(cell))
            {
                if (fill == Fill.Black) return true;//Se agregó esto
                if(Dimensions.IsValid())//Evita que se subdivida cuando alcanzó su longitud minima
                {
                    bool BlackTree = true;
                    bool Inserted = false;
                    for (int i = 0; i < 4; i++)
                    {
                        if(child[i]==null)
                        {
                            //Es posible determinar si la región quedó más pequeña antes de crear el hijo
                            var Region = Dimensions.GetQuadrant(i);
                            child[i] = new QuadTree(Region);
                        }
                        //Si no se ha insertado entonces entra en AddCell
                        if (!Inserted) { Inserted = child[i].AddCell(cell); }
                        
                        BlackTree = (child[i].fill == Fill.Black) && BlackTree;
                    }
                    
                    if (BlackTree)
                    {
                        fill = Fill.Black;
                        Array.Clear(child, 0, 4);
                    }
                    else { fill = Fill.Gray; }

                    return BlackTree;
                }
                else
                {   
                    fill = Fill.Black;
                    return true;
                }
            }
            return false;
        }
        public bool IsFilled(Cell cell)
        {
            //Se puede optimizar sacando InRange(cell)
            //Si el punto no está en cuadrante, o el cuadrante es blanco
            if ((fill == Fill.White) || !Dimensions.InRange(cell)) return false;
            if (fill == Fill.Black) return true;
            //si está aquí es porque está en el cuadrante y fill=Gray
            return child[Dimensions.IQuad(cell)].IsFilled(cell);
        }
    }
    class Square 
    {
        
        public Point Center { get; set; }
        public double Length { get; private set; }
        public double Lead { get; set; }
        private readonly int Power;

        public bool InRange(Cell cell)
        {
            var disX = Math.Abs(cell.X - Center.X);
            var disY = Math.Abs(cell.Y - Center.Y);
            var HalfLength = Length / 2;
            bool XinRange = disX < HalfLength;
            bool YinRange = disY < HalfLength;
            return XinRange && YinRange;
        }

        public bool IsValid()
        {
            return Power > 0;
        }
        public int IQuad(Cell cell)
        {
            var difX = cell.X - Center.X;
            var difY = cell.Y - Center.Y;
            if(difX>0)
            {
                return difY > 0 ? 0 : 3;
            }
            return difY > 0 ? 1 : 2;
        }
        public Square GetQuadrant(int iQuad)
        {
            var Qcenter = new Point();
            var QLength = Length / 4;
            
            switch (iQuad)//SE MODIFICARON LOS INDICES DEL SWITCH
            {
                case 0:
                    Qcenter.X = Center.X + QLength;
                    Qcenter.Y = Center.Y + QLength;
                    break;
                case 1:
                    Qcenter.X = Center.X - QLength;
                    Qcenter.Y = Center.Y + QLength;
                    break;
                case 2:
                    Qcenter.X = Center.X - QLength;
                    Qcenter.Y = Center.Y - QLength;
                    break;
                case 3:
                    Qcenter.X = Center.X + QLength;
                    Qcenter.Y = Center.Y - QLength;
                    break;
            };
            return new Square(Qcenter, Power - 1, Lead); 
        }
        public double GetHalfLength()
        {
            return Length / 2;
        }

        //Se puede optimizar si ya se pasara Length en potencia de 2
        public Square(Point center, int Power, double Lead)
        {
            this.Lead = Lead;
            Center = center;
            this.Power = Power;
            var Units = 1;//Number of rectangles
            for(int i=0; i<Power; i++)
            {
                Units *= 2;
            }
            Length = Lead * Units;
        }
    }
    enum Fill { Black, Gray, White }
}
