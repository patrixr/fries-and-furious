using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WP7QuestGame.Components
{
    class BlockFactory
    {
        public enum BlockType { Regular, Destructible, Bouncer, Slider, Accelerator, TeleporterEntrance, TeleporterExit, AutoDestruct };
        public Block _farthestBlock { get; private set; }
        private Runner _runner;
        private int _defaultAnimNb;
        private ContentManager _content;

        public BlockFactory(Runner runner, int defaultAnimNb, ContentManager content)
        {
            this._farthestBlock = null;
            this._runner = runner;
            this._defaultAnimNb = defaultAnimNb;
            this._content = content;
        }

        public Block CreateBlock(char type, Vector2 position)
        {
            Block newBlock;

            switch (type)
            {
                case 'n':
                    newBlock = new Block(_content.Load<Texture2D>("block"), position, _runner, _defaultAnimNb);
                    break;
                case 'd':
                    newBlock = new DestructibleBlock(_content.Load<Texture2D>("destructibleblock"), position, _runner, _defaultAnimNb);
                    break;
                case 'b':
                    newBlock = new ReboundBlock(_content.Load<Texture2D>("reboundBlock"), position, _runner, _defaultAnimNb);
                    break;
                case 'm':
                    newBlock = new SlidingBlock(_content.Load<Texture2D>("slideblock"), position, _runner, _defaultAnimNb);
                    break;
                case 's':
                    newBlock = new SpeedupBlock(_content.Load<Texture2D>("speedblock"), position, _runner, _defaultAnimNb);
                    break;
                case 'i':
                    newBlock = new TeleporterEntranceBlock(_content.Load<Texture2D>("teleporterentrance"), position, _runner, _defaultAnimNb);
                    break;
                case 'o':
                    newBlock = new TeleporterEntranceBlock(_content.Load<Texture2D>("teleporterexit"), position, _runner, _defaultAnimNb);
                    break;
                case 'a':
                    newBlock = new AutoDestructiveBlock(_content.Load<Texture2D>("block"), position, _runner, _defaultAnimNb);
                    break;
                default:
                    newBlock = null;
                    break;
            }
            
            if (newBlock != null
                && (_farthestBlock == null || newBlock.Position.X > _farthestBlock.Position.X))
                _farthestBlock = newBlock;
            return newBlock;
        }
    }
}
