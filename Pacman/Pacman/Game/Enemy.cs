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
        private List<Tile> myPath;
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
            myWalkToTile;
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
            this.myPath = new List<Tile>();
            this.myIsAlive = true;
            this.mySwitchAngle = true;
            this.myAngle = 0;
        }

        public void Update(GameTime aGameTime, Player aPlayer)
        {
            myBoundingBox = new Rectangle((int)myPosition.X, (int)myPosition.Y, mySize.X, mySize.Y);

            switch(myBehaviour)
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
                    IsFleeing(aGameTime, aPlayer);
                    break;
            }
        }

        public void Draw(SpriteBatch aSpriteBatch, GameTime aGameTime)
        {
            myWalkingAnimation.DrawSpriteSheet(aSpriteBatch, aGameTime, myTexture, myPosition, myOrigin, new Point(32), new Point(32), Color.White, 0.0f);
        }

        private void IsChasing(GameTime aGameTime, Player aPlayer)
        {
            if (mySwitchAngle)
            {
                myPath = Pathfinder.FindPath(myBoundingBox.Center.ToVector2(), aPlayer.BoundingBox.Center.ToVector2());

                if (myPath.Count > 1)
                {
                    myWalkToTile = 1;
                    mySwitchAngle = false;
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
                    mySwitchAngle = true;
                }

                myDirection = myPath[myWalkToTile].GetCenter() - myBoundingBox.Center.ToVector2();
                myDirection = Normalize(myDirection);

                myPosition += myDirection * mySpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }

        }
        private void IsRandom(GameTime aGameTime)
        {
            if (mySwitchAngle)
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
                mySwitchAngle = false;

                myDirection = myDestination - myBoundingBox.Center.ToVector2();
                myDirection = Normalize(myDirection);

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
                mySwitchAngle = false;

                myDirection = myDestination - myBoundingBox.Center.ToVector2();
                myDirection = Normalize(myDirection);

                myPosition += myDirection * mySpeed * (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                mySwitchAngle = true;
            }
        }
        private void IsFleeing(GameTime aGameTime, Player aPlayer)
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
