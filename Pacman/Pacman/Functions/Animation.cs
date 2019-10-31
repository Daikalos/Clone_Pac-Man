﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    class Animation
    {
        //Animation-Info
        int myCurrentFrame;
        Point myCurrentFramePos;
        bool 
            myIsFinished,
            myAtLastFrame;
        float myTimer;

        //Texture-Info
        Point
            myFrameAmount;
        float myAnimationSpeed;
        bool myIsLoop;

        public bool IsFinished
        {
            get => myIsFinished;
            set => myIsFinished = value;
        }
        public Animation(Point aFrameAmount, float aAnimationSpeed, bool aIsLoop)
        {
            this.myCurrentFrame = 0;
            this.myIsFinished = false;
            this.myAtLastFrame = false;

            this.myFrameAmount = aFrameAmount;
            this.myAnimationSpeed = aAnimationSpeed;
            this.myIsLoop = aIsLoop;
        }

        public void DrawSpriteSheet(SpriteBatch aSpriteBatch, GameTime aGameTime, Texture2D aTexture, Vector2 aPos, Vector2 aOrigin, Point aFrameSize, Point aDestSize, Color aColor, SpriteEffects aSpriteEffect)
        {
            if (myIsFinished) return;

            myTimer += (float)aGameTime.ElapsedGameTime.TotalSeconds;
            if (myTimer > myAnimationSpeed)
            {
                myCurrentFrame++;
                myCurrentFramePos.X++;
                if (myCurrentFrame >= (myFrameAmount.X * myFrameAmount.Y))
                {
                    if (myIsLoop)
                    {
                        myCurrentFrame = 0;
                        myCurrentFramePos = new Point(0, 0);
                    }
                    else
                    {
                        myCurrentFrame = (myFrameAmount.X * myFrameAmount.Y) - 1;
                        myIsFinished = true;
                    }
                }
                if (myCurrentFramePos.X >= myFrameAmount.X) //Animation
                {
                    myCurrentFramePos.Y++;
                    myCurrentFramePos.X = 0;
                }
                myTimer = 0;
            }

            aSpriteBatch.Draw(aTexture,
                new Rectangle((int)aPos.X, (int)aPos.Y, aDestSize.X, aDestSize.Y),
                new Rectangle(aFrameSize.X * myCurrentFramePos.X, aFrameSize.Y * myCurrentFramePos.Y, aFrameSize.X, aFrameSize.Y),
                aColor, 0.0f, aOrigin, aSpriteEffect, 0.0f);
        }
    }
}
