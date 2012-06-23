using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;

namespace WP7QuestGame
{
    class ScoreManager
    {
        private IsolatedStorageFile savegameStorage = null;
        private static string filename = "friesandfurious.save";

        #region SINGLETON

        private static ScoreManager singleton = null;

        public static ScoreManager getInstance()
        {
            if (singleton == null)
                singleton = new ScoreManager();
            return singleton;
        }

        private ScoreManager()
        {
            CurrentScore = 0;
            BestScore = 0;

#if WINDOWS_PHONE
            savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();

            if (savegameStorage.FileExists(filename) == false)
            {
                CreateSaveFile();
            }
            else
            {
                LoadLastSave();
            }
#endif

        }

        #endregion

        public int BestScore { get; private set; }
        public int CurrentScore { get; set; }

        private void LoadLastSave()
        {
            using (IsolatedStorageFileStream fs = savegameStorage.OpenFile(filename, System.IO.FileMode.Open))
            {
                if (fs != null)
                {
                    byte[] saveBytes = new byte[4];
                    int count = fs.Read(saveBytes, 0, 4);
                    if (count > 0)
                    {
                        BestScore = System.BitConverter.ToInt32(saveBytes, 0);
                    }
                }
            }
        }

        public void SaveScore(int score)
        {
#if WINDOWS_PHONE
            if (score > BestScore)
            {
                BestScore = score;
            }
            else
            {
                return;
            }

            if (savegameStorage.FileExists(filename))
            {
                savegameStorage.DeleteFile(filename);
            }

            using (IsolatedStorageFileStream fs = savegameStorage.CreateFile(filename))
            {
                if (fs != null)
                {
                    byte[] bytes = System.BitConverter.GetBytes(score);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
#endif
        }

        public void SaveScore()
        {
            SaveScore(CurrentScore);
        }

        private void CreateSaveFile()
        {       
#if WINDOWS_PHONE

            IsolatedStorageFileStream fs = null;

            using (fs = savegameStorage.CreateFile(filename))
            {
                if (fs != null)
                {
                    byte[] bytes = System.BitConverter.GetBytes(BestScore);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

#endif
        }





    }
}
