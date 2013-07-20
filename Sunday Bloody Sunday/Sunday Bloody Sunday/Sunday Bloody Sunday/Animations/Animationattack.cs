using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Sunday_Bloody_Sunday
{
    class AnimationAttack
    {
        // FIELDS
        public Rectangle Hitbox;
        public bool Animation;
        int FrameLine;
        int FrameColumn;
        int Timer;
        int AnimationSpeed = 6;


        // CONSTRUCTOR
        public AnimationAttack(int x, int y)
        {
            this.Hitbox = new Rectangle(x, y, 40, 59);
            Animation = true;
            this.FrameLine = 1;
            this.FrameColumn = 2;
            this.Animation = true;
            this.Timer = 0;
        }


        // METHODS
        public void Animate()
        {
            this.Timer++;
            if (this.Timer == this.AnimationSpeed)
            {
                this.Timer = 0;
                this.FrameColumn++;
                if (this.FrameColumn > 4)
                {
                    Animation = false;
                }
            }
        }


        // UPDATE & DRAW
        public void Update(MouseState mouse, KeyboardState keyboard)
        {
            if (Animation)
            {
                Animate();
            }
        }

        public void Draw(SpriteBatch spriteBatch, Rectangle MapTexture)
        {
            if (Animation)
            {
                spriteBatch.Draw(Ressources.mAttaqueEclair, new Rectangle(MapTexture.X + Hitbox.X, MapTexture.Y + Hitbox.Y, 40, 59), new Rectangle((this.FrameColumn - 1) * 138, (this.FrameLine - 1) * 183, 138, 183), Color.White);
            }
        }
    }
}