using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    public class MainGame : Game
    {
        enum GameState
        {
            isOnMenu,
            isPlaying,
            isPaused,
            isDead,
            isWon
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player myPlayer;

        GameState myGameState;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1120;
            graphics.PreferredBackBufferHeight = 992;
            graphics.ApplyChanges();

            ResourceManager.Initialize();
            Level.LoadLevel("../../../../Levels/Level01.txt");

            myGameState = GameState.isPlaying;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ResourceManager.AddTexture("Tileset", this.Content.Load<Texture2D>("Sprites/Tileset/tileset"));
            ResourceManager.AddTexture("Tile_Block-1", this.Content.Load<Texture2D>("Sprites/Tileset/tile_block-1"));
            ResourceManager.AddTexture("Tile_Block-2", this.Content.Load<Texture2D>("Sprites/Tileset/tile_block-2"));
            ResourceManager.AddTexture("Tile_Block-3", this.Content.Load<Texture2D>("Sprites/Tileset/tile_block-3"));
            ResourceManager.AddTexture("Empty", this.Content.Load<Texture2D>("Sprites/Tileset/empty"));

            Level.SetTileTexture();
        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyMouseReader.Update();
            
            switch (myGameState)
            {
                case GameState.isOnMenu:

                    break;
                case GameState.isPlaying:
                    Level.Update();
                    break;
                case GameState.isPaused:

                    break;
                case GameState.isDead:

                    break;
                case GameState.isWon:

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            switch (myGameState)
            {
                case GameState.isOnMenu:

                    break;
                case GameState.isPlaying:
                    Level.DrawTiles(spriteBatch);
                    break;
                case GameState.isPaused:

                    break;
                case GameState.isDead:

                    break;
                case GameState.isWon:

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
