using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    static class Level
    {
        private static string[] myLevelBuilder;
        private static Tile[,] myTiles;
        private static Point myTileSize;

        public static Point TileSize
        {
            get => myTileSize;
        }

        public static Tile GetTileAtPos(Vector2 aPos)
        {
            if (((int)aPos.X / myTileSize.X) >= 0 && ((int)aPos.Y / myTileSize.Y) >= 0)
            {
                if (((int)aPos.X / myTileSize.X) < myTiles.GetLength(0) && ((int)aPos.Y / myTileSize.Y) < myTiles.GetLength(1))
                {
                    return myTiles[(int)aPos.X / myTileSize.X, (int)aPos.Y / myTileSize.Y];
                }
            }
            return myTiles[0, 0];
        }

        public static void Initialize()
        {

        }
        public static void LoadLevel(string aFilePath)
        {
            myLevelBuilder = File.ReadAllLines(aFilePath);

            int tempSizeX = myLevelBuilder[0].Length;
            int tempSizeY = myLevelBuilder.Length;

            myTiles = new Tile[tempSizeX, tempSizeY];

            for (int x = 0; x < tempSizeX; x++)
            {
                for (int y = 0; y < tempSizeY; y++)
                {
                    myTileSize = new Point(32);
                    myTiles[x, y] = new Tile(
                        new Vector2(x * myTileSize.X, y * myTileSize.Y),
                        myTileSize);
                    myTiles[x, y].TileType = myLevelBuilder[y][x];
                }
            }


        }

        public static void Update()
        {
            for (int i = 0; i < myTiles.GetLength(0); i++)
            {
                for (int j = 0; j < myTiles.GetLength(1); j++)
                {
                    myTiles[i, j].Update();
                }
            }
        }

        public static void DrawTiles(SpriteBatch aSpriteBatch)
        {
            for (int i = 0; i < myTiles.GetLength(0); i++)
            {
                for (int j = 0; j < myTiles.GetLength(1); j++)
                {
                    myTiles[i, j].Draw(aSpriteBatch);
                }
            }
        }

        public static void SetTileTexture()
        {
            for (int i = 0; i < myTiles.GetLength(0); i++)
            {
                for (int j = 0; j < myTiles.GetLength(1); j++)
                {
                    int 
                        tempAmount = 0,
                        tempDirection = 0;
                    bool tempFlip = false;
                    for (int k = -1; k <= 1; k += 2)
                    {
                        if (CheckIn(i + k, j))
                        {
                            if (myTiles[i + k, j].TileType == '.' || myTiles[i + k, j].TileType == '-')
                            {
                                tempDirection = k;
                                tempAmount++;
                            }
                        }
                        if (CheckIn(i, j + k))
                        {
                            if (myTiles[i, j + k].TileType == '.' || myTiles[i, j + k].TileType == '-')
                            {
                                if (k < 0)
                                {
                                    tempFlip = true;
                                    tempDirection = k;
                                }
                                tempAmount++;
                            }
                        }
                    }

                    myTiles[i, j].TileForm = tempAmount;
                    myTiles[i, j].SetTexture();
                    if (myTiles[i, j].TileType == '#')
                    {
                        myTiles[i, j].SetRotation(tempDirection, tempFlip);
                    }
                }
            }
        }

        public static bool CheckIn(int anX, int anY)
        {
            if (anX >= 0 && anX < myTiles.GetLength(0))
            {
                if (anY >= 0 && anY < myTiles.GetLength(1))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
