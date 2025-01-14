using UnityEngine;

namespace Common.Data
{
    public static class DataService
    {
        private static ItemTableLoader itemLoader = new ItemTableLoader();                              //아이템 테이블
        private static ItemTextTableLoader itemTextLoader = new ItemTextTableLoader();                  //아이템 텍스트 테이블
        private static InteractTextTableLoader interactTextLoader = new InteractTextTableLoader();      //상호작용 텍스트 테이블

        private static DesignEnums.LanguageType language;                                               //플레이어 로드 데이터

        public static void Init()
        {
            SetLanguage(PlayerPrefs.GetInt("Language", 0));
        }

        /// <summary>
        /// 언어설정 함수
        /// </summary>
        public static void SetLanguage(int index)
        {
            language = (DesignEnums.LanguageType)index;
        }

        /// <summary>
        /// 키로 아이템 가져오기
        /// </summary>
        public static ItemTable GetItemTableByKey(int id)
        {
            return itemLoader.GetByKey(id);
        }

        /// <summary>
        /// 아이템 텍스트 가져오기
        /// </summary>
        public static string GetItemText(int id)
        {
            ItemTextTable textTable = itemTextLoader.GetByKey(id);
            string text = string.Empty;

            switch (language)
            {
                case DesignEnums.LanguageType.English:
                    text = textTable.english;
                    break;
                case DesignEnums.LanguageType.Korean:
                    text = textTable.korean;
                    break;
            }

            return text;
        }

        /// <summary>
        /// 아이템 상호작용 텍스트 가져오기
        /// </summary>
        public static string GetItemInteractText(int id)
        {
            InteractTextTable textTable = interactTextLoader.GetByKey(id);
            string text = string.Empty;

            switch (language)
            {
                case DesignEnums.LanguageType.English:
                    text = textTable.english;
                    break;
                case DesignEnums.LanguageType.Korean:
                    text = textTable.korean;
                    break;
            }

            return text;
        }
    }
}
