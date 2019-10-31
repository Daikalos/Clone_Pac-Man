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
                    int[,] tempEmptyTiles = new int[3, 3];

                    for (int k = -1; k <= 1; k += 2)
                    {
                        if (i + k > 0 && i + k < myTiles.GetLength(0))
                        {
                            if (myTiles[i + k, j]?.TileType == '.')
                            {
                                tempEmptyTiles[k + 1, 1] = 1;
                            }
                            else
                            {
                                tempEmptyTiles[k + 1, 1] = 2;
                            }
                        }
                        if (j + k > 0 && j + k < myTiles.GetLength(1))
                        {
                            if (myTiles[i, j + k]?.TileType == '.')
                            {
                                tempEmptyTiles[1, k + 1] = 1;
                            }
                            else
                            {
                                tempEmptyTiles[1, k + 1] = 2;
                            }
                        }
                    }

                    if (myTiles[i, j].TileType == '#')
                    {
                        int tempAmount = 0;
                        for (int k = 0; k < tempEmptyTiles.GetLength(0); k++)
                        {
                            for (int l = 0; l < tempEmptyTiles.GetLength(1); l++)
                            {
                                if (tempEmptyTiles[k, l] == 1)
                                {
                                    tempAmount++;
                                }
                            }
                        }
                        myTiles[i, j].SetDirection(tempAmount);
                    }

                    myTiles[i, j].SetTexture();
                }
            }
        }
    }
}
