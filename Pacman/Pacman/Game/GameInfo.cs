using System;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    static class GameInfo
    {
        private static Texture2D myRect; //Blacks out right side of screen
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

        public static void Initialize(GameWindow aWindow, float aDSTimerMax)
        {
            myDSTimerMax = aDSTimerMax;

            myDrawPos = Vector2.Zero;
            myScore = 0;
            myDSTimer = 0;
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

        public static void Draw(SpriteBatch aSpriteBatch, GameWindow aWindow, SpriteFont aFont)
        {
            aSpriteBatch.Draw(myRect, new Rectangle(Level.MapSize.X, 0, aWindow.ClientBounds.Width - Level.MapSize.X, aWindow.ClientBounds.Height), null, Color.Black);

            StringManager.DrawStringLeft(aSpriteBatch, aFont, "Score", new Vector2(Level.MapSize.X + 12, 16), Color.Yellow, 0.7f);
            StringManager.DrawStringLeft(aSpriteBatch, aFont, myScore.ToString(), new Vector2(Level.MapSize.X + 24, 48), Color.Yellow, 0.6f);

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
            myDSTimer = myDSTimerMax;
        }

        public static void SetRectTexture(string aName)
        {
            myRect = ResourceManager.RequestTexture(aName);
        }
    }
}
