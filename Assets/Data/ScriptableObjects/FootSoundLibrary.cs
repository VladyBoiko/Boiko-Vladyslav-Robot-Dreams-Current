using System;
using System.Collections.Generic;
using Audio.FootSoundControllerSystem;
using UnityEngine;

namespace Data.ScriptableObjects
{
    /// <summary>
    /// Scriptable object that holds data of which physics material corresponds to which AudioClip
    /// of footsteps
    /// Holds data of which layer of terrain corresponds to which AudioClip of footsteps
    /// </summary>
    [CreateAssetMenu(fileName = "FootstepSoundLibrary", menuName = "Data/Audio/Footstep Library", order = 0)]
    public class FootSoundLibrary : ScriptableObject
    {
        [Serializable]
        public struct FootstepData
        {
            public PhysicMaterial material;
            public AudioClip[] clips;
        }

        [Serializable]
        public struct TerrainLayerData
        {
            public AudioClip[] clips;
        }
        
        [SerializeField] private AudioClip _defaultFootstepSound;
        [SerializeField] private PhysicMaterial _terrainMaterial;
        [SerializeField] private TerrainLayerData[] _terrainSounds;
        [SerializeField] private FootstepData[] _footstepData;

        // In order to have single point and easy access to footsteps, they are cached as IFootstepResolvers
        // Each physics material from _footstepData is used as key for instance of ColliderFootstepResolver
        // Terrain material is used as key for TerrainFootstepResolver, which resolves which layer player is on depending
        // on world space position
        private Dictionary<PhysicMaterial, IFootstepResolver> _footstepResolvers = new();

        public void Init(Terrain terrain)
        {
            IFootstepResolver terrainResolver = new TerrainFootstepResolver(_terrainSounds, terrain);
            _footstepResolvers.Add(_terrainMaterial, terrainResolver);

            for (int i = 0; i < _footstepData.Length; ++i)
            {
                FootstepData footstepData = _footstepData[i];
                IFootstepResolver resolver = new ColliderFootstepResolver(footstepData.clips);
                _footstepResolvers.Add(footstepData.material, resolver);
            }
        }
        
        public AudioClip GetFootstepSound(PhysicMaterial material, Vector3 worldPosition)
        {
            // When footstep sound is needed, physics material serves as key to lookup
            // Corresponding resolver then returns an AudioClip
            // In case of missing physics material in lookup, default sound will be returned
            if (_footstepResolvers.TryGetValue(material, out IFootstepResolver footstepResolver))
            {
                return footstepResolver.Resolve(worldPosition);
            }

            return _defaultFootstepSound;
        }
    }
}