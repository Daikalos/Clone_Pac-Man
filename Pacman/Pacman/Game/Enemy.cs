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

        private Animation 
            myWalkingAnimation,
            myFleeingAnimation;
        private Texture2D myEyesTexture;
        private BehaviourAI myBehaviour;

        /// <summary>
        /// 0 = Up; 1 = Left; 2 = Down; 3 = Right;
        /// </summary>
        private int myAngle;
        private int myAIType;
        private bool myIsAlive;

        public bool IsAlive
        {
            get => myIsAlive;
        }

        public Enemy(Vector2 aPosition, Point aSize, int aAIType) : base(aPosition, aSize)
        {
            this.myWalkingAnimation = new Animation(new Point(2, 1), 0.1f, true);
            this.myBehaviour = (BehaviourAI)aAIType;
            this.myIsAlive = true;
        }

        public void Update(GameTime aGameTime)
        {
            switch(myBehaviour)
            {
                case BehaviourAI.isChasing:
                    IsChasing();
                    break;
                case BehaviourAI.isRandom:
                    IsRandom();
                    break;
                case BehaviourAI.isFullRandom:
                    IsFullRandom();
                    break;
                case BehaviourAI.isFleeing:
                    IsFleeing();
                    break;
            }
        }

        public void Draw(SpriteBatch aSpriteBatch, GameTime aGameTime)
        {
            myWalkingAnimation.DrawSpriteSheet(aSpriteBatch, aGameTime, myTexture, myPosition, myOrigin, new Point(32), new Point(32), Color.White, 0.0f);
        }

        private void IsChasing()
        {

        }
        private void IsRandom()
        {

        }
        private void IsFullRandom()
        {

        }
        private void IsFleeing()
        {

        }
    }
}
