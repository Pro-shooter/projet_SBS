using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Sunday_Bloody_Sunday
{
    class ParticuleExplosion
    {
        //FIELDS
        // The image representing the collection of images used for animation
        Texture2D spriteStrip;
        // The scale used to display the sprite strip
        float scale;
        // The time since we last updated the frame
        int elapsedTime;
        // The time we display a frame until the next one
        int frameTime;
        // The number of frames that the animation contains
        int frameCount;
        // The index of the current frame we are displaying
        int currentFrame;
        // The color of the frame we will be displaying
        Color color;
        // The area of the image strip we want to display
        Rectangle sourceRect = new Rectangle();
        // The area where we want to display the image strip in the game
        Rectangle destinationRect = new Rectangle();
        // Width of a given frame
        public int FrameWidth;
        // Height of a given frame
        public int FrameHeight;
        // The state of the Animation
        public bool Active;
        // Determines if the animation will keep playing or deactivate after one run
        public bool Looping;
        // Width of a given frame
        public Vector2 Position;
        public Rectangle Aire_explosionBomb;
        public string type;


        //CONSTRUCTOR
        public void Initialize(Texture2D texture, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frametime, Color color, float scale, bool looping, int x, int y, int largeur, string type)
        {
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;
            this.Looping = looping;
            this.Position = position;
            this.spriteStrip = texture;
            this.elapsedTime = 0;
            this.currentFrame = 0;
            this.Active = true;
            this.type = type;

            //EXPLOSION AREA
            this.Aire_explosionBomb = new Rectangle(x, y, largeur, largeur);
        }


        //UPDATE & DRAW
        public void Update(GameTime gameTime, List<Player> liste_joueurs, List<IA> liste_ia, List<DestructibleItems> liste_barrel, List<ParticuleExplosion> liste_explosion, List<ParticuleExplosion> liste_explosion2, List<ParticuleExplosion> liste_blood, List<Turret> liste_turret, List<JetPack> liste_jetpack)
        {
            // Do not update the game if we are not active
            if (Active == false)
                return;

            // Update the elapsed time
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            // If the elapsed time is larger than the frame time
            // we need to switch frames
            if (elapsedTime > frameTime)
            {
                // Move to the next frame
                currentFrame++;


                // If the currentFrame is equal to frameCount reset currentFrame to zero
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    // If we are not looping deactivate the animation
                    if (Looping == false)
                        Active = false;
                }


                // Reset the elapsed time to zero
                elapsedTime = 0;
            }

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            // Grab the correct frame in the image strip by multiplying the currentFrame index by the frame width
            destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale) / 2,
            (int)Position.Y - (int)(FrameHeight * scale) / 2,
            (int)(FrameWidth * scale),
            (int)(FrameHeight * scale));

            foreach (Player joueur in liste_joueurs)
            {
                if ((joueur.PlayerTexture.Intersects(this.Aire_explosionBomb)) && type == "explosion")
                {
                    joueur.Health -= 100;
                }
            }

            foreach (IA ia in liste_ia)
            {
                if ((ia.IATexture.Intersects(this.Aire_explosionBomb)) && type == "explosion")
                {
                    if (ia.IA_boss)
                        ia.Health -= 1;
                    else
                        ia.Health -= 10;
                }
            }

            foreach (DestructibleItems barrel in liste_barrel)
            {
                if ((barrel.BarrelTexture.Intersects(this.Aire_explosionBomb)) && type == "explosion")
                {
                    barrel.isVisible = false;
                    ParticuleExplosion explosion = new ParticuleExplosion();
                    explosion.Initialize(Ressources.ExplosionParticule, new Vector2(barrel.Aire_barrel.X + 8, barrel.Aire_barrel.Y + 8), 134, 134, 12, 45, Color.White, 1f, false, barrel.Aire_barrel.X - 16, barrel.Aire_barrel.Y - 16, 48, "explosion");
                    liste_explosion2.Add(explosion);
                }
            }

            foreach (Turret turret in liste_turret)
            {
                if (turret.turretTexture.Intersects(this.Aire_explosionBomb) && type == "explosion")
                {
                    turret.munition = 0;
                    turret.isActive = false;
                }
            }

            foreach (JetPack jetpack in liste_jetpack)
            {
                if (jetpack.JetPackTexture.Intersects(this.Aire_explosionBomb))
                {
                    jetpack.isVisible = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle Maptexture)
        {
            if (Active)
            {
                switch (type)
                {
                    case ("explosion"):
                        spriteBatch.Draw(Ressources.ExplosionParticule, new Rectangle(Maptexture.X + destinationRect.X, Maptexture.Y + destinationRect.Y, destinationRect.Width, destinationRect.Height), sourceRect, color);
                        break;
                    case ("blood"):
                        spriteBatch.Draw(Ressources.BloodParticule, new Rectangle(Maptexture.X + Aire_explosionBomb.X, Maptexture.Y + Aire_explosionBomb.Y, 30, 30), sourceRect, color);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

