using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class EditorState : State
    {
        private Tile mySelectedTile;
        private Texture2D
            myBlock,
            myEmpty,
            myPowerUp;
        private int mySelection;

        public EditorState(MainGame aGame) : base(aGame)
        {
            Level.LoadLevel("../../../../Levels/Level_Template.txt");
            aGame.IsMouseVisible = true;
        }

        public override void Update(GameWindow aWindow, GameTime aGameTime)
        {
            switch(mySelection)
            {
                case 0:
                    if (mySelectedTile != Level.GetTileAtPos(KeyMouseReader.myCurrentMouseState.Position.ToVector2()).Item1)
                    {
                        if (KeyMouseReader.LeftHold())
                        {
                            mySelectedTile = Level.GetTileAtPos(KeyMouseReader.myCurrentMouseState.Position.ToVector2()).Item1;
                            if (mySelectedTile.TileType != '#')
                            {
                                mySelectedTile.TileType = '#';
                            }
                            mySelectedTile.TileForm = 0;
                            mySelectedTile.SetTexture();
                        }
                        if (KeyMouseReader.RightHold())
                        {
                            mySelectedTile = Level.GetTileAtPos(KeyMouseReader.myCurrentMouseState.Position.ToVector2()).Item1;
                            if (mySelectedTile.TileType == '#')
                            {
                                mySelectedTile.TileType = '-';
                            }
                            mySelectedTile.SetTexture();
                        }
                    }
                    break;
                case 1:

                    break;
                case 2:

                    break;
            }

            if (!KeyMouseReader.LeftHold() && !KeyMouseReader.RightHold())
            {
                mySelectedTile = null;
            }

            Level.Update();
        }

        public override void Draw(SpriteBatch aSpriteBatch, GameWindow aWindow, GameTime aGameTime)
        {
            Level.DrawTiles(aSpriteBatch);
        }

        public override void LoadContent()
        {
            Level.SetTileTexture();

            myBlock = ResourceManager.RequestTexture("Tile_Block-0");
            myEmpty = ResourceManager.RequestTexture("Empty");
            myPowerUp = ResourceManager.RequestTexture("PowerUp_00");
        }
    }
}
