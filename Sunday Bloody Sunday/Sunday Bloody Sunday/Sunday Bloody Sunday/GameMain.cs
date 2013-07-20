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
    class GameMain
    {
        // FIELDS
        public Map MainMap;
        HUD MainHUD;


        // CONCSTRUCTOR
        public GameMain()
        {/*
            this.MainMap = new Map(LecteurMap.lecture(chemin_map));*/
            this.MainHUD = new HUD();
        }


        // METHODS


        // UPDATE & DRAW
        public void Update(MouseState mouse, KeyboardState keyboard, GameTime gameTime, GraphicsDevice graphics)
        {
            MainMap.Update(mouse, keyboard, gameTime, graphics);
            MainHUD.Update(keyboard, MainMap.liste_joueurs, MainMap.liste_ia);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            MainMap.Draw(spriteBatch);

            if (MainMap.parametre.texture_map == 4 || MainMap.parametre.texture_map == 15)
            {
                MainHUD.Draw(spriteBatch, 4);
            }
            else if (MainMap.parametre.texture_map == 9)
            {
                MainHUD.Draw(spriteBatch, 9);
            }
            else if (MainMap.parametre.texture_map == 5 || MainMap.parametre.texture_map == 6 || MainMap.parametre.texture_map == 7 || MainMap.parametre.texture_map == 8)
            {
                MainHUD.Draw(spriteBatch, 5);
            }
            else
            {
                MainHUD.Draw(spriteBatch, 0);
            }
        }
    }
}
