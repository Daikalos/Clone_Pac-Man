﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class PlayState : State
    {
        private Player myPlayer;
        private SpriteFont my8bitFont;

        public PlayState(MainGame aGame, GameWindow aWindow) : base(aGame)
        {
            Level.LoadLevel("../../../../Levels/Level01.txt");

            EnemyManager.Initialize();
            EnemyManager.AddEnemy();

            myPlayer = new Player(new Vector2(Level.TileSize.X * 12, aWindow.ClientBounds.Height - Level.TileSize.Y * 2), Level.TileSize, 140.0f);
        }

        public override void Update(GameWindow aWindow, GameTime aGameTime)
        {
            Level.Update();
            GameInfo.Update(aGameTime);
            EnemyManager.Update(aGameTime, myPlayer);
            myPlayer.Update(aGameTime);
        }

        public override void Draw(SpriteBatch aSpriteBatch, GameWindow aWindow, GameTime aGameTime)
        {
            Level.DrawTiles(aSpriteBatch);
            GameInfo.Draw(aSpriteBatch, aWindow, my8bitFont);
            EnemyManager.Draw(aSpriteBatch, aGameTime);
            myPlayer.Draw(aSpriteBatch, aGameTime);
        }

        public override void LoadContent()
        {
            Level.SetTileTexture();
            EnemyManager.SetTexture();
            myPlayer.SetTexture("Pacman_Walking");

            my8bitFont = ResourceManager.RequestFont("8-bit");
        }
    }
}