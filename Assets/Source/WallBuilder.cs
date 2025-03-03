using UnityEngine;

public class WallBuilder : MonoBehaviour
{   
    [SerializeField] private GameObject _gameObject;
    
    [SerializeField] private int _rowNumber;
    [SerializeField] private int _columnNumber;
    [SerializeField] private bool _rotate;

    [ContextMenu("Build Wall")]
    public void BuildWall()
    {
        GameObject rootObject = new GameObject();
        rootObject.transform.localPosition = _gameObject.transform.localPosition;
        rootObject.name = "Wall";
        for (int row = 0; row < _rowNumber; row++)
        {
            for (int column = 0; column < _columnNumber; column++)
            {
                GameObject newObject = Instantiate(_gameObject, rootObject.transform, true);
                Debug.Log(newObject.name);
                float xPosition = _gameObject.transform.localPosition.x + 
                                  column * _gameObject.transform.localScale.x;
                float yPosition = _gameObject.transform.localPosition.y + 
                                  row * _gameObject.transform.localScale.y;

                newObject.transform.position = new Vector3(xPosition, yPosition, _gameObject.transform.localPosition.z);
            }
        }

        if (_rotate)
        {
            rootObject.transform.Rotate(0, 90f, 0);
        }
    }
}
