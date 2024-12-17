using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Yield
{
    public static class YieldCache
    {
        private static readonly Dictionary<float, WaitForSeconds> waitForSecondsDict = new Dictionary<float, WaitForSeconds>();
        private static readonly Dictionary<float, WaitForSecondsRealtime> waitForSecondsRealtimeDict = new Dictionary<float, WaitForSecondsRealtime>();

        /// <summary>
        /// new WaitForSeconds를 dictionary에 가져오는 함수(dictionary에 값이 없을 시엔 add해줌)
        /// </summary>
        public static WaitForSeconds WaitForSeconds(float delayTime)
        {
            if (waitForSecondsDict.TryGetValue(delayTime, out WaitForSeconds wait))
            {
                return wait;
            }

            WaitForSeconds waitForSeconds = new WaitForSeconds(delayTime);
            waitForSecondsDict.Add(delayTime, waitForSeconds);
            return waitForSeconds;
        }

        /// <summary>
        /// new WaitForSecondsRealtime를 dictionary에 가져오는 함수(dictionary에 값이 없을 시엔 add해줌)
        /// </summary>
        public static WaitForSecondsRealtime WaitForSecondsRealtime(float delayTime)
        {
            if (waitForSecondsRealtimeDict.TryGetValue(delayTime, out WaitForSecondsRealtime wait))
            {
                return wait;
            }

            WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(delayTime);
            waitForSecondsRealtimeDict.Add(delayTime, waitForSecondsRealtime);
            return waitForSecondsRealtime;
        }
    }
}
