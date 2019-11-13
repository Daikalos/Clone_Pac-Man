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
            isDead
        }

        private AnimationManager 
            myWalkingAnimation,
            myDeathAnimation;

        private Rectangle myDrawBox;
        private Vector2
            myDestination,
            myDirection;
        private PlayerState myPlayerState;
        private Color myColor;

        /// <summary>
        /// 0 = Up; 1 = Left; 2 = Down; 3 = Right;
        /// </summary>
        private int 
            myAngle,
            myLives;
        private float 
            mySpeed,
            myRotation,
            myEatingTimer,
            myEatingDelay,
            myInvincibilityTimer,
            myInvincibilityDelay;
        private bool 
            mySwitchAngle,
            myIsMoving,
            myIsEatingGhosts,
            myIsEatingWalls,
            myIsDead;

        public Rectangle BoundingBox
        {
            get => myBoundingBox;
        }
        public int Lives
        {
            get => myLives;
            set => myLives = value;
        }
        public float InvincibilityTimer
        {
            get => myInvincibilityTimer;
            set => myInvincibilityTimer = myInvincibilityDelay;
        }
        public bool IsDead
        {
            get => myIsDead;
        }
        public bool IsEatingGhosts
        {
            get => myIsEatingGhosts;
        }

        public Player(Vector2 aPosition, Point aSize, float aSpeed, float aEatingDelay, float aInvincibilityDelay, int someLives) : base(aPosition, aSize)
        {
            this.mySpeed = aSpeed;
            this.myEatingDelay = aEatingDelay;
            this.myInvincibilityDelay = aInvincibilityDelay;
            this.myLives = someLives;

            this.myWalkingAnimation = new AnimationManager(new Point(4, 1), 0.1f, true);
            this.myDeathAnimation = new AnimationManager(new Point(5, 3), 0.2f, false);
            this.myBoundingBox = new Rectangle((int)myPosition.X, (int)myPosition.Y, mySize.X, mySize.Y);
            this.myDestination = myBoundingBox.Center.ToVector2();
            this.myPlayerState = PlayerState.isWalking;
            this.myOrigin = Vector2.Zero;
            this.myAngle = -1;
        }

        public void Update(GameTime aGameTime)
        {
            myBoundingBox = new Rectangle((int)myPosition.X, (int)myPosition.Y, mySize.X, mySize.Y);
            myDrawBox = new Rectangle((int)myPosition.X + (int)myOrigin.X, (int)myPosition.Y + (int)myOrigin.Y, mySize.X, mySize.Y);

            switch (myPlayerState)
            {
                case PlayerState.isWalking:
                    Movement(aGameTime);
                    OutsideBounds();

                    if (myLives <= 0 || KeyMouseReader.KeyPressed(Keys.Enter))
                    {
                        myPlayerState = PlayerState.isDead;
                        myTexture = ResourceManager.RequestTexture("Pacman_Death");
                    }
                    break;
                case PlayerState.isDead:
                    if (myDeathAnimation.IsFinished)
                    {
                        myIsDead = true;
                    }
                    break;
            }

            Invincibility(aGameTime);
            Eating(aGameTime);

            CollisionCheck();
        }

        public void Draw(SpriteBatch aSpriteBatch, GameTime aGameTime)
        {
            switch(myPlayerState)
            {
                case PlayerState.isWalking:
                    if (myIsMoving)
                    {
                        myWalkingAnimation.DrawSpriteSheet(aSpriteBatch, aGameTime, myTexture, myDrawBox.Location.ToVector2(), myOrigin, new Point(32), mySize, myColor, myRotation);
                    }
                    else
                    {
                        aSpriteBatch.Draw(myTexture, myDrawBox, new Rectangle(myTexture.Width / 4, 0, myTexture.Width / 4, myTexture.Height), myColor, myRotation, myOrigin, SpriteEffects.None, 0.0f);
                    }
                    break;
                case PlayerState.isDead:
                    myDeathAnimation.DrawSpriteSheet(aSpriteBatch, aGameTime, myTexture, myDrawBox.Location.ToVector2(), myOrigin, new Point(32), mySize, Color.White, myRotation);
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
                myDirection = Normalize(myDirection);

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
        private Vector2 Normalize(Vector2 aVector)
        {
            if (aVector != Vector2.Zero)
            {
                aVector.Normalize();
                return aVector;
            }
            return new Vector2();
        }

        private void Eating(GameTime aGameTime)
        {
            if (myEatingTimer > 0)
            {
                myEatingTimer -= (float)aGameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                myIsEatingGhosts = false;
            }
        }

        private void Invincibility(GameTime aGameTime)
        {
            if (myInvincibilityTimer > 0)
            {
                myInvincibilityTimer -= (float)aGameTime.ElapsedGameTime.TotalSeconds;
                myColor = Color.LightBlue;
            }
            else
            {
                myInvincibilityTimer = 0;
                myColor = Color.White;
            }
        }
        private void CollisionCheck()
        {
            if (myInvincibilityTimer <= 0)
            {
                CollisionSnack();
                CollisionFruit();
                CollisionPowerUp();
            }
        }
        private void CollisionSnack()
        {
            Tile tempTile = Level.GetTileAtPos(myBoundingBox.Center.ToVector2()).Item1;
            if (tempTile.TileType == '.')
            {
                GameInfo.AddScore(myBoundingBox.Center.ToVector2(), 10);

                tempTile.TileType = '-';
                tempTile.SetTexture();
            }
        }
        private void CollisionFruit()
        {
            Tile tempTile = Level.GetTileAtPos(myBoundingBox.Center.ToVector2()).Item1;
            if (tempTile.TileType == '^')
            {
                GameInfo.AddScore(myBoundingBox.Center.ToVector2(), 300);

                tempTile.TileType = '-';
                tempTile.SetTexture();
            }
        }
        private void CollisionPowerUp()
        {
            Tile tempTile = Level.GetTileAtPos(myBoundingBox.Center.ToVector2()).Item1;
            if (tempTile.TileType == '/')
            {
                myIsEatingGhosts = true;
                myEatingTimer = myEatingDelay;

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
