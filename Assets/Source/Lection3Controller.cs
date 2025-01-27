using System.Collections.Generic;
using UnityEngine;

public class Lection3Controller : MonoBehaviour
{
    [SerializeField] private string _element;
    private List<string> _elementsList = new List<string>();

    [ContextMenu("Print List")]
    public void PrintList()
    {
        if (IsListEmpty()) return;
        
        string result = $"List: {string.Join(", ", _elementsList)}.";
        Debug.Log(result);
    }
    
    [ContextMenu("Add Element To List")]
    public void AddElement()
    {
        if (!IsElementValid()) return;
        
        if (_elementsList.Contains(_element))
        {
            Debug.Log($"Element \"{_element}\" already in the list");
            return;
        }
        
        _elementsList.Add(_element);
        Debug.Log($"Element \"{_element}\" added to the list");
    }

    [ContextMenu("Remove Element From List")]
    public void RemoveElement()
    {
        if (!IsElementValid()) return;

        Debug.Log(_elementsList.Remove(_element)
            ? $"Element \"{_element}\" removed from the list"
            : $"Element \"{_element}\" not found in the list");
    }

    [ContextMenu("Clear List")]
    public void ClearList()
    {
        _elementsList.Clear();
        Debug.Log("The list has been cleared");
    }

    [ContextMenu("Sort List")]
    public void SortList()
    {
        if (IsListEmpty()) return;
        
        _elementsList.Sort();
        Debug.Log($"The list has been sorted.\nList: {string.Join(", ", _elementsList)}.");
    }
    
    private bool IsListEmpty()
    {
        if (_elementsList.Count == 0)
        {
            Debug.Log("List is empty");
            return true;
        }
        return false;
    }
    
    private bool IsElementValid()
    {
        if (string.IsNullOrEmpty(_element))
        {
            Debug.Log("Invalid element. Please try again");
            return false;
        }
        return true;
    }
}
