using System;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    static class GameInfo
    {
        private static Vector2 myDrawPos;
        private static int[] myHighScores;
        private static int
            myScore,
            myDrawScore;
        private static float
            myDSTimer,
            myDSTimerMax;

        public static Vector2 DrawPos
        {
            set => myDrawPos = value;
        }
        public static int[] HighScores
        {
            get => myHighScores;
        }
        public static int Score
        {
            get => myScore;
        }
        public static int HighScore
        {
            get => myHighScores.Max();
        }

        public static void Initialize(float aDSTimerMax)
        {
            myDrawPos = Vector2.Zero;
            myScore = 0;
            myDSTimer = 0;
            myDSTimerMax = aDSTimerMax;
        }

        public static void LoadHighScore(string aPath)
        {
            string[] tempScores = FileReader.FindInfo(aPath, "Highscore", '=');
            myHighScores = Array.ConvertAll(tempScores, s => Int32.Parse(s));
            Array.Sort(myHighScores);
            Array.Reverse(myHighScores);
        }
        public static void SaveHighScore(string aPath)
        {
            File.AppendAllText(aPath, Environment.NewLine + "Highscore=" + myScore.ToString());
        }

        public static void Update(GameTime aGameTime)
        {
            if (myDSTimer >= 0)
            {
                myDSTimer -= (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch, SpriteFont aFont)
        {
            if (myDSTimer >= 0)
            {
                StringManager.DrawStringMid(aSpriteBatch, aFont, myDrawScore.ToString(), myDrawPos, Color.White, 0.5f);
            }
        }

        public static void AddScore(Vector2 aPos, int someScore)
        {
            myDrawPos = new Vector2(aPos.X, aPos.Y - Level.TileSize.Y);
            myScore += someScore;
            myDrawScore = someScore;
            myDSTimer = myDSTimerMax;
        }
    }
}
