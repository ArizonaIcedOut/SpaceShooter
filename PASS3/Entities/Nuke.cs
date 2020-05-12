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
    /// Class for the nuke projectile spawned by the buff.
    /// </summary>
    class Nuke
    {
        public int X;
        public int Y;
        public float VelocityY;

        public float Gravity;

        public int Width;
        public int Height;

        public Nuke()
        {
            Width = 100;
            Height = 100;

            X = Globals.ScreenWidth / 2 - Width / 2;
            Y = 0;
            VelocityY = 4;

            Gravity = 9.81f / 60;
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Moves the nuke.
        /// </summary>
        public void Move()
        {
            VelocityY += Gravity;
            Y = (int)(Y + VelocityY);
        }

        /// <summary>
        /// Pre: n/a
        /// Post: Returns a rectangle with entity's X, Y, width, and height values
        /// Description: Returns a rectangle with the entity's dimensions.
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRec()
        {
            return new Rectangle(X, Y, Width, Height);
        }
    }
}
