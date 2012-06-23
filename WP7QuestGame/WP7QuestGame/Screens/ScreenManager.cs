using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace WP7QuestGame.Screens
{
    public class ScreenManager
    {
        private Stack<Screen> Screens = new Stack<Screen>();
        private int ToPop = 0;

        public Screen TopScreen
        {
            get
            {
                if (Screens.Count == 0)
                    return null;
                return Screens.Peek();
            }
        }

        #region SINGLETON

        private static ScreenManager singleton = null;
        public static ScreenManager getInstance()
        {
            if (singleton == null)
                singleton = new ScreenManager();
            return singleton;
        }

        #endregion

        public int ScreenCount
        {
            get
            {
                return Screens.Count;
            }
            private set
            {
                ;
            }
        }


        private ScreenManager()
        {
        }

        public void Draw()
        {
            if (Screens.Count == 0)
                return;
            int first = 0;

            foreach (Screen scr in Screens)
            {
                if (scr.BlocksDraw)
                    break;
                ++first;
            }
            while (first >= 0)
            {
                if (first <= Screens.Count - 1 && Screens.ElementAt(first).Visible)
                    Screens.ElementAt(first).Draw();
                --first;
            }
        }

        public void Update(GameTime gameTime)
        {
            while (ToPop > 0)
            {
                if (Screens.Count > 0)
                {
                    Screens.Peek().UnloadContent();
                    Screens.Pop();
                }
                ToPop--;
            }
            foreach (Screen scr in Screens)
            {
                scr.HandleInput();
                if (scr.BlocksInput)
                    break;
            }
            foreach (Screen scr in Screens)
            {
                scr.Update(gameTime);
                if (scr.BlocksUpdate)
                    break;
            }
        }

        public void pushScreen(Screen scr)
        {
            scr.Initialize();
            Screens.Push(scr);
        }

        public Screen popScreen()
        {
            ToPop++;
            return Screens.Peek();
        }

    }
}
