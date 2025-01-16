using System.IO;
using UnityEditor;
using UnityEngine;

namespace Common.Save
{
    public static class SaveService
    {
        private const string originalPath = "JSON/SaveData/Floor01_Original";

        private static readonly string savePath = $"{Application.streamingAssetsPath}/Floor01_Save.json";

        private static bool useSave;

        /// <summary>
        /// Save파일이 있는지 반환
        /// </summary>
        public static bool Exists
        {
            get
            {
                return File.Exists(savePath);
            }
        }

        public static void SetCurPath(bool useSavePath)
        {
            if(Exists)
                useSave = useSavePath;
            else
                useSave = false;
        }

        public static void Save(string json)
        {
            File.WriteAllText(savePath, json);
        }

        public static SaveData Load()
        {
            SaveData saveData = null;
            if (!useSave)
            {
                TextAsset json = Resources.Load<TextAsset>(originalPath);
                saveData = JsonUtility.FromJson<SaveData>(json.text);
            }
            else
            {
                saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));
            }
            return saveData;
        }

    }
}