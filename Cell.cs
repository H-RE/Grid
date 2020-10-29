using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace TestQuadTree
{
    class Cell
    {
        public double Y { get; set; }
        public double X { get; set; }
        public List<int> Id { get; }


        public Cell(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
            
            //this.Id.Add(Id);
        }
        public void AddIdentifier(int Id)
        {
            this.Id.Add(Id);
        }
        public void RemoveIdentifier(int Id)
        {
            this.Id.Remove(Id);
        }
    }
}
