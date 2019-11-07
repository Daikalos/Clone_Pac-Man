﻿using System;
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

        private Animation myWalkingAnimation;

        private Rectangle myDrawBox;
        private Vector2
            myDestination,
            myDirection;
        private PlayerState myPlayerState;

        /// <summary>
        /// 0 = Up; 1 = Left; 2 = Down; 3 = Right;
        /// </summary>
        private int myAngle;
        private float 
            mySpeed,
            myRotation;
        private bool 
            mySwitchAngle,
            myIsMoving;

        public Player(Vector2 aPosition, Point aSize, float aSpeed) : base(aPosition, aSize)
        {
            this.mySpeed = aSpeed;

            this.myWalkingAnimation = new Animation(new Point(4, 1), 0.1f, true);
            this.myPlayerState = PlayerState.isWalking;
            this.myOrigin = Vector2.Zero;
        }

        public void Update(GameTime aGameTime)
        {
            myBoundingBox = new Rectangle((int)myPosition.X, (int)myPosition.Y, mySize.X, mySize.Y);
            myDrawBox = new Rectangle((int)myPosition.X + (int)myOrigin.X, (int)myPosition.Y + (int)myOrigin.Y, mySize.X, mySize.Y);

            switch(myPlayerState)
            {
                case PlayerState.isWalking:
                    Movement(aGameTime);
                    OutsideBounds();
                    break;
                case PlayerState.isEating:

                    break;
                case PlayerState.isDead:

                    break;
            }

            CollisionCheck();
        }

        public void Draw(SpriteBatch aSpriteBatch, GameTime aGameTime)
        {
            switch(myPlayerState)
            {
                case PlayerState.isWalking:
                    if (myIsMoving)
                    {
                        myWalkingAnimation.DrawSpriteSheet(aSpriteBatch, aGameTime, myTexture, myDrawBox.Location.ToVector2(), myOrigin, new Point(32), mySize, Color.White, myRotation);
                    }
                    else
                    {
                        aSpriteBatch.Draw(myTexture, myDrawBox, new Rectangle(myTexture.Width / 4, 0, myTexture.Width / 4, myTexture.Height), Color.White, myRotation, myOrigin, SpriteEffects.None, 0.0f);
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
                    myRotation = -(MathHelper.Pi / 2);
                    myAngle = 0;
                }
                else if (KeyMouseReader.KeyHold(Keys.Left) && !IsTileBlock(new Vector2(-Level.TileSize.X, 0)))
                {
                    myRotation = MathHelper.Pi;
                    myAngle = 1;
                }
                else if (KeyMouseReader.KeyHold(Keys.Down) && !IsTileBlock(new Vector2(0, Level.TileSize.Y)))
                {
                    myRotation = MathHelper.Pi / 2;
                    myAngle = 2;
                }
                else if (KeyMouseReader.KeyHold(Keys.Right) && !IsTileBlock(new Vector2(Level.TileSize.X, 0)))
                {
                    myRotation = 0.0f;
                    myAngle = 3;
                }
            }

            switch (myAngle)
            {
                case 0:
                    MoveTo(new Vector2(0, -Level.TileSize.Y));
                    break;
                case 1:
                    MoveTo(new Vector2(-Level.TileSize.X, 0));
                    break;
                case 2:
                    MoveTo(new Vector2(0, Level.TileSize.Y));
                    break;
                case 3:
                    MoveTo(new Vector2(Level.TileSize.X, 0));
                    break;
            }


            if (Vector2.Distance(myDestination, myBoundingBox.Center.ToVector2()) > 1.0f)
            {
                myIsMoving = true;

                myDirection = myDestination - myBoundingBox.Center.ToVector2();
                myDirection.Normalize();

                myPosition += myDirection * mySpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                myIsMoving = false;
            }

            if (Vector2.Distance(Level.GetTileAtPos(myBoundingBox.Center.ToVector2()).Item1.GetCenter(), myBoundingBox.Center.ToVector2()) > 1.0f)
            {
                mySwitchAngle = false;
                if (KeyMouseReader.KeyHold(Keys.Up))
                {
                    if (myAngle == 2)
                    {
                        mySwitchAngle = true;
                    }
                }
                else if (KeyMouseReader.KeyHold(Keys.Left))
                {
                    if (myAngle == 3)
                    {
                        mySwitchAngle = true;
                    }
                }
                else if (KeyMouseReader.KeyHold(Keys.Down))
                {
                    if (myAngle == 0)
                    {
                        mySwitchAngle = true;
                    }
                }
                else if (KeyMouseReader.KeyHold(Keys.Right))
                {
                    if (myAngle == 1)
                    {
                        mySwitchAngle = true;
                    }
                }
            }
            else
            {
                mySwitchAngle = true;
            }
        }
        private void MoveTo(Vector2 aPosition)
        {
            Vector2 tempDirection = new Vector2(myBoundingBox.Center.X + aPosition.X, myBoundingBox.Center.Y + aPosition.Y);
            if (Level.GetTileAtPos(tempDirection).Item2)
            {
                if (Level.GetTileAtPos(tempDirection).Item1.TileType != '#')
                {
                    myDestination = Level.GetTileAtPos(tempDirection).Item1.GetCenter();
                }
            }
            else //If headed outside map
            {
                switch (myAngle)
                {
                    case 0:
                        myDestination = new Vector2(myBoundingBox.Center.X, myBoundingBox.Center.Y - Level.TileSize.Y);
                        break;
                    case 1:
                        myDestination = new Vector2(myBoundingBox.Center.X - Level.TileSize.X, myBoundingBox.Center.Y);
                        break;
                    case 2:
                        myDestination = new Vector2(myBoundingBox.Center.X, myBoundingBox.Center.Y + Level.TileSize.Y);
                        break;
                    case 3:
                        myDestination = new Vector2(myBoundingBox.Center.X + Level.TileSize.X, myBoundingBox.Center.Y);
                        break;
                }
            }
        }
        private void OutsideBounds()
        {
            if (myPosition.X > Level.MapSize.X)
            {
                myPosition.X = -mySize.X;
            }
            if (myPosition.X < -mySize.X)
            {
                myPosition.X = Level.MapSize.X;
            }
            if (myPosition.Y > Level.MapSize.Y)
            {
                myPosition.Y = -mySize.Y;
            }
            if (myPosition.Y < -mySize.Y)
            {
                myPosition.Y = Level.MapSize.Y;
            }
        }
        private bool IsTileBlock(Vector2 aPosition)
        {
            if (Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X + aPosition.X, myBoundingBox.Center.Y + aPosition.Y)).Item1.TileType == '#')
            {
                return true;
            }
            return false;
        }

        private void CollisionCheck()
        {
            CollisionSnack();
        }
        private void CollisionSnack()
        {
            Tile tempTile = Level.GetTileAtPos(myBoundingBox.Center.ToVector2()).Item1;
            if (tempTile.TileType == '.')
            {
                GameInfo.AddScore(myBoundingBox.Center.ToVector2(), 100);

                tempTile.TileType = '-';
                tempTile.SetTexture();
            }
        }

        public override void SetTexture(string aName)
        {
            myTexture = ResourceManager.RequestTexture(aName);
            SetOrigin(new Point(4, 1));
        }
    }
}
