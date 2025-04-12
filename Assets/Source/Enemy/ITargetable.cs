using UnityEngine;

namespace Enemy
{
    public interface ITargetable
    {
        public Transform TargetPivot { get; }
    }
}