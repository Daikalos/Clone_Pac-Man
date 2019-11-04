using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    class GameObject
    {
        protected Texture2D myTexture;

        protected Vector2 myPosition;
        protected Rectangle myBoundingBox;
        protected Point mySize;

        protected GameObject(Vector2 aPosition, Point aSize)
        {
            this.myPosition = aPosition;
            this.mySize = aSize;
        }

        public void SetTexture(string aName)
        {
            myTexture = ResourceManager.RequestTexture(aName);
        }
    }
}
