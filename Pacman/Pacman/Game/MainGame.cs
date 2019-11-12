using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    public class MainGame : Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        State myGameState;

        public void ChangeState(State aNewState)
        {
            myGameState = aNewState;
            myGameState.LoadContent();
        }

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

            GameInfo.Initialize(Window, 0.5f);
            GameInfo.CurrentLevel = "Level01";
            GameInfo.FolderLevels = "../../../../Levels/";
            GameInfo.FolderHighScores = "../../../../HighScores/";
            GameInfo.LoadHighScore(GameInfo.FolderHighScores + "Level01_HighScores.txt");

            ChangeState(new MenuState(this));

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
            ResourceManager.AddTexture("PowerUp_00", this.Content.Load<Texture2D>("Sprites/Tileset/powerup_00"));
            ResourceManager.AddTexture("Empty", this.Content.Load<Texture2D>("Sprites/Tileset/empty"));
            ResourceManager.AddTexture("Tile_Ghost", this.Content.Load<Texture2D>("Sprites/Tileset/tile_ghost"));
            ResourceManager.AddTexture("Black_Rect", this.Content.Load<Texture2D>("Sprites/blackRect"));
            ResourceManager.AddTexture("Pacman_Walking", this.Content.Load<Texture2D>("Sprites/pacman_walking"));
            ResourceManager.AddTexture("Ghost", this.Content.Load<Texture2D>("Sprites/ghost"));
            ResourceManager.AddTexture("Ghost_Frightened", this.Content.Load<Texture2D>("Sprites/ghost_frightened"));
            ResourceManager.AddTexture("Ghost-Eyes", this.Content.Load<Texture2D>("Sprites/ghost-eyes"));
            ResourceManager.AddTexture("Ghost-Eyes_Frightened", this.Content.Load<Texture2D>("Sprites/ghost-eyes_frightened"));

            GameInfo.SetRectTexture("Black_Rect");

            myGameState.LoadContent();
        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyMouseReader.Update();

            myGameState.Update(Window, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            myGameState.Draw(spriteBatch, Window, gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
