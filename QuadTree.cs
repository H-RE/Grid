using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using System.Windows.Markup;

namespace TestQuadTree
{
    class QuadTree
    {
        //Hacer que las celdas sean moviles para que se desplazen rápido
        readonly QuadTree[] child;
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
            PAddCell(point);
            return true;
        }
        private void PAddCell(Point point)
        {
            if (fill == true) return;

            if (Dimensions.IsValid())//Evita que se subdivida cuando alcanzó su longitud minima
            {
                bool BlackTree = true;//Determina si los 4 cuadrantes pueden formar 1 solo nodo
                var Quad = Dimensions.IQuad(point);
                if (child[Quad] == null)
                {
                    var Region = Dimensions.GetQuadrant(Quad);
                    child[Quad] = new QuadTree(Region);
                }
                child[Quad].PAddCell(point);
                for (int i = 0; i < 4; i++)//Verifica si todos los cuadrantes tienen el mismo valor
                {
                    if (i == Quad) continue;
                    if (child[i] == null) { BlackTree = false; break; }
                    BlackTree = (child[i].fill == true) && BlackTree;
                }
                if (BlackTree)//si todos tienen el mismo valor, une todos los elementos
                {
                    fill = true;
                    Array.Clear(child, 0, 4);
                }
            }
            else fill = true;
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
            if (!Dimensions.IsValid())
            {
                points.Add(Dimensions.Center);
                return points;
            }

            if (fill == true)
            {
                var Ytemp = Dimensions.YCellMax();
                var Xtemp = Dimensions.CenterOnCell(X);

                for (int i = 0; i < Dimensions.Units; i++)
                {
                    points.Add(new Point(Xtemp, Ytemp));
                    Ytemp -= Dimensions.Lead;
                }
                return points;
            }

            if (X >= Dimensions.Center.X)//OPTIMIZAR ESTO
            {// X se encuentra en la parte derecha del cuadrante
                GoTo(0);
                GoTo(3);
            }
            else
            {
                GoTo(1);
                GoTo(2);
            }

            void GoTo(int Quad)
            {
                if (child[Quad] != null)
                {
                    var values = child[Quad].FindXCol(X);
                    if (values != null) points.AddRange(values);
                }
            }
            return points;
        } 
        public Point FindMaxInCol(double X)
        {
            if (!Dimensions.XinRange(X)) return null;
            
            if (!Dimensions.IsValid())
            {
                return Dimensions.Center;
            }

            if (fill == true)
            {
                var Ytemp = Dimensions.YCellMax();
                var Xtemp = Dimensions.CenterOnCell(X);
                return new Point(Xtemp,Ytemp);
            }

            if (X >= Dimensions.Center.X)
            {
                if(child[0] != null)
                {
                    return child[0].FindMaxInCol(X);
                }
                else if(child[3] != null)//SE PUEDE OMITIR ESTE IF?
                {
                    return child[3].FindMaxInCol(X);
                }
            }
            else
            {
                if (child[1] != null)
                {
                    return child[1].FindMaxInCol(X);
                }
                else if (child[2] != null)
                {
                    return child[2].FindMaxInCol(X);
                }
            }
            return null;
        }
        public Point FindMinInCol(double X)
        {
            if (!Dimensions.XinRange(X)) return null;

            if (!Dimensions.IsValid())
            {
                return Dimensions.Center;
            }

            if (fill == true)
            {
                var Ytemp = Dimensions.YCellMin();
                var Xtemp = Dimensions.CenterOnCell(X);
                return new Point(Xtemp, Ytemp);
            }

            if (X >= Dimensions.Center.X)
            {
                if (child[3] != null)
                {
                    return child[3].FindMinInCol(X);
                }
                else if (child[0] != null)//SE PUEDE OMITIR ESTE IF?
                {
                    return child[0].FindMinInCol(X);
                }
            }
            else
            {
                if (child[2] != null)
                {
                    return child[2].FindMinInCol(X);
                }
                else if (child[1] != null)
                {
                    return child[1].FindMinInCol(X);
                }
            }
            return null;
        }
    }
}
