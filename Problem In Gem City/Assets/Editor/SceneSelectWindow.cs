using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSelectWindow : EditorWindow{

    [MenuItem ("Custom/Scene Select")]

    public static void ShowWindow(){
        EditorWindow.GetWindow(typeof(SceneSelectWindow));
    }

    private void OnGUI() {
        
        //Window Code
        string[] scenePaths = GetActiveScenePaths();

        foreach ( string s in scenePaths) {
            bool isActive = false;
            if( GUILayout.Button(isActive ? "Reload" : "Load", GUILayout.Width(56f))) {
                if( EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                    EditorSceneManager.OpenScene(s);
                }
            }
            EditorGUILayout.LabelField(s);
        }
        
    }

    private static string[] ReadNames() {
        List<string> temp = new List<string>();
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes) {
            if (S.enabled) {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                temp.Add(name);
            }
        }
        return temp.ToArray();

    }

    private static string[] GetActiveScenePaths() {
        string[] activeScenePaths = new string[EditorSceneManager.loadedSceneCount];

        for( int i = 0; i < EditorSceneManager.loadedSceneCount; i++) {
            activeScenePaths[i] = EditorSceneManager.GetSceneAt(i).path;
        }

        return activeScenePaths;
    }

}


