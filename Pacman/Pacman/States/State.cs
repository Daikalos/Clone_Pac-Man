﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    public abstract class State
    {
        protected MainGame myGame;

        public State(MainGame aGame)
        {
            this.myGame = aGame;
        }

        public abstract void Update(GameWindow aWindow, GameTime aGameTime);

        public abstract void Draw(SpriteBatch aSpriteBatch, GameWindow aWindow, GameTime aGameTime);

        public abstract void LoadContent();
    }
}
