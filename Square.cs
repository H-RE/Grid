using System;

namespace TestQuadTree
{
    class Square 
    {
        public Point Center { get; set; }
        public double Length { get; private set; }
        public double Lead { get; set; }
        private readonly ushort Power;
        public uint Units { get; }
        public double HalfLength { get; }

        public bool InRange(Point point)
        {
            //Referencia a objeto no establecida como instancia de un objeto
            //if (point == null) return false;
            var disX = Math.Abs(point.X - Center.X);
            var disY = Math.Abs(point.Y - Center.Y);
            bool XinRange = disX <= HalfLength;
            bool YinRange = disY <= HalfLength;
            return XinRange && YinRange;
        }
        public bool XinRange(double X)
        {
            var disX = Math.Abs(X - Center.X);
            return disX <= HalfLength;
        }
        public bool YinRange(double Y)
        {
            var disY = Math.Abs(Y - Center.Y);
            return disY <= HalfLength;
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
            var QLength = Length * 0.25;
            
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
            return new Square(Qcenter, (ushort)(Power - 1), Lead); 
        }

        //Se puede optimizar si ya se pasara Length en potencia de 2
        public Square(Point center, ushort Power, double Lead)
        {
            //if(Power<64)
            this.Lead = Lead;
            Center = center;
            this.Power = Power;
            Units = 1;//Number of rectangles

            for(int i=1; i<Power; i++)
            {
                Units += Units;
            }
            HalfLength = Lead * Units;
            Units += Units;
            Length = HalfLength + HalfLength;
        }
    }
}
