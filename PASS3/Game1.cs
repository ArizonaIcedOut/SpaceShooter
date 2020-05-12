// Author: Eric Pu
// File Name: PASS3
// Project Name: PASS3
// Creation Date: May 6th, 2019
// Modified Date: May 20th, 2019
// Description: Cool Christmas themed Space Shooter game. Play as Krampus and sabotage Christmas by destroying all the presents!

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

namespace PASS3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Entities, etc.
        static Player player;
        static List<PlayerShot> shots;
        static List<Target> bigTargets;
        static List<Target> medTargets;
        static List<Target> smallTargets;
        static Buff buff;
        static ScreenFade screenFade;
        static Nuke nuke;

        // Cooldowns
        static Cooldown bigTargetCd;
        static Cooldown shootCd;

        // Leaderboard properties
        static List<PlayerRanking> leaderboard;
        static int leaderboardSize;

        // Stats for end screen
        static int bigTargetsKilled;
        static int medTargetsKilled;
        static int smallTargetsKilled;
        static int timeSurvived;
        static int buffsCollected;

        // Constants for buffs
        const int CLONES = 1;
        const int INSTAKILL = 2;
        const int SHIELD = 3;
        const int NUKE = 4;

        // Amount targets bounce whenever they touch player's shield
        int shieldBounceHeight;

        // Name properties
        static string name;
        static int maxNameLength;
        static int minNameLength;

        // Menu mode selection constants
        int menuSelection;
        const int MENU = 0;
        const int GAMEPLAY = 1;
        const int HELP = 2;
        const int LEADERBOARD = 3;
        const int EXIT = 4;
        const int ENDSCREEN = 5;

        static bool shotCollided;

        // Max amounts of each target and score given
        static Dictionary<string, int> maxAmounts;
        static Dictionary<string, int> hitScores;
        static Dictionary<string, int> killScores;

        // List of backgrounds for snow effect
        static List<Background> backgrounds;
        static Texture2D background1Img;
        static Texture2D background2Img;

        // Sprites
        Texture2D blankRecImg;
        Texture2D playerImg;
        Texture2D shotImg;
        Texture2D strongShotImg;

        Texture2D instakillImg;
        Texture2D bubbleImg;
        Texture2D nukeImg;
        Texture2D fallingNukeImg;
        Texture2D clonesImg;

        Texture2D bgImg;
        Texture2D heartImg;

        Texture2D bigTargetImg;
        Texture2D medTargetImg;
        Texture2D smallTargetImg;

        Texture2D logoImg;

        // Player animation
        Animation playerAni;

        // Fonts
        SpriteFont bigFont;
        SpriteFont smallFont;

        // Background music
        static Song bgMusic;

        // Sound effects
        static SoundEffect targetHitSnd;
        static SoundEffect targetKillSnd;
        static SoundEffect playerHitSnd;
        static SoundEffect deathSnd;
        static SoundEffect errorSnd;
        static SoundEffect nukeSnd;
        static SoundEffect itemUseSnd;
        static SoundEffect itemGrabSnd;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 800;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Sprites
            logoImg = Content.Load<Texture2D>("Images/Misc/logo");
            blankRecImg = Content.Load<Texture2D>("Images/Misc/blankrec");
            playerImg = Content.Load<Texture2D>("Images/Sprites/krampus");
            shotImg = Content.Load<Texture2D>("Images/Sprites/shot");
            strongShotImg = Content.Load<Texture2D>("Images/Sprites/strongshot");

            instakillImg = Content.Load<Texture2D>("Images/Sprites/instakill");
            bubbleImg = Content.Load<Texture2D>("Images/Sprites/bubble");
            nukeImg = Content.Load<Texture2D>("Images/Sprites/nuke");
            fallingNukeImg = Content.Load<Texture2D>("Images/Sprites/fallingnuke");
            clonesImg = Content.Load<Texture2D>("Images/Sprites/clone");

            bigTargetImg = Content.Load<Texture2D>("Images/Sprites/bigtarget");
            medTargetImg = Content.Load<Texture2D>("Images/Sprites/medtarget");
            smallTargetImg = Content.Load<Texture2D>("Images/Sprites/smltarget");
            heartImg = Content.Load<Texture2D>("Images/Sprites/heart");

            background1Img = Content.Load<Texture2D>("Images/Backgrounds/stars");
            background2Img = Content.Load<Texture2D>("Images/Backgrounds/stars2");
            bgImg = Content.Load<Texture2D>("Images/Backgrounds/snowybg");

            // Loading fonts
            bigFont = Content.Load<SpriteFont>("Fonts/bigFont");
            smallFont = Content.Load<SpriteFont>("Fonts/smallFont");

            // Starting animation
            playerAni = new Animation(playerImg, 4, 1, 4, 1, 1, Animation.ANIMATE_FOREVER, 10, new Vector2(0, 0), 1, true);


            // Initializing backgrounds for snow effect
            backgrounds = new List<Background>{
                new Background(background1Img, new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), 2),
                new Background(background1Img, new Rectangle(0, Globals.ScreenHeight * -1, Globals.ScreenWidth, Globals.ScreenHeight), 2),
                new Background(background2Img, new Rectangle(0, 0, Globals.ScreenWidth, Globals.ScreenHeight), 4),
                new Background(background2Img, new Rectangle(0, Globals.ScreenHeight * -1, Globals.ScreenWidth, Globals.ScreenHeight), 4) };

            // Loading audio
            bgMusic = Content.Load<Song>("Sound/theme");

            targetHitSnd = Content.Load<SoundEffect>("Sound/hit");
            targetKillSnd = Content.Load<SoundEffect>("Sound/death");
            playerHitSnd = Content.Load<SoundEffect>("Sound/takedamage");
            deathSnd = Content.Load<SoundEffect>("Sound/ouchouch");
            errorSnd = Content.Load<SoundEffect>("Sound/error");
            nukeSnd = Content.Load<SoundEffect>("Sound/boom");
            itemUseSnd = Content.Load<SoundEffect>("Sound/itemuse");
            itemGrabSnd = Content.Load<SoundEffect>("Sound/itemgrab");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.6f;
            MediaPlayer.Play(bgMusic);

            // Loading leaderboard
            leaderboard = new List<PlayerRanking>();
            leaderboardSize = 10;
            LoadLeaderboard(leaderboard);

            // Loading menu
            Globals.Gamestate = MENU;
            menuSelection = GAMEPLAY;

            // Loading name
            name = string.Empty;
            maxNameLength = 5;
            minNameLength = 2;

            // Defining dictionaries
            maxAmounts = new Dictionary<string, int>
            {
                {"BIG", 3},
                {"MED", 6},
                {"SMALL", 18}
            };

            hitScores = new Dictionary<string, int>
            {
                {"BIG", 100},
                {"MED", 50},
                {"SMALL", 30}
            };

            killScores = new Dictionary<string, int>
            {
                {"BIG", 500},
                {"MED", 250},
                {"SMALL", 150}
            };

            // Misc
            shieldBounceHeight = 10;

            screenFade = new ScreenFade();

            // 1000ms per second divided by 150ms cooldown per shot gives 6.66 shots per second
            // 60 frames per second divided by 6.66 shots per second gives about 9 frame cooldown per shot
            shootCd = new Cooldown(9);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

            Globals.UpdateGlobals();

            // Moves each of the backgrounds for the snow effect
            foreach (Background bg in backgrounds)
            {
                bg.MoveBackground();
            }

            // If the screen is currently fading, none of the logic is used
            if (screenFade.Active)
            {
                screenFade.UpdateFade();
            }
            else
            {
                switch (Globals.Gamestate)
                {
                    case MENU:
                        {
                            // Screen selection logic
                            if (menuSelection > GAMEPLAY && Globals.CheckKey(Keys.Up)) menuSelection--;
                            else if (menuSelection < EXIT && Globals.CheckKey(Keys.Down)) menuSelection++;

                            if (Globals.CheckKey(Keys.Enter) || Globals.CheckKey(Keys.Space))
                            {
                                switch (menuSelection)
                                {
                                    case GAMEPLAY:
                                        {
                                            screenFade.StartFade(GAMEPLAY, 1);
                                            ResetGame();
                                            break;
                                        }
                                    case HELP:
                                        {
                                            screenFade.StartFade(HELP, 1);
                                            break;
                                        }
                                    case LEADERBOARD:
                                        {
                                            screenFade.StartFade(LEADERBOARD, 1);
                                            break;
                                        }
                                    case EXIT:
                                        {
                                            Exit();
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case GAMEPLAY:
                        {
                            // Updates all timers
                            bigTargetCd.Timer--;
                            shootCd.Timer--;

                            // Increments time survived stat
                            timeSurvived++;

                            // Updates player and animations
                            player.Move();
                            playerAni.Update(gameTime);
                            playerAni.destRec = player.GetRec();

                            // Buff logic 
                            buff.UpdateCooldown();

                            // Logic for if buff exists as an entity, but has not been collected yet
                            if (buff.Exists)
                            {
                                // Moves the entity
                                buff.Move();

                                // Collects the buff if it touches the player
                                if (buff.CheckCollision(player.GetRec()))
                                {
                                    buff.Despawn();
                                    buff.Held = true;
                                    buffsCollected++;
                                    itemGrabSnd.CreateInstance().Play();
                                }
                            }

                            // Logic for if the buff has been collected, but has not been used yet
                            if (buff.Held && Globals.CheckKey(Keys.F))
                            {
                                buff.Held = false;
                                buff.Active = true;
                                buff.BuffDuration.ResetCooldown();
                                itemUseSnd.CreateInstance().Play();

                                // if the buff is a nuke, the nuke is created
                                if (buff.Type == NUKE)
                                {
                                    nuke = new Nuke();
                                }
                            }

                            // Logic for if the buff has been used and has not ended yet
                            if (buff.Active)
                            {
                                // If the buff is nuke, the nuke's logic is used. Otherwise, it simply checks the duration of buff.
                                if (buff.Type == NUKE)
                                {
                                    // If the nuke reaches the ground, the effect activates and the buff is reset. Otherwise, the nuke moves.
                                    if (nuke.Y + nuke.Height >= Globals.ScreenHeight)
                                    {
                                        // Score is given for all the 
                                        Globals.Score += (bigTargets.Count * killScores["BIG"]) + (medTargets.Count * killScores["MED"]) + (smallTargets.Count * killScores["SMALL"]);

                                        foreach (Target target in bigTargets)
                                        {
                                            target.Health = 0;
                                        }

                                        foreach (Target target in medTargets)
                                        {
                                            target.Health = 0;
                                        }

                                        foreach (Target target in smallTargets)
                                        {
                                            target.Health = 0;
                                        }

                                        buff.Active = false;
                                        buff.BuffDuration.ResetCooldown();
                                        nukeSnd.CreateInstance().Play();
                                    }
                                    else
                                    {
                                        nuke.Move();
                                    }
                                }
                                else if (buff.BuffDuration.CheckCooldown())
                                {
                                    // Buff deactivates if it reaches its duration
                                    buff.Active = false;
                                    buff.BuffDuration.ResetCooldown();
                                }
                            }

                            // Big target spawns if the requirements are met
                            if (bigTargetCd.CheckCooldown() && bigTargets.Count < maxAmounts["BIG"] && medTargets.Count < maxAmounts["MED"])
                            {
                                bigTargets.Add(new Target(0, 0, 0, 0, Globals.BIG));
                                bigTargetCd.ResetCooldown();
                            }

                            // Shooting logic
                            if (Globals.kbCurrent.IsKeyDown(Keys.Space))
                            {
                                // If the clone buff is active, the player shoots three shots instead of one
                                if (shootCd.CheckCooldown() && shots.Count < player.MaxShots * 3 && buff.Active && buff.Type == CLONES)
                                {
                                    shots.Add(new PlayerShot(player.X + (player.Width / 2) - (Globals.ShotWidth / 2) + 100, player.Y, -8));
                                    shots.Add(new PlayerShot(player.X + (player.Width / 2) - (Globals.ShotWidth / 2) - 100, player.Y, -8));
                                    shots.Add(new PlayerShot(player.X + (player.Width / 2) - (Globals.ShotWidth / 2), player.Y, -8));
                                    shootCd.ResetCooldown();
                                }
                                else if (shootCd.CheckCooldown() && shots.Count < player.MaxShots)
                                {
                                    shots.Add(new PlayerShot(player.X + (player.Width / 2) - (Globals.ShotWidth / 2), player.Y, -8));
                                    shootCd.ResetCooldown();
                                }
                            }

                            shotCollided = false;

                            // Iterates through each player shot
                            for (int i = 0; i < shots.Count; i++)
                            {
                                shots[i].UpdateProjectile();

                                // If the shot goes off screen, it is removed (off-screen projectiles cannot hit off-screen targets)
                                if (shots[i].Y < 0)
                                {
                                    shots.RemoveAt(i);
                                    i--;
                                    continue;
                                }

                                // Checks each big target to see if it collides with the current shot
                                foreach (Target target in bigTargets)
                                { 
                                    if (target.CheckCollision(shots[i].GetRec()))
                                    {
                                        // If instakill is active, the target dies immediately. Otherwise, health goes down by one and player is given points
                                        if (buff.Active && buff.Type == INSTAKILL)
                                        {
                                            target.Health = 0;
                                        }
                                        else
                                        {
                                            target.Health--;
                                            Globals.Score += hitScores["BIG"];

                                            // Shot is removed and indicated as collided
                                            shots.RemoveAt(i);
                                            shotCollided = true;
                                            i--;
                                        }

                                        targetHitSnd.CreateInstance().Play();
                                        break;
                                    }
                                }

                                // Code is executed only if shot has not collided yet
                                if (!shotCollided)
                                {
                                    // Same logic as big targets, but for medium instead
                                    foreach (Target target in medTargets)
                                    {
                                        if (target.CheckCollision(shots[i].GetRec()))
                                        {
                                            if (buff.Active && buff.Type == INSTAKILL)
                                            {
                                                target.Health = 0;
                                            }
                                            else
                                            {
                                                target.Health--;
                                                Globals.Score += hitScores["MED"];
                                                shots.RemoveAt(i);
                                                shotCollided = true;
                                                i--;
                                            }

                                            targetHitSnd.CreateInstance().Play();
                                            break;
                                        }
                                    }
                                }

                                // Code is executed if shot did not collide with a big or medium target
                                if (!shotCollided)
                                {
                                    // Same logic as for big and medium targets
                                    foreach (Target target in smallTargets)
                                    {
                                        if (target.CheckCollision(shots[i].GetRec()))
                                        {
                                            if (buff.Active && buff.Type == INSTAKILL)
                                            {
                                                target.Health = 0;
                                            }
                                            else
                                            {
                                                target.Health--;
                                                Globals.Score += hitScores["SMALL"];
                                                shots.RemoveAt(i);
                                                i--;
                                                shotCollided = true;
                                            }

                                            targetHitSnd.CreateInstance().Play();
                                            break;
                                        }
                                    }
                                }
                            }

                            // Iterates through each big target
                            for (int i = 0; i < bigTargets.Count; i++)
                            {
                                // Checks if target is touching player
                                if (bigTargets[i].CheckCollision(player.GetRec()))
                                {
                                    // If player has shield active, the target simply bounces off
                                    // Otherwise, player takes damage
                                    if (buff.Type == SHIELD && buff.Active)
                                    {
                                        bigTargets[i].VelocityY *= -1;
                                        bigTargets[i].Y = player.Y - bigTargets[i].Height - 10;
                                    }
                                    else TakeDamage();
                                    break;
                                }

                                // Moves the target
                                bigTargets[i].Move();
                                bigTargets[i].CheckBoundaries();

                                // If target has 0 health, it dies
                                if (bigTargets[i].Health <= 0)
                                {
                                    // Creates two new medium targets
                                    medTargets.Add(new Target(bigTargets[i].X, bigTargets[i].Y, 5, bigTargets[i].VelocityY * -1 - 1, Globals.MED));
                                    medTargets.Add(new Target(bigTargets[i].X, bigTargets[i].Y, -5, bigTargets[i].VelocityY * -1 + 1, Globals.MED));

                                    // Removes the target, grants score, and updates stats
                                    bigTargets.RemoveAt(i);
                                    Globals.Score += killScores["BIG"];
                                    bigTargetsKilled++;
                                }
                            }

                            // Same logic as for big targets, but for medium targets instead
                            for (int i = 0; i < medTargets.Count; i++)
                            {
                                if (medTargets[i].CheckCollision(player.GetRec()))
                                {
                                    if (buff.Type == SHIELD && buff.Active)
                                    {
                                        medTargets[i].VelocityY *= -1;
                                        medTargets[i].Y = player.Y - medTargets[i].Height - shieldBounceHeight;
                                    }
                                    else TakeDamage();
                                    break;
                                }

                                medTargets[i].Move();
                                medTargets[i].CheckBoundaries();

                                if (medTargets[i].Health <= 0)
                                {
                                    smallTargets.Add(new Target(medTargets[i].X, medTargets[i].Y, 0, medTargets[i].VelocityY * -1 + 1, Globals.SML));
                                    smallTargets.Add(new Target(medTargets[i].X, medTargets[i].Y, medTargets[i].VelocityX * -1, medTargets[i].VelocityY * -1 - 1, Globals.SML));
                                    smallTargets.Add(new Target(medTargets[i].X, medTargets[i].Y, medTargets[i].VelocityX, medTargets[i].VelocityY * -1, Globals.SML));

                                    medTargets.RemoveAt(i);
                                    Globals.Score += killScores["MED"];
                                    medTargetsKilled++;
                                }
                            }

                            // Same logic as for big targets, but for small targets
                            for (int i = 0; i < smallTargets.Count; i++)
                            {
                                if (smallTargets[i].CheckCollision(player.GetRec()))
                                {
                                    if (buff.Type == SHIELD && buff.Active)
                                    {
                                        smallTargets[i].VelocityY *= -1;
                                        smallTargets[i].Y = player.Y - smallTargets[i].Height - 10;
                                    }
                                    else TakeDamage();
                                    break;
                                }

                                smallTargets[i].Move();
                                smallTargets[i].CheckBoundaries();

                                if (smallTargets[i].Health <= 0)
                                {
                                    smallTargets.RemoveAt(i);
                                    Globals.Score += 150;
                                    smallTargetsKilled++;
                                }
                            }
                        }
                        break;
                    case HELP:
                        {
                            // Switches screen to menu if player leaves
                            if (Globals.CheckKey(Keys.Space) || Globals.CheckKey(Keys.Enter))
                            {
                                screenFade.StartFade(MENU, 1);
                            }
                            break;
                        }
                    case LEADERBOARD:
                        {
                            // Switches screen to menu if player leaves
                            if (Globals.CheckKey(Keys.Space) || Globals.CheckKey(Keys.Enter))
                            {
                                screenFade.StartFade(MENU, 1);
                            }
                            break;
                        }
                    case ENDSCREEN:
                        {
                            // Checks if player has tried to leave screen
                            if ((Globals.CheckKey(Keys.Space) || Globals.CheckKey(Keys.Enter)))
                            {
                                // Updates leaderboard and changes screen if name is entered. Otherwise, plays error sound.
                                if (name.Length > minNameLength)
                                {
                                    // Iterates through each leaderboard ranking. If player is within top 10, they are placed on it
                                    for (int i = 0; i < leaderboard.Count; i++)
                                    {
                                        if (leaderboard[i].Score < Globals.Score)
                                        {
                                            leaderboard.Insert(i, new PlayerRanking(name, Globals.Score));
                                            leaderboard.RemoveAt(leaderboard.Count - 1);
                                            break;
                                        }
                                    }

                                    // Empties leaderboard document
                                    File.WriteAllText("leaderboard.txt", string.Empty);

                                    // Rewrites leaderboard document with new leaderboard
                                    using (StreamWriter sw = new StreamWriter("leaderboard.txt"))
                                    {
                                        foreach (PlayerRanking user in leaderboard)
                                        {
                                            sw.WriteLine(user.Name);
                                            sw.WriteLine(user.Score);
                                        }
                                    }

                                    // Fades to leaderboard screen
                                    screenFade.StartFade(LEADERBOARD, 1);
                                }
                                else errorSnd.CreateInstance().Play();
                            }

                            // Backspace for deleting last letter in name
                            if (Globals.CheckKey(Keys.Back) && name.Length > 0)
                            {
                                name = name.Substring(0, name.Length - 1);
                            }

                            // Logic for typing letters for name
                            if (name.Length < maxNameLength)
                            {
                                for (int i = (int)Keys.A; i < (int)Keys.Z; i++)
                                {
                                    if (Globals.CheckKey((Keys)i))
                                    {
                                        name += (char)i;
                                    }
                                }
                            }
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            spriteBatch.Begin();
            
            // Background
            spriteBatch.Draw(bgImg, Globals.GetRec(), Color.White * .6f);

            // Snow effect
            foreach (Background bg in backgrounds)
            {
                spriteBatch.Draw(bg.Img, bg.Rec, Color.White);
            }

            // If the screen is currently fading, nothing else is drawn
            if (screenFade.Active)
            {
                spriteBatch.Draw(blankRecImg, Globals.GetRec(), Color.Black * screenFade.Opacity);
            }
            else
            {
                switch (Globals.Gamestate)
                {
                    case MENU:
                        {
                            // Logo
                            spriteBatch.Draw(logoImg, new Rectangle(Globals.ScreenWidth / 2 - 300 / 2, 50, 300, 200), Color.White);

                            // Text for menu selection options
                            spriteBatch.DrawString(bigFont, "START GAME", new Vector2(CentreText(Globals.GetRec(), "START GAME", bigFont).X, 300), Color.Yellow);
                            spriteBatch.DrawString(bigFont, "HELP", new Vector2(CentreText(Globals.GetRec(), "HELP", bigFont).X, 400), Color.Yellow);
                            spriteBatch.DrawString(bigFont, "LEADERBOARD", new Vector2(CentreText(Globals.GetRec(), "LEADERBOARD", bigFont).X, 500), Color.Yellow);
                            spriteBatch.DrawString(bigFont, "EXIT", new Vector2(CentreText(Globals.GetRec(), "EXIT", bigFont).X, 600), Color.Yellow);

                            // Highlights selected menu option
                            switch (menuSelection)
                            {
                                case GAMEPLAY:
                                {
                                    spriteBatch.DrawString(bigFont, "START GAME", new Vector2(CentreText(Globals.GetRec(), "START GAME", bigFont).X, 300), Color.DarkRed);
                                    break;
                                }
                                case HELP:
                                {
                                    spriteBatch.DrawString(bigFont, "HELP", new Vector2(CentreText(Globals.GetRec(), "HELP", bigFont).X, 400), Color.DarkRed);
                                    break;
                                }
                                case LEADERBOARD:
                                {
                                    spriteBatch.DrawString(bigFont, "LEADERBOARD", new Vector2(CentreText(Globals.GetRec(), "LEADERBOARD", bigFont).X, 500), Color.DarkRed);
                                    break;
                                }
                                case EXIT:
                                {
                                    spriteBatch.DrawString(bigFont, "EXIT", new Vector2(CentreText(Globals.GetRec(), "EXIT", bigFont).X, 600), Color.DarkRed);
                                    break;
                                }
                            }
                            break;
                        }
                    case GAMEPLAY:
                        {
                            // Draws player animation
                            playerAni.Draw(spriteBatch, Color.White, SpriteEffects.None);

                            // If buff entity exists, it is drawn. If it is active, the effect is drawn.
                            if (buff.Exists)
                            {
                                // Draws the buff image for each type of buff entity
                                switch (buff.Type)
                                {
                                    case CLONES:
                                        {
                                            spriteBatch.Draw(clonesImg, buff.GetRec(), Color.White);
                                            break;
                                        }
                                    case INSTAKILL:
                                        {
                                            spriteBatch.Draw(instakillImg, buff.GetRec(), Color.White);
                                            break;
                                        }
                                    case SHIELD:
                                        {
                                            spriteBatch.Draw(bubbleImg, buff.GetRec(), Color.White);
                                            break;
                                        }
                                    case NUKE:
                                        {
                                            spriteBatch.Draw(nukeImg, buff.GetRec(), Color.White);
                                            break;
                                        }
                                }
                            }
                            else if (buff.Active)
                            {
                                // Draws bar showing duration of buff remaining
                                spriteBatch.Draw(blankRecImg, new Rectangle((Globals.ScreenWidth - Convert.ToInt16(200.0 * buff.BuffDuration.Timer / buff.BuffDuration.Duration)) / 2,
                                    110, Convert.ToInt16(200.0 * buff.BuffDuration.Timer / buff.BuffDuration.Duration), 20), Color.Orange);

                                // Draws effects of each buff
                                switch (buff.Type)
                                {
                                    case CLONES:
                                        {
                                            // Draws clones to the side of the player
                                            spriteBatch.Draw(clonesImg, new Rectangle(player.X + 100, player.Y, player.Width, player.Height), Color.White);
                                            spriteBatch.Draw(clonesImg, new Rectangle(player.X - 100, player.Y, player.Width, player.Height), Color.White);
                                            break;
                                        }
                                    case SHIELD:
                                        {
                                            // Draws bubble around player
                                            spriteBatch.Draw(bubbleImg, new Rectangle(player.X - 25, player.Y - 25, player.Width + 50, player.Height + 50), Color.White);
                                            break;
                                        }
                                    case NUKE:
                                        {
                                            // Draws nuke entity
                                            spriteBatch.Draw(fallingNukeImg, nuke.GetRec(), Color.White);
                                            break;
                                        }
                                }
                            }
                            else if (buff.Held)
                            {
                                // Draws message alerting player to use according buff.
                                switch (buff.Type)
                                {
                                    case CLONES:
                                        {
                                            spriteBatch.DrawString(bigFont, "PRESS F TO ACTIVATE CLONES",
                                                new Vector2(CentreText(Globals.GetRec(), "PRESS F TO ACTIVATE CLONES", bigFont).X, 200), Color.Red);
                                            break;
                                        }
                                    case INSTAKILL:
                                        {
                                            spriteBatch.DrawString(bigFont, "PRESS F TO ACTIVATE POISON",
                                                new Vector2(CentreText(Globals.GetRec(), "PRESS F TO ACTIVATE POISON", bigFont).X, 200), Color.Red);
                                            break;
                                        }
                                    case SHIELD:
                                        {
                                            spriteBatch.DrawString(bigFont, "PRESS F TO ACTIVATE SHIELD",
                                                new Vector2(CentreText(Globals.GetRec(), "PRESS F TO ACTIVATE SHIELD", bigFont).X, 200), Color.Red);
                                            break;
                                        }
                                    case NUKE:
                                        {
                                            spriteBatch.DrawString(bigFont, "PRESS F TO ACTIVATE NUKE",
                                                new Vector2(CentreText(Globals.GetRec(), "PRESS F TO ACTIVATE NUKE", bigFont).X, 200), Color.Red);
                                            break;
                                        }
                                }
                            }

                            // Draws each target and its remaining health
                            foreach (Target i in bigTargets)
                            {
                                spriteBatch.Draw(bigTargetImg, i.GetRec(), Color.Red);
                                spriteBatch.DrawString(bigFont, Convert.ToString(i.Health), CentreText(i.GetRec(), Convert.ToString(i.Health), bigFont), Color.White);
                            }
                            foreach (Target i in medTargets)
                            {
                                spriteBatch.Draw(medTargetImg, i.GetRec(), Color.Blue);
                                spriteBatch.DrawString(bigFont, Convert.ToString(i.Health), CentreText(i.GetRec(), Convert.ToString(i.Health), bigFont), Color.White);
                            }
                            foreach (Target i in smallTargets)
                            {
                                spriteBatch.Draw(smallTargetImg, i.GetRec(), Color.Green);
                                spriteBatch.DrawString(smallFont, Convert.ToString(i.Health), CentreText(i.GetRec(), Convert.ToString(i.Health), smallFont), Color.White);
                            }

                            // If instakill buff is active, shots are green. Otherwise, normal shots are drawn.
                            if (buff.Active && buff.Type == INSTAKILL)
                            {
                                foreach (PlayerShot i in shots)
                                {
                                    spriteBatch.Draw(strongShotImg, i.GetRec(), Color.White);
                                }
                            }
                            else
                            {
                                foreach (PlayerShot i in shots)
                                {
                                    spriteBatch.Draw(shotImg, i.GetRec(), Color.White);
                                }
                            }

                            // Draws remaining hearts representing player health.
                            for (int i = 0; i < player.Health; i++)
                            {
                                spriteBatch.Draw(heartImg, new Rectangle(30 + (i * 50), 30, 40, 40), Color.White);
                            }

                            // Draws player's score.
                            spriteBatch.DrawString(bigFont, Convert.ToString(Globals.Score),
                                new Vector2(CentreText(Globals.ScreenRec, Convert.ToString(Globals.Score), bigFont).X, 50), Color.White);
                            break;
                        }
                    case HELP:
                        {
                            // Help texts and images
                            spriteBatch.DrawString(bigFont, "Use arrow keys to move!", 
                                new Vector2(CentreText(Globals.GetRec(), "Use arrow keys to move!", bigFont).X, 100), Color.Yellow);
                            spriteBatch.DrawString(bigFont, "Shoot down targets with SPACE!", 
                                new Vector2(CentreText(Globals.GetRec(), "Shoot down targets with SPACE!", bigFont).X, 200), Color.Yellow);
                            spriteBatch.DrawString(bigFont, "Targets break into smaller ones!", 
                                new Vector2(CentreText(Globals.GetRec(), "Targets break into smaller ones!", bigFont).X, 300), Color.Yellow);
                            spriteBatch.DrawString(bigFont, "Collect buffs from the sky!", 
                                new Vector2(CentreText(Globals.GetRec(), "Collect buffs from the sky!", bigFont).X, 400), Color.Yellow);
                            spriteBatch.DrawString(bigFont, "Press F to use the buff!", 
                                new Vector2(CentreText(Globals.GetRec(), "Press F to use the buff!", bigFont).X, 500), Color.Yellow);
                            spriteBatch.DrawString(bigFont, "Get hit 3 times, and you lose!", 
                                new Vector2(CentreText(Globals.GetRec(), "Get hit 3 times, and you lose!", bigFont).X, 600), Color.Yellow);
                            break;
                        }
                    case LEADERBOARD:
                        {
                            spriteBatch.DrawString(bigFont, "NAME: ", new Vector2(100, 50), Color.Red);
                            spriteBatch.DrawString(bigFont, "SCORE: ", new Vector2(400, 50), Color.Red);
                            // Draws each user on the leaderboard
                            for (int i = 0; i < leaderboard.Count; i++)
                            {
                                spriteBatch.DrawString(bigFont, leaderboard[i].Name, new Vector2(100, 150 + (i * 50)), Color.White);
                                spriteBatch.DrawString(bigFont, Convert.ToString(leaderboard[i].Score), new Vector2(400, 150 + (i * 50)), Color.White);
                            }
                            break;
                        }
                    case ENDSCREEN:
                        {
                            // Draws stats and player's name
                            spriteBatch.DrawString(bigFont, "Big targets killed: " + Convert.ToString(bigTargetsKilled), 
                                new Vector2(CentreText(Globals.GetRec(),"Big targets killed: " + Convert.ToString(bigTargetsKilled), bigFont).X, 100), Color.Yellow);

                            spriteBatch.DrawString(bigFont, "Medium targets killed: " + Convert.ToString(medTargetsKilled), 
                                new Vector2(CentreText(Globals.GetRec(), "Medium targets killed: " + Convert.ToString(medTargetsKilled), bigFont).X, 200), Color.Yellow);

                            spriteBatch.DrawString(bigFont, "Small targets killed: " + Convert.ToString(smallTargetsKilled), 
                                new Vector2(CentreText(Globals.GetRec(), "Small targets killed: " + Convert.ToString(smallTargetsKilled), bigFont).X, 300), Color.Yellow);

                            spriteBatch.DrawString(bigFont, "Buffs collected: " + Convert.ToString(buffsCollected), 
                                new Vector2(CentreText(Globals.GetRec(), "Buffs collected: " + Convert.ToString(buffsCollected), bigFont).X, 400), Color.Yellow);

                            spriteBatch.DrawString(bigFont, "Time survived: " + Convert.ToString(timeSurvived / 60), 
                                new Vector2(CentreText(Globals.GetRec(), "Time survived: " + Convert.ToString(timeSurvived / 60), bigFont).X, 500), Color.Yellow);

                            spriteBatch.DrawString(bigFont, "ENTER YOUR NAME:", 
                                new Vector2(CentreText(Globals.GetRec(), "ENTER YOUR NAME:", bigFont).X, 600), Color.Yellow);

                            spriteBatch.DrawString(bigFont, name,
                                new Vector2(CentreText(Globals.GetRec(), name, bigFont).X, 700), Color.Black);
                            break;
                        }
                }
            }
            spriteBatch.End();
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: All entities are wiped, cooldowns are reset, and health goes down.
        /// </summary>
        public static void TakeDamage()
        {
            // Wipes out entities
            bigTargets = new List<Target>();
            medTargets = new List<Target>();
            smallTargets = new List<Target>();
            shots = new List<PlayerShot>();
            buff = new Buff();

            // Reduces health
            player.Health--;

            // Resets cooldown
            bigTargetCd.ResetCooldown();
            shootCd.ResetCooldown();

            // If player's health is greater than 0, screen fades. Otherwides, game ends.
            if (player.Health > 0)
            {
                screenFade.StartFade(GAMEPLAY, .5f);
                playerHitSnd.CreateInstance().Play();
            }
            else
            {
                screenFade.StartFade(ENDSCREEN, 1);
                deathSnd.CreateInstance().Play();
            }
        }

        /// <summary>
        /// Pre: rec is a rectangle, text is the text being centred, font is the font used
        /// Post: Returns a Vector2 with centred X and Y value
        /// Description: Centres text given a rectangle, text, and the font.
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Vector2 CentreText(Rectangle rec, string text, SpriteFont font)
        {
            return new Vector2(rec.X + (rec.Width - font.MeasureString(text).X) / 2, rec.Y + (rec.Height - font.MeasureString(text).Y) / 2);
        }

        /// <summary>
        /// Pre: n/a
        /// Post: n/a
        /// Description: Resets all values for the game.
        /// </summary>
        public static void ResetGame()
        {
            player = new Player();

            bigTargets = new List<Target>();
            medTargets = new List<Target>();
            smallTargets = new List<Target>();

            shots = new List<PlayerShot>();

            bigTargetCd = new Cooldown(120);

            buff = new Buff();

            Globals.Score = 0;

            bigTargetsKilled = 0;
            medTargetsKilled = 0;
            smallTargetsKilled = 0;
            timeSurvived = 0;
            buffsCollected = 0;
        }

        /// <summary>
        /// Pre: rankingsList is a list of PlayerRankings
        /// Post: Returns the loaded leaderboard
        /// Description: Reads through leaderboard document and returns the leaderboard.
        /// </summary>
        /// <param name="rankingsList"></param>
        /// <returns></returns>
        public static List<PlayerRanking> LoadLeaderboard(List<PlayerRanking> rankingsList)
        {
            using (StreamReader sr = new StreamReader("leaderboard.txt"))
            {
                for (int i = 0; i < leaderboardSize; i++)
                {
                    rankingsList.Add(new PlayerRanking(sr.ReadLine(), Convert.ToInt32(sr.ReadLine())));
                }
            }
            return rankingsList;
        }
    }
}
