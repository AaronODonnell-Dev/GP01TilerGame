using CameraNS;
using Engine.Engines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using Tiler;
using Tiling;
using AnimatedSprite;
using Helpers;

namespace TileBasedPlayer20172018
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>

    public class Game1 : Game
    {
        //Music
        Song bkMusic;
        SoundEffect death;
        SoundEffect winner;
        SoundEffect explosion;
        SoundEffect shot;

        //Booleans
        bool sound = false;
        bool win = false;

        //Timer
        TimeSpan timeSpan;

        //GameOver screen
        Texture2D txwinner;
        Texture2D txGameOver;
        Rectangle gameOver;

        //Properties for Projectile, Explosion and Player
        public Explosion explode { get; set; }
        public Projectile bullet { get; set; }
        public TilePlayer player { get; set; }


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //SentryTurret variables
        SentryTurret sentryTurret;
        int tileWidth = 64;
        int tileHeight = 64;
        List<SentryTurret> sentryList = new List<SentryTurret>();


        List<TileRef> TileRefs = new List<TileRef>();
        List<Collider> colliders = new List<Collider>();
        string[] backTileNames = { "blue box", "pavement", "blue steel", "green box", "home", "exit" };
        public enum TileType { BLUEBOX, PAVEMENT, BLUESTEEL, GREENBOX, HOME, EXIT };
        int[,] tileMap = new int[,]


    {
        {1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {1,1,4,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2},
        {2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,3,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,3,0,0,0,0,0,0,0,0,3,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,3,0,0,0,0,0,0,0,0,0,3,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,0,0,0,0,0,0,0,0,3,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,3,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,3,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,3,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,3,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
        {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
    };
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            new Camera(this, Vector2.Zero,
                new Vector2(tileMap.GetLength(1) * tileWidth, tileMap.GetLength(0) * tileHeight));
            new InputEngine(this);

            //Player Service
            Services.AddService(new TilePlayer(this, new Vector2(64, 128), new List<TileRef>()
            {
                new TileRef(15, 2, 0),
                new TileRef(15, 3, 0),
                new TileRef(15, 4, 0),
                new TileRef(15, 5, 0),
                new TileRef(15, 6, 0),
                new TileRef(15, 7, 0),
                new TileRef(15, 8, 0),
            }, 64, 64, 0f));

            TilePlayer tilePlayer = Services.GetService<TilePlayer>();
            tilePlayer.AddHealthBar(new HealthBar(tilePlayer.Game, tilePlayer.PixelPosition));
            SetColliders(TileType.BLUEBOX);

            player = (TilePlayer)Services.GetService(typeof(TilePlayer));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Audio Load
            bkMusic = Content.Load<Song>("bkMusic");
            death = Content.Load<SoundEffect>("Wasted");
            shot = Content.Load<SoundEffect>("Grenade");
            explosion = Content.Load<SoundEffect>("Explosion");
            winner = Content.Load<SoundEffect>("Winner");

            //Tiles for EndGame Screen
            txGameOver = Content.Load<Texture2D>(@"Tiles/Wasted");
            txwinner = Content.Load<Texture2D>(@"Tiles/Winner");
            gameOver = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(spriteBatch);
            Services.AddService(Content.Load<Texture2D>(@"Tiles/tank tiles 64 x 64"));
            Services.AddService(Content.Load<SpriteFont>(@"font"));

            // Tile References to be drawn on the Map corresponding to the entries in the defined 
            // Tile Map
            // "free", "pavement", "ground", "blue", "home", "exit" 
            TileRefs.Add(new TileRef(4, 2, 0));
            TileRefs.Add(new TileRef(3, 3, 1));
            TileRefs.Add(new TileRef(6, 3, 2));
            TileRefs.Add(new TileRef(6, 2, 3));
            TileRefs.Add(new TileRef(0, 2, 4));
            TileRefs.Add(new TileRef(3, 3, 5));
            // Names of the Tiles
            new SimpleTileLayer(this, backTileNames, tileMap, TileRefs, tileWidth, tileHeight);

            TilePlayer player = Services.GetService<TilePlayer>();

            //Projectile Service
            Projectile playerProjectile = new Projectile(this, new List<TileRef>() { new TileRef(7, 0, 0) },
                new AnimateSheetSprite(this, player.PixelPosition, new List<TileRef>()
                { new TileRef(0, 0, 0),
                  new TileRef(1, 0, 1),
                  new TileRef(2, 0, 2),
                }, 64, 64, 0), player.PixelPosition, 1);

            SetColliders(TileType.BLUEBOX);
            SetColliders(TileType.EXIT);

            playerProjectile.AddShot(shot);
            playerProjectile.AddExplosionSound(explosion);

            player.LoadProjectile(playerProjectile);

            //This is the only bit of code done by either of my teammates, this part was done by Conor Flannery
            //Sentry Turret
            List<Tile> found = SimpleTileLayer.getNamedTiles(backTileNames[(int)TileType.GREENBOX]);
            // TODO: use this.Content to load your game content here

            for (int i = 0; i < found.Count; i++)
            {
                sentryTurret = new SentryTurret(this, new Vector2(found[i].X * tileWidth, found[i].Y * tileHeight), new List<TileRef>()
                {
                new TileRef(21, 2, 0),
                new TileRef(21, 3, 0),
                new TileRef(21, 4, 0),
                new TileRef(21, 5, 0),
                new TileRef(21, 6, 0),
                new TileRef(21, 7, 0),
                new TileRef(21, 8, 0),
                }, 64, 64, 0f);
                sentryList.Add(sentryTurret);
            }
            //This is the end of the code done by Conor Flannery

            for (int i = 0; i < sentryList.Count; i++)
            {
                Projectile projectile = new Projectile(this, new List<TileRef>() {
                new TileRef(8, 0, 0)
                },
                new AnimateSheetSprite(this, sentryList[i].PixelPosition, new List<TileRef>() {
                    new TileRef(0, 0, 0),
                    new TileRef(1, 0, 1),
                    new TileRef(2, 0, 2)
                }, 64, 64, 0), sentryList[i].PixelPosition, 1);

                projectile.AddShot(shot);
                projectile.AddExplosionSound(explosion);

                sentryList[i].LoadProjectile(projectile);
                sentryList[i].Health = 20;
            }

        }


        public void SetColliders(TileType t)
        {
            for (int x = 0; x < tileMap.GetLength(1); x++)
                for (int y = 0; y < tileMap.GetLength(0); y++)
                {
                    if (tileMap[y, x] == (int)t)
                    {
                        colliders.Add(new Collider(this,
                            Content.Load<Texture2D>(@"Tiles/collider"),
                            x, y
                            ));
                    }

                }
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

                TilePlayer player = Services.GetService<TilePlayer>();

            if (player.Health > 0)
            {
                if (!sound && player.Health > 0 && timeSpan.TotalSeconds > 0 && !win)
                {
                    MediaPlayer.Play(bkMusic);
                    sound = true;
                }

                if (player.Health <= 0 || timeSpan.TotalSeconds <= 0)
                {
                    if (sound)
                    {
                        death.Play();
                        sound = false;
                    }
                }

                if (win)
                {
                    if (sound)
                    {
                        winner.Play();
                        sound = false;
                        timeSpan = new TimeSpan(0, 0, 1);
                    }
                }
            }

            for (int i = 0; i < sentryList.Count; i++)
            {
                sentryList[i].follow(player);

                if (sentryList[i].sentryProjectile.ProjectileState == Projectile.PROJECTILE_STATE.EXPOLODING && sentryList[i].sentryProjectile.collisionDetect(player))
                {
                    if (!sentryList[i].sentryProjectile.hit)
                        player.Health -= 20;
                    sentryList[i].sentryProjectile.hit = true;
                }

                if (player.myProjectile.ProjectileState == Projectile.PROJECTILE_STATE.EXPOLODING && player.myProjectile.collisionDetect(sentryList[i]))
                {
                    if (!player.myProjectile.hit)
                    {
                        sentryList[i].KillTurret();
                        player.myProjectile.hit = true;
                    }
                }
            }

            if (SentryTurret.aliveSentries <= 0 && Winning())
            {
                win = true;
            }

            if (SentryTurret.aliveSentries <= 0)
            {
                SimpleTileLayer.Tiles[15, 37].TileRef = TileRefs[5];
            }

            timeSpan = new TimeSpan(0, 0, 200 - gameTime.TotalGameTime.Seconds);

            //for if there are no sentries left, show the exit tile as tile enum 5
            if (SentryTurret.aliveSentries <= 0)
            {
                SimpleTileLayer.Tiles[15, 37].TileRef = TileRefs[5];
            }

            //sentries set to follow the player
            for (int i = 0; i < sentryList.Count; i++)
            {
                sentryList[i].follow(player);
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            TilePlayer player = Services.GetService<TilePlayer>();
            SpriteFont font = Services.GetService<SpriteFont>();

            if (player.Health <= 0)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(txGameOver, GraphicsDevice.Viewport.Bounds, Color.White);
                spriteBatch.End();
            }
            else if (win)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(txwinner, GraphicsDevice.Viewport.Bounds, Color.White);
                spriteBatch.End();
            }
            ///When all enemies are dead draws the you win Screen.
            ///Needs to be edited to allow the player to stand on the flag tile to Win.

            else
            {
                base.Draw(gameTime);
                spriteBatch.Begin();
                spriteBatch.DrawString(font, timeSpan.TotalSeconds.ToString(), new Vector2(10, 10), Color.White);
                spriteBatch.End();

                if (timeSpan.TotalSeconds <= 0)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(txGameOver, GraphicsDevice.Viewport.Bounds, Color.White);
                    spriteBatch.End();
                }
            }
        }

        private bool Winning()
        {
            TilePlayer player = Services.GetService<TilePlayer>();
            if (player.PixelPosition.X > 37 * 64 && player.PixelPosition.X < 38 * 64)
            {
                if (player.PixelPosition.Y > 15 * 64 && player.PixelPosition.Y < 16 * 64)
                {
                    return true;
                }
            }
            return false;
        }
    }
    
}
