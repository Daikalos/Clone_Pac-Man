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

        SpriteFont my8bitFont;
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

            EnemyManager.Initialize();
            GameInfo.Initialize(Window, 0.5f);

            EnemyManager.AddEnemy();
            myPlayer = new Player(new Vector2(Level.TileSize.X * 12, Window.ClientBounds.Height - Level.TileSize.Y * 2), new Point(32), 140.0f);

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
            ResourceManager.AddTexture("Snack", this.Content.Load<Texture2D>("Sprites/Tileset/snack"));
            ResourceManager.AddTexture("Empty", this.Content.Load<Texture2D>("Sprites/Tileset/empty"));
            ResourceManager.AddTexture("Black_Rect", this.Content.Load<Texture2D>("Sprites/blackRect"));
            ResourceManager.AddTexture("Pacman_Walking", this.Content.Load<Texture2D>("Sprites/pacman_walking"));
            ResourceManager.AddTexture("Ghost", this.Content.Load<Texture2D>("Sprites/ghost"));

            Level.SetTileTexture();
            EnemyManager.SetTexture();
            GameInfo.SetRectTexture("Black_Rect");
            myPlayer.SetTexture("Pacman_Walking");

            my8bitFont = ResourceManager.RequestFont("8-bit");

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
                    EnemyManager.Update(gameTime);
                    myPlayer.Update(gameTime);
                    GameInfo.Update(gameTime);
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
                    EnemyManager.Draw(spriteBatch, gameTime);
                    myPlayer.Draw(spriteBatch, gameTime);
                    GameInfo.Draw(spriteBatch, Window, my8bitFont);
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
