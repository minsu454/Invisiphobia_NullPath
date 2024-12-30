using Common.Timer;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Common.AnimationEx
{ 
    public static class AnimationExtansions
    {
        //animType에 hash를 저장해주는 Dictionary
        private static readonly Dictionary<AnimType, int> _animHashDic = new Dictionary<AnimType, int>();

        /// <summary>
        /// 애니메이션 세팅해주는 함수
        /// </summary>
        public static void SetAnimation(Animator animator, AnimType animType) {
            if (animType == AnimType.None) {
                Debug.LogError("Animation type is not set!!");
                return;
            }

            if (compareAnimatorAnim(animator, animType, out int hash)) {
                animator.Play(hash, -1, 0);
            }
        }

        /// <summary>
        /// hash와 animator anim비교하는 함수
        /// </summary>
        public static bool compareAnimatorAnim(Animator animator, AnimType animType) {
            int hash = animType.GetAnimHash();
            if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != hash)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// hash와 animator anim비교하는 함수(hash코드 뱉어냄)
        /// </summary>
        public static bool compareAnimatorAnim(Animator animator, AnimType animType, out int hash)
        {
            int tempHash = animType.GetAnimHash();
            if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash != tempHash)
            {
                hash = tempHash;
                return true;
            }

            hash = 0;
            return false;
        }

        /// <summary>
        /// 해쉬를 dictionary에 가져오는 함수(dictionary에 값이 없을 시엔 add해줌)
        /// </summary>
        private static int GetAnimHash(this AnimType animType) {
            if (_animHashDic.TryGetValue(animType, out int hush)) {
                return hush;
            }

            string animTypeStr = animType.ToString();
            hush = Animator.StringToHash(animTypeStr);

            _animHashDic.Add(animType, hush);

            return hush;
        }
    }
}
