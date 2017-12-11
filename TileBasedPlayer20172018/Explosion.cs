using AnimatedSprite;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiling;

namespace TileBasedPlayer20172018
{
    public class Explosion : RotatingSprite
    {
        private Game myGame;
        private Vector2 StartPosition;
        private Vector2 textureCenter;
        private bool visible;

        public Explosion(Game g, List<TileRef> texture, Vector2 userPosition, int frameCount)
                : base(g, userPosition, texture, 64, 64, frameCount)
        {
            myGame = g;
            textureCenter = new Vector2(32, 32);
            StartPosition = PixelPosition;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
