using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    class WinState : State
    {
        SpriteFont my8bitFont;

        public WinState(MainGame aGame) : base(aGame)
        {

        }

        public override void Update(GameWindow aWindow, GameTime aGameTime)
        {
            
        }

        public override void Draw(SpriteBatch aSpriteBatch, GameWindow aWindow, GameTime aGameTime)
        {
            
        }

        public override void LoadContent()
        {
            my8bitFont = ResourceManager.RequestFont("8-bit");
        }
    }
}
