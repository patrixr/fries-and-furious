using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace WP7QuestGame.Components
{
    public class ChunkGenerator
    {
        private Runner _runner;
        private int _mapHeight, _mapWidth;
        private Random _random = new Random();
        private ContentManager _content;
        private BlockFactory _blockFactory;
        private Queue<int> _lastChunks = new Queue<int>();

        private Dictionary<String, String[]> LoadedChunks = new Dictionary<string, string[]>();

        public ChunkGenerator(Runner r, int mapW, int mapH, ContentManager Content)
            : base()
        {
            _mapHeight = mapH;
            _mapWidth = mapW;
            _runner = r;
            _content = Content;
            _blockFactory = new BlockFactory(_runner, 8, _content);
        }

        public List<Block> GenerateChunk()
        {
            List<Block> newBlocks = null;

            if (_blockFactory._farthestBlock == null || _blockFactory._farthestBlock.Position.X <= _mapWidth + GameSettings.BlockWidth)
            {
#if DEBUG
                if (GameSettings.MapsToTest != null && GameSettings.MapsToTest.Length > 0)
                {
                    newBlocks = LoadChunkFile("Maps\\" + GameSettings.MapsToTest[_random.Next(GameSettings.MapsToTest.Length)]);
                }
                else
                {
                    int tmp = _random.Next(GameSettings.ChunkFileCount);
                    int loop = 0;
                    while (_lastChunks.Contains(tmp) == true)
                    {
                        tmp = _random.Next(GameSettings.ChunkFileCount);
                        loop++;
                        if (loop >= 10)
                            break;
                    }

                    _lastChunks.Enqueue(tmp);
                    if (_lastChunks.Count >= 10)
                        _lastChunks.Dequeue();
                    newBlocks = LoadChunkFile("Maps\\" + tmp + ".txt");
                }
#else
                 int tmp = _random.Next(GameSettings.ChunkFileCount);
                    int loop = 0;
                    while (_lastChunks.Contains(tmp) == true)
                    {
                        tmp = _random.Next(GameSettings.ChunkFileCount);
                        loop++;
                        if (loop >= 10)
                            break;
                    }

                    _lastChunks.Enqueue(tmp);
                    if (_lastChunks.Count >= 5)
                        _lastChunks.Dequeue();
                    newBlocks = LoadChunkFile("Maps\\" + tmp + ".txt");
#endif
            }

            return newBlocks;
        }

        private List<Block> LoadChunkFile(String path)
        {
            try
            {
                if (LoadedChunks.ContainsKey(path) == false)
                {
                    Stream st = TitleContainer.OpenStream(path);
                    StreamReader sr = new StreamReader(st);

                    String[] map = new String[10];

                    for (int i = 0; i < 10; ++i)
                    {
                        String tmp = sr.ReadLine();

                        if (tmp != null)
                        {
                            tmp.Replace("\t", "    ");
                        }

                        map[i] = (tmp != null ? tmp : "");
                    }
                    LoadedChunks.Add(path, map);
                    return ProcessMapData(map);
                }
                return ProcessMapData(LoadedChunks[path]);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private List<Block> ProcessMapData(String[] map)
        {
            List<Block> newBlocks = new List<Block>();
            int startoffset;
            if (_blockFactory._farthestBlock == null)
                startoffset = _mapWidth;
            else
                startoffset = (int)_blockFactory._farthestBlock.Position.X + GameSettings.BlockWidth;

            TeleporterEntranceBlock telIn = null;
            TeleporterEntranceBlock telOut = null;

            int i = 0;
            foreach (String line in map)
            {
                int offset = startoffset;

                //foreach (char c in line)
                for (int j = 0; j < line.Length; ++j)
                {
                    char c = line[j];
                    int value = Convert.ToInt32(c) - 48;

                    if (Char.IsWhiteSpace(c))
                    {
                        offset += GameSettings.BlockWidth;
                        continue;
                    }
                    else if (value >= 0 && value <= 9)
                    {
                        int tmp = _random.Next(11);
                        if (tmp <= value)
                        {
                            Block b = _blockFactory.CreateBlock('n', new Vector2(offset, i * GameSettings.BlockHeight));
                            newBlocks.Add(b);
                            if (j < line.Length - 1 && line[j + 1] == '#')
                            {
                                ++j;
                                offset += GameSettings.BlockWidth;
                                while (j < line.Length && line[j] == '#')
                                {
                                    newBlocks.Add(_blockFactory.CreateBlock('n', new Vector2(offset, i * GameSettings.BlockHeight)));
                                    offset += GameSettings.BlockWidth;
                                    ++j;
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        Block b = _blockFactory.CreateBlock(c, new Vector2(offset, i * GameSettings.BlockHeight));
                        if (b == null)
                        {
                            offset += GameSettings.BlockWidth;
                            continue;
                        }
                        if (c == 'i')
                        {
                            telIn = (TeleporterEntranceBlock)b;
                        }
                        else if (c == 'o')
                        {
                            telOut = (TeleporterEntranceBlock)b;
                        }
                        newBlocks.Add(b);
                    }
                    offset += GameSettings.BlockWidth;
                }
                ++i;
                if (i == 10)
                    break;
            }
            if (telIn != null && telOut != null)
                telIn.Exit = telOut;
            return newBlocks;
        }


        /*delegate void ChunkGeneratorFunc(List<Block> blocks);
        struct ChunkGeneratorDescriptor
        {
            public double probability;
            public ChunkGeneratorFunc function;

            public ChunkGeneratorDescriptor(double probability, ChunkGeneratorFunc function)
            {
                this.probability = probability;
                this.function = function;
            }
        };
        private ChunkGeneratorDescriptor[] _chunkGenerators;

        public ChunkGenerator(Runner r, int mapW, int mapH, ContentManager Content)
            : base()
        {
            _mapHeight = mapH;
            _mapWidth = mapW;
            _runner = r;
            _content = Content;
            _blockFactory = new BlockFactory(_runner, 8, _content);
            _chunkGenerators = new ChunkGeneratorDescriptor[]
            {
                new ChunkGeneratorDescriptor(1.2, ChunkGenerator_Tunnel),
                new ChunkGeneratorDescriptor(0.2, ChunkGenerator_Pipe),
                new ChunkGeneratorDescriptor(0.2, ChunkGenerator_CeilingOnly),
                new ChunkGeneratorDescriptor(0.2, ChunkGenerator_FloorOnly),
                new ChunkGeneratorDescriptor(0.0, ChunkGenerator_Stairs),
                new ChunkGeneratorDescriptor(0.2, ChunkGenerator_PerforatedPipe),
            };
        }

        public List<Block> GenerateChunk()
        {
            List<Block> newBlocks = null;

            if (_blockFactory._farthestBlock == null || _blockFactory._farthestBlock.Position.X <= _mapWidth + GameSettings.BlockWidth)
                newBlocks = PickChunkGenerator();

            return newBlocks;
        }

        private List<Block> PickChunkGenerator()
        {
            List<Block> newBlocks = new List<Block>();
            double rand = _random.NextDouble();
            ChunkGeneratorFunc func = null;
            double counter = 0.0;

            foreach (ChunkGeneratorDescriptor generator in _chunkGenerators)
            {
                if ((counter += generator.probability) >= rand)
                {
                    func = generator.function;
                    break;
                }
            }
            if (func != null)
                func(newBlocks);
            else
                return null;

            return newBlocks;
        }

        private void ChunkGenerator_Tunnel(List<Block> blocks)
        {
            GeneratePlatform(blocks, 5, new Vector2(0, 0));
            GeneratePlatform(blocks, 5, new Vector2(0, 9));
            //GenerateRandomModules(blocks, new Rectangle(2, 1, 18, 8), 0.3F);
            GenerateRandomPattern(blocks, 5);
            int lastX = ((int)_blockFactory._farthestBlock.Position.X - _mapWidth) / GameSettings.BlockWidth;
            GeneratePlatform(blocks, 5, new Vector2(lastX + 1, 3));
            GeneratePlatform(blocks, 5, new Vector2(lastX + 1, 6));
        }

        private void ChunkGenerator_Pipe(List<Block> blocks)
        {
            GeneratePlatform(blocks, 20, new Vector2(0, 2));
            GeneratePlatform(blocks, 20, new Vector2(0, 7));
        }

        private void ChunkGenerator_CeilingOnly(List<Block> blocks)
        {
            GeneratePlatform(blocks, 20, new Vector2(0, 0));
            GenerateRandomModules(blocks, new Rectangle(2, 3, 18, 8), 0.3F);
        }

        private void ChunkGenerator_FloorOnly(List<Block> blocks)
        {
            GenerateRandomModules(blocks, new Rectangle(2, 1, 15, 8), 0.3F);
            GeneratePlatform(blocks, 20, new Vector2(0, 9));
        }

        private void ChunkGenerator_Stairs(List<Block> blocks)
        {
            
        }

        private void ChunkGenerator_PerforatedPipe(List<Block> blocks)
        {
            GeneratePlatform(blocks, 5, new Vector2(0, 3));
            GeneratePlatform(blocks, 5, new Vector2(0, 6));
            GeneratePlatform(blocks, 5, new Vector2(5, 3));
            GeneratePlatform(blocks, 5, new Vector2(9, 5));
            GeneratePlatform(blocks, 5, new Vector2(13, 3));
            GeneratePlatform(blocks, 5, new Vector2(17, 5));
            GeneratePlatform(blocks, 5, new Vector2(21, 3));
        }

        private void GenerateRandomPattern(List<Block> blocks, int x)
        {
            double counter, pick;

            counter = 0.0;
            pick = _random.NextDouble();

            if (false || (counter += 0.6) > pick) // Platforms
            {
                GeneratePlatform(blocks, 6, new Vector2(x, 3));
                GeneratePlatform(blocks, 6, new Vector2(x, 6));
            }
            else if ((counter += 0.4) >= pick) // Walls
            {
                GeneratePlatform(blocks, 6, new Vector2(x, 0));
                GenerateWall(blocks, 3, new Vector2(x, 1));
                GenerateWall(blocks, 3, new Vector2(x, 1));
                GeneratePlatform(blocks, 6, new Vector2(x, 9));
            }
        }
        
        private void GenerateRandomModules(List<Block> blocks, Rectangle area, float density)
        {
            int x, y, size;
            double counter, pick;

            // Set static blocks
            for (float i = 0F; i < density; i += 0.1F)
            {
                counter = 0.0;
                pick = _random.NextDouble();
                x = _random.Next(area.X, area.X + area.Width);
                y = _random.Next(area.Y, area.Y + area.Height);

                if ((counter += 0.6) > pick) // Regular platform
                {
                    size = _random.Next(1, 5);
                    GeneratePlatform(blocks, size, new Vector2(x, y));
                }
                else if ((counter += 0.2) > pick) // Vertical wall
                {
                    size = _random.Next(1, 5);
                    GenerateWall(blocks, size, new Vector2(x, y));
                }
            }

            // Set dynamic blocks
            for (float i = 0F; i < density; i += 0.1F)
            {
                counter = 0.0;
                pick = _random.NextDouble();
                do
                {
                    x = _random.Next(area.X, area.X + area.Width);
                    y = _random.Next(area.Y, area.Y + area.Height);
                }
                while (isUsedPosition(blocks, new Vector2(x, y)));

                if ((counter += 0.1) > pick) // Breakable block
                    blocks.Add(_blockFactory.CreateBlock(BlockFactory.BlockType.Destructible, new Vector2(_mapWidth + x * GameSettings.BlockWidth, GameSettings.BlockHeight * y)));
                else if ((counter += 0.1) > pick) // Bouncer
                    blocks.Add(_blockFactory.CreateBlock(BlockFactory.BlockType.Bouncer, new Vector2(_mapWidth + x * GameSettings.BlockWidth, GameSettings.BlockHeight * y)));
            }
        }

        private bool isUsedPosition(List<Block> blocks, Vector2 position)
        {
            foreach (Block block in blocks)
                if (block.Position == position)
                    return true;
            return false;
        }

        private void GeneratePlatform(List<Block> newBlocks, int size, Vector2 position)
        {
            for (int i = 0; i < size; ++i)
                newBlocks.Add(_blockFactory.CreateBlock(BlockFactory.BlockType.Regular,
                    new Vector2(_mapWidth + (position.X + i) * GameSettings.BlockWidth, position.Y * GameSettings.BlockHeight)));
        }

        private void GenerateWall(List<Block> newBlocks, int size, Vector2 position)
        {
            for (int i = 0; i < size; ++i)
                newBlocks.Add(_blockFactory.CreateBlock(BlockFactory.BlockType.Regular,
                    new Vector2(_mapWidth + position.X * GameSettings.BlockWidth, (position.Y + i) * GameSettings.BlockHeight)));
        }

        private void GenerateSpeedOrNot(List<Block> newBlocks)
        {
            //speed top
            for (int i = 0; i < 3; ++i)
                newBlocks.Add(new SpeedupBlock(_content.Load<Texture2D>("speedblock"), new Vector2(_mapWidth + (i * GameSettings.BlockWidth), 0), _runner, 8));
            GeneratePlatform(newBlocks, 20, new Vector2(3, 0));

            //speed bottom
            for (int i = 0; i < 3; ++i)
                newBlocks.Add( new SpeedupBlock(_content.Load<Texture2D>("speedblock"), new Vector2(_mapWidth + (i * GameSettings.BlockWidth), 9 * GameSettings.BlockHeight), _runner, 8));
                
            GeneratePlatform(newBlocks, 20, new Vector2(3, 9));

            // main wall
            newBlocks.Add(new DestructibleBlock(_content.Load<Texture2D>("destructibleblock"), new Vector2(_mapWidth + 3 * GameSettings.BlockWidth, 1 * GameSettings.BlockHeight), _runner, 8));
            //GeneratePlatform(newBlocks, 1, new Vector2(3, 2));
            //GeneratePlatform(newBlocks, 1, new Vector2(3, 3));
            GenerateWall(newBlocks, 2, new Vector2(3,2));
            newBlocks.Add(new DestructibleBlock(_content.Load<Texture2D>("destructibleblock"), new Vector2(_mapWidth + 3 * GameSettings.BlockWidth, 4 * GameSettings.BlockHeight), _runner, 8));
            newBlocks.Add(new DestructibleBlock(_content.Load<Texture2D>("destructibleblock"), new Vector2(_mapWidth + 3 * GameSettings.BlockWidth, 5 * GameSettings.BlockHeight), _runner, 8));
            //GeneratePlatform(newBlocks, 1, new Vector2(3, 6));
            //GeneratePlatform(newBlocks, 1, new Vector2(3, 7));
            GenerateWall(newBlocks, 2, new Vector2(3, 6));
            newBlocks.Add(new DestructibleBlock(_content.Load<Texture2D>("destructibleblock"), new Vector2(_mapWidth + 3 * GameSettings.BlockWidth, 8 * GameSettings.BlockHeight), _runner, 8));

            //stair up
            GeneratePlatform(newBlocks, 2, new Vector2(4, 3));
            GeneratePlatform(newBlocks, 2, new Vector2(5, 2));
            GeneratePlatform(newBlocks, 2, new Vector2(6, 1));

            //starDown
            GeneratePlatform(newBlocks, 2, new Vector2(4, 6));
            GeneratePlatform(newBlocks, 2, new Vector2(5, 7));
            GeneratePlatform(newBlocks, 2, new Vector2(6, 8));

            //tp down
            TeleporterEntranceBlock tpd1 = new TeleporterEntranceBlock(_content.Load<Texture2D>("teleporterentrance"), new Vector2(_mapWidth + 5 * GameSettings.BlockWidth, 8 * GameSettings.BlockHeight), _runner, 8);
            TeleporterEntranceBlock tpd2 = new TeleporterEntranceBlock(_content.Load<Texture2D>("teleporterexit"), new Vector2(_mapWidth + 14 * GameSettings.BlockWidth, 8 * GameSettings.BlockHeight), _runner, 8);
            TeleporterEntranceBlock tpd3 = new TeleporterEntranceBlock(_content.Load<Texture2D>("teleporterentrance"), new Vector2(_mapWidth + 12 * GameSettings.BlockWidth, 8 * GameSettings.BlockHeight), _runner, 8);
            TeleporterEntranceBlock tpd4 = new TeleporterEntranceBlock(_content.Load<Texture2D>("teleporterexit"), new Vector2(_mapWidth + 8 * GameSettings.BlockWidth, 8 * GameSettings.BlockHeight), _runner, 8);

            tpd3.Exit = tpd2;
            tpd1.Exit = tpd4;
            newBlocks.Add(tpd1);
            newBlocks.Add(tpd2);

            //tp up
            TeleporterEntranceBlock tpt1 = new TeleporterEntranceBlock(_content.Load<Texture2D>("teleporterentrance"), new Vector2(_mapWidth + 5 * GameSettings.BlockWidth, 1 * GameSettings.BlockHeight), _runner, 8);
            tpt1.Exit = tpd2;
            newBlocks.Add(tpt1);
            TeleporterEntranceBlock tpt2 = new TeleporterEntranceBlock(_content.Load<Texture2D>("teleporterentrance"), new Vector2(_mapWidth + 12 * GameSettings.BlockWidth, 1 * GameSettings.BlockHeight), _runner, 8);
            TeleporterEntranceBlock tpt3 = new TeleporterEntranceBlock(_content.Load<Texture2D>("teleporterexit"), new Vector2(_mapWidth + 8 * GameSettings.BlockWidth, 8 * GameSettings.BlockHeight), _runner, 8);

            tpt2.Exit = tpd2;
            newBlocks.Add(tpd3);
            newBlocks.Add(tpt2);
            newBlocks.Add(tpd4);
            newBlocks.Add(tpt3);



            //mainWall 2 only midle destructible
            // main wall
            //GeneratePlatform(newBlocks, 1, new Vector2(13, 1));
            //GeneratePlatform(newBlocks, 1, new Vector2(13, 2));
            //GeneratePlatform(newBlocks, 1, new Vector2(13, 3));
            GenerateWall(newBlocks, 3, new Vector2(13,1));
            newBlocks.Add(new DestructibleBlock(_content.Load<Texture2D>("destructibleblock"), new Vector2(_mapWidth + 13 * GameSettings.BlockWidth, 4 * GameSettings.BlockHeight), _runner, 8));
            newBlocks.Add(new DestructibleBlock(_content.Load<Texture2D>("destructibleblock"), new Vector2(_mapWidth + 13 * GameSettings.BlockWidth, 5 * GameSettings.BlockHeight), _runner, 8));
            //GeneratePlatform(newBlocks, 1, new Vector2(13, 6));
            //GeneratePlatform(newBlocks, 1, new Vector2(13, 7));
            //GeneratePlatform(newBlocks, 1, new Vector2(13, 8));
            GenerateWall(newBlocks, 3, new Vector2(13,6));
        }*/

    }

}
