using UnityEngine;

namespace Audio.FootSoundControllerSystem
{
    /// <summary>
    /// Abstraction of a resolver, that gives AudioClip of footstep taking world position into consideration
    /// </summary>
    public interface IFootstepResolver
    {
        AudioClip Resolve(Vector3 position);
    }
}