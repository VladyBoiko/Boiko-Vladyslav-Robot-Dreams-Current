using System;
using UnityEngine;

namespace Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "Data/Scene Data", order = 0)]
    public class SceneData : ScriptableObject
    {
        [Serializable]
        public struct SceneEntry
        {
            public string sceneName;
            public string scenePath;
        }
        
        [SerializeField] private string _bootSceneName;
        [SerializeField] private SceneEntry[] _lobbySceneNames;
        [SerializeField] private SceneEntry[] _gameplaySceneNames;
        
        private string[] _sceneNames;
        public string BootSceneName => _bootSceneName;
        public SceneEntry[] LobbySceneNames => _lobbySceneNames;
        public SceneEntry[] GameplaySceneNames => _gameplaySceneNames;
        
        public string[] SceneNames => _sceneNames;

        private void OnValidate()
        {
            Array.Resize(ref _sceneNames, _gameplaySceneNames.Length + _lobbySceneNames.Length);
            for (int i = 0; i < _gameplaySceneNames.Length; ++i)
                _sceneNames[i] = _gameplaySceneNames[i].sceneName;
            for (int i = 0; i < _lobbySceneNames.Length; ++i)
            {
                _sceneNames[i+_gameplaySceneNames.Length] = _lobbySceneNames[i].sceneName;
            }
        }
    }
}