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
    /// Class for the player.
    /// </summary>
    class Player
    {
        public int Health;
        public int X;
        public int Y;
        public int VelocityX;
        public int Width;
        public int Height;
        public int MaxShots;

        public Player()
        {
            Health = 3;
            
            Width = 100;
            Height = 100;

            X = Globals.ScreenWidth / 2 - Width / 2; 
            Y = Globals.ScreenHeight - Height - 20;

            VelocityX = 8;
            MaxShots = 10;
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
        /// Description: Checks player's input and moves the player accordingly.
        /// </summary>
        public void Move()
        {
            if (Globals.kbCurrent.IsKeyDown(Keys.Left) && X > 0 + VelocityX) X -= VelocityX;
            else if (Globals.kbCurrent.IsKeyDown(Keys.Right) && X + Width + VelocityX < Globals.ScreenWidth) X += VelocityX;
        }
    }
}
