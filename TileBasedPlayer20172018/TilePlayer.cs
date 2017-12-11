using AnimatedSprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiling;

namespace Tiler
{

    public class TilePlayer : RotatingSprite
    {
        //List<TileRef> images = new List<TileRef>() { new TileRef(15, 2, 0)};
        //TileRef currentFrame;
        int speed = 5;
        float turnspeed = 0.03f;
        public Vector2 previousPosition;
        public CrossHair crossHair;
        public Projectile myProjectile;

        public TilePlayer(Game game, Vector2 userPosition, 
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth) 
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            DrawOrder = 1;
            crossHair = new CrossHair(game, new List<TileRef>()
            {
                new TileRef(11,6,0)
            }, userPosition, 1);
            crossHair.DrawOrder = 2;
        }

        public void Collision(Collider c)
        {
            if (BoundingRectangle.Intersects(c.CollisionField))
                PixelPosition = previousPosition;
        }

        public void LoadProjectile(Projectile p)
        {
            myProjectile = p;
        }

        public override void Update(GameTime gameTime)
        {
            previousPosition = PixelPosition;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.PixelPosition += new Vector2(1, 0) * speed;
                this.angleOfRotation = 0;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.PixelPosition += new Vector2(-1, 0) * speed;
                this.angleOfRotation = 3.20f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                this.PixelPosition += new Vector2(0, -1) * speed;
                this.angleOfRotation = 4.75f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                this.PixelPosition += new Vector2(0, 1) * speed;
                this.angleOfRotation = 1.66f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                this.angleOfRotation -= turnspeed;
            if (Keyboard.GetState().IsKeyDown(Keys.X))
                this.angleOfRotation += turnspeed;

            if (myProjectile != null && myProjectile.ProjectileState == Projectile.PROJECTILE_STATE.STILL)
            {
                myProjectile.PixelPosition = this.PixelPosition;
                myProjectile.hit = false;
                // fire the rocket and it looks for the target
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    myProjectile.fire(crossHair.PixelPosition);
            }

            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
   }
}
