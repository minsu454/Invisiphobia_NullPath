using Common.EnumExtensions;
using Common.Objects;
using Common.Path;
using Common.SceneEx;
using UnityEngine;
using UnityEngine.Rendering;

namespace Common.VolumeEx
{
    public static class VolumeManagerEx
    {
        private static SetVolume volume;
        public static Volume Volume { get { return volume.Volume; } }

        public static void Init()
        {
            SceneJobLoader.Add(LoadPriorityType.Volume, OnSceneLoaded);
        }

        /// <summary>
        /// 씬 로드시 bgm깔아주는 이벤트 함수
        /// </summary>
        private static void OnSceneLoaded(string sceneName)
        {
            GameObject prefab = ObjectManager.Return<GameObject>(AddressablePath.VolumePath(sceneName));

            GameObject volumeGo = Object.Instantiate(prefab);

            if (!volumeGo.TryGetComponent(out volume))
            {
                Debug.LogError($"GameObject Is Not SetVolume Inheritance : {volumeGo}");
                return;
            }

            volume.Init();
        }

        public static void SetBrightness(float value)
        {
            volume.SetBrightness(value);
        }

        public static void SetGamma(float value)
        {
            volume.SetGamma(value);
        }

        public static void SetMotionBlur(float index)
        {
            volume.SetMotionBlur(index == 1 ? "true" : "false");
        }
    }
}

