#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class DefaultSceneLoader
{
    static DefaultSceneLoader()
    {
        var pathOfFirstScene = EditorBuildSettings.scenes[0].path; // 시작할 씬 번호
        var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
        EditorSceneManager.playModeStartScene = sceneAsset;
        //Debug.Log(pathOfFirstScene + " 씬이 에디터 플레이 모드 시작 씬으로 지정됨");
    }
}
#endif