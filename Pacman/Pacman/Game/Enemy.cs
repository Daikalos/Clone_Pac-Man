using System;
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

        private AnimationManager 
            myWalkingAnimation,
            myFleeingAnimation;
        private Texture2D myEyesTexture;
        private BehaviourAI myBehaviour;
        private Vector2
            myDestination,
            myDirection;

        /// <summary>
        /// 0 = Up; 1 = Left; 2 = Down; 3 = Right;
        /// </summary>
        private int myAngle;
        private int myAIType;
        private float mySpeed;
        private bool 
            mySwitchAngle,
            myIsAlive;

        public bool IsAlive
        {
            get => myIsAlive;
        }

        public Enemy(Vector2 aPosition, Point aSize, float aSpeed, int aAIType) : base(aPosition, aSize)
        {
            this.mySpeed = aSpeed;
            this.myBehaviour = (BehaviourAI)aAIType;

            this.myWalkingAnimation = new AnimationManager(new Point(2, 1), 0.1f, true);
            this.myBoundingBox = new Rectangle((int)myPosition.X, (int)myPosition.Y, mySize.X, mySize.Y);
            this.myDestination = myBoundingBox.Center.ToVector2();
            this.myIsAlive = true;
            this.myAngle = 0;
        }

        public void Update(GameTime aGameTime)
        {
            myBoundingBox = new Rectangle((int)myPosition.X, (int)myPosition.Y, mySize.X, mySize.Y);

            switch(myBehaviour)
            {
                case BehaviourAI.isChasing:
                    IsChasing(aGameTime);
                    break;
                case BehaviourAI.isRandom:
                    IsRandom(aGameTime);
                    break;
                case BehaviourAI.isFullRandom:
                    IsFullRandom(aGameTime);
                    break;
                case BehaviourAI.isFleeing:
                    IsFleeing(aGameTime);
                    break;
            }
        }

        public void Draw(SpriteBatch aSpriteBatch, GameTime aGameTime)
        {
            myWalkingAnimation.DrawSpriteSheet(aSpriteBatch, aGameTime, myTexture, myPosition, myOrigin, new Point(32), new Point(32), Color.White, 0.0f);
        }

        private void IsChasing(GameTime aGameTime)
        {

        }
        private void IsRandom(GameTime aGameTime)
        {
            if (Level.GetTileAtPos(myBoundingBox.Center.ToVector2()).Item1.TileType != '-' && Level.GetTileAtPos(myBoundingBox.Center.ToVector2()).Item1.TileType != '&') //Prevent AI getting stuck in starting area
            {
                if (mySwitchAngle)
                {
                    int tempRNG = StaticRandom.RandomNumber(0, 4);
                    myAngle = tempRNG;
                }
            }
            else
            {
                myAngle = 0;
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
                mySwitchAngle = false;

                myDirection = myDestination - myBoundingBox.Center.ToVector2();
                myDirection.Normalize();

                myPosition += myDirection * mySpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                mySwitchAngle = true;
            }
        }
        private void IsFullRandom(GameTime aGameTime)
        {
            if (mySwitchAngle)
            {
                if (Level.GetTileAtPos(myBoundingBox.Center.ToVector2()).Item1.TileType != '-' && Level.GetTileAtPos(myBoundingBox.Center.ToVector2()).Item1.TileType != '&') //Prevent AI getting stuck in starting area
                {
                    int tempRNG = StaticRandom.RandomNumber(0, 4);
                    myAngle = tempRNG;
                }
                else
                {
                    myAngle = 0;
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
            }

            if (Vector2.Distance(myDestination, myBoundingBox.Center.ToVector2()) > 1.0f)
            {
                mySwitchAngle = false;

                myDirection = myDestination - myBoundingBox.Center.ToVector2();
                myDirection.Normalize();

                myPosition += myDirection * mySpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                mySwitchAngle = true;
            }
        }
        private void IsFleeing(GameTime aGameTime)
        {

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
        private bool IsTileBlock(Vector2 aPosition)
        {
            if (Level.GetTileAtPos(new Vector2(myBoundingBox.Center.X + aPosition.X, myBoundingBox.Center.Y + aPosition.Y)).Item1.TileType == '#')
            {
                return true;
            }
            return false;
        }
    }
}
