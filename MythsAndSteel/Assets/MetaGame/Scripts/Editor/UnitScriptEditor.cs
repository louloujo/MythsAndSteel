using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Transport_UnitScript))]
//[CustomEditor(typeof(UnitScript))]
public class UnitScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UnitScript script = (UnitScript)target;

        if(GUILayout.Button("Update Tile Unit")){
            script.AddTileUnderUnit();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            EditorUtility.SetDirty(FindObjectOfType<TilesManager>().TileList[script.ActualTiledId]);
        }

        if(GUILayout.Button("Update Unit Stat"))
        {
            script.UpdateUnitStat();
        }
    }
}
