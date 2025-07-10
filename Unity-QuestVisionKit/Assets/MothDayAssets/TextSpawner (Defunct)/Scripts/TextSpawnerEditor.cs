using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextSpawner))]
public class TextSpawnerEditor : Editor
{
    private TextSpawner textSpawner;
    
    void OnEnable()
    {
        textSpawner = (TextSpawner)target;
        
        // Auto-spawn text when script is first added or selected
        if (textSpawner.letterData != null && textSpawner.prefabToSpawn != null)
        {
            textSpawner.UpdateText();
        }
    }
    
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        
        // Draw the default inspector
        DrawDefaultInspector();
        
        // Check if any values changed
        if (EditorGUI.EndChangeCheck())
        {
            // Update text immediately when values change in editor
            if (textSpawner.letterData != null && textSpawner.prefabToSpawn != null)
            {
                textSpawner.UpdateText();
            }
        }
        
        EditorGUILayout.Space();
        
        // Manual control buttons
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Spawn Text"))
        {
            textSpawner.SpawnText();
        }
        
        if (GUILayout.Button("Clear Text"))
        {
            textSpawner.ClearText();
        }
        
        EditorGUILayout.EndHorizontal();
        
        // Info display
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("3D Text Info", EditorStyles.boldLabel);
        
        if (textSpawner.letterData != null)
        {
            EditorGUILayout.LabelField($"Available Letters: {textSpawner.letterData.letters.Count}");
        }
        
        var container = textSpawner.GetTextContainer();
        if (container != null)
        {
            EditorGUILayout.LabelField($"Text Container: {container.name}");
            EditorGUILayout.LabelField($"Spawned Objects: {container.transform.childCount}");
        }
        
        // Target object controls
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Target Controls", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Attach to Selected"))
        {
            if (Selection.activeTransform != null && Selection.activeTransform != textSpawner.transform)
            {
                textSpawner.AttachToTarget(Selection.activeTransform, textSpawner.offsetFromTarget);
                textSpawner.UpdateText();
            }
            else
            {
                EditorUtility.DisplayDialog("No Target", "Please select a GameObject in the scene to attach the text to.", "OK");
            }
        }
        if (GUILayout.Button("Detach"))
        {
            textSpawner.DetachFromTarget();
        }
        EditorGUILayout.EndHorizontal();
        
        // Direction helpers
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Direction Presets", EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("→ Right"))
        {
            textSpawner.textDirection = Vector3.right;
            textSpawner.lineDirection = Vector3.down;
            textSpawner.UpdateText();
        }
        if (GUILayout.Button("↑ Up"))
        {
            textSpawner.textDirection = Vector3.up;
            textSpawner.lineDirection = Vector3.left;
            textSpawner.UpdateText();
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("→ Forward"))
        {
            textSpawner.textDirection = Vector3.forward;
            textSpawner.lineDirection = Vector3.down;
            textSpawner.UpdateText();
        }
        if (GUILayout.Button("↺ Around Y"))
        {
            textSpawner.textDirection = Vector3.right;
            textSpawner.lineDirection = Vector3.forward;
            textSpawner.UpdateText();
        }
        EditorGUILayout.EndHorizontal();
        
        // Warning if missing references
        if (textSpawner.letterData == null)
        {
            EditorGUILayout.HelpBox("Letter Data is not assigned! Create one using Tools → Letter Data Creator", MessageType.Warning);
        }
        
        if (textSpawner.prefabToSpawn == null)
        {
            EditorGUILayout.HelpBox("Prefab To Spawn is not assigned! Assign a prefab to spawn for each letter point.", MessageType.Warning);
        }
    }
    
    void OnSceneGUI()
    {
        // Draw direction handles in scene view
        if (textSpawner == null) return;
        
        Vector3 pos = textSpawner.transform.position;
        
        // Draw text direction arrow
        Handles.color = Color.green;
        Handles.DrawLine(pos, pos + textSpawner.textDirection.normalized * textSpawner.fontSize);
        Handles.ConeHandleCap(0, pos + textSpawner.textDirection.normalized * textSpawner.fontSize, 
                             Quaternion.LookRotation(textSpawner.textDirection), 0.1f, EventType.Repaint);
        
        // Draw line direction arrow
        Handles.color = Color.blue;
        Handles.DrawLine(pos, pos + textSpawner.lineDirection.normalized * textSpawner.fontSize);
        Handles.ConeHandleCap(0, pos + textSpawner.lineDirection.normalized * textSpawner.fontSize, 
                             Quaternion.LookRotation(textSpawner.lineDirection), 0.1f, EventType.Repaint);
        
        // Labels
        Handles.Label(pos + textSpawner.textDirection.normalized * (textSpawner.fontSize * 1.2f), "Text Direction");
        Handles.Label(pos + textSpawner.lineDirection.normalized * (textSpawner.fontSize * 1.2f), "Line Direction");
    }
}