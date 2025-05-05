using System;

namespace SaveSystem
{
    [Serializable]
    public struct SaveData
    {
        public SoundSaveData soundData;
        public LocalizationSaveData localizationData;
        public ScoreSaveData scoreData;
        public InventorySaveData inventoryData;
    }
}