using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class EditorState : State
    {
        private Tuple<Tile, bool> mySelectedTile;
        private Texture2D
            myBlock,
            mySnack,
            myFruit,
            myPowerUp00,
            myPowerUp01,
            myGhost;
        private SpriteFont my8bitFont;
        private string[] myLevelNames;
        private int 
            mySelection,
            mySelectionAmount;
        private bool myLoadLevel;

        public EditorState(MainGame aGame) : base(aGame)
        {
            GameInfo.CurrentLevel = "Level_Template.txt";
            Level.LoadLevel(new Point(32));
            mySelectionAmount = 8;
            aGame.IsMouseVisible = true;
        }

        public override void Update(GameWindow aWindow, GameTime aGameTime)
        {
            if (!myLoadLevel)
            {
                EditMap();

                if (KeyMouseReader.KeyPressed(Keys.Enter)) //Load or save map
                {
                    switch (mySelection)
                    {
                        case 7:
                            myLoadLevel = true;
                            myLevelNames = FileReader.FindFileNames("../../../../Levels/");
                            mySelectionAmount = myLevelNames.Length - 1;
                            mySelection = 0;
                            break;
                        case 8:
                            SaveLevel();
                            break;
                    }
                }

                if (KeyMouseReader.KeyPressed(Keys.Back)) //Return to menu
                {
                    myGame.ChangeState(new MenuState(myGame));
                }
            }
            else
            {
                LoadLevel();
            }

            if (KeyMouseReader.KeyPressed(Keys.Up))
            {
                if (mySelection > 0)
                {
                    mySelection--;
                }
            }
            if (KeyMouseReader.KeyPressed(Keys.Down))
            {
                if (mySelection < mySelectionAmount)
                {
                    mySelection++;
                }
            }

            Level.Update();
        }

        public override void Draw(SpriteBatch aSpriteBatch, GameWindow aWindow, GameTime aGameTime)
        {
            Level.DrawTiles(aSpriteBatch);

            StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, ">",
                new Vector2(Level.MapSize.X + 10, 70 + (40 * mySelection)),
                Color.GhostWhite, 0.6f);

            if (!myLoadLevel)
            {
                aSpriteBatch.Draw(myBlock, new Vector2(Level.MapSize.X + 40, 70), null, Color.White, 0.0f,
                    new Vector2(myBlock.Width / 2, myBlock.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
                aSpriteBatch.Draw(mySnack, new Vector2(Level.MapSize.X + 40, 110), null, Color.White, 0.0f,
                    new Vector2(mySnack.Width / 2, mySnack.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
                aSpriteBatch.Draw(myFruit, new Vector2(Level.MapSize.X + 40, 150), new Rectangle(0, 0, myFruit.Width / 5, myFruit.Height), Color.White, 0.0f,
                    new Vector2(myFruit.Width / 10, myFruit.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
                aSpriteBatch.Draw(myPowerUp00, new Vector2(Level.MapSize.X + 40, 230), null, Color.White, 0.0f,
                    new Vector2(myPowerUp00.Width / 2, myPowerUp00.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
                aSpriteBatch.Draw(myPowerUp01, new Vector2(Level.MapSize.X + 40, 270), null, Color.White, 0.0f,
                    new Vector2(myPowerUp01.Width / 2, myPowerUp01.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
                aSpriteBatch.Draw(myGhost, new Vector2(Level.MapSize.X + 40, 310), new Rectangle(0, 0, myGhost.Width / 2, myGhost.Height), Color.White, 0.0f,
                    new Vector2(myGhost.Width / 4, myGhost.Height / 2), 1.0f, SpriteEffects.None, 0.0f);

                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "= Block", new Vector2(Level.MapSize.X + 70, 70), Color.White, 0.5f);
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "= Snack", new Vector2(Level.MapSize.X + 70, 110), Color.White, 0.5f);
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "= Fruits", new Vector2(Level.MapSize.X + 70, 150), Color.White, 0.5f);
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "= Void", new Vector2(Level.MapSize.X + 70, 190), Color.White, 0.5f);
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "= PU 1", new Vector2(Level.MapSize.X + 70, 230), Color.White, 0.5f);
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "= PU 2", new Vector2(Level.MapSize.X + 70, 270), Color.White, 0.5f);
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "= Ghost", new Vector2(Level.MapSize.X + 70, 310), Color.White, 0.5f);

                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "LOAD", new Vector2(Level.MapSize.X + 30, 350), Color.White, 1.0f);
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "SAVE", new Vector2(Level.MapSize.X + 30, 390), Color.White, 1.0f);

                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "Press return to go back to menu", new Vector2(16, aWindow.ClientBounds.Height - 16), Color.DarkOrange * 0.5f, 0.5f);
            }
            else
            {
                for (int i = 0; i < myLevelNames.Length; i++)
                {
                    string tempName = myLevelNames[i];
                    tempName = tempName.Replace(".txt", "");

                    StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, tempName,
                        new Vector2(Level.MapSize.X + 50, 70 + (40 * i)),
                        Color.White, 0.5f);
                }
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "Press return to go back to editor", new Vector2(16, aWindow.ClientBounds.Height - 16), Color.DarkOrange * 0.5f, 0.5f);
            }
        }

        private void SaveLevel()
        {
            int tempLevel = 1;
            string[] tempName = FileReader.FindFileNames(GameInfo.FolderLevels);
            for (int i = 0; i < tempName.Length; i++)
            {
                tempName[i] = tempName[i].Replace("Level", "");
                tempName[i] = tempName[i].Replace(".txt", "");
            }
            for (int i = 0; i < tempName.Length; i++)
            {
                if (tempName[i] != "Level_Template")
                {
                    int tempResult = 0;
                    Int32.TryParse(tempName[i], out tempResult);

                    if (tempResult != (i + 1) && i > 0)
                    {
                        tempLevel = (i + 1);
                        break;
                    }
                }
            }
            if (tempLevel < 10)
            {
                Level.SaveLevel("Level0" + tempLevel + ".txt");
            }
            else
            {
                Level.SaveLevel("Level" + tempLevel + ".txt");
            }
        }
        private void LoadLevel()
        {
            if (KeyMouseReader.KeyPressed(Keys.Back))
            {
                myLoadLevel = false;
                mySelectionAmount = 8;
            }
            if (KeyMouseReader.KeyPressed(Keys.Enter) && myLevelNames.Length > 0)
            {
                GameInfo.CurrentLevel = myLevelNames[mySelection];
                Level.LoadLevel(Level.TileSize);
                Level.SetTileTexture();
                Level.SetTileTextureEditor();
            }
        }

        private void EditMap()
        {
            mySelectedTile = Level.GetTileAtPos(KeyMouseReader.myCurrentMouseState.Position.ToVector2());
            if (mySelectedTile.Item2)
            {
                if (KeyMouseReader.LeftHold())
                {
                    switch (mySelection)
                    {
                        case 0:
                            mySelectedTile.Item1.TileType = '#';
                            mySelectedTile.Item1.TileForm = 0;
                            break;
                        case 1:
                            mySelectedTile.Item1.TileType = '.';
                            break;
                        case 2:
                            mySelectedTile.Item1.TileType = '^';
                            break;
                        case 3:
                            mySelectedTile.Item1.TileType = '%';
                            break;
                        case 4:
                            mySelectedTile.Item1.TileType = '/';
                            break;
                        case 5:
                            mySelectedTile.Item1.TileType = '=';
                            break;
                        case 6:
                            mySelectedTile.Item1.TileType = '&';
                            break;
                    }
                }
                if (KeyMouseReader.RightHold())
                {
                    mySelectedTile.Item1.TileType = '-';
                }
                mySelectedTile.Item1.SetTextureEditor();
            }
        }

        public override void LoadContent()
        {
            Level.SetTileTextureEditor();

            my8bitFont = ResourceManager.RequestFont("8-bit");

            myBlock = ResourceManager.RequestTexture("Tile_Block-0");
            mySnack = ResourceManager.RequestTexture("Snack");
            myFruit = ResourceManager.RequestTexture("Fruits");
            myPowerUp00 = ResourceManager.RequestTexture("PowerUp_00");
            myPowerUp01 = ResourceManager.RequestTexture("PowerUp_01");
            myGhost = ResourceManager.RequestTexture("Ghost");
        }
    }
}
