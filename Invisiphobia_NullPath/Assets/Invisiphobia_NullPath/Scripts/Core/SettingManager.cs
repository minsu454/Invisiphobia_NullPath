using Common.EnumExtensions;
using Common.Objects;
using Common.Path;
using Common.SceneEx;
using UnityEngine;
using UnityEngine.Rendering;

namespace Common.Setting
{
    public static class SettingManager
    {
        private static SetVolume volume;
        public static Volume Volume { get { return volume.Volume; } }

        private static float mouseSensitivity;
        public static float MouseSensitivity { get { return mouseSensitivity; } }

        private static bool runHold;
        public static bool RunHold { get { return runHold; } }

        private static bool zoomHold;
        public static bool ZoomHold { get { return zoomHold; } }

        private static bool crouchHold;
        public static bool CrouchHold { get { return crouchHold; } }

        public static void Init()
        {
            SceneJobLoader.Add(LoadPriorityType.Volume, OnSceneLoaded);

            SetLookSensitivity(PlayerPrefs.GetFloat("LookSensitivity", 2f));
            SetRunHold(PlayerPrefs.GetFloat("Run", 0));
            SetZoomHold(PlayerPrefs.GetFloat("Zoom", 1));
            SetCrouchHold(PlayerPrefs.GetFloat("Crouch", 0));
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

        public static void SetLookSensitivity(float value)
        {
            mouseSensitivity = value;
        }

        public static void SetZoomHold(float index)
        {
            zoomHold = index == 1 ? true : false;
        }

        public static void SetRunHold(float index)
        {
            runHold = index == 1 ? true : false;
        }

        public static void SetCrouchHold(float index)
        {
            crouchHold = index == 1 ? true : false;
        }
    }
}

