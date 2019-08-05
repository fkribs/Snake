using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class EndOfGameException:Exception 
    {
        public string DeathMessage { get; set; }
        public int Score { get; set; }
        public DateTime Start { get; set; }
        public EndOfGameException()
        {

        }
        public EndOfGameException(string message, int score, DateTime start): base()
        {
            DeathMessage= message;
            Score = score;
            Start = start;
        }
    }
}
