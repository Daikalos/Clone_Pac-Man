using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    class Tile
    {
        Texture2D myTexture;
        Vector2 
            myPosition,
            myOrigin;
        Rectangle 
            myBoundingBox,
            mySourceRect;
        Point mySize;
        char myTileType;
        float myRotation;
        int myDirection;

        /// <summary>
        /// # = Block;
        /// </summary>
        public char TileType
        {
            get => myTileType;
            set => myTileType = value;
        }
        /// <summary>
        /// 1 = Left/Up, Corner;
        /// 2 = Right/Up, Corner;
        /// 3 = Left/Down, Corner;
        /// 4 = Right/Down, Corner;
        /// 5 = Left/Right, Wall;
        /// 6 = Right/Left, Wall;
        /// 7 = Up/Down, Wall,
        /// 8 = Down/Up, Wall;
        /// </summary>
        public void SetDirection(int aValue)
        {
            myDirection = aValue;

            switch (myDirection)
            {
                case 1:
                    myRotation = (MathHelper.Pi);
                    break;
                case 2:
                    myRotation = -(MathHelper.Pi / 2);
                    break;
                case 3:
                    myRotation = (MathHelper.Pi / 2);
                    break;
                case 4:
                    myRotation = 0;
                    break;
                case 5:
                    myRotation = (MathHelper.Pi);
                    break;
                case 6:
                    myRotation = 0;
                    break;
                case 7:
                    myRotation = -(MathHelper.Pi / 2);
                    break;
                case 8:
                    myRotation = (MathHelper.Pi / 2);
                    break;
            }
        }
        public int Direction
        {
            get => myDirection;
        }

        public Rectangle BoundingBox
        {
            get => myBoundingBox;
        }

        public Vector2 Position
        {
            get => myPosition;
            set => myPosition = value;
        }

        public Tile(Vector2 aPosition, Point aSize)
        {
            this.myPosition = aPosition;
            this.mySize = aSize;

            this.myDirection = 0;
            this.myOrigin = Vector2.Zero;
            this.myBoundingBox = new Rectangle((int)aPosition.X, (int)aPosition.Y, aSize.X, aSize.Y);
        }

        public void Update()
        {
            myBoundingBox = new Rectangle((int)myPosition.X + (int)myOrigin.X, (int)myPosition.Y + (int)myOrigin.Y, mySize.X, mySize.Y);
        }

        public void Draw(SpriteBatch aSpriteBatch)
        {
            aSpriteBatch.Draw(myTexture, myBoundingBox, 
                mySourceRect, Color.White, myRotation, myOrigin, SpriteEffects.None, 0.0f);
        }

        public void SetTexture()
        {
            switch (myTileType)
            {
                case '.':
                    myTexture = ResourceManager.RequestTexture("Empty");
                    break;
                case '#':
                    myTexture = ResourceManager.RequestTexture("Tileset");
                    break;
            }
            mySourceRect = new Rectangle(0, 0, myTexture.Width / 4, myTexture.Height / 4);
        }
    }
}
