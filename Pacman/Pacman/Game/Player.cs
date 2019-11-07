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
            myDirection,
            myOrigin;
        PlayerState myPlayerState:

        /// <summary>
        /// 0 = Up;
        /// 1 = Left;
        /// 2 = Down;
        /// 3 = Right;
        /// </summary>
        int myAngle;
        bool 
            mySwitchAngle,
            myIsMoving;

        public Player(Vector2 aPosition, Point aSize) : base(aPosition, aSize)
        {
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
                    if (myIsMoving)
                    {
                        myWalkingAnimation.DrawSpriteSheet(aSpriteBatch, aGameTime, myTexture, myPosition, Vector2.Zero, new Point(32), mySize, Color.White, SpriteEffects.None);
                    }
                    else
                    {
                        aSpriteBatch.Draw(myTexture, myPosition, new Rectangle(myTexture.Width / 4, 0, myTexture.Width / 4, myTexture.Height), Color.White);
                    }
                    break;
                case PlayerState.isEating:

                    break;
                case PlayerState.isDead:

                    break;
            }
        }

        private void Movement(GameTime aGameTime)
        {
            if (mySwitchAngle)
            {
                if (KeyMouseReader.KeyHold(Keys.Up) && !IsTileBlock(new Vector2(0, -Level.TileSize.Y)))
                {
                    if (myAngle == 2)
                    {
                        mySwitchAngle = true;
                    }
                    myAngle = 0;
                }
                if (KeyMouseReader.KeyHold(Keys.Left) && !IsTileBlock(new Vector2(-Level.TileSize.X, 0)))
                {
                    if (myAngle == 3)
                    {
                        mySwitchAngle = true;
                    }
                    myAngle = 1;
                }
                if (KeyMouseReader.KeyHold(Keys.Down) && !IsTileBlock(new Vector2(0, Level.TileSize.Y)))
                {
                    if (myAngle == 0)
                    {
                        mySwitchAngle = true;
                    }
                    myAngle = 2;
                }
                if (KeyMouseReader.KeyHold(Keys.Right) && !IsTileBlock(new Vector2(Level.TileSize.X, 0)))
                {
                    if (myAngle == 1)
                    {
                        mySwitchAngle = true;
                    }
                    myAngle = 3;
                }
            }

            switch (myAngle)
            {
                case 0:
                    MoveTo(Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X, myBoundingBox.Center.Y - Level.TileSize.Y)).GetCenter());
                    break;
                case 1:
                    MoveTo(Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X - Level.TileSize.X, myBoundingBox.Center.Y)).GetCenter());
                    break;
                case 2:
                    MoveTo(Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X, myBoundingBox.Center.Y + Level.TileSize.Y)).GetCenter());
                    break;
                case 3:
                    MoveTo(Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X + Level.TileSize.X, myBoundingBox.Center.Y)).GetCenter());
                    break;
            }


            if (Vector2.Distance(myDestination, myBoundingBox.Center.ToVector2()) > 1.0f)
            {
                myIsMoving = true;

                myDirection = myDestination - myBoundingBox.Center.ToVector2();
                myDirection.Normalize();

                myPosition += myDirection * (float)aGameTime.ElapsedGameTime.TotalSeconds * 180;
            }
            else
            {
                myIsMoving = false;
            }

            if (Vector2.Distance(Level.GetTileAtPos(myBoundingBox.Center.ToVector2()).GetCenter(), myBoundingBox.Center.ToVector2()) > 1.0f)
            {
                mySwitchAngle = false;
            }
            else
            {
                mySwitchAngle = true;
            }

            if (!mySwitchAngle) //Provide better movement control
            {
                if (KeyMouseReader.KeyHold(Keys.Up))
                {
                    if (myAngle == 2)
                    {
                        mySwitchAngle = true;
                    }
                }
                if (KeyMouseReader.KeyHold(Keys.Left))
                {
                    if (myAngle == 3)
                    {
                        mySwitchAngle = true;
                    }
                }
                if (KeyMouseReader.KeyHold(Keys.Down))
                {
                    if (myAngle == 0)
                    {
                        mySwitchAngle = true;
                    }
                }
                if (KeyMouseReader.KeyHold(Keys.Right))
                {
                    if (myAngle == 1)
                    {
                        mySwitchAngle = true;
                    }
                }
            }
        }
        public void MoveTo(Vector2 aPosition)
        {
            if (Level.GetTileAtPos(aPosition).TileType != '#')
            {
                myDestination = Level.GetTileAtPos(aPosition).GetCenter();
            }
        }
        public bool IsTileBlock(Vector2 aPosition)
        {
            if (Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X + aPosition.X, myBoundingBox.Center.Y + aPosition.Y)).TileType == '#')
            {
                return true;
            }
            return false;
        }
    }
}
