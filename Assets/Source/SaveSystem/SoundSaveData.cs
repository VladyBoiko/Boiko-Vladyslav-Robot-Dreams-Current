using System;

namespace SaveSystem
{
    [Serializable]
    public struct SoundSaveData
    {
        public static readonly SoundSaveData Default = new SoundSaveData()
            { masterVolume = 0.5f, sfxVolume = 0.5f, ambienceVolume = 0.5f };

        public float masterVolume;
        public float sfxVolume;
        public float ambienceVolume;
    }
}