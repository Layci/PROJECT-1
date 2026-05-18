using UnityEngine;
using UnityEditor;

public class MissingScriptRemover
{
    [MenuItem("Tools/Remove Missing Scripts In Selection")]
    static void RemoveMissingScripts()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        foreach (GameObject go in selectedObjects)
        {
            RemoveMissingScriptsRecursively(go);
        }

        Debug.Log("Missing Scripts Removed!");
    }

    static void RemoveMissingScriptsRecursively(GameObject obj)
    {
        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);

        foreach (Transform child in obj.transform)
        {
            RemoveMissingScriptsRecursively(child.gameObject);
        }
    }
}