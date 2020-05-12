using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows;


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
    /// Class for projectiles fired by the player.
    /// </summary>
    class PlayerShot
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public int VelocityY;

        public PlayerShot(int x, int y, int velocityY)
        {
            X = x;
            Y = y;
            Width = Globals.ShotWidth;
            Height = Globals.ShotHeight;
            VelocityY = velocityY;
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

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Moves projectile.
        /// </summary>
        public void UpdateProjectile()
        {
            Y += VelocityY;
        }
    }
}
