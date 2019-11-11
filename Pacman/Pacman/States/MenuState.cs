using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class MenuState : State
    {
        private SpriteFont my8bitFont;
        /// <summary>
        /// 0 = Play;
        /// 1 = Editor;
        /// 2 = HighScore;
        /// 3 = Exit;
        /// </summary>
        private int 
            mySelection,
            mySelectionAmount;
        private string[] myLevelNames;
        private bool myLoadLevel;

        public MenuState(MainGame aGame) : base(aGame)
        {
            this.mySelection = 0;
            this.mySelection = 3;
        }

        public override void Update(GameWindow aWindow, GameTime aGameTime)
        {
            if (!myLoadLevel)
            {
                if (KeyMouseReader.KeyPressed(Keys.Enter))
                {
                    switch (mySelection)
                    {
                        case 0:
                            myLoadLevel = true;
                            myLevelNames = FileReader.FindFileNames("../../../../Levels/");
                            mySelectionAmount = myLevelNames.Length - 2;
                            break;
                        case 1:
                            myGame.ChangeState(new EditorState(myGame));
                            break;
                        case 2:
                            myGame.ChangeState(new LeaderboardState(myGame));
                            break;
                        case 3:
                            myGame.Exit();
                            break;
                    }
                }
            }
            else
            {
                if (KeyMouseReader.KeyPressed(Keys.Back))
                {
                    myGame.ChangeState(new MenuState(myGame));
                }
                if (KeyMouseReader.KeyPressed(Keys.Enter))
                {
                    GameInfo.CurrentLevel = myLevelNames[mySelection];
                    myGame.ChangeState(new PlayState(myGame, aWindow));
                }
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
        }

        public override void Draw(SpriteBatch aSpriteBatch, GameWindow aWindow, GameTime aGameTime)
        {
            StringManager.DrawStringMid(aSpriteBatch, my8bitFont, "Pac-Man", new Vector2(aWindow.ClientBounds.Width / 2, (aWindow.ClientBounds.Height / 2) - 60), Color.Yellow, 1.8f);

            StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, ">", 
                new Vector2((aWindow.ClientBounds.Width / 2) - 90, (aWindow.ClientBounds.Height / 2) + 20 + (30 * mySelection)), 
                Color.GhostWhite, 0.6f);

            if (!myLoadLevel)
            {
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "Play",
                    new Vector2((aWindow.ClientBounds.Width / 2) - 70, (aWindow.ClientBounds.Height / 2) + 20),
                    Color.White, 0.7f);
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "Editor",
                    new Vector2((aWindow.ClientBounds.Width / 2) - 70, (aWindow.ClientBounds.Height / 2) + 50),
                    Color.White, 0.7f);
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "HighScore",
                    new Vector2((aWindow.ClientBounds.Width / 2) - 70, (aWindow.ClientBounds.Height / 2) + 80),
                    Color.White, 0.7f);
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "Exit",
                    new Vector2((aWindow.ClientBounds.Width / 2) - 70, (aWindow.ClientBounds.Height / 2) + 110),
                    Color.White, 0.7f);
            }
            else
            {
                for (int i = 0; i < myLevelNames.Length; i++)
                {
                    string tempName = myLevelNames[i];
                    tempName = tempName.Replace(".txt", "");

                    if (tempName != "Level_Template")
                    {
                        StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, tempName,
                            new Vector2((aWindow.ClientBounds.Width / 2) - 70, (aWindow.ClientBounds.Height / 2) + 20 + (30 * i)),
                            Color.White, 0.7f);
                    }
                }
                StringManager.DrawStringLeft(aSpriteBatch, my8bitFont, "Press return to go back to menu", new Vector2(12, aWindow.ClientBounds.Height - 12), Color.DarkOrange, 0.5f);
            }
        }

        public override void LoadContent()
        {
            my8bitFont = ResourceManager.RequestFont("8-bit");
        }
    }
}
