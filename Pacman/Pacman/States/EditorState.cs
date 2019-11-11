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
            myPowerUp,
            myGhost;
        SpriteFont my8bitFont;
        private int mySelection;

        public EditorState(MainGame aGame) : base(aGame)
        {
            GameInfo.CurrentLevel = "Level_Template.txt";
            Level.LoadLevel(new Point(32));
            aGame.IsMouseVisible = true;
        }

        public override void Update(GameWindow aWindow, GameTime aGameTime)
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
                            mySelectedTile.Item1.TileType = '%';
                            break;
                        case 3:
                            mySelectedTile.Item1.TileType = '/';
                            break;
                        case 4:
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

            if (KeyMouseReader.KeyPressed(Keys.Up))
            {
                if (mySelection > 0)
                {
                    mySelection--;
                }
            }
            if (KeyMouseReader.KeyPressed(Keys.Down))
            {
                if (mySelection < 6)
                {
                    mySelection++;
                }
            }

            if (KeyMouseReader.KeyPressed(Keys.Enter))
            {
                switch (mySelection)
                {
                    case 5:

                        break;
                    case 6:
                        SaveLevel();
                        break;
                }
            }
            if (KeyMouseReader.KeyPressed(Keys.Escape))
            {
                myGame.ChangeState(new MenuState(myGame));
            }

            Level.Update();
        }

        public override void Draw(SpriteBatch aSpriteBatch, GameWindow aWindow, GameTime aGameTime)
        {
            Level.DrawTiles(aSpriteBatch);

            StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, ">",
                new Vector2(Level.MapSize.X + 20, 70 + (40 * mySelection)),
                Color.GhostWhite, 0.6f);

            aSpriteBatch.Draw(myBlock, new Vector2(Level.MapSize.X + 50, 70), null, Color.White, 0.0f,
                new Vector2(myBlock.Width / 2, myBlock.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            aSpriteBatch.Draw(mySnack, new Vector2(Level.MapSize.X + 50, 110), null, Color.White, 0.0f,
                new Vector2(mySnack.Width / 2, mySnack.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            aSpriteBatch.Draw(myPowerUp, new Vector2(Level.MapSize.X + 50, 190), null, Color.White, 0.0f, 
                new Vector2(myPowerUp.Width / 2, myPowerUp.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
            aSpriteBatch.Draw(myGhost, new Vector2(Level.MapSize.X + 50, 230), new Rectangle(0, 0, myGhost.Width / 2, myGhost.Height), Color.White, 0.0f, 
                new Vector2(myGhost.Width / 4, myGhost.Height / 2), 1.0f, SpriteEffects.None, 0.0f);

            StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "LOAD", new Vector2(Level.MapSize.X + 40, 270), Color.White, 1.0f);
            StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "SAVE", new Vector2(Level.MapSize.X + 40, 310), Color.White, 1.0f);
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

                    if (tempResult != tempLevel)
                    {
                        tempLevel = (i + 1);
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

        public override void LoadContent()
        {
            Level.SetTileTexture();

            my8bitFont = ResourceManager.RequestFont("8-bit");

            myBlock = ResourceManager.RequestTexture("Tile_Block-0");
            mySnack = ResourceManager.RequestTexture("Snack");
            myPowerUp = ResourceManager.RequestTexture("PowerUp_00");
            myGhost = ResourceManager.RequestTexture("Ghost");
        }
    }
}
