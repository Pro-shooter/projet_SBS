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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState mouse;
        Editeur editeur;

        // Début fichier généré par XNA
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Préférences
            this.IsMouseVisible = true;
            this.graphics.PreferredBackBufferWidth = 1200;
            this.graphics.PreferredBackBufferHeight = 800;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Content.RootDirectory = "Content";
            Texture2D map = Content.Load<Texture2D>("Map");

            Ressources.LoadContent(Content);
            int largeur = 0;
            int hauteur = 0;
            Lecture.lire(ref largeur, ref hauteur);
            editeur = new Editeur(Ressources.Map, Ressources.valide,Ressources.invalide,hauteur,largeur);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            editeur.Update(Mouse.GetState(),Keyboard.GetState());
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
                editeur.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
