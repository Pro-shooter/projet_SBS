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
    class Ressources
    {
        // STATICS FIELDS
        public static Texture2D Player1, Player2, Player3, Player1_jetpack,
                                Map, Map02, Map03, Map04, Map05, Map06, Map07, Map08, Map09, Map10, Map11, Map12, Map13, Map14, Map15, Map16,
                                Map_transparent, Map02_transparent, Map06_transparent, Map07_transparent, Map08_transparent,
                                ThumbnailsMap01, ThumbnailsMap02, ThumbnailsMap03, ThumbnailsMap03_bonus, ThumbnailsMap04, ThumbnailsMap05, ThumbnailsMap06, ThumbnailsMap07, ThumbnailsMap08, ThumbnailsMap09, ThumbnailsMap10, ThumbnailsMap11, ThumbnailsMap12, ThumbnailsMap13, ThumbnailsMap14, ThumbnailsMap15, ThumbnailsMap16,
                                Projectile,
                                BloodParticule, ExplosionParticule, mRain, FireParticule,
                                IA1, IA1attack, IA2, IA5attack, IA3, IA3attack, IA4, IA4attack, IA5, IA6, IA6attack, IA7, IA8, IA8attack, IA9, IA9attack, IA10, IA11, IA12, IA12attack, IA13, IA13attack, IA14, IA14attack, IA15, IA15attack, IA16, IA16attack,
                                mHealthBox, mAmmoBox,
                                mExplosiveBox, mBomb, mJetPack, mPlane, mAttaqueEclair, mMolotov,
                                mTitleScreen, mGameOverScreen, mWinSreen, mPlayScreen, mCredits, mChatBox, mResetProgress, mPresents, mLogoTeam, mLogoTeamComplet,
                                mCross, mTurret,
                                BSOD, mWarning;
        // HUD
        public static SpriteFont HUD;
        // Musics
        public static Song GamePlayMusic, MenuMusic, HightVoltage, BlackIce,FireYourGuns, PikaPikaSong, RaveCat, Montagne,Suspense;
        //Sound Effects
        public static SoundEffect mSentryReady, mSentryShoot, mTire, mPlaneEffect,
                                  mExplosionEffect, mPop, mBloodEffect, mSonEclair, mRainEffect,
                                  mPika, mPika2, mRaichu, mRaichu2, mTortank, mCarabaffe, mSpectrum, mkaimorse, mloklas, monigali, motaria, mponyta, mvulcaropod,
                                  mIntroEffect, mLoseEffect, mWinEffect,
                                  mBeep;


        // LOAD CONTENTS
        public static void LoadContent(ContentManager content)
        {
            Map = content.Load<Texture2D>("Imgs/Map");
            Map02 = content.Load<Texture2D>("Imgs/Map02");
            Map03 = content.Load<Texture2D>("Imgs/Map03");
            Map04 = content.Load<Texture2D>("Imgs/Map04");
            Map05 = content.Load<Texture2D>("Imgs/Map05");
            Map06 = content.Load<Texture2D>("Imgs/Map06");
            Map07 = content.Load<Texture2D>("Imgs/Map07");
            Map08 = content.Load<Texture2D>("Imgs/Map08");
            Map09 = content.Load<Texture2D>("Imgs/Map09");
            Map10 = content.Load<Texture2D>("Imgs/Map10");
            Map11 = content.Load<Texture2D>("Imgs/Map11");
            Map12 = content.Load<Texture2D>("Imgs/Map12");
            Map13 = content.Load<Texture2D>("Imgs/Map13");
            Map14 = content.Load<Texture2D>("Imgs/Map14");
            Map15 = content.Load<Texture2D>("Imgs/Map15");
            Map16 = content.Load<Texture2D>("Imgs/Map16");
            Map_transparent = content.Load<Texture2D>("Imgs/Map_Transparent");
            Map02_transparent = content.Load<Texture2D>("Imgs/Map02_surcouche");
            Map06_transparent = content.Load<Texture2D>("Imgs/Map06_transparent");
            Map07_transparent = content.Load<Texture2D>("Imgs/Map07_transparent");
            Map08_transparent = content.Load<Texture2D>("Imgs/Map08_transparent");
            ThumbnailsMap01 = content.Load<Texture2D>("Imgs/thumbnails_map_01");
            ThumbnailsMap02 = content.Load<Texture2D>("Imgs/thumbnails_map_02");
            ThumbnailsMap03 = content.Load<Texture2D>("Imgs/thumbnails_map_03");
            ThumbnailsMap03_bonus = content.Load<Texture2D>("Imgs/thumbnails_map_03_bonus");
            ThumbnailsMap04 = content.Load<Texture2D>("Imgs/thumbnails_map_04");
            ThumbnailsMap05 = content.Load<Texture2D>("Imgs/thumbnails_map_05");
            ThumbnailsMap06 = content.Load<Texture2D>("Imgs/thumbnails_map_06");
            ThumbnailsMap07 = content.Load<Texture2D>("Imgs/thumbnails_map_07");
            ThumbnailsMap08 = content.Load<Texture2D>("Imgs/thumbnails_map_08");
            ThumbnailsMap09 = content.Load<Texture2D>("Imgs/thumbnails_map_09");
            ThumbnailsMap10 = content.Load<Texture2D>("Imgs/thumbnails_map_10");
            ThumbnailsMap11 = content.Load<Texture2D>("Imgs/thumbnails_map_11");
            ThumbnailsMap12 = content.Load<Texture2D>("Imgs/thumbnails_map_12");
            ThumbnailsMap13 = content.Load<Texture2D>("Imgs/thumbnails_map_13");
            ThumbnailsMap14 = content.Load<Texture2D>("Imgs/thumbnails_map_14");
            ThumbnailsMap15 = content.Load<Texture2D>("Imgs/thumbnails_map_15");
            ThumbnailsMap16 = content.Load<Texture2D>("Imgs/thumbnails_map_16");
            Player1 = content.Load<Texture2D>("Imgs/Chara");
            Player1_jetpack = content.Load<Texture2D>("Imgs/Chara_jetpack");
            Player2 = content.Load<Texture2D>("Imgs/Chara2");
            Player3 = content.Load<Texture2D>("Imgs/Chara3");
            Projectile = content.Load<Texture2D>("Imgs/balle");
            ExplosionParticule = content.Load<Texture2D>("Imgs/explosion");
            BloodParticule = content.Load<Texture2D>("Imgs/blood");
            FireParticule = content.Load<Texture2D>("Imgs/fire");
            mAttaqueEclair = content.Load<Texture2D>("Imgs/eclair_attack");
            IA1 = content.Load<Texture2D>("Imgs/pikachu");
            IA1attack = content.Load<Texture2D>("Imgs/pika_attack");
            IA2 = content.Load<Texture2D>("Imgs/pikachu_2");
            IA3 = content.Load<Texture2D>("Imgs/Carabaffe");
            IA3attack = content.Load<Texture2D>("Imgs/carabaffe_attack");
            IA4 = content.Load<Texture2D>("Imgs/Spectrum");
            IA4attack = content.Load<Texture2D>("Imgs/spectrum_attack");
            IA5 = content.Load<Texture2D>("Imgs/Raichu");
            IA5attack = content.Load<Texture2D>("Imgs/raichu_attack");
            IA6 = content.Load<Texture2D>("Imgs/Tortank");
            IA6attack = content.Load<Texture2D>("Imgs/tortank_attack");
            IA7 = content.Load<Texture2D>("Imgs/Elector");
            IA8 = content.Load<Texture2D>("Imgs/morse");
            IA8attack = content.Load<Texture2D>("Imgs/morseattack");
            IA9 = content.Load<Texture2D>("Imgs/Loklas");
            IA9attack = content.Load<Texture2D>("Imgs/Loklasattack");
            IA10 = content.Load<Texture2D>("Imgs/articodin");
            IA11 = content.Load<Texture2D>("Imgs/sulfura");
            IA12 = content.Load<Texture2D>("Imgs/racaillou");
            IA12attack = content.Load<Texture2D>("Imgs/racaillouattack");
            IA13 = content.Load<Texture2D>("Imgs/bouboule");
            IA13attack = content.Load<Texture2D>("Imgs/boubouleattack");
            IA14 = content.Load<Texture2D>("Imgs/lamentin");
            IA14attack = content.Load<Texture2D>("Imgs/lamentinattack");
            IA15 = content.Load<Texture2D>("Imgs/slugma");
            IA15attack = content.Load<Texture2D>("Imgs/slugmaattack");
            IA16 = content.Load<Texture2D>("Imgs/ponyta");
            IA16attack = content.Load<Texture2D>("Imgs/ponytaattack");
            HUD = content.Load<SpriteFont>("Fonts/gameFont");
            GamePlayMusic = content.Load<Song>("Music/gameplay_music");
            MenuMusic = content.Load<Song>("Music/elevator_music");
            mIntroEffect = content.Load<SoundEffect>("SoundEffects/zombie_groan");
            mLoseEffect = content.Load<SoundEffect>("SoundEffects/lose_effect");
            mWinEffect = content.Load<SoundEffect>("SoundEffects/win_effect");
            mExplosionEffect = content.Load<SoundEffect>("SoundEffects/explosion_effect");
            mBloodEffect = content.Load<SoundEffect>("SoundEffects/blood_effect");
            mPop = content.Load<SoundEffect>("SoundEffects/pop");
            mTire = content.Load<SoundEffect>("SoundEffects/tire");
            mPika = content.Load<SoundEffect>("SoundEffects/pikachu001");
            mPika2 = content.Load<SoundEffect>("SoundEffects/pikachu002");
            mRaichu = content.Load<SoundEffect>("SoundEffects/raichu_son");
            mCarabaffe = content.Load<SoundEffect>("SoundEffects/carabaffe_son");
            mTortank = content.Load<SoundEffect>("SoundEffects/tortank_son");
            mSpectrum = content.Load<SoundEffect>("SoundEffects/spectrum_son");
            mRaichu2 = content.Load<SoundEffect>("SoundEffects/raichu_son2");
            mkaimorse = content.Load<SoundEffect>("SoundEffects/kaimorse");
            mloklas = content.Load<SoundEffect>("SoundEffects/loklas");
            monigali = content.Load<SoundEffect>("SoundEffects/onigali");
            motaria = content.Load<SoundEffect>("SoundEffects/otaria");
            mponyta = content.Load<SoundEffect>("SoundEffects/ponyta");
            mvulcaropod = content.Load<SoundEffect>("SoundEffects/vulcaropod");
            mSonEclair = content.Load<SoundEffect>("SoundEffects/eclair_son");
            mHealthBox = content.Load<Texture2D>("Imgs/health_box");
            mAmmoBox = content.Load<Texture2D>("Imgs/ammo_box");
            mExplosiveBox = content.Load<Texture2D>("Imgs/explosive_box");
            mBomb = content.Load<Texture2D>("Imgs/bomb");
            mTitleScreen = content.Load<Texture2D>("Imgs/Title");
            mGameOverScreen = content.Load<Texture2D>("Imgs/GameOver");
            mWinSreen = content.Load<Texture2D>("Imgs/WinScreen");
            mPlayScreen = content.Load<Texture2D>("Imgs/melodelf");
            mCross = content.Load<Texture2D>("Imgs/cross");
            mRain = content.Load<Texture2D>("Imgs/rain");
            mPlane = content.Load<Texture2D>("Imgs/plane");
            mSentryReady = content.Load<SoundEffect>("SoundEffects/place_sentry");
            mSentryShoot = content.Load<SoundEffect>("SoundEffects/sentry_shoot");
            mRainEffect = content.Load<SoundEffect>("SoundEffects/rain");
            mPlaneEffect = content.Load<SoundEffect>("SoundEffects/plane_running");
            mCredits = content.Load<Texture2D>("Imgs/credits");
            Suspense = content.Load<Song>("Music/Suspense");
            Montagne = content.Load<Song>("Music/Montagne");
            HightVoltage = content.Load<Song>("Music/HightVoltage");
            BlackIce = content.Load<Song>("Music/BlackIce");
            FireYourGuns = content.Load<Song>("Music/Fire your guns");
            mChatBox = content.Load<Texture2D>("Imgs/chat_box");
            mJetPack = content.Load<Texture2D>("Imgs/jetpack");
            mResetProgress = content.Load<Texture2D>("Imgs/reset_progress");
            mMolotov = content.Load<Texture2D>("Imgs/molotov");
            mPresents = content.Load<Texture2D>("Imgs/presents");
            mLogoTeam = content.Load<Texture2D>("Imgs/logo_team");
            mLogoTeamComplet = content.Load<Texture2D>("Imgs/logo_team_complet");
            PikaPikaSong = content.Load<Song>("Music/pika_pika_song");
            BSOD = content.Load<Texture2D>("Imgs/bsod");
            mWarning = content.Load<Texture2D>("Imgs/warning");
            mBeep = content.Load<SoundEffect>("SoundEffects/beep");
            RaveCat = content.Load<Song>("Music/rave_cat");
            mTurret = content.Load<Texture2D>("Imgs/turretf");
        }
    }
}
