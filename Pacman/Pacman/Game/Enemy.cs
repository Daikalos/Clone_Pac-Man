﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    class Enemy : GameObject
    {
        private enum BehaviourAI
        {
            isChasing,
            isRandom,
            isFullRandom,
            isFleeing,
        }

        private AnimationManager myWalkingAnimation;
        private Texture2D myEyesTexture;
        private List<Tile> myPath;
        private Color myColor;
        private BehaviourAI myBehaviour;
        private Vector2
            myDestination,
            myDirection;

        /// <summary>
        /// 0 = Up; 1 = Left; 2 = Down; 3 = Right;
        /// </summary>
        private int myAngle;
        private int 
            myAIType,
            myWalkToTile,
            myEyesDirection;
        private float mySpeed;
        private bool 
            mySwitchDirection,
            myIsAlive;

        public bool IsAlive
        {
            get => myIsAlive;
        }
        public int AIType
        {
            get => myAIType;
        }

        public Enemy(Vector2 aPosition, Point aSize, float aSpeed, int aAIType) : base(aPosition, aSize)
        {
            this.mySpeed = aSpeed;
            this.myAIType = aAIType;

            this.myWalkingAnimation = new AnimationManager(new Point(2, 1), 0.1f, true);
            this.myBoundingBox = new Rectangle((int)myPosition.X, (int)myPosition.Y, mySize.X, mySize.Y);
            this.myDestination = myBoundingBox.Center.ToVector2();
            this.myBehaviour = (BehaviourAI)myAIType;
            this.myPath = new List<Tile>();
            this.myIsAlive = true;
            this.mySwitchDirection = true;

            SetGhost();
        }

        public void Update(GameTime aGameTime, Player aPlayer)
        {
            myBoundingBox = new Rectangle((int)myPosition.X, (int)myPosition.Y, mySize.X, mySize.Y);

            switch (myBehaviour)
            {
                case BehaviourAI.isChasing:
                    IsChasing(aGameTime, aPlayer);
                    break;
                case BehaviourAI.isRandom:
                    IsRandom(aGameTime);
                    break;
                case BehaviourAI.isFullRandom:
                    IsFullRandom(aGameTime);
                    break;
                case BehaviourAI.isFleeing:
                    if ((BehaviourAI)myAIType != BehaviourAI.isFleeing)
                    {
                        IsFleeing(aGameTime, aPlayer, 5.0f);
                    }
                    else
                    {
                        IsFleeing(aGameTime, aPlayer, 1.0f); //Custom AI for fleeing type
                    }
                    break;
            }

            FleeingCheck(aPlayer);
            SetEyeDirection();
        }

        public void Draw(SpriteBatch aSpriteBatch, GameTime aGameTime)
        {
            myWalkingAnimation.DrawSpriteSheet(aSpriteBatch, aGameTime, myTexture, myPosition, myOrigin, new Point(32), new Point(32), myColor, 0.0f);

            if (myBehaviour != BehaviourAI.isFleeing || (BehaviourAI)myAIType == BehaviourAI.isFleeing)
            {
                if ((BehaviourAI)myAIType != BehaviourAI.isFleeing)
                {
                    aSpriteBatch.Draw(myEyesTexture, myPosition, new Rectangle((myEyesTexture.Width / 4) * myEyesDirection, 0, myEyesTexture.Width / 4, myEyesTexture.Height), Color.White);
                }
                else
                {
                    aSpriteBatch.Draw(myEyesTexture, myPosition, null, Color.White);
                }
            }
        }

        private void FleeingCheck(Player aPlayer)
        {
            if (aPlayer.IsEating)
            {
                if (myBehaviour != BehaviourAI.isFleeing)
                {
                    myBehaviour = BehaviourAI.isFleeing;
                    if ((BehaviourAI)myAIType != BehaviourAI.isFleeing)
                    {
                        myTexture = ResourceManager.RequestTexture("Ghost_Frightened");
                        myColor = Color.White;
                    }
                    mySwitchDirection = true;
                }
            }
            else if (myBehaviour == BehaviourAI.isFleeing)
            {
                myBehaviour = (BehaviourAI)myAIType;
                myTexture = ResourceManager.RequestTexture("Ghost");

                myDestination = myBoundingBox.Center.ToVector2();
                SetGhost();
            }
        }
        private void SetEyeDirection()
        {
            if (myDirection.X <= myDirection.Y)
            {
                if (myDirection.X < 0)
                {
                    myEyesDirection = 1;
                }
                if (myDirection.Y > 0)
                {
                    myEyesDirection = 3;
                }
            }
            if (myDirection.X > myDirection.Y)
            {
                if (myDirection.X > 0)
                {
                    myEyesDirection = 0;
                }
                if (myDirection.Y < 0)
                {
                    myEyesDirection = 2;
                }
            }
        }

        private void IsChasing(GameTime aGameTime, Player aPlayer)
        {
            if (mySwitchDirection)
            {
                myPath = Pathfinder.FindPath(myBoundingBox.Center.ToVector2(), aPlayer.BoundingBox.Center.ToVector2());

                if (myPath.Count > 1)
                {
                    myWalkToTile = 1;
                    mySwitchDirection = false;
                }
            }

            if (myPath.Count > 1)
            {
                if (Vector2.Distance(myPath[myWalkToTile].GetCenter(), myBoundingBox.Center.ToVector2()) <= 1.0f)
                {
                    if (myWalkToTile + 1 < myPath.Count)
                    {
                        myWalkToTile++;
                    }
                    mySwitchDirection = true;
                }

                myDirection = myPath[myWalkToTile].GetCenter() - myBoundingBox.Center.ToVector2();
                myDirection = Normalize(myDirection);

                myPosition += myDirection * mySpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }
        }
        private void IsRandom(GameTime aGameTime)
        {
            if (mySwitchDirection)
            {
                int tempRNG = StaticRandom.RandomNumber(0, 4);
                myAngle = tempRNG;
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
                mySwitchDirection = false;

                myDirection = myDestination - myBoundingBox.Center.ToVector2();
                myDirection = Normalize(myDirection);

                myPosition += myDirection * mySpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                mySwitchDirection = true;
            }
        }
        private void IsFullRandom(GameTime aGameTime)
        {
            if (mySwitchDirection)
            {
                int tempRNG = StaticRandom.RandomNumber(0, 4);
                myAngle = tempRNG;

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
            }

            if (Vector2.Distance(myDestination, myBoundingBox.Center.ToVector2()) > 1.0f)
            {
                mySwitchDirection = false;

                myDirection = myDestination - myBoundingBox.Center.ToVector2();
                myDirection = Normalize(myDirection);

                myPosition += myDirection * mySpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                mySwitchDirection = true;
            }
        }
        private void IsFleeing(GameTime aGameTime, Player aPlayer, float aDistance)
        {
            if (mySwitchDirection)
            {
                float tempXDiff = myBoundingBox.Center.X - aPlayer.BoundingBox.Center.X;
                float tempYDiff = myBoundingBox.Center.Y - aPlayer.BoundingBox.Center.Y;
                float tempDirection = (float)Math.Atan2(tempYDiff, tempXDiff);

                Vector2 tempDestination = new Vector2(
                    myBoundingBox.Center.X + Level.TileSize.X * aDistance * (float)Math.Cos(tempDirection), 
                    myBoundingBox.Center.Y + Level.TileSize.Y * aDistance * (float)Math.Sin(tempDirection));

                myPath = Pathfinder.FindPath(myBoundingBox.Center.ToVector2(), tempDestination);

                if (myPath.Count > 1)
                {
                    myWalkToTile = 1;
                    mySwitchDirection = false;
                }
            }

            if (myPath.Count > 1)
            {
                if (Vector2.Distance(myPath[myWalkToTile].GetCenter(), myBoundingBox.Center.ToVector2()) <= 1.0f)
                {
                    if (myWalkToTile + 1 < myPath.Count)
                    {
                        myWalkToTile++;
                    }
                    mySwitchDirection = true;
                }

                myDirection = myPath[myWalkToTile].GetCenter() - myBoundingBox.Center.ToVector2();
                myDirection = Normalize(myDirection);

                myPosition += myDirection * mySpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }

            if ((BehaviourAI)myAIType != BehaviourAI.isFleeing)
            {
                if (Vector2.Distance(myBoundingBox.Center.ToVector2(), aPlayer.BoundingBox.Center.ToVector2()) < Level.TileSize.X / 2)
                {
                    myIsAlive = false;
                    GameInfo.AddScore(aPlayer.BoundingBox.Center.ToVector2(), 300);
                }
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
        }
        private void SetGhost()
        {
            myEyesTexture = ResourceManager.RequestTexture("Ghost-Eyes");
            switch (myAIType)
            {
                case 0:
                    myColor = new Color(200, 0, 0);
                    break;
                case 1:
                    myColor = new Color(0, 200, 0);
                    break;
                case 2:
                    myColor = new Color(255, 160, 0);
                    break;
                case 3:
                    myColor = new Color(0, 0, 200);
                    myEyesTexture = ResourceManager.RequestTexture("Ghost-Eyes_Frightened");
                    break;
            }
        }
        private Vector2 Normalize(Vector2 aVector)
        {
            if (aVector != Vector2.Zero)
            {
                aVector.Normalize();
                return aVector;
            }
            return new Vector2();
        }
    }
}
