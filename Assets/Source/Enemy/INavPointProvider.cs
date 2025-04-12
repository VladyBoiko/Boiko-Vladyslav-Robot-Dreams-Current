using UnityEngine;

namespace Enemy
{
    public interface INavPointProvider
    {
        Vector3 GetPoint();
    }
}