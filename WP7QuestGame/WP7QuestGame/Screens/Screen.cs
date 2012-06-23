using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WP7QuestGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WP7QuestGame.Screens
{
    public class Screen
    {
        public bool Visible { get; set; }
        public bool BlocksUpdate { get; set; }
        public bool BlocksDraw { get; set; }
        public bool BlocksInput { get; set; }

        protected GameLoop Game;
        protected SpriteBatch SpriteBatch;
        protected List<AComponent> Components = new List<AComponent>();
        protected ContentManager Content;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">The parent game</param>
        /// <param name="visible">Whether the screen is visible or not</param>
        public Screen(GameLoop game, bool visible)
        {
            this.Game = game;
            this.SpriteBatch = new SpriteBatch(game.GraphicsDevice);
            this.Visible = visible;
            this.BlocksUpdate = true;
            this.BlocksInput = true;
            this.BlocksDraw = true;
            Content = game.Content;
        }

        public Screen(GameLoop game)
        {
            this.Game = game;
            this.SpriteBatch = new SpriteBatch(game.GraphicsDevice);
            this.Visible = true;
            this.BlocksUpdate = true;
            this.BlocksInput = true;
            this.BlocksDraw = true;
            Content = game.Content;
        }

        /// <summary>
        /// Called when the zindex of a child component is modified
        /// </summary>
        public void OnZIndexChange(AComponent child)
        {
            //@TODO optimize
            this.Components = Components.OrderBy(x => x.ZIndex).ToList();
            /*if (this.Components.Remove(child))
                for (int i = 0; i < this.Components.Count; ++i)
                {
                    if (this.Components[i].ZIndex >= child.ZIndex)
                        this.Components.Insert(i, child);
                }*/
            //this.Components = Components.OrderBy(x => x.ZIndex).ToList();
        }

        /// <summary>
        /// Adds a component to the screen
        /// </summary>
        /// <param name="newComponent">The component to add</param>
        public void AddComponent(AComponent newComponent)
        {
            newComponent.ParentScreen = this;
            if (newComponent.Initialized == false)
            {
                newComponent.Initialize();
                newComponent.Initialized = true;
            }
            if (Components.Count == 0)
                Components.Add(newComponent);
            else
            {
                /* for (int i = 0; i < Components.Count; ++i)
                 {
                     if (i == Components.Count - 1)
                     {
                         Components.Add(newComponent);
                         break;
                     }
                     else if (Components[i].ZIndex < newComponent.ZIndex)
                         continue;
                     else
                     {
                         Components.Insert(i, newComponent);
                         break;
                     }
                 }*/
                this.Components.Add(newComponent);
                this.Components = Components.OrderBy(x => x.ZIndex).ToList();
            }
        }

        public virtual void Initialize()
        {
            foreach (AComponent c in Components)
                c.Initialize();

            return;
        }

        public virtual void Draw()
        {
            SpriteBatch.Begin();
            for (int i = 0; i < Components.Count; ++i)
                Components[i].Draw(SpriteBatch);
            SpriteBatch.End();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (AComponent cmp in Components)
                cmp.Update(gameTime);
        }

        public virtual void HandleInput()
        {
            return;
        }

        public virtual void UnloadContent()
        {
            return;
        }

    }
}
