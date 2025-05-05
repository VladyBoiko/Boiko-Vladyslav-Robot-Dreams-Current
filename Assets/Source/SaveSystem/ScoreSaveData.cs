using System;
using UnityEngine;

namespace SaveSystem
{
    [Serializable]
    public struct ScoreSaveData
    {
        public Vector2Int kd;
        public int score;
        public int hitCount;
        public int shotCount;
    }
}