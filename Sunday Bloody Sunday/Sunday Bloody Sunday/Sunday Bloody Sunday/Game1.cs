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
using System.Net;
using System.Net.Sockets;

namespace Sunday_Bloody_Sunday
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //FIELDS
        KeyboardState oldKeyboard;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameMain Main;
        Input IP_ = new Input(20);
        IPAddress IPMulti = IPAddress.Parse("127.0.0.1");
        Input Chat = new Input(20);
        Input Pseudo = new Input(15);
        string path_map;
        string IP = "";
        int joueur;
        bool multi;
        Lobby lobby;
        bool Lobby_ready = false;
        //Enum of the Screen States
        public enum Screen
        {
            menu_principal, menu_pause, menu_preferences, menu_parametres, jeu, game_over, selecteur_map, win, credits, multioupas, transition, connection, deconnexion, lobby, compte, intro, reset_progression, Campagne, bsod
        };
        //Init Screen States + Menu
        public static Screen ecran = Screen.intro;
        Menu menuMain = new Menu(Menu.MenuType.Intro);
        int button_timer = 0;
        //Prochainement dans Sound.cs
        Song GamePlayMusic;
        Song MenuMusic;
        SoundEffect introEffect, loseEffect, winEffect, beep;
        //Compteur Sélecteur Map
        int compteur_thumbnails = 0;
        int compteur_delai = 30;

        Compte Joueur = new Compte("I AM A PONEY");
        bool Compte_chargé = false;
        bool Loading = false;

        int CurrentLevel = 0;
        Campagne Campagne1;
        Campagne Campagne2;
        Campagne Campagne3;
        Campagne Campagne4;
        Campagne CurrentCampagne;


        // Début fichier généré par XNA
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Préférences
            this.IsMouseVisible = true;

            Campagne1 = new Campagne(new List<string>(), 0);
            Campagne1.Maps.Add("map02.txt");
            Campagne1.Maps.Add("map11.txt");
            Campagne1.Maps.Add("map10.txt");
            Campagne1.Maps.Add("map04.txt");
            Campagne1.progression = 1;

            Campagne2 = new Campagne(new List<string>(), 1);
            Campagne2.Maps.Add("map12.txt");
            Campagne2.Maps.Add("map13.txt");
            Campagne2.Maps.Add("map14.txt");
            Campagne2.Maps.Add("map15.txt");
            Campagne2.progression = 1;


            Campagne3 = new Campagne(new List<string>(), 2);
            Campagne3.Maps.Add("map06.txt");
            Campagne3.Maps.Add("map07.txt");
            Campagne3.Maps.Add("map08.txt");
            Campagne3.Maps.Add("map09.txt");
            Campagne3.progression = 1;

            Campagne4 = new Campagne(new List<string>(), 3);
            Campagne4.Maps.Add("map01.txt");
            Campagne4.Maps.Add("map03.txt");
            Campagne4.Maps.Add("map03_bonus.txt");
            Campagne4.Maps.Add("map05.txt");
            Campagne4.Maps.Add("map16.txt");
            Campagne4.progression = 5;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Ressources.LoadContent(Content);
            // Chargement musique
            GamePlayMusic = Ressources.GamePlayMusic;
            MenuMusic = Ressources.MenuMusic;
            // Chargement effet
            introEffect = Ressources.mIntroEffect;
            loseEffect = Ressources.mLoseEffect;
            winEffect = Ressources.mWinEffect;
            beep = Ressources.mBeep;
            introEffect.Play();
        }

        // Prochainement dans Sound.cs
        public void PlayMusic(Song song)
        {
            try
            {
                // Joue la musique
                MediaPlayer.Play(song);
                // Active la répétition de la musique
                MediaPlayer.IsRepeating = true;
            }
            catch { }
        }

        public void StopMusic(Song song)
        {
            try
            {
                // Stop la musique
                MediaPlayer.Stop();
            }
            catch { }
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        private void EcranCompte()
        {
            int action = 0;

            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }

            if (action == 1 && Pseudo.input != "")
            {
                ecran = Screen.menu_principal;
                menuMain = new Menu(Menu.MenuType.MainMenu);
                Compte.Enregistrer(new Compte(Pseudo.input), "Content/Save/Save.poney");
            }
            else
                Pseudo.Update(Keyboard.GetState(), oldKeyboard);
        }

        private void EcranPrincipale()
        {

            int action = 0;

            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1)
            {
                ecran = Screen.multioupas;
                menuMain = new Menu(Menu.MenuType.MultiOrNot);
                button_timer = 0;
            }
            if (action == 2)
            {
                ecran = Screen.menu_parametres;
                menuMain = new Menu(Menu.MenuType.MenuGeneralSettings);
                button_timer = 0;
            }
            if (action == 3)
            {
                UnloadContent();
                Exit();
            }
        }

        private void EcranChoixMode()
        {
            int action = 0;

            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1)
            {
                joueur = 0;
                multi = false;
                ecran = Screen.Campagne;
                menuMain = new Menu(Menu.MenuType.Campagne);
                button_timer = 0;
            }
            if (action == 2)
            {
                joueur = 1;
                multi = true;
                ecran = Screen.connection;
                menuMain = new Menu(Menu.MenuType.Connection);
                button_timer = 0;
            }
            if (action == 3)
            {
                ecran = Screen.menu_principal;
                menuMain = new Menu(Menu.MenuType.MainMenu);
                button_timer = 0;
            }
        }

        private void EcranChoixCarte()
        {
            int action = 0;
            compteur_delai++;

            if (compteur_delai > 20)
            {
                compteur_delai = 20;
            }
            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1 && compteur_delai >= 20)
            {
                compteur_delai = 0;
                compteur_thumbnails -= 1;
                if (compteur_thumbnails < 0)
                    compteur_thumbnails = CurrentCampagne.progression - 1;
            }
            if (action == 2 && compteur_delai >= 20)
            {
                compteur_delai = 0;
                compteur_thumbnails += 1;
                if (compteur_thumbnails == CurrentCampagne.progression)
                    compteur_thumbnails = 0;
            }
            if (action == 3)
            {
                Main = new GameMain();
                Main.MainMap = new Map(LecteurMap.lecture(CurrentCampagne.Maps[compteur_thumbnails]), multi, joueur, IPMulti, 1, 0);
                path_map = CurrentCampagne.Maps[compteur_thumbnails];
                if (Main.MainMap.parametre.texture_map == 0 || Main.MainMap.parametre.texture_map == 1 || Main.MainMap.parametre.texture_map == 2 || Main.MainMap.parametre.texture_map == 3 || Main.MainMap.parametre.texture_map == 10 || Main.MainMap.parametre.texture_map == 11)
                {
                    GamePlayMusic = Ressources.GamePlayMusic;
                }
                else if (Main.MainMap.parametre.texture_map == 4)
                {
                    GamePlayMusic = Ressources.HightVoltage;
                }
                else if (Main.MainMap.parametre.texture_map == 6 || Main.MainMap.parametre.texture_map == 7 || Main.MainMap.parametre.texture_map == 8)
                {
                    GamePlayMusic = Ressources.Suspense;
                }
                else if (Main.MainMap.parametre.texture_map == 5 || Main.MainMap.parametre.texture_map == 9)
                {
                    GamePlayMusic = Ressources.BlackIce;
                }
                else if (Main.MainMap.parametre.texture_map == 13)
                {
                    GamePlayMusic = Ressources.RaveCat;
                }
                else if (Main.MainMap.parametre.texture_map == 12 || Main.MainMap.parametre.texture_map == 14)
                {
                    GamePlayMusic = Ressources.Montagne;
                }
                else if (Main.MainMap.parametre.texture_map == 15)
                {
                    GamePlayMusic = Ressources.FireYourGuns;
                }
                else if (Main.MainMap.parametre.texture_map == 16)
                {
                    GamePlayMusic = Ressources.PikaPikaSong;
                }
                ecran = Screen.jeu;
                PlayMusic(GamePlayMusic);
                button_timer = 0;
            }
            if (action == 4)
            {
                ecran = Screen.Campagne;
                menuMain = new Menu(Menu.MenuType.Campagne);
                button_timer = 0;
            }
        }

        private void EcranLobby()
        {

            if (lobby.connection)
            {
                lobby.Update(Keyboard.GetState(), oldKeyboard);
                int action = 0;
                compteur_delai++;

                if (compteur_delai > 20)
                {
                    compteur_delai = 20;
                }
                if (button_timer == 20)
                {
                    action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
                }
                if (action == 1 && compteur_delai >= 20)
                {
                    compteur_delai = 0;
                    compteur_thumbnails -= 1;
                    if (compteur_thumbnails < 0)
                    {
                        compteur_thumbnails = 2;
                    }
                }
                if (action == 2 && compteur_delai >= 20)
                {
                    compteur_delai = 0;
                    compteur_thumbnails += 1;
                    if (compteur_thumbnails > 2)
                    {
                        compteur_thumbnails = 0;
                    }
                }
                if (action == 3 && compteur_delai >= 20)
                {
                    Lobby_ready = !Lobby_ready;
                }
                if (action == 4)
                {
                    ecran = Screen.multioupas;
                    menuMain = new Menu(Menu.MenuType.MultiOrNot);
                    button_timer = 0;
                    Reseau.Clear();
                }
                if (Lobby_ready)
                    lobby.Envoie(action == 5, compteur_thumbnails, 1, ref lobby.connection);
                else
                    lobby.Envoie(action == 5, compteur_thumbnails, 0, ref lobby.connection);

                if (lobby.Reception())
                {
                    {
                        if (compteur_thumbnails == 0)
                        {
                            Main = new GameMain();
                            Main.MainMap = new Map(LecteurMap.lecture("map01.txt"), multi, joueur, IPMulti, lobby.nb_joueur, lobby.id_joueur);
                            path_map = "map01.txt";
                            GamePlayMusic = Ressources.GamePlayMusic;
                        }
                        else if (compteur_thumbnails == 1)
                        {
                            Main = new GameMain();
                            Main.MainMap = new Map(LecteurMap.lecture("map03.txt"), multi, joueur, IPMulti, lobby.nb_joueur, lobby.id_joueur);
                            path_map = "map03.txt";
                            GamePlayMusic = Ressources.FireYourGuns;
                        }
                        else if (compteur_thumbnails == 2)
                        {
                            Main = new GameMain();
                            Main.MainMap = new Map(LecteurMap.lecture("map05.txt"), multi, joueur, IPMulti, lobby.nb_joueur, lobby.id_joueur);
                            path_map = "map05.txt";
                            GamePlayMusic = Ressources.BlackIce;
                        }
                        ecran = Screen.jeu;
                        PlayMusic(GamePlayMusic);
                        button_timer = 0;
                    }
                }
            }
            else
            {
                ecran = Screen.deconnexion;
                menuMain = new Menu(Menu.MenuType.Deconnexion);
                StopMusic(GamePlayMusic);
                Reseau.Clear();
            }
        }

        private void EcranPause()
        {
            int action = 0;

            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1)
            {
                ecran = Screen.jeu;
                button_timer = 0;
                PlayMusic(GamePlayMusic);
            }
            if (action == 2)
            {
                ecran = Screen.menu_preferences;
                menuMain = new Menu(Menu.MenuType.MenuPreferences);
                button_timer = 0;
            }
            if (action == 3)
            {
                ecran = Screen.selecteur_map;
                menuMain = new Menu(Menu.MenuType.MapSelector);
                button_timer = 0;
                Reseau.Clear();
                StopMusic(MenuMusic);
            }
        }

        private void EcranPreference()
        {
            int action = 0;

            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1)
            {
                MenuButton.language = "French";
                menuMain = new Menu(Menu.MenuType.MenuPreferences);
                button_timer = 0;
            }
            if (action == 2)
            {
                MenuButton.language = "English";
                menuMain = new Menu(Menu.MenuType.MenuPreferences);
                button_timer = 0;
            }
            if (action == 3)
            {
                MediaPlayer.Volume -= 0.1f;
                if (SoundEffect.MasterVolume == 0)
                {
                }
                else
                {
                    try
                    {
                        SoundEffect.MasterVolume -= 0.1f;
                    }
                    catch { }
                }
                button_timer = 0;
            }
            if (action == 4)
            {
                MediaPlayer.Volume += 0.1f;
                if (SoundEffect.MasterVolume < 0.9f)
                {
                    try
                    {
                        SoundEffect.MasterVolume += 0.1f;
                    }
                    catch { }
                }
                button_timer = 0;
            }
            if (action == 5)
            {
                MediaPlayer.Volume = 0;
                SoundEffect.MasterVolume = 0;
                button_timer = 0;
            }
            if (action == 6)
            {
                ecran = Screen.menu_pause;
                menuMain = new Menu(Menu.MenuType.PauseMenu);
                button_timer = 0;
            }
        }

        private void EcranParametres()
        {
            int action = 0;

            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1)
            {
                MenuButton.language = "French";
                menuMain = new Menu(Menu.MenuType.MenuGeneralSettings);
                button_timer = 0;
            }
            if (action == 2)
            {
                MenuButton.language = "English";
                menuMain = new Menu(Menu.MenuType.MenuGeneralSettings);
                button_timer = 0;
            }
            if (action == 3)
            {
                MediaPlayer.Volume -= 0.1f;
                if (SoundEffect.MasterVolume == 0)
                {
                }
                else
                {
                    try
                    {
                        SoundEffect.MasterVolume -= 0.1f;
                    }
                    catch { }
                }
                button_timer = 0;
            }
            if (action == 4)
            {
                MediaPlayer.Volume += 0.1f;
                if (SoundEffect.MasterVolume < 0.9f)
                {
                    try
                    {
                        SoundEffect.MasterVolume += 0.1f;
                    }
                    catch { }
                }
                button_timer = 0;
            }
            if (action == 5)
            {
                MediaPlayer.Volume = 0;
                SoundEffect.MasterVolume = 0;
                button_timer = 0;
            }
            if (action == 6)
            {
                ecran = Screen.menu_principal;
                menuMain = new Menu(Menu.MenuType.MainMenu);
                button_timer = 0;
            }
            if (action == 7 && graphics.IsFullScreen == false)
            {
                graphics.ToggleFullScreen();
                button_timer = 0;
            }
            if (action == 8 && graphics.IsFullScreen == true)
            {
                graphics.ToggleFullScreen();
                button_timer = 0;
            }
            if (action == 9)
            {
                ecran = Screen.credits;
                menuMain = new Menu(Menu.MenuType.Credits);
                button_timer = 0;
            }
            if (action == 10)
            {
                ecran = Screen.reset_progression;
                menuMain = new Menu(Menu.MenuType.ResetProgress);
                button_timer = 0;
            }
        }

        private void EcranReset()
        {
            int action = 0;

            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1)
            {
                Campagne1.progression = 1;
                Campagne2.progression = 1;
                Campagne3.progression = 1;

                Joueur.campagne1 = 1;
                Joueur.campagne2 = 1;
                Joueur.campagne3 = 1;

                Compte.Enregistrer(Joueur, "Content/Save/Save.poney");

                ecran = Screen.menu_parametres;
                menuMain = new Menu(Menu.MenuType.MenuGeneralSettings);
                button_timer = 0;
            }
            if (action == 2)
            {
                ecran = Screen.menu_parametres;
                menuMain = new Menu(Menu.MenuType.MenuGeneralSettings);
                button_timer = 0;
            }
        }

        private void EcranCredit()
        {
            int action = 0;

            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1)
            {
                ecran = Screen.menu_parametres;
                menuMain = new Menu(Menu.MenuType.MenuGeneralSettings);
                button_timer = 0;
            }
        }

        private void EcranJeu(GameTime gameTime)
        {
            Main.Update(Mouse.GetState(), Keyboard.GetState(), gameTime, graphics.GraphicsDevice);

            if (Main.MainMap.game_over)
            {
                try
                {
                  Reseau.Clear();  
                }
                catch
                {}
                ecran = Screen.game_over;
                menuMain = new Menu(Menu.MenuType.GameOver);
                StopMusic(GamePlayMusic);
                loseEffect.Play();
            }

            if (Main.MainMap.fin_niveau.est_arrivee)
            {
                ecran = Screen.selecteur_map;
                menuMain = new Menu(Menu.MenuType.MapSelector);
                button_timer = 0;
                StopMusic(GamePlayMusic);
                try
                {
                    Reseau.Clear();
                }
                catch
                { }
                if (compteur_thumbnails == CurrentCampagne.progression - 1 && compteur_thumbnails < CurrentCampagne.Maps.Count)
                {
                    compteur_thumbnails++;
                    CurrentCampagne.progression++;
                    if (Campagne1.id == CurrentCampagne.id)
                        Campagne1 = CurrentCampagne;
                    else if (Campagne2.id == CurrentCampagne.id)
                        Campagne2 = CurrentCampagne;
                    else if (Campagne3.id == CurrentCampagne.id)
                        Campagne3 = CurrentCampagne;

                    Joueur.campagne1 = Campagne1.progression;
                    Joueur.campagne2 = Campagne2.progression;
                    Joueur.campagne3 = Campagne3.progression;
                    Compte.Enregistrer(Joueur, "Content/Save/Save.poney");
                }
            }
            if (Main.MainMap.boss_entry.combat_boss)
            {
                if (Main.MainMap.parametre.texture_map == 10)
                {
                    Main = new GameMain();
                    Main.MainMap = new Map(LecteurMap.lecture("map04.txt"), multi, joueur, IPMulti, 1, 0);
                    path_map = "map04.txt";
                    ecran = Screen.jeu;
                    GamePlayMusic = Ressources.HightVoltage;
                    PlayMusic(GamePlayMusic);
                    button_timer = 0;
                    Campagne1.progression++;
                }
                else if (Main.MainMap.parametre.texture_map == 14)
                {
                    Main = new GameMain();
                    Main.MainMap = new Map(LecteurMap.lecture("map15.txt"), multi, joueur, IPMulti, 1, 0);
                    path_map = "map15.txt";
                    ecran = Screen.jeu;
                    GamePlayMusic = Ressources.FireYourGuns;
                    PlayMusic(GamePlayMusic);
                    button_timer = 0;
                    Campagne2.progression++;
                }
                else if (Main.MainMap.parametre.texture_map == 8)
                {
                    Main = new GameMain();
                    Main.MainMap = new Map(LecteurMap.lecture("map09.txt"), multi, joueur, IPMulti, 1, 0);
                    path_map = "map09.txt";
                    ecran = Screen.jeu;
                    GamePlayMusic = Ressources.BlackIce;
                    PlayMusic(GamePlayMusic);
                    button_timer = 0;
                    Campagne3.progression++;
                }
            }
            if (Main.MainMap.gagne)
            {
                try
                {
                    Reseau.Clear();
                }
                catch
                { }
                if (Main.MainMap.parametre.texture_map == 4)
                {
                    ecran = Screen.win;
                    menuMain = new Menu(Menu.MenuType.WinScreen);
                    button_timer = 0;
                    StopMusic(GamePlayMusic);
                    winEffect.Play();
                }
                if (Main.MainMap.parametre.texture_map == 9)
                {
                    ecran = Screen.win;
                    menuMain = new Menu(Menu.MenuType.WinScreen);
                    button_timer = 0;
                    StopMusic(GamePlayMusic);
                    winEffect.Play();
                }
                if (Main.MainMap.parametre.texture_map == 15)
                {
                    ecran = Screen.win;
                    menuMain = new Menu(Menu.MenuType.WinScreen);
                    button_timer = 0;
                    StopMusic(GamePlayMusic);
                    winEffect.Play();
                }
            }
            if (Main.MainMap.failleActive)
            {
                if (this.graphics.IsFullScreen == false)
                {
                    this.graphics.ToggleFullScreen();
                }
                ecran = Screen.bsod;
                menuMain = new Menu(Menu.MenuType.BSOD);
                button_timer = 0;
                this.IsMouseVisible = false;
                StopMusic(GamePlayMusic);
                beep.Play();
            }
            if (!Main.MainMap.connection)
            {
                ecran = Screen.deconnexion;
                menuMain = new Menu(Menu.MenuType.Deconnexion);
                StopMusic(GamePlayMusic);
            }
        }

        private void EcranGameover()
        {
            int action = 0;

            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1)
            {
                ecran = Screen.selecteur_map;
                menuMain = new Menu(Menu.MenuType.MapSelector);
                button_timer = 0;
            }
        }

        private void EcranGamewin()
        {
            int action = 0;

            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1)
            {
                ecran = Screen.Campagne;
                menuMain = new Menu(Menu.MenuType.Campagne);
                button_timer = 0;
            }
        }

        private void EcranDeconnection()
        {
            int action = 0;

            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1)
            {
                ecran = Screen.menu_principal;
                menuMain = new Menu(Menu.MenuType.MainMenu);
                button_timer = 0;
            }
        }

        private void EcranConnection()
        {
            Lobby_ready = false;
            int action = 0;
            IP_.Update(Keyboard.GetState(), oldKeyboard);
            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1)
            {
                try
                {
                    IPMulti = IPAddress.Parse(IP_.input);
                    ecran = Screen.selecteur_map;
                    menuMain = new Menu(Menu.MenuType.Lobby);
                    ecran = Screen.lobby;
                    lobby = new Lobby(IPMulti, Joueur.pseudo);
                    button_timer = 0;
                }
                catch
                {
                    IP_.input = "Mauvaise IP";
                }
            }
            if (action == 2)
            {
                ecran = Screen.multioupas;
                menuMain = new Menu(Menu.MenuType.MultiOrNot);
                button_timer = 0;
            }
        }

        private void EcranCampagne()
        {
            int action = 0;
            compteur_thumbnails = 0;

            if (button_timer == 20)
            {
                action = menuMain.Update(Mouse.GetState(), Keyboard.GetState());
            }
            if (action == 1)
            {
                CurrentCampagne = Campagne1;
                ecran = Screen.selecteur_map;
                menuMain = new Menu(Menu.MenuType.MapSelector);
                button_timer = 0;
            }
            if (action == 2)
            {
                CurrentCampagne = Campagne2;
                ecran = Screen.selecteur_map;
                menuMain = new Menu(Menu.MenuType.MapSelector);
                button_timer = 0;
            }
            if (action == 3)
            {
                CurrentCampagne = Campagne3;
                ecran = Screen.selecteur_map;
                menuMain = new Menu(Menu.MenuType.MapSelector);
                button_timer = 0;
            }
            if (action == 4)
            {
                ecran = Screen.multioupas;
                menuMain = new Menu(Menu.MenuType.MultiOrNot);
                button_timer = 0;
            }
            if (action == 5)
            {
                CurrentCampagne = Campagne4;
                ecran = Screen.selecteur_map;
                menuMain = new Menu(Menu.MenuType.MapSelector);
                button_timer = 0;
            }
        }

        private void EcranIntro(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalSeconds >= 7 || Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Loading = true;
            }
        }

        private void EcranBSOD()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ecran = Screen.game_over;
                menuMain = new Menu(Menu.MenuType.GameOver);
                button_timer = 0;
                this.IsMouseVisible = true;
                if (this.graphics.IsFullScreen == true)
                {
                    graphics.ToggleFullScreen();
                }
                loseEffect.Play();
            }
        }

        Texture2D Tumbnails(Campagne CurrentCampagne, int currentChoice)
        {
            if (CurrentCampagne.id == 0)//Plaine
            {
                if (currentChoice == 0)
                    return Ressources.ThumbnailsMap02;
                else if (currentChoice == 1)
                    return Ressources.ThumbnailsMap11;
                else if (currentChoice == 2)
                    return Ressources.ThumbnailsMap10;
                else if (currentChoice == 3)
                    return Ressources.ThumbnailsMap04;
            }
            else if (CurrentCampagne.id == 1)//Volcan
            {
                if (currentChoice == 0)
                    return Ressources.ThumbnailsMap13;
                else if (currentChoice == 1)
                    return Ressources.ThumbnailsMap12;
                else if (currentChoice == 2)
                    return Ressources.ThumbnailsMap14;
                else if (currentChoice == 3)
                    return Ressources.ThumbnailsMap15;
            }
            else if (CurrentCampagne.id == 2)//Glace
            {
                if (currentChoice == 0)
                    return Ressources.ThumbnailsMap06;
                else if (currentChoice == 1)
                    return Ressources.ThumbnailsMap07;
                else if (currentChoice == 2)
                    return Ressources.ThumbnailsMap08;
                else if (currentChoice == 3)
                    return Ressources.ThumbnailsMap09;
            }
            else if (CurrentCampagne.id == 3)//Survie
            {
                if (currentChoice == 0)
                    return Ressources.ThumbnailsMap01;
                else if (currentChoice == 1)
                    return Ressources.ThumbnailsMap03;
                else if (currentChoice == 2)
                    return Ressources.ThumbnailsMap03_bonus;
                else if (currentChoice == 3)
                    return Ressources.ThumbnailsMap05;
                else if (currentChoice == 4)
                    return Ressources.ThumbnailsMap16;
            }
            else
            {
                return null;
            }
            return null;
        }

        Color colorThumbnails(Campagne CurrentCampagne, int currentChoice)
        {
            if (CurrentCampagne.id == 0)//Plaine
            {
                if (currentChoice == 0)
                    return Color.CadetBlue;
                else if (currentChoice == 1)
                    return Color.CadetBlue;
                else if (currentChoice == 2)
                    return Color.CadetBlue;
                else if (currentChoice == 3)
                    return Color.White;
            }
            else if (CurrentCampagne.id == 1)//Volcan
            {
                if (currentChoice == 0)
                    return Color.CadetBlue;
                else
                    return Color.White;
            }
            else if (CurrentCampagne.id == 2)//Glace
            {
                return Color.White;
            }
            else if (CurrentCampagne.id == 3)//Survie
            {
                if (currentChoice == 0)
                    return Color.CadetBlue;
                else
                    return Color.White;
            }
            else
            {
                return Color.White;
            }
            return Color.White;
        }

        //UPDATE & DRAW
        protected override void Update(GameTime gameTime)
        {
            if (button_timer < 20)
            {
                button_timer++;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (ecran == Screen.jeu)
                {
                    ecran = Screen.menu_pause;
                    menuMain = new Menu(Menu.MenuType.PauseMenu);
                    PlayMusic(MenuMusic);
                }
                else if (ecran == Screen.menu_principal)
                {
                    UnloadContent();
                    Exit();
                }
            }

            if (Loading)
            {
                if (!Compte_chargé)
                {
                    Compte_chargé = Compte.Charger("Content/Save/Save.poney", ref Joueur);
                    if (!Compte_chargé)
                    {
                        ecran = Screen.compte;
                        menuMain = new Menu(Menu.MenuType.Compte);
                        Loading = false;
                    }
                }
                else
                {
                    ecran = Screen.menu_principal;
                    menuMain = new Menu(Menu.MenuType.MainMenu);
                    Loading = false;
                    Campagne1.progression = Joueur.campagne1;
                    Campagne2.progression = Joueur.campagne2;
                    Campagne3.progression = Joueur.campagne3;
                }
            }

            if (ecran == Screen.intro)
            {
                EcranIntro(gameTime);
            }
            else if (ecran == Screen.compte)
            {
                EcranCompte();
            }
            else if (ecran == Screen.menu_principal)
            {
                EcranPrincipale();
            }
            else if (ecran == Screen.multioupas)
            {
                EcranChoixMode();
            }
            else if (ecran == Screen.selecteur_map)
            {
                EcranChoixCarte();
            }
            else if (ecran == Screen.lobby)
            {
                EcranLobby();
            }
            else if (ecran == Screen.menu_pause)
            {
                EcranPause();
            }
            else if (ecran == Screen.menu_preferences)
            {
                EcranPreference();
            }
            else if (ecran == Screen.menu_parametres)
            {
                EcranParametres();
            }
            else if (ecran == Screen.reset_progression)
            {
                EcranReset();
            }
            else if (ecran == Screen.credits)
            {
                EcranCredit();
            }
            else if (ecran == Screen.jeu)
            {
                EcranJeu(gameTime);
            }
            else if (ecran == Screen.game_over)
            {
                EcranGameover();
            }
            else if (ecran == Screen.win)
            {
                EcranGamewin();
            }
            else if (ecran == Screen.deconnexion)
            {
                EcranDeconnection();
            }
            else if (ecran == Screen.connection)
            {
                EcranConnection();
            }
            else if (ecran == Screen.Campagne)
            {
                EcranCampagne();
            }
            else if (ecran == Screen.bsod)
            {
                EcranBSOD();
            }

            oldKeyboard = Keyboard.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (ecran == Screen.menu_principal)
            {
                spriteBatch.Draw(Ressources.mTitleScreen, new Rectangle(0, 0, 800, 480), Color.White);
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.multioupas)
            {
                spriteBatch.Draw(Ressources.mPlayScreen, new Rectangle(0, 0, 800, 480), Color.White);
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.selecteur_map)
            {
                spriteBatch.Draw(Tumbnails(CurrentCampagne, compteur_thumbnails), new Rectangle(Divers.WidthScreen / 2 - 200, Divers.HeightScreen / 2 - 120, 400, 240), colorThumbnails(CurrentCampagne, compteur_thumbnails));
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.menu_pause)
            {
                Main.Draw(spriteBatch);
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.menu_preferences)
            {
                Main.Draw(spriteBatch);
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.menu_parametres)
            {
                GraphicsDevice.Clear(Color.Black);
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.reset_progression)
            {
                spriteBatch.Draw(Ressources.mResetProgress, new Rectangle(0, 0, 800, 480), Color.White);
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.credits)
            {
                spriteBatch.Draw(Ressources.mCredits, new Rectangle(0, 0, 800, 480), Color.White);
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.credits)
            {
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.jeu)
            {
                Main.Draw(spriteBatch);
            }
            else if (ecran == Screen.game_over)
            {
                spriteBatch.Draw(Ressources.mGameOverScreen, new Rectangle(0, 0, 800, 480), Color.White);
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.win)
            {
                spriteBatch.Draw(Ressources.mWinSreen, new Rectangle(0, 0, 800, 480), Color.White);
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.deconnexion)
            {
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.connection)
            {
                spriteBatch.Draw(Ressources.mPlayScreen, new Rectangle(0, 0, 800, 480), Color.White);
                menuMain.Draw(spriteBatch);
                IP_.DrawButton(spriteBatch, Divers.WidthScreen / 2 - 50, 150, 1f);
            }
            else if (ecran == Screen.lobby)
            {
                if (compteur_thumbnails == 0)
                {
                    spriteBatch.Draw(Ressources.ThumbnailsMap01, new Rectangle(Divers.WidthScreen / 2 + 100, Divers.HeightScreen / 2 - 120, 200, 120), Color.CadetBlue);
                    menuMain.Draw(spriteBatch);
                }
                else if (compteur_thumbnails == 1)
                {
                    spriteBatch.Draw(Ressources.ThumbnailsMap03, new Rectangle(Divers.WidthScreen / 2 + 100, Divers.HeightScreen / 2 - 120, 200, 120), Color.White);
                    menuMain.Draw(spriteBatch);
                }
                else if (compteur_thumbnails == 2)
                {
                    spriteBatch.Draw(Ressources.ThumbnailsMap05, new Rectangle(Divers.WidthScreen / 2 + 100, Divers.HeightScreen / 2 - 120, 200, 120), Color.White);
                    menuMain.Draw(spriteBatch);
                }
                lobby.Draw(spriteBatch);
            }
            else if (ecran == Screen.compte)
            {
                Pseudo.DrawButton(spriteBatch, Divers.WidthScreen / 2 - 50, 150, 1f);
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.Campagne)
            {
                spriteBatch.Draw(Ressources.mPlayScreen, new Rectangle(0, 0, 800, 480), Color.White);
                menuMain.Draw(spriteBatch);
            }
            else if (ecran == Screen.intro)
            {
                spriteBatch.Draw(Ressources.mLogoTeam, new Rectangle(0, 0, 800, 480), Color.White);
                menuMain.Draw(spriteBatch);
                if (gameTime.TotalGameTime.TotalSeconds >= 2)
                {
                    spriteBatch.Draw(Ressources.mLogoTeamComplet, new Rectangle(0, 0, 800, 480), Color.White);
                    menuMain.Draw(spriteBatch);
                }
                if (gameTime.TotalGameTime.TotalSeconds >= 4)
                {
                    spriteBatch.Draw(Ressources.mPresents, new Rectangle(0, 0, 800, 480), Color.White);
                    menuMain.Draw(spriteBatch);
                }
                if (gameTime.TotalGameTime.TotalSeconds >= 5)
                {
                    spriteBatch.Draw(Ressources.mTitleScreen, new Rectangle(0, 0, 800, 480), Color.White);
                    menuMain.Draw(spriteBatch);
                }
            }
            else if (ecran == Screen.bsod)
            {
                spriteBatch.Draw(Ressources.BSOD, new Rectangle(0, 0, Divers.WidthScreen, Divers.HeightScreen), Color.White);
                menuMain.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
