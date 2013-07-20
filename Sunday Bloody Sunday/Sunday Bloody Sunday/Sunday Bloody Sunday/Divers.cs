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
    static class Divers
    {
        //Récupération des paramètres relatifs à la fenêtre de jeu
        static private int widthScreen = 800;
        static private int heightScreen = 480;


        //METHODS
        static public int WidthScreen
        { get { return widthScreen; } }

        static public int HeightScreen
        { get { return heightScreen; } }
    }
}
