using UnityEngine;

namespace Audio.FootSoundControllerSystem
{
    /// <summary>
    /// Implementation if IFootstepResolver
    /// It is created with array of AudioClips, and it resolves a random one
    /// </summary>
    public class ColliderFootstepResolver : IFootstepResolver
    {
        private readonly AudioClip[] _audioClips;

        public ColliderFootstepResolver(AudioClip[] audioClips)
        {
            _audioClips = audioClips;
        }
        
        public AudioClip Resolve(Vector3 position)
        {
            return _audioClips[Random.Range(0, _audioClips.Length)];
        }
    }
}