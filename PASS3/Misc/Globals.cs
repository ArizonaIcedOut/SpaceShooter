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

/// <summary>
/// Class for global variables.
/// </summary>
class Globals
{
    // RNG
    public static Random Rng = new Random();

    // Keyboard states for past and present
    public static KeyboardState kbPast = new KeyboardState();
    public static KeyboardState kbCurrent = Keyboard.GetState();

    public static int Gamestate;
    public static int Score;

    public static int ScreenWidth = 600;
    public static int ScreenHeight = 800;

    public static Rectangle ScreenRec = new Rectangle(0, 0, ScreenWidth, ScreenHeight);

    public const int BIG = 1;
    public const int MED = 2;
    public const int SML = 3;

    public static int ShotWidth = 20;
    public static int ShotHeight = 20;

    public static void UpdateGlobals()
    {
        kbPast = kbCurrent;
        kbCurrent = Keyboard.GetState();
    }

    /// <summary>
    /// Pre: original as the color being inverted
    /// Post: Returns the inverted color
    /// Description: Inverts a given color. Not used in this project, but it's cool
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static Color InvertColor(Color original)
    {
        return new Color(255 - original.R, 255 - original.G, 255 - original.B);
    }

    /// <summary>
    /// Pre: key as key being checked
    /// Post: Returns if the key is being pressed or not
    /// Description: Checks if a given key is being pressed or not.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool CheckKey(Keys key)
    {
        if (kbCurrent.IsKeyDown(key) && !kbPast.IsKeyDown(key)) return true;
        else return false;
    }

    /// <summary>
    /// Pre: n/a
    /// Post: Returns the dimensions of the screen as a rectangle
    /// Description: Gives dimensions of the screen.
    /// </summary>
    /// <returns></returns>
    public static Rectangle GetRec()
    {
        return new Rectangle(0, 0, ScreenWidth, ScreenHeight);
    }
}
