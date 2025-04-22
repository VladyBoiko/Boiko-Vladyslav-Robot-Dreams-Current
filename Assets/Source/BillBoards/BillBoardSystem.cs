using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using HealthSystems;
using Services;

namespace BillBoards
{
    public class BillBoardSystem : MonoServiceBase
    {
        [SerializeField] protected Camera _camera;

        // [SerializeField] protected BillboardBase[] _billboards;
        [SerializeField] protected List<BillboardBase> _billboards = new List<BillboardBase>();
        
        public override Type Type { get; } = typeof(BillBoardSystem);
        
        [ContextMenu("Find Billboards")]
        private void FindBillboards()
        {
#if UNITY_EDITOR
            _billboards.AddRange(FindObjectsOfType<BillboardBase>(true));
            EditorUtility.SetDirty(this);
#endif
        }

        protected override void Awake()
        {
            base.Awake();
            
            for (int i = 0; i < _billboards.Count; ++i)
                _billboards[i].SetCamera(_camera);
        }
        
        public void AddBillboard(BillboardBase billboard)
        {
            if (billboard == null) return;

            if (!_billboards.Contains(billboard))
            {
                _billboards.Add(billboard);
                billboard.SetCamera(_camera);
            }
        }
    }
}
