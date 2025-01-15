using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Path
{
    public static class AddressablePath
    {
        /// <summary>
        /// Loader 경로 반환 함수
        /// </summary>
        public static string LoaderPath(string name)
        {
            return $"Loader/{name}";
        }

        /// <summary>
        /// UI 경로 반환 함수
        /// </summary>
        public static string UIPath(string name)
        {
            return $"UI/{name}";
        }

        /// <summary>
        /// Puzzle 경로 반환 함수
        /// </summary>
        public static string PuzzlePath(string name)
        {
            return $"UI/Puzzle/{name}.prefab";
        }

        /// <summary>
        /// ObjectPoolSO 경로 반환 함수
        /// </summary>
        public static string ObjectPoolSOPath(string name)
        {
            return $"Pool/{name}";
        }

        /// <summary>
        /// ObjectPool 경로 반환 함수
        /// </summary>
        public static string ObjectPoolPath(string sceneName, string name)
        {
            return $"Pool/{sceneName}/{name}.prefab";
        }

        /// <summary>
        /// BGM 경로 반환 함수
        /// </summary>
        public static string BGMPath(string name)
        {
            return $"Sound/{name}";
        }

        /// <summary>
        /// 맵 파일 경로 반환 함수
        /// </summary>
        public static string MapFilePath(string name)
        {
            return $"Map/MapFile/{name}.json";
        }

        /// <summary>
        /// 아이템 경로 반환 함수
        /// </summary>
        public static string ItemPartsPath(string name)
        {
            return $"Save/ItemParts/{name}.prefab";
        }

        /// <summary>
        /// 이벤트 경로 반환 함수
        /// </summary>
        public static string EventPartsPath(string name)
        {
            return $"Save/EventParts/{name}.prefab";
        }

        /// <summary>
        /// Entity 경로 반환 함수
        /// </summary>
        public static string EntityPath(string name)
        {
            return $"Entity/{name}";
        }

        /// <summary>
        /// Entity 경로 반환 함수
        /// </summary>
        public static string VolumePath(string name)
        {
            return $"Volume/{name}Volume";
        }
    }

    public static class ScenePath
    {
        /// <summary>
        /// 실질적인 씬 이름 반환해주는 함수
        /// </summary>
        public static string SceneName(SceneType type)
        {
            return $"{type}_Scene";
        }

        /// <summary>
        /// 실질적인 씬 이름 반환해주는 함수
        /// </summary>
        public static string SceneName(string sceneName)
        {
            return $"{sceneName}_Scene";
        }
    }
}

