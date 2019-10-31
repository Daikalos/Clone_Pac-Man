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
        /// . = Empty;
        /// # = Block;
        /// </summary>
        public char TileType
        {
            get => myTileType;
            set => myTileType = value;
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetDirection(int aValue)
        {
            myDirection = aValue;
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
            if (myTexture != null)
            {
                aSpriteBatch.Draw(myTexture, myBoundingBox,
                    null, Color.White, myRotation, myOrigin, SpriteEffects.None, 0.0f);
            }
        }

        public void SetTexture()
        {
            switch (myTileType)
            {
                case '.':
                    myTexture = ResourceManager.RequestTexture("Empty");
                    break;
                case '#':
                    myTexture = ResourceManager.RequestTexture("Tile_Block-" + myDirection.ToString());
                    break;
            }
        }
    }
}
