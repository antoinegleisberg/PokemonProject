using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class GuidHolder : MonoBehaviour
{
    [SerializeField] string uid = "";
    static Dictionary<string, GuidHolder> GlobalLookup = new Dictionary<string, GuidHolder>();

    public string UniqueId => uid;


#if UNITY_EDITOR
    // Update method used for generating UUID of the SavableEntity
    private void Update()
    {
        // don't execute in playmode
        if (Application.IsPlaying(gameObject)) return;

        // don't generate Id for prefabs (prefab scene will have path as null)
        if (String.IsNullOrEmpty(gameObject.scene.path)) return;

        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty property = serializedObject.FindProperty("uid");

        while (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
        {
            property.stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }

        GlobalLookup[property.stringValue] = this;
    }
#endif

    private bool IsUnique(string candidate)
    {
        if (!GlobalLookup.ContainsKey(candidate)) return true;

        if (GlobalLookup[candidate] == this) return true;

        // Handle scene unloading cases
        if (GlobalLookup[candidate] == null)
        {
            GlobalLookup.Remove(candidate);
            return true;
        }

        // Handle edge cases like designer manually changing the UUID
        if (GlobalLookup[candidate].UniqueId != candidate)
        {
            GlobalLookup.Remove(candidate);
            return true;
        }

        return false;
    }
}
