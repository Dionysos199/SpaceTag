using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LetterDataCreator : EditorWindow
{
private LetterPointData letterData;
    private char currentChar = 'A';
    private Vector2 scrollPosition;
    private float gridSize = 0.1f;
    
    [MenuItem("Tools/Letter Data Creator")]
    static void Init()
    {
        LetterDataCreator window = (LetterDataCreator)EditorWindow.GetWindow(typeof(LetterDataCreator));
        window.titleContent = new GUIContent("Letter Data Creator");
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Label("Letter Data Creator", EditorStyles.boldLabel);
        
        letterData = (LetterPointData)EditorGUILayout.ObjectField("Letter Data", letterData, typeof(LetterPointData), false);
        
        if (letterData == null)
        {
            if (GUILayout.Button("Create New Letter Data"))
            {
                CreateNewLetterData();
            }
            return;
        }
        
        EditorGUILayout.Space();
        
        currentChar = (char)EditorGUILayout.IntField("Current Character (ASCII)", (int)currentChar);
        EditorGUILayout.LabelField("Character: " + currentChar);
        
        gridSize = EditorGUILayout.FloatField("Grid Size", gridSize);
        
        if (GUILayout.Button("Add Sample Letters"))
        {
            AddSampleLetters();
        }
        
        EditorGUILayout.Space();
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        // Display existing letters
        for (int i = 0; i < letterData.letters.Count; i++)
        {
            var letter = letterData.letters[i];
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"'{letter.character}'", GUILayout.Width(30));
            letter.width = EditorGUILayout.FloatField("Width", letter.width, GUILayout.Width(100));
            
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                letterData.letters.RemoveAt(i);
                EditorUtility.SetDirty(letterData);
                break;
            }
            EditorGUILayout.EndHorizontal();
            
            // Show points count
            EditorGUILayout.LabelField($"Points: {letter.points.Length}");
            EditorGUILayout.Space();
        }
        
        EditorGUILayout.EndScrollView();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(letterData);
        }
    }
    
    void CreateNewLetterData()
    {
        letterData = CreateInstance<LetterPointData>();
        
        string path = EditorUtility.SaveFilePanelInProject("Save Letter Data", "NewLetterData", "asset", "Please enter a file name to save the letter data to");
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(letterData, path);
            AssetDatabase.SaveAssets();
        }
    }
    
    void AddSampleLetters()
    {
        // Add sample letter patterns - you can expand this
        AddLetter('A', new Vector2[] {
            new Vector2(0f, 0f), new Vector2(0.1f, 0.2f), new Vector2(0.2f, 0.4f),
            new Vector2(0.3f, 0.6f), new Vector2(0.4f, 0.8f), new Vector2(0.5f, 1f),
            new Vector2(0.6f, 0.8f), new Vector2(0.7f, 0.6f), new Vector2(0.8f, 0.4f),
            new Vector2(0.9f, 0.2f), new Vector2(1f, 0f),
            new Vector2(0.3f, 0.5f), new Vector2(0.4f, 0.5f), new Vector2(0.5f, 0.5f),
            new Vector2(0.6f, 0.5f), new Vector2(0.7f, 0.5f)
        }, 1f);
        
        AddLetter('B', new Vector2[] {
            new Vector2(0f, 0f), new Vector2(0f, 0.2f), new Vector2(0f, 0.4f),
            new Vector2(0f, 0.6f), new Vector2(0f, 0.8f), new Vector2(0f, 1f),
            new Vector2(0.1f, 0f), new Vector2(0.2f, 0f), new Vector2(0.3f, 0f),
            new Vector2(0.4f, 0.1f), new Vector2(0.4f, 0.2f), new Vector2(0.4f, 0.3f),
            new Vector2(0.1f, 0.5f), new Vector2(0.2f, 0.5f), new Vector2(0.3f, 0.5f),
            new Vector2(0.4f, 0.6f), new Vector2(0.4f, 0.7f), new Vector2(0.4f, 0.8f),
            new Vector2(0.1f, 1f), new Vector2(0.2f, 1f), new Vector2(0.3f, 1f)
        }, 0.5f);
        
        AddLetter('!', new Vector2[] {
            new Vector2(0f, 0.3f), new Vector2(0f, 0.4f), new Vector2(0f, 0.5f),
            new Vector2(0f, 0.6f), new Vector2(0f, 0.7f), new Vector2(0f, 0.8f),
            new Vector2(0f, 0.9f), new Vector2(0f, 1f),
            new Vector2(0f, 0f), new Vector2(0f, 0.1f)
        }, 0.2f);
        
        EditorUtility.SetDirty(letterData);
    }
    
    void AddLetter(char character, Vector2[] points, float width)
    {
        var existingLetter = letterData.letters.Find(l => l.character == character);
        if (existingLetter != null)
        {
            existingLetter.points = points;
            existingLetter.width = width;
        }
        else
        {
            letterData.letters.Add(new LetterPointData.LetterPoints
            {
                character = character,
                points = points,
                width = width
            });
        }
    }
}
