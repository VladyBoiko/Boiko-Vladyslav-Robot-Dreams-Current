using System;
using Data.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Source.Editor
{
    /// <summary>
    /// Editor of Localization Data Scriptable object
    /// </summary>
    [CustomEditor(typeof(LocalizationData))]
    public class LocalizationDataEditor : UnityEditor.Editor
    {
        // Set of cached values that allows custom foldouts to function properly
        private bool _languagesFoldout = false;
        private bool _termsFoldout = false;
        private bool _dataFoldout = false;
        private bool[] _languageFoldouts;
        private GUIContent[] _termLabels;

        /// <summary>
        /// Called by UnityEditor to draw an inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Draw base inspector, for example, serialzied fields
            base.OnInspectorGUI();

            // Foldout of languages group
            _languagesFoldout = EditorGUILayout.Foldout(_languagesFoldout, "Languages");
            
            // Caching serialized properties of scriptable object, in order to have native way to read and write
            SerializedProperty languages = serializedObject.FindProperty("_languages");
            SerializedProperty terms = serializedObject.FindProperty("_terms");
            SerializedProperty languageEntries = serializedObject.FindProperty("_languageEntries");

            // Draw next block only when foldout is open
            if (_languagesFoldout)
            {
                // Increase indent level by 1, meaning padding from left side
                EditorGUI.indentLevel++;
                
                // For each language in _languages array of scriptable object
                for (int i = 0; i < languages.arraySize; ++i)
                {
                    SerializedProperty language = languages.GetArrayElementAtIndex(i);
                    
                    // Begin a horizontal group
                    EditorGUILayout.BeginHorizontal();
                    
                    // Regular string property (if with attribute, appropriate drawer will be called by UnityEditor)
                    EditorGUILayout.PropertyField(language);
                    
                    // - button, in order to remove element
                    if (GUILayout.Button("-", GUILayout.Width(20f)))
                    {
                        languages.DeleteArrayElementAtIndex(i);
                        languageEntries.DeleteArrayElementAtIndex(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                // Button to add new language name into collection of languages
                if (GUILayout.Button("Add Language"))
                {
                    languages.InsertArrayElementAtIndex(languages.arraySize);
                }
                
                EditorGUI.indentLevel--;
            }

            // if languages where changed, added new or removed old, foldouts need to be resized
            if (_languageFoldouts == null || _languageFoldouts.Length != languages.arraySize)
                Array.Resize(ref _languageFoldouts, languages.arraySize);
            
            // Foldout for terms group
            _termsFoldout = EditorGUILayout.Foldout(_termsFoldout, "Terms");
            
            /*for (int i = 0; i < _languageFoldouts.Length; ++i)
            {
                if (_languageFoldouts[i] == null || _languageFoldouts[i].Length != terms.arraySize)
                    Array.Resize(ref _languageFoldouts[i], terms.arraySize);
            }*/
            
            // Draw next block only when foldout is open
            if (_termsFoldout)
            {
                // Increase indent level by 1, meaning padding from left side
                EditorGUI.indentLevel++;
                
                // For each term in _terms array of scriptable object
                for (int i = 0; i < terms.arraySize; ++i)
                {
                    SerializedProperty term = terms.GetArrayElementAtIndex(i);
                    
                    // Begin a horizontal group
                    EditorGUILayout.BeginHorizontal();
                    
                    // Regular string property (if with attribute, appropriate drawer will be called by UnityEditor)
                    EditorGUILayout.PropertyField(term);
                    
                    // - button, in order to remove element
                    if (GUILayout.Button("-", GUILayout.Width(20f)))
                    {
                        terms.DeleteArrayElementAtIndex(i);
                        // when term is deleted, it needs to be deleted from language entries
                        for (int j = 0; j < languageEntries.arraySize; ++j)
                        {
                            languageEntries.GetArrayElementAtIndex(j).FindPropertyRelative("termValues").DeleteArrayElementAtIndex(i);
                        }
                    }

                    EditorGUILayout.EndHorizontal();
                }

                // Button to add new term name into collection of terms
                if (GUILayout.Button("Add Term"))
                {
                    terms.InsertArrayElementAtIndex(terms.arraySize);
                }
                
                EditorGUI.indentLevel--;
            }
            
            // If terms collection where changed, cached labels need to be resized
            if (_termLabels == null || _termLabels.Length != terms.arraySize)
            {
                Array.Resize(ref _termLabels, terms.arraySize);
            }
            
            // if any of labels id null, it needs to be created. Otherwise, name needs to be updated
            for (int i = 0; i < terms.arraySize; ++i)
            {
                string term = terms.GetArrayElementAtIndex(i).stringValue;
                if (_termLabels[i] == null)
                {
                    _termLabels[i] = new GUIContent(term);
                }
                else if (_termLabels[i].text != term)
                {
                    _termLabels[i].text = term;
                }
            }
            
            // Foldout of data itself, entries that map which term in which language to specific string
            _dataFoldout = EditorGUILayout.Foldout(_dataFoldout, "Data");

            // If languages where added or removed, entries should become the same size
            if (languageEntries.arraySize != languages.arraySize)
            {
                languageEntries.arraySize = languages.arraySize;
            }
            
            // In each language entry, collection of terms needs to be exactly the size of registered terms collection
            for (int i = 0; i < languageEntries.arraySize; ++i)
            {
                if (languageEntries.GetArrayElementAtIndex(i).FindPropertyRelative("termValues").arraySize != terms.arraySize)
                {
                    languageEntries.GetArrayElementAtIndex(i).FindPropertyRelative("termValues").arraySize = terms.arraySize;
                }
            }
            
            // Draw block only if data foldout is open
            if (_dataFoldout)
            {
                // Increase indent level by 1, meaning padding from left side
                EditorGUI.indentLevel++;

                // For each language entry
                for (int i = 0; i < languageEntries.arraySize; ++i)
                {
                    // Draw foldout for each language
                    _languageFoldouts[i] = EditorGUILayout.Foldout(_languageFoldouts[i], languages.GetArrayElementAtIndex(i).stringValue);
                    
                    // Draw this block only if corresponding foldout is open
                    if (_languageFoldouts[i])
                    {
                        // Increase indent level by 1, meaning padding from left side
                        EditorGUI.indentLevel++;
                        SerializedProperty termArray = languageEntries.GetArrayElementAtIndex(i).FindPropertyRelative("termValues");
                        
                        // Draw a property for each term in this entry, using cached labels
                        for (int j = 0; j < termArray.arraySize; ++j)
                        {
                            EditorGUILayout.PropertyField(termArray.GetArrayElementAtIndex(j), _termLabels[j]);
                        }
                        
                        EditorGUI.indentLevel--;
                    }
                }
                
                EditorGUI.indentLevel--;
            }

            // if anything was changed, serialize changes so they can be saved
            serializedObject.ApplyModifiedProperties();
        }
    }
}