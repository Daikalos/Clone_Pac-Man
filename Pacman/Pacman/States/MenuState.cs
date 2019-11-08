using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class MenuState : State
    {
        /// <summary>
        /// 0 = Play;
        /// 1 = Editor;
        /// 2 = HighScore;
        /// 3 = Exit;
        /// </summary>
        private int mySelection;
        private SpriteFont my8bitFont;

        public MenuState(MainGame aGame) : base(aGame)
        {
            this.mySelection = 0;
        }

        public override void Update(GameWindow aWindow, GameTime aGameTime)
        {
            if (KeyMouseReader.KeyPressed(Keys.Up))
            {
                if (mySelection > 0)
                {
                    mySelection--;
                }
            }
            if (KeyMouseReader.KeyPressed(Keys.Down))
            {
                if (mySelection < 3)
                {
                    mySelection++;
                }
            }

            if (KeyMouseReader.KeyPressed(Keys.Enter))
            {
                switch (mySelection)
                {
                    case 0:
                        myGame.ChangeState(new PlayState(myGame, aWindow));
                        break;
                    case 1:
                        myGame.ChangeState(new EditorState(myGame));
                        break;
                    case 2:
                        myGame.ChangeState(new HighScoreState(myGame));
                        break;
                    case 3:
                        myGame.Exit();
                        break;
                }
            }
        }

        public override void Draw(SpriteBatch aSpriteBatch, GameWindow aWindow, GameTime aGameTime)
        {
            StringManager.DrawStringMid(aSpriteBatch, my8bitFont, "Pac-Man", new Vector2(aWindow.ClientBounds.Width / 2, (aWindow.ClientBounds.Height / 2) - 60), Color.Yellow, 1.8f);

            StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, ">", 
                new Vector2((aWindow.ClientBounds.Width / 2) - 80, (aWindow.ClientBounds.Height / 2) + 20 + (30 * mySelection)), 
                Color.GhostWhite, 0.6f);

            StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "Play",
                new Vector2((aWindow.ClientBounds.Width / 2) - 60, (aWindow.ClientBounds.Height / 2) + 20),
                Color.White, 0.7f);
            StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "Editor",
                new Vector2((aWindow.ClientBounds.Width / 2) - 60, (aWindow.ClientBounds.Height / 2) + 50),
                Color.White, 0.7f);
            StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "HighScore",
                new Vector2((aWindow.ClientBounds.Width / 2) - 60, (aWindow.ClientBounds.Height / 2) + 80),
                Color.White, 0.7f);
            StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "Exit",
                new Vector2((aWindow.ClientBounds.Width / 2) - 60, (aWindow.ClientBounds.Height / 2) + 110),
                Color.White, 0.7f);
        }

        public override void LoadContent()
        {
            my8bitFont = ResourceManager.RequestFont("8-bit");
        }
    }
}
