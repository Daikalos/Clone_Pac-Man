using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman.States
{
    class DeadState : State
    {
        public DeadState(MainGame aGame) : base(aGame)
        {

        }

        public override void Update(GameWindow aWindow, GameTime aGameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch aSpriteBatch, GameWindow aWindow, GameTime aGameTime)
        {
            throw new NotImplementedException();
        }

        public override void LoadContent()
        {
            throw new NotImplementedException();
        }
    }
}
