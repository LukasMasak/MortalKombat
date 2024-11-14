using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterForceChoose))]
public class SomeEditor : Editor
{
    
    List<string> _choices;
    private SerializedProperty _choiceIndex1;
    private SerializedProperty _choiceIndex2;

    private int _idx1 = 0;
    private int _idx2 = 0;

    public void OnEnable()
    {
        _choices = CharacterLoader.GetAllPossibleCharacters();

        _choiceIndex1 = serializedObject.FindProperty("_chosenCharacter1");
        _choiceIndex2 = serializedObject.FindProperty("_chosenCharacter2");
        _idx1 = _choices.IndexOf(_choiceIndex1.stringValue);
        _idx2 = _choices.IndexOf(_choiceIndex2.stringValue);
    }

    public override void OnInspectorGUI ()
    {
        serializedObject.UpdateIfRequiredOrScript();

        _choices = CharacterLoader.GetAllPossibleCharacters();
        string[] choicesArray = _choices.ToArray();

        _idx1 = EditorGUILayout.Popup("Character 1:", _idx1, choicesArray);
        _idx2 = EditorGUILayout.Popup("Character 2:", _idx2, choicesArray);

        _choiceIndex1.stringValue = _choices[_idx1];
        _choiceIndex2.stringValue = _choices[_idx2];

        serializedObject.ApplyModifiedProperties();
    }
}