using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Tiling;
using Helpers;
using Microsoft.Xna.Framework.Audio;

namespace AnimatedSprite
{
    public class Projectile : RotatingSprite
    {

        public enum PROJECTILE_STATE { STILL, FIRING, EXPOLODING };
        PROJECTILE_STATE projectileState = PROJECTILE_STATE.STILL;
        protected Game myGame;
        protected float RocketVelocity = 4.0f;
        Vector2 textureCenter;
        Vector2 Target;
        AnimateSheetSprite explosion;
        float ExplosionTimer = 0;
        float ExplosionVisibleLimit = 1000;
        Vector2 StartPosition;
        public bool hit = false;

        SoundEffect shot;
        SoundEffect explode;

        bool shotPlaying = false;
        bool explodePlaying = false;


        public PROJECTILE_STATE ProjectileState
        {
            get { return projectileState; }
            set { projectileState = value; }
        }

        public AnimateSheetSprite Explosion
        {
            get { return explosion; }
            set { explosion = value; }
        }

        public Projectile(Game g, List<TileRef> texture, AnimateSheetSprite rocketExplosion, Vector2 userPosition, int framecount)
            : base(g, userPosition, texture, 64, 64, framecount)
        {
            Target = Vector2.Zero;
            myGame = g;
            textureCenter = new Vector2(32, 32);
            explosion = rocketExplosion;
            explosion.PixelPosition -= textureCenter;
            explosion.Visible = false;
            StartPosition = PixelPosition;
            ProjectileState = PROJECTILE_STATE.STILL;

        }
        public override void Update(GameTime gametime)
        {
            switch (projectileState)
            {
                case PROJECTILE_STATE.STILL:
                    this.Visible = false;
                    explosion.Visible = false;
                    break;
                // Using Lerp here could use target - pos and normalise for direction and then apply
                // Velocity
                case PROJECTILE_STATE.FIRING:
                    this.Visible = true;
                    PixelPosition = Vector2.Lerp(PixelPosition, Target, 0.02f * RocketVelocity);
                    // rotate towards the Target
                    this.angleOfRotation = TurnToFace(PixelPosition,
                                            Target, angleOfRotation, 1f);
                    if (Vector2.Distance(PixelPosition, Target) < 2)
                        projectileState = PROJECTILE_STATE.EXPOLODING;
                    break;
                case PROJECTILE_STATE.EXPOLODING:
                    explosion.PixelPosition = Target;
                    explosion.Visible = true;
                    break;
            }
            // if the explosion is visible then just play the animation and count the timer
            if (explosion.Visible)
            {
                explosion.Update(gametime);
                ExplosionTimer += gametime.ElapsedGameTime.Milliseconds;
            }
            // if the timer goes off the explosion is finished
            if (ExplosionTimer > ExplosionVisibleLimit)
            {
                explosion.Visible = false;
                ExplosionTimer = 0;
                projectileState = PROJECTILE_STATE.STILL;
            }

            base.Update(gametime);
        }
        public void fire(Vector2 SiteTarget)
        {
            projectileState = PROJECTILE_STATE.FIRING;
            Target = SiteTarget;
        }

        public void AddShot(SoundEffect sound)
        {
            shot = sound;
        }

        public void PlayShotSound(SoundEffect sound)
        {
            if (!shotPlaying)
            {
                shot.Play();
                shotPlaying = true;
            }
        }

        public void PlayExplosion()
        {
            if (!explodePlaying)
            {
                explode.Play();
                explodePlaying = true;
            }
        }

        public void AddExplosionSound(SoundEffect sound)
        {
            explode = sound;
        }
        public override void Draw(GameTime gametime)
        {
            base.Draw(gametime);
            //spriteBatch.Begin();
            //spriteBatch.Draw(spriteImage, position, SourceRectangle,Color.White);
            //spriteBatch.End();
            if (explosion.Visible)
                explosion.Draw(gametime);


        }

    }
}
