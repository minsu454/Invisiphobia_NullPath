using System.IO;
using UnityEditor;
using UnityEngine;

namespace Common.Save
{
    public static class SaveService
    {
        private const string originalPath = "JSON/SaveData/Floor01_Original";
        private const string tempSavePath = "JSON/SaveData/Floor01_Save";

        private static readonly string savePath;

        private static string curPath = originalPath;

        static SaveService()
        {
            savePath = $"{Application.dataPath}/Resources/{tempSavePath}.json";
        }

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

        /// <summary>
        /// 로드 파일경로 반환
        /// </summary>
        public static string LoadPath
        {
            get
            {
                return curPath;
            }
        }

        public static void SetCurPath(bool useSavePath)
        {
            curPath = useSavePath ? tempSavePath : originalPath;
        }

        public static void Save(string json)
        {
            Debug.Log(savePath);
            File.WriteAllText(savePath, json);
            
        }
    }
}