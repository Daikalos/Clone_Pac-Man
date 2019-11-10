using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    static class EnemyManager
    {
        static List<Enemy> myEnemies;
        static float[] myRespawnTimer;
        static float myRespawnDelay;

        public static List<Enemy> Enemies
        {
            get => myEnemies;
        }

        public static void Initialize(float aRespawnDelay)
        {
            myEnemies = new List<Enemy>();
            myRespawnDelay = aRespawnDelay;
        }

        public static void Update(GameTime aGameTime, Player aPlayer)
        {
            for (int i = 0; i < myRespawnTimer.Length; i++)
            {
                if (myRespawnTimer[i] > 0)
                {
                    myRespawnTimer[i] -= (float)aGameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            for (int i = myEnemies.Count; i > 0; i--)
            {
                myEnemies[i - 1].Update(aGameTime, aPlayer);
                if (!myEnemies[i - 1].IsAlive)
                {
                    myEnemies.RemoveAt(i - 1);
                    myRespawnTimer[i - 1] = myRespawnDelay;
                }
            }
        }

        public static void Draw(SpriteBatch aSpriteBatch, GameTime aGameTime)
        {
            for (int i = myEnemies.Count; i > 0; i--)
            {
                myEnemies[i - 1].Draw(aSpriteBatch, aGameTime);
            }
        }

        public static void AddEnemy()
        {
            int tempAIType = 0;
            for (int x = 0; x < Level.GetTiles.GetLength(0); x++)
            {
                for (int y = 0; y < Level.GetTiles.GetLength(1); y++)
                {
                    if (Level.GetTiles[x, y].TileType == '&')
                    {
                        myEnemies.Add(new Enemy(Level.GetTiles[x, y].Position, Level.TileSize, 100.0f, tempAIType));
                        tempAIType++;
                    }
                }
            }
            myRespawnTimer = new float[myEnemies.Count];
        }
        public static void RemoveAll()
        {
            myEnemies.RemoveAll(x => x.IsAlive);
        }
        public static void SetTexture()
        {
            for (int i = 0; i < myEnemies.Count; i++)
            {
                myEnemies[i].SetTexture("Ghost");
            }
        }
    }
}
