using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MONO_TEST
{
    /// <summary>
    /// Class for fading screen logic.
    /// </summary>
    class ScreenFade
    {
        public bool Active;
        public int NextState;
        public float Opacity;
        public bool Transitioned;
        public float Duration;

        public ScreenFade()
        {
            Active = false;
        }

        public void StartFade(int nextState, float duration)
        {
            NextState = nextState;
            Opacity = 0f;
            Duration = duration;
            Transitioned = false;
            Active = true;
        }

        public void UpdateFade()
        {
            if (!Transitioned)
            {
                Opacity += 1 / (60 * Duration);

                if (Opacity >= 1f)
                {
                    Globals.Gamestate = NextState;
                    Transitioned = true;
                }
            }
            else
            {
                Opacity -= 1 / (60 * Duration);

                if (Opacity <= 0f)
                {
                    Active = false;
                }
            }
        }
    }
}
