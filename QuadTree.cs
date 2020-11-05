using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using System.Windows.Markup;

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
        readonly QuadTree []child;
        readonly Square Dimensions;
        private bool fill;

        public QuadTree(Square Region)
        {
            child = new QuadTree[4];
            Dimensions = Region;
            child[0] = null;
            child[1] = null;
            child[2] = null;
            child[3] = null;
            fill = false;
        }
        public bool AddCell(Point point)
        {
            if (!Dimensions.InRange(point)) return false;
            if (fill == true) return true;

            if(Dimensions.IsValid())//Evita que se subdivida cuando alcanzó su longitud minima
            {
                bool BlackTree = true;//Determina si los 4 cuadrantes pueden formar 1 solo nodo
                var Quad = Dimensions.IQuad(point);
                if (child[Quad] == null)
                {
                    var Region = Dimensions.GetQuadrant(Quad);
                    child[Quad] = new QuadTree(Region);
                }
                child[Quad].AddCell(point);
                for (int i = 0;i < 4; i++)//Verifica si todos los cuadrantes tienen el mismo valor
                {
                    if (i == Quad) continue;
                    if (child[i] == null) { BlackTree = false; break; }
                    BlackTree=(child[i].fill == true) && BlackTree;
                }
                if (BlackTree)//si todos tienen el mismo valor, une todos los elementos
                {
                    fill = true;
                    Array.Clear(child, 0, 4);
                }
                return true;
            }
            fill = true;
            return true;
        }
        public bool IsFilled(Point point)
        {
            //Se puede optimizar sacando InRange(point)
            //Si el punto no está en cuadrante, o el cuadrante es blanco
            //if ((fill == Fill.White) || !Dimensions.InRange(point)) return false;
            if (!Dimensions.InRange(point)) return false;
            if (fill == true) return true;
            var Child = child[Dimensions.IQuad(point)];
            if (Child == null) return false;

            return Child.IsFilled(point);
        }
        public List<Point> FindXCol(double X)
        {
            if (!Dimensions.XinRange(X)) return null;
            var points = new List<Point>();
            //if (fill == true)
            //{
            //    points.Add(Dimensions.Center);
            //    return points;
            //}
            if(!Dimensions.IsValid())
            {
                points.Add(Dimensions.Center);
                return points;
            }

            if(fill==true)
            {
                var Ytemp = Dimensions.Center.Y + Dimensions.Length / 2 - Dimensions.Lead / 2;
                
                var Xdif = X - Dimensions.Center.X;
                var Xuni = (int)((Xdif)/Dimensions.Lead);
                var Xtemp = Dimensions.Center.X + Xuni * Dimensions.Lead;
                if (Xdif > 0)//X está a la derecha del centro
                    Xtemp += Dimensions.Lead / 2;
                else
                    Xtemp -= Dimensions.Lead / 2;

                for (int i=0; i<Dimensions.Units; i++)
                {
                    points.Add(new Point(Xtemp,Ytemp));
                    Ytemp -= Dimensions.Lead;
                }
                return points;
            }

            if (X > Dimensions.Center.X)//OPTIMIZAR ESTO
            {// X se encuentra en la parte derecha del cuadrante
                if (child[0] != null)
                {
                    var values = child[0].FindXCol(X);
                    if (values != null) points.AddRange(values);
                }

                if (child[3] != null)
                {
                    var values = child[3].FindXCol(X);
                    if (values != null) points.AddRange(values);
                }  
            }
            else
            {
                if (child[1] != null)
                {
                    var values = child[1].FindXCol(X);
                    if (values != null) points.AddRange(values);
                }

                if (child[2] != null)
                {
                    var values = child[2].FindXCol(X);
                    if (values != null) points.AddRange(values);
                }
            }
            return points;
        }
    }
    class Square 
    {
        public Point Center { get; set; }
        public double Length { get; private set; }
        public double Lead { get; set; }
        private readonly int Power;
        public int Units { get; }

        public bool InRange(Point point)
        {
            var disX = Math.Abs(point.X - Center.X);
            var disY = Math.Abs(point.Y - Center.Y);
            var HalfLength = Length / 2;//checar si se puede establecer desde el constructor
            bool XinRange = disX <= HalfLength;
            bool YinRange = disY <= HalfLength;
            return XinRange && YinRange;
        }
        public bool XinRange(double X)
        {
            var disX = Math.Abs(X - Center.X);
            return disX <= Length / 2;
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
            Units = 1;//Number of rectangles
            for(int i=0; i<Power; i++)
            {
                Units *= 2;
            }
            Length = Lead * Units;
        }
    }
}
