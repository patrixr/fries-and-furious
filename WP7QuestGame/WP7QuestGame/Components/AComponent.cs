using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WP7QuestGame.Screens;

namespace WP7QuestGame.Components
{
    public abstract class AComponent
    {
        public bool Visible { get; set; }
        public bool Initialized { get; set; }

        public Screen ParentScreen { get; set; }

        protected int zindex;
        public int ZIndex
        {
            get { return zindex; }
            set
            {
                if (value != zindex)
                {
                    if (ParentScreen != null)
                        ParentScreen.OnZIndexChange(this);
                    zindex = value;
                }
            }
        }

        public AComponent(bool visible, int zidx)
        {
            ParentScreen = null;
            Visible = visible;
            ZIndex = zidx;
        }

        public AComponent()
        {
            ParentScreen = null;
            Visible = true;
            ZIndex = 0;
        }

        /// <summary>
        /// This function is called when a component is added to a screen.
        /// </summary>
        public abstract void Initialize();

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);
    }
}
