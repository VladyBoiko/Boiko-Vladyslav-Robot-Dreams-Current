using System.Collections.Generic;
using UnityEngine;

namespace CanvasSystem
{
    public class ClosableCanvasRegistry : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        private static readonly List<Canvas> _registered = new();

        public static IReadOnlyList<Canvas> Registered => _registered;

        private void Awake()
        {
            if (_canvas != null && !_registered.Contains(_canvas))
                _registered.Add(_canvas);
        }
        
        private void OnDestroy()
        {
            if (_registered.Contains(_canvas))
                _registered.Remove(_canvas);
        }
        
        public static void Clear()
        {
            _registered.Clear();
        }
    }
}