using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MONO_TEST
{
    /// <summary>
    /// Class for cooldowns.
    /// </summary>
    class Cooldown
    {
        public int Duration;
        public int Timer;

        public Cooldown(int duration)
        {
            Duration = duration;
            Timer = duration;
        }

        /// <summary>
        /// Pre: n/a
        /// Post: Returns whether cooldown is met or not.
        /// Description: Checks if timer has exceeded duration of the cooldown.
        /// </summary>
        /// <returns></returns>
        public bool CheckCooldown()
        {
            if (Timer <= 0) return true;
            else return false;
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Resets the cooldown.
        /// </summary>
        public void ResetCooldown()
        {
            Timer = Duration; 
        }
    }
}
