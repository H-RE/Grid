using System;

namespace TestQuadTree
{
    class Square 
    {
        public Point Center { get; set; }
        public double Length { get; private set; }
        public double Lead { get; set; }
        private readonly int Power;
        public int Units { get; }
        public double HalfLength { get; }

        public bool InRange(Point point)
        {
            //Referencia a objeto no establecida como instancia de un objeto
            //if (point == null) return false;
            var disX = Math.Abs(point.X - Center.X);
            var disY = Math.Abs(point.Y - Center.Y);
            //var HalfLength = Length / 2;//checar si se puede establecer desde el constructor
            bool XinRange = disX <= HalfLength;
            bool YinRange = disY <= HalfLength;
            return XinRange && YinRange;
        }
        public bool XinRange(double X)
        {
            var disX = Math.Abs(X - Center.X);
            return disX <= HalfLength;
        }

        public bool IsValid()
        {
            return Power > 0;
        }
        public int IQuad(Point point)//Se optimizó
        {
            if( point.X > Center.X)                  
            {
                return point.Y > Center.Y ? 0 : 3;
            }
            return point.Y > Center.Y ? 1 : 2;
        }
        public Square GetQuadrant(int iQuad)
        {
            var Qcenter = new Point();
            var QLength = Length / 4;
            
            switch (iQuad)
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

        //Se puede optimizar si ya se pasara Length en potencia de 2
        public Square(Point center, int Power, double Lead)
        {
            this.Lead = Lead;
            Center = center;
            this.Power = Power;
            Units = 1;//Number of rectangles
            
            for(int i=1; i<Power; i++)
            {
                Units *= 2;
            }
            HalfLength = Lead * Units;
            Units *= 2;
            Length = HalfLength + HalfLength;
        }
    }
}
