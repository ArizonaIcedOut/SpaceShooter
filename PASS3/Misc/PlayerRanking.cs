using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MONO_TEST
{
    /// <summary>
    /// Class for individual rankings on a leaderboard.
    /// Necessary because 2D arrays cannot hold different variable types.
    /// </summary>
    public class PlayerRanking
    {
        public string Name;
        public int Score;

        public PlayerRanking(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}
