using System;
using CommandLine;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Snake
{
    class Program
    {
        public static Board TheBoard;
        public static PlayerDirection CurrentDirection { get; set; }
        private static int FrameWait { get; set; }
        static async Task Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed((options) =>
                {
                    var difficulty = GetDifficulty().GetAwaiter().GetResult();
                    var frameWaitMultiplier = (1 - (difficulty * 0.3));
                    Run((int)(options.FrameWait * frameWaitMultiplier)).GetAwaiter().GetResult();
                });
        }

        static async Task<int> GetDifficulty()
        {
            int selection = 1;
            ConsoleKey key;
            Console.WriteLine($"Use Left/Right arrow keys to select difficulty:");
            Console.Write("Easy ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("Medium ");
            Console.ResetColor();
            Console.Write("Hard");
            do
            {
                key = Console.ReadKey(true).Key;
                Console.Clear();
                Console.ResetColor();
                Console.WriteLine($"Use Left/Right arrow keys to select difficulty:");
                if (key == ConsoleKey.LeftArrow)
                {
                    selection--;
                }
                if (key == ConsoleKey.RightArrow)
                {
                    selection++;
                }

                if (selection < 0) selection = 0;
                if (selection > 2) selection = 2;
                if (selection == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                else { Console.ResetColor(); }
                Console.Write("Easy ");
                if (selection == 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                else { Console.ResetColor(); }
                Console.Write("Medium ");
                if (selection == 2)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                else { Console.ResetColor(); }
                Console.WriteLine("Hard");
                Console.ResetColor();
                Console.WriteLine("Press enter to begin...");

            } while (key != ConsoleKey.Enter);
            return selection;
        }
        static async Task Run(int frameWait)
        {
            var ThePlayer = new Player(new System.Drawing.Point(25, 25));
            TheBoard = new Board((int)(Console.WindowHeight * .9), (int)(Console.WindowWidth * .9), ThePlayer);
            FrameWait = frameWait;
            do
            {
                while (!Console.KeyAvailable)
                {
                    await Delay();
                    try
                    {
                        await ProcessBoardState();
                    }
                    catch (EndOfGameException e)
                    {
                        await WriteResults(e);
                    }
                    await UpdateBoard();
                }
            } while (!GetUserInput());
            try
            {
                TheBoard.EndGame("Escape key pressed!");
            }
            catch (EndOfGameException e)
            {
                await WriteResults(e);
            }
        }

        private static bool GetUserInput()
        {
            var escapePressed = false;
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (CurrentDirection != PlayerDirection.Down) CurrentDirection = PlayerDirection.Up;
                    break;
                case ConsoleKey.DownArrow:
                    if (CurrentDirection != PlayerDirection.Up) CurrentDirection = PlayerDirection.Down;
                    break;
                case ConsoleKey.LeftArrow:
                    if (CurrentDirection != PlayerDirection.Right) CurrentDirection = PlayerDirection.Left;
                    break;
                case ConsoleKey.RightArrow:
                    if (CurrentDirection != PlayerDirection.Left) CurrentDirection = PlayerDirection.Right;
                    break;
                case ConsoleKey.Escape:
                    escapePressed = true;
                    break;
                default:
                    break;
            }
            return escapePressed;
        }
        private static async Task ProcessBoardState()
        {
            TheBoard.CheckCollisions();
            var front = TheBoard.GetPlayerFront();
            switch (CurrentDirection)
            {
                case PlayerDirection.Up:
                    front.Y -= 1;
                    break;
                case PlayerDirection.Down:
                    front.Y += 1;
                    break;
                case PlayerDirection.Left:
                    front.X -= 1;
                    break;
                case PlayerDirection.Right:
                    front.X += 1;
                    break;
            }
            TheBoard.MovePlayer(front);
            TheBoard.Refresh();
        }
        private static async Task UpdateBoard()
        {
            Console.Clear();
            Console.WriteLine(TheBoard);
            Console.WriteLine($"Direction: {CurrentDirection} | Score: {TheBoard.Score} | Time Elapsed: {(DateTime.Now - TheBoard.Start).ToString(@"dd\.hh\:mm\:ss")}");
        }

        private static async Task Delay()
        {
            decimal multiplier = 1;
            if (CurrentDirection == PlayerDirection.Left || CurrentDirection == PlayerDirection.Right)
            {
                multiplier = 0.5m;
            }
            await Task.Delay((int)(FrameWait * multiplier));
        }

        private static async Task WriteResults(EndOfGameException e)
        {
            Console.Clear();
            var elapsedTime = DateTime.Now - e.Start;
            Console.WriteLine($"{e.DeathMessage} Game Over!");
            Console.WriteLine($"Score: {e.Score}");
            Console.WriteLine($"Time Played: {elapsedTime}");
            await Task.Delay(3000);
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
