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
    class Manual
    {
        /*  project_SBS is the "Sunday Bloody Sunday" project made by Alexis Guiho, David Baron, André Milon & Andry Razafindrazaka.
            It's a zombie-shooter game in the Pokemon world developed in C# with the XNA library.
            More information on : www.projet-sbs.fr/game.html

            To run the game, you must have :
            - Microsoft .NET Framework 4
            - Microsoft XNA Framework Redistributable 4.0

            To launch the game, go in :
            \projet_sbs_s4\Sunday Bloody Sunday\Sunday Bloody Sunday\Sunday Bloody Sunday\bin\x86\Debug and open Sunday Bloody Sunday.exe

            To use the collisions editor for your own Maps, go in :
            \projet_sbs_s4\Editeur\Sunday Bloody Sunday\Sunday Bloody Sunday\Sunday Bloody Sunday\bin\x86\Debug and open "data.txt"
            Set the width of your Map (the width of your picture div 16) and at the next line its height (the height of your picture div 16)
            For example, if you have a picture with the size 800x480 enter 50 and at the next line, 30
            Then go in : \projet_sbs_s4\Editeur\Sunday Bloody Sunday and open Sunday Bloody Sunday.sln with Microsoft Visual C# 2010 Express,
            add the picture of your Map to the project content, add the path of this picture in "Ressources.cs" and rebuild the project by pressing "F5"
            Choose all non-crossable boxes with a right-click and then click on "Enter" (you can use arrows to move the camera)
            You can now recover your collisons file called "map.txt" in :
            \projet_sbs_s4\Editeur\Sunday Bloody Sunday\Sunday Bloody Sunday\Sunday Bloody Sunday\bin\x86\Debug

            ~ Current manual of the game ~
            Home Screen :
                Click on "Play" to start the game
		            -Solo
			            - Electri-City
				            - You can start the first campaign
			            - Enigmagma
				            - You can start the second campaign
			            - Winter Time
				            - You can start the third campaign
			            - Survival Mode
		            - Multiplayer (/!\ You have to launch Interface.exe before which is in \projet_sbs_s4\Interface\Interface\bin\Debug)
			            Type the ip server & then click on connexion
			            - Chat available
			            - Survival tips are given
			            - Both players have to select the same Map to start the game
                Click on "Options" to set parameters
		            - Language
			            - French
			            - English
		            - Music Volume
			            - Mute
		            - Resolution Settings
			            - Full Sreen
			            - Normal Screen
		            - Reset Progress
			            - Yes
			            - No
		            - Credits
                Click on "Quit" to close the game...
	
            In Game :
                Use arrows to control the player
	            Press "N" to shoot, "P" to place a bomb, "Enter" to explode it, "T" to place a turret & "F" (then click) to call an air support on the targeted place
                Press "Escape" to pause the game and access to options
	            Press "F1" to change your weapon */
    }
}
