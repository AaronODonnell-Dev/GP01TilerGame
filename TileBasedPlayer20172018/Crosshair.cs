using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Tiling;

namespace AnimatedSprite
{
    public class CrossHair : AnimateSheetSprite
    {
        private Game myGame;
        private float CrossHairVelocity = 5.0f;

        public CrossHair(Game g, List<TileRef> texture, Vector2 userPosition, int framecount) : base(g, userPosition, texture, 64, 64, 0)
        {
            myGame = g;
        }

        public override void Update(GameTime gametime)
        {
            Viewport gameScreen = myGame.GraphicsDevice.Viewport;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                this.PixelPosition += new Vector2(1, 0) * CrossHairVelocity;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                this.PixelPosition += new Vector2(-1, 0) * CrossHairVelocity;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                this.PixelPosition += new Vector2(0, -1) * CrossHairVelocity;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                this.PixelPosition += new Vector2(0, 1) * CrossHairVelocity;

            // Make sure the Cross Hair stays in the bounds see previous lab for details
            PixelPosition = Vector2.Clamp(PixelPosition, Vector2.Zero,
                                            new Vector2(52 * 64 - FrameWidth,
                                                        19 * 64 - FrameHeight));

            base.Update(gametime);
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}
