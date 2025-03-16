#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class BillBoardSystem : MonoBehaviour
{
    [SerializeField] protected Camera _camera;

    [SerializeField] protected BillboardBase[] _billboards;
        
    [ContextMenu("Find Billboards")]
    private void FindBillboards()
    {
        #if UNITY_EDITOR
        _billboards = FindObjectsOfType<BillboardBase>(true);
        EditorUtility.SetDirty(this);
        #endif
    }

    protected virtual void Awake()
    {
        for (int i = 0; i < _billboards.Length; ++i)
            _billboards[i].SetCamera(_camera);
    }
}
