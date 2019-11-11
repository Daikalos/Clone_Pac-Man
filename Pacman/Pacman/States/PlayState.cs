using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class PlayState : State
    {
        private Player myPlayer;
        private SpriteFont my8bitFont;
        private bool myIsPaused;

        public PlayState(MainGame aGame, GameWindow aWindow) : base(aGame)
        {
            Level.LoadLevel(new Point(32));
            aGame.IsMouseVisible = false;

            EnemyManager.Initialize(10.0f);
            EnemyManager.AddEnemies();

            this.myPlayer = new Player(new Vector2(Level.TileSize.X * 12, aWindow.ClientBounds.Height - Level.TileSize.Y * 2), Level.TileSize, 140.0f, 9.0f);
            this.myIsPaused = false;
        }

        public override void Update(GameWindow aWindow, GameTime aGameTime)
        {
            if (!myIsPaused)
            {
                Level.Update();
                myPlayer.Update(aGameTime);
                EnemyManager.Update(aGameTime, myPlayer);
                GameInfo.Update(aGameTime);
            }
            else
            {
                if (KeyMouseReader.KeyPressed(Keys.Back))
                {
                    myGame.ChangeState(new MenuState(myGame));
                }
            }

            if (KeyMouseReader.KeyPressed(Keys.Escape))
            {
                if (!myIsPaused)
                {
                    myIsPaused = true;
                }
                else
                {
                    myIsPaused = false;
                }
            }
        }

        public override void Draw(SpriteBatch aSpriteBatch, GameWindow aWindow, GameTime aGameTime)
        {
            if (!myIsPaused)
            {
                Level.DrawTiles(aSpriteBatch);
                myPlayer.Draw(aSpriteBatch, aGameTime);
                EnemyManager.Draw(aSpriteBatch, aGameTime);
                GameInfo.Draw(aSpriteBatch, aWindow, my8bitFont);
            }
            else
            {
                StringManager.DrawStringMid(aSpriteBatch, my8bitFont, "PAUSED", new Vector2(aWindow.ClientBounds.Width / 2, aWindow.ClientBounds.Height / 2), Color.DarkOrange, 2.0f);
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "Press return to go back to menu", new Vector2(12, aWindow.ClientBounds.Height - 12), Color.DarkOrange, 0.5f);
            }
        }

        public override void LoadContent()
        {
            Level.SetTileTexture();
            myPlayer.SetTexture("Pacman_Walking");

            my8bitFont = ResourceManager.RequestFont("8-bit");
        }
    }
}
