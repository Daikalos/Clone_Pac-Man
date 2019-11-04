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

            myPlayer = new Player(new Vector2(Level.TileSize.X * 12, Window.ClientBounds.Height - Level.TileSize.Y * 2), new Point(32));

            myGameState = GameState.isPlaying;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ResourceManager.AddFont("8-bit", this.Content.Load<SpriteFont>("Fonts/8bit")); 

            ResourceManager.AddTexture("Tile_Block-0", this.Content.Load<Texture2D>("Sprites/Tileset/tile_block-0"));
            ResourceManager.AddTexture("Tile_Block-1", this.Content.Load<Texture2D>("Sprites/Tileset/tile_block-1"));
            ResourceManager.AddTexture("Tile_Block-2", this.Content.Load<Texture2D>("Sprites/Tileset/tile_block-2"));
            ResourceManager.AddTexture("Tile_Block-3", this.Content.Load<Texture2D>("Sprites/Tileset/tile_block-3"));
            ResourceManager.AddTexture("Empty", this.Content.Load<Texture2D>("Sprites/Tileset/empty"));

            ResourceManager.AddTexture("Pacman_Walking", this.Content.Load<Texture2D>("Sprites/pacman_walking"));

            Level.SetTileTexture();

            myPlayer.SetTexture("Pacman_Walking");
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
                    myPlayer.Update(gameTime);
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
                    myPlayer.Draw(spriteBatch, gameTime);
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
