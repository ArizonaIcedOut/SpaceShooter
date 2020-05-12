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
    /// Class for all buffs.
    /// </summary>
    class Buff
    {
        public int X;
        public int Y;
        public float VelocityY;

        public float Gravity;

        public int Width;
        public int Height;

        public int Type;

        public bool Exists;
        public bool Active;
        public bool Held;

        public Cooldown SpawnCooldown;
        public Cooldown BuffDuration;

        public Buff()
        {
            Width = 100;
            Height = 100;
            Gravity = 9.81f / 60;

            X = 0;
            Y = 0;

            VelocityY = 0;

            Exists = false;
            Active = false;
            Held = false;

            SpawnCooldown = new Cooldown(100);
            BuffDuration = new Cooldown(240);
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Updates each of the cooldowns for the buff.
        /// </summary>
        public void UpdateCooldown()
        {
            // If the buff does not exist, is not active, and is not held
            if (!Exists && !Active && !Held)
            {
                // Spawns buff if cooldown is over. Otherwise, timer is updated.
                if (SpawnCooldown.CheckCooldown())
                {
                    Spawn();
                    BuffDuration.ResetCooldown();
                }
                else SpawnCooldown.Timer--;
            }
            else if (Active)
            {
                // Ends buff if duration is over. Otherwise, timer is updated.
                if (BuffDuration.CheckCooldown())
                {
                    Active = false;
                    BuffDuration.ResetCooldown();
                }
                else BuffDuration.Timer--;
            }
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Spawns a new buff entity.
        /// </summary>
        public void Spawn()
        {
            SpawnCooldown.ResetCooldown();

            X = Globals.Rng.Next(0, Globals.ScreenWidth - Width);
            Y = Globals.Rng.Next(-3 * Height, -2 * Height);
            Type = Globals.Rng.Next(1, 5);
            Exists = true;
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Destroys the buff entity.
        /// </summary>
        public void Despawn()
        {
            X = 0;
            Y = 0;
            VelocityY = 0;
            Exists = false;
        }

        /// <summary>
        /// Pre: n/a
        /// Post n/a
        /// Description: Moves the buff entity.
        /// </summary>
        public void Move()
        {
            VelocityY += Gravity;
            Y = (int)(Y + VelocityY);

            // If buff goes off the screen, it despawns.
            if (Y + Height >= Globals.ScreenHeight) Despawn();
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
    }
}
