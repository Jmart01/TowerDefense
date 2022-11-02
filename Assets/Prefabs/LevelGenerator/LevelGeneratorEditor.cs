using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{    
    bool autoUpdate = false;
    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI();
        autoUpdate = GUILayout.Toggle(autoUpdate, "Auto Update Materials");
        if(GUILayout.Button("Update Floors"))
        {
            ((LevelGenerator)target).RebuildFloor();
        }
        if(autoUpdate)
        {
            ((LevelGenerator)target).SnapLinePointToFloor();
        }
        else
        {
            if(GUILayout.Button("Update Floor Materials"))
            {
                ((LevelGenerator)target).SnapLinePointToFloor();
            }
        }
    }
}
