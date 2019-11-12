using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    static class Level
    {
        private static string[] myLevelBuilder;
        private static Tile[,] myTiles;
        private static Point 
            myTileSize,
            myMapSize;

        public static Point TileSize
        {
            get => myTileSize;
        }
        public static Point MapSize
        {
            get => myMapSize;
        }

        public static Tile[,] GetTiles
        {
            get => myTiles;
        }

        public static Tuple<Tile, bool> GetTileAtPos(Vector2 aPos)
        {
            if (aPos.X > 0 && aPos.Y > 0)
            {
                if (((int)aPos.X / myTileSize.X) >= 0 && ((int)aPos.Y / myTileSize.Y) >= 0)
                {
                    if (((int)aPos.X / myTileSize.X) < myTiles.GetLength(0) && ((int)aPos.Y / myTileSize.Y) < myTiles.GetLength(1))
                    {
                        return new Tuple<Tile, bool>(myTiles[(int)aPos.X / myTileSize.X, (int)aPos.Y / myTileSize.Y], true);
                    }
                }
            }
            return new Tuple<Tile, bool>(myTiles[0, 0], false);
        }
        public static Tile GetClosestTile(Vector2 aPos)
        {
            Tile tempClosest = null;
            float tempMinDistance = float.MaxValue;

            for (int i = 0; i < myTiles.GetLength(0); i++)
            {
                for (int j = 0; j < myTiles.GetLength(1); j++)
                {
                    if (myTiles[i, j].TileType != '#')
                    {
                        float tempDistance = Vector2.Distance(myTiles[i, j].GetCenter(), aPos);
                        if (tempDistance < tempMinDistance)
                        {
                            tempClosest = myTiles[i, j];
                            tempMinDistance = tempDistance;
                        }
                    }               
                }
            }
            return tempClosest;
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

        public static void LoadLevel(Point aTileSize)
        {
            if (File.Exists(GameInfo.FolderLevels + GameInfo.CurrentLevel))
            {
                myLevelBuilder = File.ReadAllLines(GameInfo.FolderLevels + GameInfo.CurrentLevel);
                myTileSize = aTileSize;

                int tempSizeX = myLevelBuilder[0].Length;
                int tempSizeY = myLevelBuilder.Length;

                myTiles = new Tile[tempSizeX, tempSizeY];

                for (int x = 0; x < tempSizeX; x++)
                {
                    for (int y = 0; y < tempSizeY; y++)
                    {
                        myTiles[x, y] = new Tile(
                            new Vector2(x * myTileSize.X, y * myTileSize.Y),
                            myTileSize);
                        myTiles[x, y].TileType = myLevelBuilder[y][x];
                    }
                }

                myMapSize = new Point(
                    myTiles.GetLength(0) * myTileSize.X,
                    myTiles.GetLength(1) * myTileSize.Y);
            }
            else //Create custom level
            {
                myTileSize = aTileSize;

                int tempSizeX = 28;
                int tempSizeY = 31;

                myTiles = new Tile[tempSizeX, tempSizeY];

                for (int x = 0; x < tempSizeX; x++)
                {
                    for (int y = 0; y < tempSizeY; y++)
                    {
                        myTiles[x, y] = new Tile(
                            new Vector2(x * myTileSize.X, y * myTileSize.Y),
                            myTileSize);
                        myTiles[x, y].TileType = '-';
                    }
                }

                myMapSize = new Point(
                myTiles.GetLength(0) * myTileSize.X,
                myTiles.GetLength(1) * myTileSize.Y);
            }
        }
        public static void SaveLevel(string aLevelName)
        {
            string tempPathLevels = GameInfo.FolderLevels + aLevelName;

            string tempName = aLevelName;
            tempName = tempName.Replace(".txt", "");

            string tempPathHighScores = GameInfo.FolderHighScores + tempName + "_HighScores.txt";

            if (File.Exists(tempPathLevels))
            {
                File.Delete(tempPathLevels);
            }

            if (File.Exists(tempPathHighScores))
            {
                File.Delete(tempPathHighScores);
            }

            FileStream tempFS = File.Create(tempPathLevels);
            tempFS.Close();

            tempFS = File.Create(tempPathHighScores);
            tempFS.Close();

            for (int i = 0; i < myTiles.GetLength(1); i++)
            {
                for (int j = 0; j < myTiles.GetLength(0); j++)
                {
                    File.AppendAllText(tempPathLevels, myTiles[j, i].TileType.ToString());
                }
                File.AppendAllText(tempPathLevels, Environment.NewLine);
            }
        }

        public static bool CheckIfWon()
        {
            bool tempWon = true;
            for (int i = 0; i < myTiles.GetLength(0); i++)
            {
                for (int j = 0; j < myTiles.GetLength(1); j++)
                {
                    if (myTiles[i, j].TileType == '.')
                    {
                        tempWon = false;
                    }
                }
            }
            return tempWon;
        }

        public static void SetTileTexture()
        {
            for (int i = 0; i < myTiles.GetLength(0); i++)
            {
                for (int j = 0; j < myTiles.GetLength(1); j++)
                {
                    int 
                        tempTileForm = 0,
                        tempDirection = 0;
                    bool 
                        tempFlip = false,
                        tempHorizontal = false,
                        tempVertical = false;

                    for (int k = -1; k <= 1; k += 2)
                    {
                        if (CheckIn(i + k, j))
                        {
                            if (myTiles[i + k, j].TileType != '#' && myTiles[i + k, j].TileType != '%')
                            {
                                tempDirection = k;
                                tempTileForm++;
                                tempHorizontal = true;
                            }
                        }
                        if (CheckIn(i, j + k))
                        {
                            if (myTiles[i, j + k].TileType != '#' && myTiles[i, j + k].TileType != '%')
                            {
                                if (k < 0)
                                {
                                    tempFlip = true;
                                    tempDirection = k;
                                }
                                tempTileForm++;
                                tempVertical = true;
                            }
                        }
                    }

                    if (tempHorizontal && !tempVertical || tempVertical && !tempHorizontal) //Fix corridor block type
                    {
                        tempTileForm = 1;
                        if (tempHorizontal)
                        {
                            tempDirection = -1;
                        }
                    }
                    if (tempTileForm == 3) //Fix rotation for situational tile direction on 3-sided block types
                    {
                        if (CheckIn(i, j + 1))
                        {
                            if (myTiles[i, j + 1].TileType == '#')
                            {
                                tempFlip = true;
                                tempDirection = -1;
                            }
                        }
                        if (CheckIn(i + 1, j)) 
                        {
                            if (myTiles[i + 1, j].TileType == '#')
                            {
                                tempFlip = false;
                                tempDirection = -1;
                            }
                        }
                    }
                    if (tempTileForm == 0) //Fix tiles in corners of obstacles
                    {
                        for (int k = -1; k <= 1; k += 2)
                        {
                            for (int l = -1; l <= 1; l += 2)
                            {
                                if (CheckIn(i + k, j + l))
                                {
                                    if (myTiles[i + k, j + l].TileType != '#' && myTiles[i + k, j + l].TileType != '%')
                                    {
                                        tempTileForm = 2;
                                        tempDirection = k;
                                        if (l < 0)
                                        {
                                            tempFlip = true;
                                        }
                                        myTiles[i, j].Rotation += MathHelper.Pi;
                                    }
                                }
                            }
                        }
                    }
                    if (tempTileForm == 4 && myTiles[i, j].TileType == '#')
                    {
                        tempTileForm = 0;
                    }

                    myTiles[i, j].TileForm = tempTileForm;
                    myTiles[i, j].SetTexture();
                    if (myTiles[i, j].TileType == '#')
                    {
                        myTiles[i, j].SetRotation(tempDirection, tempFlip);
                    }
                }
            }
        }
        public static void SetTileTextureEditor()
        {
            for (int i = 0; i < myTiles.GetLength(0); i++)
            {
                for (int j = 0; j < myTiles.GetLength(1); j++)
                {
                    myTiles[i, j].SetTextureEditor();
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
