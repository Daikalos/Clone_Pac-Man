using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    class Tile : GameObject
    {
        private List<Tile> myHistory; //Used for pathfinding

        private Rectangle mySourceRect;
        private char myTileType;
        private float myRotation;
        private int myTileForm;

        public List<Tile> History
        {
            get => myHistory;
            set => myHistory = value;
        }

        /// <summary>
        /// % = Void;
        /// - = Empty;
        /// . = Snack;
        /// # = Block;
        /// / = PowerUp 0;
        /// = = PowerUp 1
        /// </summary>
        public char TileType
        {
            get => myTileType;
            set => myTileType = value;
        }
        public void SetRotation(int aDirection, bool aFlip)
        {
            switch (aFlip)
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
        /// 3 = 3-sided;
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

        public Vector2 GetCenter()
        {
            return new Rectangle(myBoundingBox.X - (int)myOrigin.X, myBoundingBox.Y - (int)myOrigin.Y, mySize.X, mySize.Y).Center.ToVector2();
        }
        public Vector2 Position
        {
            get => myPosition;
            set => myPosition = value;
        }

        public Tile(Vector2 aPosition, Point aSize) : base(aPosition, aSize)
        {
            this.myTileForm = 0;
            this.myOrigin = Vector2.Zero;
            this.myBoundingBox = new Rectangle((int)aPosition.X, (int)aPosition.Y, aSize.X, aSize.Y);
        }

        public void Update()
        {
            myBoundingBox = new Rectangle((int)myPosition.X + (int)myOrigin.X, (int)myPosition.Y + (int)myOrigin.Y, mySize.X, mySize.Y);
        }

        public override void Draw(SpriteBatch aSpriteBatch)
        {
            if (myTexture != null)
            {
                aSpriteBatch.Draw(myTexture, myBoundingBox,
                    mySourceRect, Color.White, myRotation, myOrigin, SpriteEffects.None, 0.0f);
            }
        }

        public void SetTexture()
        {
            switch (myTileType)
            {
                case '-':
                case '&':
                    myTexture = ResourceManager.RequestTexture("Empty");
                    break;
                case '#':
                    myTexture = ResourceManager.RequestTexture("Tile_Block-" + myTileForm.ToString());
                    break;
                case '.':
                    myTexture = ResourceManager.RequestTexture("Snack");
                    break;
                case '/':
                    myTexture = ResourceManager.RequestTexture("PowerUp_00");
                    break;
                case '=':
                    myTexture = ResourceManager.RequestTexture("PowerUp_01");
                    break;
                case '^':
                    myTexture = ResourceManager.RequestTexture("Fruits");
                    mySourceRect = new Rectangle((myTexture.Width / 5) * StaticRandom.RandomNumber(0, 5), 0, myTexture.Width / 5, myTexture.Height);
                    break;
                case '%':
                    myTexture = null;
                    break;
            }
            if (myTexture != null && myTileType != '^')
            {
                mySourceRect = new Rectangle(0, 0, myTexture.Width, myTexture.Height);
            }
        }

        public void SetTextureEditor()
        {
            switch (myTileType)
            {
                case '-':
                    myTexture = ResourceManager.RequestTexture("Empty_Editor");
                    break;
                case '#':
                    myTexture = ResourceManager.RequestTexture("Tile_Block-" + myTileForm.ToString());
                    break;
                case '.':
                    myTexture = ResourceManager.RequestTexture("Snack_Editor");
                    break;
                case '/':
                    myTexture = ResourceManager.RequestTexture("PowerUp_00_Editor");
                    break;
                case '=':
                    myTexture = ResourceManager.RequestTexture("PowerUp_01_Editor");
                    break;
                case '&':
                    myTexture = ResourceManager.RequestTexture("Tile_Ghost");
                    break;
                case '^':
                    myTexture = ResourceManager.RequestTexture("Fruits_Editor");
                    mySourceRect = new Rectangle((myTexture.Width / 5) * StaticRandom.RandomNumber(0, 5), 0, myTexture.Width / 5, myTexture.Height);
                    break;
                case '%':
                    myTexture = null;
                    break;
            }
            if (myTexture != null && myTileType != '^')
            {
                mySourceRect = new Rectangle(0, 0, myTexture.Width, myTexture.Height);
            }
        }
    }
}
