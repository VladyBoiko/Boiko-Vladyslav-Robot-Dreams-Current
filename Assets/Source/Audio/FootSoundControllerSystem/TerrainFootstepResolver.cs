using Data.ScriptableObjects;
using UnityEngine;

namespace Audio.FootSoundControllerSystem
{
    /// <summary>
    /// Implementation of footstep resolver
    /// Holds data on arrays of audio clip, one array of random footstep sounds per
    /// Terrain layer
    /// Calculates world position into UV, then searches for index of greatest by value channel and texture index
    /// Said index is computed into a layer index, which corresponds to the index of array, from where to choose random sound
    /// </summary>
    public class TerrainFootstepResolver : IFootstepResolver
    {
        private readonly AudioClip[][] _audioClips;
        private readonly Color[][] _alphaMaps;
        private readonly Vector3 _position;
        private readonly Vector2 _size;
        private readonly int _resolution;
        private readonly Vector2 _leftSouthCorner;

        public TerrainFootstepResolver(FootSoundLibrary.TerrainLayerData[] layerData, Terrain terrain)
        {
            _audioClips = new AudioClip[layerData.Length][];

            for (int i = 0; i < layerData.Length; ++i)
            {
                _audioClips[i] = layerData[i].clips;
            }
            
            // In order to have fast access to texture data, texture data is cached as arrays of Color
            Texture2D[] alphamapTextures = terrain.terrainData.alphamapTextures;
            _alphaMaps = new Color[alphamapTextures.Length][];
            for (int i = 0; i < alphamapTextures.Length; ++i)
            {
                _alphaMaps[i] = alphamapTextures[i].GetPixels();
            }
            // Data of terrain is cached
            _position = terrain.transform.position;
            _size = new Vector2(terrain.terrainData.size.x, terrain.terrainData.size.z);
            _leftSouthCorner = new Vector2(_position.x, _position.z);
            _resolution = terrain.terrainData.alphamapResolution;
        }
        
        public AudioClip Resolve(Vector3 position)
        {
            // Normalized position, meaning a percentage, from 0 to 1
            // of where a world position is relative to size and position of terrain
            Vector2 normalizedPosition = new Vector2((position.x - _leftSouthCorner.x) / _size.x, (position.z - _leftSouthCorner.y) / _size.y);
            // uv is a normalized position (percents) multiplied by size of texture, used by terrain
            Vector2Int uv = new Vector2Int((int)(normalizedPosition.x * _resolution), (int)(normalizedPosition.y * _resolution));

            // Run through texture data in search for maximum value
            // Each channel, red, green, blue and alpha, holds weight of a separate layer, therefore are compared
            // separately, and all indicies are times 4
            int layerIndex = 0;
            float layerWeight = -1;
            for (int i = 0; i < _alphaMaps.Length; ++i)
            {
                Color[] alphaMap = _alphaMaps[i];
                Color weight = alphaMap[uv.y * _resolution + uv.x];
                if (weight.r > layerWeight)
                {
                    layerIndex = i * 4;
                    layerWeight = weight.r;
                }
                if (weight.g > layerWeight)
                {
                    layerIndex = i * 4 + 1;
                    layerWeight = weight.g;
                }
                if (weight.b > layerWeight)
                {
                    layerIndex = i * 4 + 2;
                    layerWeight = weight.b;
                }
                if (weight.a > layerWeight)
                {
                    layerIndex = i * 4 + 3;
                    layerWeight = weight.a;
                }
            }
            
            if (layerIndex >= _audioClips.Length)
                return _audioClips[0][Random.Range(0, _audioClips[0].Length)];
            return _audioClips[layerIndex][Random.Range(0, _audioClips[layerIndex].Length)];
        }
    }
}