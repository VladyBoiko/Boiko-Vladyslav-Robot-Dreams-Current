using Services;
using UnityEngine;

namespace Audio.FootSoundControllerSystem
{
    /// <summary>
    /// Abstraction of a service, that returns a footstep sound given a physics material of a collider
    /// character is standing on and a world position
    /// </summary>
    public interface IFootstepSoundService : IService
    {
        AudioClip GetFootstepSound(PhysicMaterial physicMaterial, Vector3 worldPosition);
    }
}