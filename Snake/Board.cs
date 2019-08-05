using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Snake
{
    public class Board
    {
        private const char FloorChar = '-';
        private const char CeilingChar = '_';
        private const char HorizontalWallChar = '|';
        private const char PlayerChar = 'X';
        private const char FoodChar = '0';
        public int BoardHeight { get; }
        public int BoardWidth { get; }
        private Player ThePlayer { get; set; }
        private Point Food { get; set; }
        public int Score { get; set; }
        public List<string> Contents { get; set; }
        public DateTime Start { get; set; }
        public Board(int boardHeight, int boardWidth, Player player)
        {
            Start = DateTime.Now;
            BoardHeight = boardHeight;
            BoardWidth = boardWidth;
            ThePlayer = player;
            SpawnFood();
            Contents = new List<string>();
            Initialize();
        }
        private void Initialize()
        {
            CreateBorders();
            DrawFood();
            DrawPlayer();
        }

        public void Refresh()
        {
            Contents = new List<string>();
            Initialize();
        }
        private void DrawPlayer()
        {
            foreach (var coord in ThePlayer.Coordinates)
            {
                var y = coord.Y;
                var x = coord.X;
                Contents[y] = Contents[y].Remove(x, 1).Insert(x, PlayerChar.ToString());
            }
        }

        private void DrawFood()
        {
            var y = Food.Y;
            var x = Food.X;
            Contents[y] = Contents[y].Remove(x, 1).Insert(x, FoodChar.ToString());
        }

        private void CreateBorders()
        {
            var top = new String(CeilingChar, BoardWidth);
            Contents.Add(top);
            var middle = new String(' ', BoardWidth - 2)
                .Insert(0, HorizontalWallChar.ToString()) + HorizontalWallChar;
            for (var i = 0; i < BoardHeight - 2; i++)
            {
                Contents.Add(middle);
            }
            var bot = new String(FloorChar, BoardWidth);
            Contents.Add(bot);

        }
        public void MovePlayer(Point point)
        {
            if ((point.X < BoardWidth - 1 && point.X > 0) && (point.Y < BoardHeight - 1 && point.Y > 0))
            {
                ThePlayer.Move(point);
            }
            else
            {
                EndGame("You collided with the wall!");
            }
        }

        public void SpawnFood()
        {
            Random r = new Random();
            Food = new Point(r.Next(1, BoardWidth - 1), r.Next(1, BoardHeight - 1));
        }
        public void CheckCollisions()
        {
            if (PlayerOnFood())
            {
                Score += 100;
                SpawnFood();
                ThePlayer.Grow();
            }
            else if (PlayerOnPlayer())
            {
                EndGame("You collided with yourself!");
            }

        }

        private bool PlayerOnPlayer()
        {
            //foreach (var coord in ThePlayer.Coordinates)
            //{
            //    var matches = 0;
            //    foreach (var coord2 in ThePlayer.Coordinates)
            //    {
            //        if (coord.X == coord2.X && coord.Y == coord2.Y)
            //        {
            //            matches += 1;
            //        }
            //    }
            //    if (matches > 1)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            foreach (var coord in ThePlayer.Coordinates.Take(ThePlayer.Coordinates.Count -1))
            {
                if(coord.X == ThePlayer.Front.X && coord.Y == ThePlayer.Front.Y)
                {
                    return true;
                }
            }
            return false;
        }
        private bool PlayerOnFood()
        {
            if (ThePlayer.Front == Food)
            {
                return true;
            }
            return false;
        }

        public Point GetPlayerFront()
        {
            return ThePlayer.Front;
        }
        public override string ToString()
        {
            return String.Join("\n", Contents.ToArray());
        }

        public void EndGame(string message)
        {
            throw new EndOfGameException(message, Score, Start);
        }

    }
}
