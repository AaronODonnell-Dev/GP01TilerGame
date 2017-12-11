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

    public class SentryTurret : RotatingSprite
    {
        //List<TileRef> images = new List<TileRef>() { new TileRef(15, 2, 0)};
        //TileRef currentFrame;
        int speed = 5;
        float turnspeed = 0.03f;
        public Projectile MyProjectile;
        public float chaseRadius = 200;
        float previousAngleOfRotation = 0;
        bool following = false;
        Vector2 target = new Vector2(0, 0);
        public Vector2 previousPosition;
        bool alive = true;
        public static int aliveSentries;
        public Projectile sentryProjectile { get; set; }

        public SentryTurret(Game game, Vector2 userPosition,
            List<TileRef> sheetRefs, int frameWidth, int frameHeight, float layerDepth)
                : base(game, userPosition, sheetRefs, frameWidth, frameHeight, layerDepth)
        {
            DrawOrder = 1;
            aliveSentries++;
        }

        public void Collision(Collider c)
        {
            if (BoundingRectangle.Intersects(c.CollisionField))
                PixelPosition = previousPosition;
        }

        public bool inChaseZone(TilePlayer p)
        {
            float distance = Math.Abs(Vector2.Distance(this.PixelPosition, p.PixelPosition));
            if (distance <= chaseRadius)
                return true;
            return false;
        }

        public void follow(TilePlayer p)
        {
            bool inchaseZone = inChaseZone(p);
            if (inchaseZone)
            {
                this.angleOfRotation = TurnToFace(PixelPosition, p.PixelPosition, angleOfRotation, .3f);
                this.following = true;
                target = p.PixelPosition;
            }else
            {
                this.following = false;
            }

        }

        public void LoadProjectile(Projectile r)
        {
            sentryProjectile = r;
            sentryProjectile.DrawOrder = 2;
        }

        public void KillTurret()
        {
            alive = false;
            aliveSentries--;
        }

        public override void Update(GameTime gameTime)
        {
            if (alive)
            {
                if (sentryProjectile != null && sentryProjectile.ProjectileState == Projectile.PROJECTILE_STATE.STILL)
                {
                    sentryProjectile.PixelPosition = this.PixelPosition;
                    sentryProjectile.hit = false;
                    // fire the rocket and it looks for the target
                    if (following && previousAngleOfRotation == angleOfRotation)
                        sentryProjectile.fire(target);
                }

                if (sentryProjectile != null)
                    sentryProjectile.Update(gameTime);

                previousAngleOfRotation = angleOfRotation;

                base.Update(gameTime);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            if (alive)
            {
                if (sentryProjectile != null && sentryProjectile.ProjectileState != Projectile.PROJECTILE_STATE.STILL)
                    sentryProjectile.Draw(gameTime);

                base.Draw(gameTime);
            }
        }
    }
}