using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    static class EnemyManager
    {
        static List<Enemy> myEnemies;
        static float[] myRespawnTimer;
        static bool[] myRespawnEnemy;
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
                if (myRespawnEnemy[i] && !aPlayer.IsEating)
                {
                    if (myRespawnTimer[i] > 0)
                    {
                        myRespawnTimer[i] -= (float)aGameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        AddEnemy(i);
                        myRespawnEnemy[i] = false;
                    }
                }
            }

            for (int i = myEnemies.Count; i > 0; i--)
            {
                myEnemies[i - 1].Update(aGameTime, aPlayer);
                if (!myEnemies[i - 1].IsAlive)
                {
                    myRespawnTimer[myEnemies[i - 1].AIType] = myRespawnDelay;
                    myRespawnEnemy[myEnemies[i - 1].AIType] = true;

                    myEnemies.RemoveAt(i - 1);
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

        public static void AddEnemy(int aAIType)
        {
            for (int x = 0; x < Level.GetTiles.GetLength(0); x++)
            {
                for (int y = 0; y < Level.GetTiles.GetLength(1); y++)
                {
                    if (Level.GetTiles[x, y].TileType == '&')
                    {
                        myEnemies.Add(new Enemy(Level.GetTiles[x, y].Position, Level.TileSize, 100.0f, 5.0f, aAIType));
                        EnemyManager.SetTexture();
                        return;
                    }
                }
            }
        }
        public static void AddEnemies()
        {
            int tempAIType = 0;
            for (int x = 0; x < Level.GetTiles.GetLength(0); x++)
            {
                for (int y = 0; y < Level.GetTiles.GetLength(1); y++)
                {
                    if (Level.GetTiles[x, y].TileType == '&')
                    {
                        float tempSpeed = 100.0f;
                        if (tempAIType == 3)
                        {
                            tempSpeed = 60.0f;
                        }

                        myEnemies.Add(new Enemy(Level.GetTiles[x, y].Position, Level.TileSize, tempSpeed, 5.0f, tempAIType));
                        tempAIType++;

                        if (tempAIType > 3)
                        {
                            tempAIType = 0;
                        }
                    }
                }
            }
            EnemyManager.SetTexture();

            myRespawnTimer = new float[myEnemies.Count];
            myRespawnEnemy = new bool[myEnemies.Count];
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
