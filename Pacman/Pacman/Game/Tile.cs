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
        Rectangle myBoundingBox;
        Point mySize;
        char myTileType;
        float myRotation;
        int myTileForm;

        /// <summary>
        /// . = Empty;
        /// # = Block;
        /// </summary>
        public char TileType
        {
            get => myTileType;
            set => myTileType = value;
        }
        public void SetRotation(int aDirection, bool aFlip)
        {
            switch(aFlip)
            {
                case true:
                    if (aDirection == -1)
                    {
                        myRotation += MathHelper.Pi;
                    }
                    if (aDirection == 1)
                    {
                        myRotation += -(MathHelper.Pi / 2);
                    }
                    break;
                case false:
                    if (aDirection == -1)
                    {
                        myRotation += (MathHelper.Pi / 2);
                    }
                    if (aDirection == 1)
                    {
                        myRotation += 0;
                    }
                    break;
            }
            myOrigin = new Vector2(myTexture.Width / 2, myTexture.Height / 2);
        }
        /// <summary>
        /// 0 = Block;
        /// 1 = Corridor;
        /// 2 = Corner;
        /// 3 = Dead-End;
        /// </summary>
        public int TileForm
        {
            get => myTileForm;
            set => myTileForm = value;
        }
        public float Rotation
        {
            get => myRotation;
            set => myRotation = value;
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle(myBoundingBox.X - (int)myOrigin.X, myBoundingBox.Y - (int)myOrigin.Y, mySize.X, mySize.Y);
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

            this.myTileForm = 0;
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
                case '.': case '-':
                    myTexture = ResourceManager.RequestTexture("Empty");
                    break;
                case '#':
                    myTexture = ResourceManager.RequestTexture("Tile_Block-" + myTileForm.ToString());
                    break;
            }
        }
    }
}
