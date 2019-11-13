﻿using System;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    static class GameInfo
    {
        private static Texture2D 
            myRect, //Blacks out right side of screen
            myPacMan; //Draw lives
        private static Vector2 myDrawPos;
        private static string 
            myCurrentLevel,
            myFolderLevels,
            myFolderHighScores;
        private static int[] myHighScores;
        private static int
            myScore,
            myDrawScore;
        private static float
            myDSTimer,
            myDSDelay;

        public static Vector2 DrawPos
        {
            set => myDrawPos = value;
        }
        public static string CurrentLevel
        {
            get => myCurrentLevel;
            set => myCurrentLevel = value;
        }
        public static string FolderLevels
        {
            get => myFolderLevels;
            set => myFolderLevels = value;
        }
        public static string FolderHighScores
        {
            get => myFolderHighScores;
            set => myFolderHighScores = value;
        }
        public static int[] HighScores
        {
            get => myHighScores;
        }
        public static int Score
        {
            get => myScore;
            set => myScore = value;
        }
        public static int HighScore
        {
            get => myHighScores.Max();
        }

        public static void Initialize(GameWindow aWindow, float aDSDelay)
        {
            myDSDelay = aDSDelay;

            myDrawPos = Vector2.Zero;
            myScore = 0;
            myDSTimer = 0;
        }

        public static void LoadHighScore(string aPath)
        {
            string[] tempScores = FileReader.FindInfo(aPath, "HighScore", '=');
            myHighScores = Array.ConvertAll(tempScores, s => Int32.Parse(s));

            if (myHighScores.Length == 0)
            {
                myHighScores = new int[] { 0 };
            }

            Array.Sort(myHighScores);
            Array.Reverse(myHighScores);
        }
        public static void SaveHighScore(string aPath)
        {
            if (myHighScores.Length > 0)
            {
                if (myHighScores[0] != 0)
                {
                    File.AppendAllText(aPath, Environment.NewLine + "HighScore=" + myScore.ToString());
                }
                else
                {
                    File.AppendAllText(aPath, "HighScore=" + myScore.ToString());
                }
            }
        }

        public static void Update(GameTime aGameTime)
        {
            if (myDSTimer >= 0)
            {
                myDSTimer -= (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch, GameWindow aWindow, SpriteFont aFont, Player aPlayer)
        {
            aSpriteBatch.Draw(myRect, new Rectangle(Level.MapSize.X, 0, aWindow.ClientBounds.Width - Level.MapSize.X, aWindow.ClientBounds.Height), null, Color.Black);

            StringManager.DrawStringLeft(aSpriteBatch, aFont, "Score", new Vector2(Level.MapSize.X + 12, 20), Color.DarkOrange, 0.6f);
            StringManager.DrawStringLeft(aSpriteBatch, aFont, myScore.ToString(), new Vector2(Level.MapSize.X + 24, 52), Color.Yellow, 0.5f);

            StringManager.DrawStringLeft(aSpriteBatch, aFont, "HighScore", new Vector2(Level.MapSize.X + 12, 100), Color.DarkOrange, 0.6f);
            StringManager.DrawStringLeft(aSpriteBatch, aFont, HighScore.ToString(), new Vector2(Level.MapSize.X + 24, 132), Color.Yellow, 0.5f);

            StringManager.DrawStringLeft(aSpriteBatch, aFont, "Lives", new Vector2(Level.MapSize.X + 12, 180), Color.DarkOrange, 0.6f);
            for (int i = 0; i < aPlayer.Lives; i++)
            {
                aSpriteBatch.Draw(myPacMan, new Vector2(Level.MapSize.X + 24 + (i * 32), 200), 
                    new Rectangle(myPacMan.Width / 4, 0, myPacMan.Width / 4, myPacMan.Height), Color.White);
            }

            if (myDSTimer >= 0)
            {
                StringManager.DrawStringMid(aSpriteBatch, aFont, myDrawScore.ToString(), myDrawPos, Color.White, 0.3f);
            }
        }

        public static void AddScore(Vector2 aPos, int someScore)
        {
            myDrawPos = new Vector2(aPos.X, aPos.Y - Level.TileSize.Y);
            myScore += someScore;
            myDrawScore = someScore;
            myDSTimer = myDSDelay;
        }

        public static void SetRectTexture(string aName)
        {
            myRect = ResourceManager.RequestTexture(aName);
        }

        public static void SetPacManTexture(string aName)
        {
            myPacMan = ResourceManager.RequestTexture(aName);
        }
    }
}
