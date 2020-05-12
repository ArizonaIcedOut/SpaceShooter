using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Animation2D;
using MONO_TEST;

namespace MONO_TEST
{
    /// <summary>
    /// Class for parallax effects with backgrounds.
    /// </summary>
    class Background
    {
        public Texture2D Img;
        public Rectangle Rec;
        public int VelocityY;

        public Background(Texture2D img, Rectangle rec, int velocity)
        {
            Img = img;
            Rec = rec;
            VelocityY = velocity;
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Moves the background. If it goes off the screen, it is reset.
        /// </summary>
        public void MoveBackground()
        {
            Rec.Y += VelocityY;

            if (Rec.Y > Globals.ScreenHeight)
            {
                Rec.Y = Globals.ScreenHeight * -1;
            }
        }
    }
}
