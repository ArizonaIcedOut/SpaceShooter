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
    /// Class for all three types of targets.
    /// </summary>
    class Target
    {
        public int Health;
        public int X;
        public int Y;
        public float VelocityX;
        public float VelocityY;      

        public float Gravity;

        public int Width;
        public int Height;

        public int MaxAmount;
        public int HitScore;
        public int KillScore;
        
        public Target(int x, int y, float velocityX, float velocityY, int type)
        {
            // Constructs target according to what type it is
            switch (type)
            {
                case Globals.BIG:
                    { 
                        Health = 5;

                        Width = 100;
                        Height = 100;
                        Gravity = 9.81f / 60;

                        X = Globals.Rng.Next(0, Globals.ScreenWidth - Width);
                        Y = Globals.Rng.Next(-3 * Height, -2 * Height);

                        VelocityY = 0;
                        break;
                    }
                case Globals.MED:
                    {
                        Health = 3;

                        Width = 50;
                        Height = 50;
                        Gravity = 9.81f / 60;

                        X = x;
                        Y = y;

                        VelocityX = velocityX;
                        VelocityY = velocityY;
                        break;
                    }
                case Globals.SML:
                    {
                        Health = 1;

                        Width = 25;
                        Height = 25;
                        Gravity = 9.81f / 60;

                        X = x;
                        Y = y;

                        VelocityX = velocityX;
                        VelocityY = velocityY;
                        break;
                    }
            }
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Moves the target.
        /// </summary>
        public void Move()
        {
            VelocityY += Gravity;
            Y = (int)(Y + VelocityY);
            X = (int)(X + VelocityX);
        }

        /// <summary>
        /// Pre: rec as rectangle being checked
        /// Post: Whether projectile collided with rec or not
        /// Description: Checks if projectile is colliding with given rectangle.
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public bool CheckCollision(Rectangle rec)
        {
            if (rec.X + rec.Width >= X && rec.X <= X + Width && rec.Y + rec.Height >= Y && rec.Y <= Y + Height) return true;
            else return false;
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
        /// Description: Checks if target is hitting edges of the screen.
        /// </summary>
        public void CheckBoundaries()
        {
            if (Y + Height >= Globals.ScreenHeight)
            {
                VelocityY *= -1;
            }

            if (X + Width >= Globals.ScreenWidth || X <= 0)
            {
                VelocityX *= -1;
            }
        }
    }
}
