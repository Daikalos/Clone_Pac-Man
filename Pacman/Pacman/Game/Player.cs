using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class Player : GameObject
    {
        private enum PlayerState
        {
            isWalking,
            isEating,
            isDead
        }

        Animation myWalkingAnimation;

        Vector2 
            myDestination,
            myDirection;
        PlayerState myPlayerState;
        SpriteEffects myFlipSprite;

        int myAngle;
        bool myCanSwitchAngle;

        public Player(Vector2 aPosition, Point aSize) : base(aPosition, aSize)
        {
            this.myPosition = Level.GetTileAtPos(aPosition).Position;

            this.myBoundingBox = new Rectangle((int)myPosition.X, (int)myPosition.Y, mySize.X, mySize.Y);
            this.myDirection = Vector2.Zero;
            this.myDestination = myBoundingBox.Center.ToVector2();
            this.myWalkingAnimation = new Animation(new Point(4, 1), 0.1f, true);
            this.myPlayerState = PlayerState.isWalking;
        }

        public void Update(GameTime aGameTime)
        {
            myBoundingBox = new Rectangle((int)myPosition.X, (int)myPosition.Y, mySize.X, mySize.Y);

            switch(myPlayerState)
            {
                case PlayerState.isWalking:
                    Movement(aGameTime);
                    break;
                case PlayerState.isEating:

                    break;
                case PlayerState.isDead:

                    break;
            }
        }

        public void Draw(SpriteBatch aSpriteBatch, GameTime aGameTime)
        {
            myBoundingBox = new Rectangle((int)myPosition.X, (int)myPosition.Y, mySize.X, mySize.Y);

            switch(myPlayerState)
            {
                case PlayerState.isWalking:
                    myWalkingAnimation.DrawSpriteSheet(aSpriteBatch, aGameTime, myTexture, myPosition, Vector2.Zero, new Point(32), mySize, Color.White, SpriteEffects.None);
                    break;
                case PlayerState.isEating:

                    break;
                case PlayerState.isDead:

                    break;
            }
        }

        private void Movement(GameTime aGameTime)
        {
            if (myCanSwitchAngle)
            {
                if (KeyMouseReader.KeyHold(Keys.Up) && Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X, myBoundingBox.Center.Y - Level.TileSize.Y)).TileType != '#')
                {
                    myDestination = Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X, myBoundingBox.Center.Y - Level.TileSize.Y)).GetBoundingBox().Center.ToVector2();
                }

                if (KeyMouseReader.KeyHold(Keys.Left) && Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X - Level.TileSize.X, myBoundingBox.Center.Y)).TileType != '#')
                {
                    myDestination = Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X - Level.TileSize.X, myBoundingBox.Center.Y)).GetBoundingBox().Center.ToVector2();
                }

                if (KeyMouseReader.KeyHold(Keys.Down) && Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X, myBoundingBox.Center.Y + Level.TileSize.Y)).TileType != '#')
                {
                    myDestination = Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X, myBoundingBox.Center.Y + Level.TileSize.Y)).GetBoundingBox().Center.ToVector2();
                }

                if (KeyMouseReader.KeyHold(Keys.Right) && Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X + Level.TileSize.X, myBoundingBox.Center.Y)).TileType != '#')
                {
                    myDestination = Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X + Level.TileSize.X, myBoundingBox.Center.Y)).GetBoundingBox().Center.ToVector2();
                }
            }

            if (Vector2.Distance(myDestination, myBoundingBox.Center.ToVector2()) > 1.0f)
            {
                myCanSwitchAngle = false;

                myDirection = myDestination - myBoundingBox.Center.ToVector2();
                myDirection.Normalize();

                myPosition += myDirection * (float)aGameTime.ElapsedGameTime.TotalSeconds * 180;
            }
            else
            {
                myCanSwitchAngle = true;
            }
        }
    }
}
