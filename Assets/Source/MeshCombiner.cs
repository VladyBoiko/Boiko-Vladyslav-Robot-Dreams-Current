using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeshCombiner : MonoBehaviour
{
    [SerializeField] private string _meshName;
    [SerializeField] private bool _mergeSubMeshes = false;
    
    [SerializeField] private MeshFilter[] _meshFilters;
    
    private Mesh _mesh;

    [ContextMenu("Combine")]
    public void Combine()
    {
        _mesh = new Mesh();
        
        Matrix4x4 objectMatrix = transform.worldToLocalMatrix;
        CombineInstance[] instances = new CombineInstance[_meshFilters.Length];

        for (int i = 0; i < _meshFilters.Length; i++)
        {
            CombineInstance instance = new CombineInstance();
            instance.mesh = _meshFilters[i].sharedMesh;
            instance.transform = objectMatrix * _meshFilters[i].transform.localToWorldMatrix;
            instances[i] = instance;
        }
        
        _mesh.CombineMeshes(instances, _mergeSubMeshes, true);
        
#if UNITY_EDITOR
        string path = EditorUtility.SaveFilePanelInProject("Save new mesh", _meshName, "asset", "");
        AssetDatabase.CreateAsset(_mesh, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
}
