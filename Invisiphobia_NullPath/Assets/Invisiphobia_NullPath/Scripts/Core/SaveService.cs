using System.IO;
using UnityEngine;

namespace Common.Save
{
    public static class SaveService
    {
        private const string originalPath = "JSON/SaveData/Floor01_Original";
        private const string tempSavePath = "JSON/SaveData/Floor01_Save";

        private static readonly string savePath;

        static SaveService()
        {
            savePath = $"{Application.dataPath}/Resources/{tempSavePath}.json";
        }

        public static bool Exists
        {
            get
            {
                return File.Exists(savePath);
            }
        }

        public static string OriginalPath
        {
            get
            {
                return originalPath;
            }
        }

        public static string SavePath
        {
            get
            {
                return tempSavePath;
            }
        }

        public static void Save(string json)
        {
            File.WriteAllText(savePath, json);
        }
    }
}