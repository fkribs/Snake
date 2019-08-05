using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Snake
{
    public class Player
    {
        public List<Point> Coordinates { get; set; }
        public int Length { get; set; }
        public Point Front { get => Coordinates[Coordinates.Count -1]; }
        public Player(Point coordinates)
        {
            Coordinates = new List<Point>() { coordinates };
            Length = 1;
        }

        public void Move(Point newLocation)
        {
            for (var i = 0; i<Coordinates.Count-1; i++)
            {
                Coordinates[i] = Coordinates[i+ 1];
            }
            Coordinates[Coordinates.Count-1]=newLocation;
            //Coordinates.RemoveAll(0);
        }

        public void Grow()
        {
            Coordinates.Add(new Point(Front.X,Front.Y));
        }
    }
    enum PlayerDirection { Up, Down, Left, Right };
}
