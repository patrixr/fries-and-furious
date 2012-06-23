using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WP7QuestGame
{
    public static class GameSettings
    {




        public static int RunnerHeight = 70;
        public static int RunnerWidth = 50;
        public static int BlockWidth = 50;
        public static int BlockHeight = 48;


        public static int RunnerAnimationSwitchDelay = 38;
        public static int BlockAnimationSwitchDelay = 38;
        public static int BlockDestructionSwitchDelay = 20;

        public static string BackgroundName = "background";
        public static string RunnerName = "runner";
        public static string BlockName = "block";

        public static int ParticleGenerationCount = 1;

        public static int ChunkFileCount = 28;

#if DEBUG

        public static String[] MapsToTest =  null;//new String[] { "20.txt" };


#endif

        public static bool AllowMusic = false;

        public static bool VolumeOn = false;

        /// <summary>
        /// Speed in px/s
        /// </summary> 
        /// 
        public static int DefaultScrollSpeed = 375;
        public static int BackGroundScrollSpeed = 80;
        public static int ScrollSpeed = 375;
        public static int MaxScrollSpeed = 1000;
        public static int RunnerYSpeed = 550 ;


    }
}
