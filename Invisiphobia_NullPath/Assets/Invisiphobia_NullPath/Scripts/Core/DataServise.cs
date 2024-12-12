namespace Common.Data
{
    public static class DataServise
    {
        private static ItemTableLoader itemLoader = new ItemTableLoader();
        private static ItemTextTableLoader itemTextLoader = new ItemTextTableLoader();

        private static PlayerLoadData playerLoader = new PlayerLoadData();

        public static void Init()
        {
            playerLoader.Language = DesignEnums.LanguageType.Korean;
        }

        /// <summary>
        /// 키로 아이템 가져오기
        /// </summary>
        public static ItemTable GetItemTableByKey(int key)
        {
            return itemLoader.GetByKey(key);
        }

        /// <summary>
        /// 인덱스로 아이템 가져오기
        /// </summary>
        public static ItemTable GetItemTableByIndex(int index)
        {
            return itemLoader.GetByIndex(index);
        }

        /// <summary>
        /// 아이템 텍스트 가져오기
        /// </summary>
        public static string GetItemText(int key)
        {
            ItemTextTable textTable = itemTextLoader.GetByKey(key);
            string text = string.Empty;

            switch (playerLoader.Language)
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
