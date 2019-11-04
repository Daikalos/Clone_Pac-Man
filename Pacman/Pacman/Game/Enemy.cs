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
        public Enemy(Vector2 aPosition, Point aSize) : base(aPosition, aSize)
        {
            myPosition = Level.GetTileAtPos(aPosition).Position;
        }
    }
}
