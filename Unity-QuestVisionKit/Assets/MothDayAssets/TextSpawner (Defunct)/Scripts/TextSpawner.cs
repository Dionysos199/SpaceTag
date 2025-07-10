using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSpawner : MonoBehaviour
{
[Header("References")]
    public LetterPointData letterData;
    public GameObject prefabToSpawn;
    
    [Header("Settings")]
    public float fontSize = 1f;
    public float letterSpacing = 1.2f;
    public float lineHeight = 1.5f;
    public float spawnDelay = 0.05f;
    public bool spawnOnStart = true;
    
    [Header("Text")]
    [TextArea(3, 10)]
    public string textToSpawn = "HELLO WORLD!";
    
    [Header("3D Settings")]
    public Vector3 textDirection = Vector3.right;
    public Vector3 lineDirection = Vector3.down;
    public bool faceCamera = false;
    
    [Header("Positioning")]
    public Transform targetObject;
    public Vector3 offsetFromTarget = Vector3.up;
    public bool followTarget = true;
    public bool useTargetAsOrigin = true;
    
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private GameObject textContainer;
    private string lastTextToSpawn = "";
    private float lastFontSize = 1f;
    private float lastLetterSpacing = 1.2f;
    private float lastLineHeight = 1.5f;
    
    void Start()
    {
        if (spawnOnStart)
            SpawnText();
    }
    
    void Update()
    {
        // Auto-update in editor when values change
        if (HasValuesChanged())
        {
            UpdateText();
        }
        
        // Follow target if enabled
        if (followTarget && targetObject != null && textContainer != null)
        {
            textContainer.transform.position = targetObject.position + offsetFromTarget;
            
            if (faceCamera && Camera.main != null)
            {
                Vector3 directionToCamera = Camera.main.transform.position - textContainer.transform.position;
                textContainer.transform.rotation = Quaternion.LookRotation(directionToCamera);
            }
        }
    }
    
    bool HasValuesChanged()
    {
        return textToSpawn != lastTextToSpawn || 
               fontSize != lastFontSize || 
               letterSpacing != lastLetterSpacing || 
               lineHeight != lastLineHeight;
    }
    
    void UpdateCachedValues()
    {
        lastTextToSpawn = textToSpawn;
        lastFontSize = fontSize;
        lastLetterSpacing = letterSpacing;
        lastLineHeight = lineHeight;
    }
    
    [ContextMenu("Spawn Text")]
    public void SpawnText()
    {
        if (Application.isPlaying && spawnDelay > 0)
        {
            StartCoroutine(SpawnTextCoroutine());
        }
        else
        {
            SpawnTextImmediate();
        }
    }
    
    public void UpdateText()
    {
        SpawnTextImmediate();
        UpdateCachedValues();
    }
    
    [ContextMenu("Clear Text")]
    public void ClearText()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null)
            {
                if (Application.isPlaying)
                    Destroy(obj);
                else
                    DestroyImmediate(obj);
            }
        }
        spawnedObjects.Clear();
        
        // Clean up container
        if (textContainer != null)
        {
            if (Application.isPlaying)
                Destroy(textContainer);
            else
                DestroyImmediate(textContainer);
            textContainer = null;
        }
    }
    
    void CreateTextContainer()
    {
        if (textContainer != null)
        {
            if (Application.isPlaying)
                Destroy(textContainer);
            else
                DestroyImmediate(textContainer);
        }
        
        textContainer = new GameObject($"TextContainer_{textToSpawn.Substring(0, Mathf.Min(10, textToSpawn.Length))}");
        
        // Position the container
        if (useTargetAsOrigin && targetObject != null)
        {
            textContainer.transform.position = targetObject.position + offsetFromTarget;
        }
        else
        {
            textContainer.transform.position = transform.position;
        }
        
        // Parent to this TextSpawner for organization
        textContainer.transform.SetParent(transform);
    }
    
    void SpawnTextImmediate()
    {
        ClearText();
        
        if (letterData == null || prefabToSpawn == null)
        {
            Debug.LogError("LetterData or PrefabToSpawn is not assigned!");
            return;
        }
        
        CreateTextContainer();
        
        Vector3 currentPosition = Vector3.zero; // Start at container origin
        Vector3 lineStartPosition = currentPosition;
        
        // Normalize directions
        Vector3 normalizedTextDirection = textDirection.normalized;
        Vector3 normalizedLineDirection = lineDirection.normalized;
        
        foreach (char c in textToSpawn)
        {
            if (c == '\n')
            {
                // New line
                currentPosition = lineStartPosition + normalizedLineDirection * (lineHeight * fontSize);
                lineStartPosition = currentPosition;
                continue;
            }
            
            if (c == ' ')
            {
                // Space
                currentPosition += normalizedTextDirection * (letterSpacing * fontSize);
                continue;
            }
            
            var letterPoints = letterData.GetLetterPoints(c);
            if (letterPoints != null)
            {
                // Spawn points for this letter
                foreach (var point in letterPoints.points)
                {
                    // Calculate local position relative to container
                    Vector3 localOffset = normalizedTextDirection * (point.x * fontSize) + 
                                        Vector3.up * (point.y * fontSize);
                    Vector3 localPos = currentPosition + localOffset;
                    
                    GameObject spawned = Instantiate(prefabToSpawn, textContainer.transform);
                    spawned.transform.localPosition = localPos;
                    spawned.transform.localRotation = Quaternion.identity;
                    spawnedObjects.Add(spawned);
                }
                
                // Move to next letter position
                currentPosition += normalizedTextDirection * (letterPoints.width * fontSize * letterSpacing);
            }
            else
            {
                Debug.LogWarning($"No data found for character: {c}");
                // Still advance position for unknown characters
                currentPosition += normalizedTextDirection * (letterSpacing * fontSize);
            }
        }
    }
    
    Quaternion GetSpawnRotation()
    {
        if (faceCamera && Camera.main != null)
        {
            Vector3 directionToCamera = Camera.main.transform.position - transform.position;
            return Quaternion.LookRotation(directionToCamera);
        }
        return Quaternion.identity;
    }
    
    IEnumerator SpawnTextCoroutine()
    {
        ClearText();
        
        if (letterData == null || prefabToSpawn == null)
        {
            Debug.LogError("LetterData or PrefabToSpawn is not assigned!");
            yield break;
        }
        
        CreateTextContainer();
        
        Vector3 currentPosition = Vector3.zero; // Start at container origin
        Vector3 lineStartPosition = currentPosition;
        
        // Normalize directions
        Vector3 normalizedTextDirection = textDirection.normalized;
        Vector3 normalizedLineDirection = lineDirection.normalized;
        
        foreach (char c in textToSpawn)
        {
            if (c == '\n')
            {
                // New line
                currentPosition = lineStartPosition + normalizedLineDirection * (lineHeight * fontSize);
                lineStartPosition = currentPosition;
                continue;
            }
            
            if (c == ' ')
            {
                // Space
                currentPosition += normalizedTextDirection * (letterSpacing * fontSize);
                continue;
            }
            
            var letterPoints = letterData.GetLetterPoints(c);
            if (letterPoints != null)
            {
                // Spawn points for this letter
                foreach (var point in letterPoints.points)
                {
                    // Calculate local position relative to container
                    Vector3 localOffset = normalizedTextDirection * (point.x * fontSize) + 
                                        Vector3.up * (point.y * fontSize);
                    Vector3 localPos = currentPosition + localOffset;
                    
                    GameObject spawned = Instantiate(prefabToSpawn, textContainer.transform);
                    spawned.transform.localPosition = localPos;
                    spawned.transform.localRotation = Quaternion.identity;
                    spawnedObjects.Add(spawned);
                    
                    if (spawnDelay > 0)
                        yield return new WaitForSeconds(spawnDelay);
                }
                
                // Move to next letter position
                currentPosition += normalizedTextDirection * (letterPoints.width * fontSize * letterSpacing);
            }
            else
            {
                Debug.LogWarning($"No data found for character: {c}");
                // Still advance position for unknown characters
                currentPosition += normalizedTextDirection * (letterSpacing * fontSize);
            }
        }
    }
    
    void OnValidate()
    {
        // Update spacing values from letterData if available
        if (letterData != null)
        {
            letterSpacing = letterData.defaultSpacing;
            lineHeight = letterData.defaultLineHeight;
        }
    }
    
    // Public methods for controlling text position
    public void SetTextPosition(Vector3 position)
    {
        if (textContainer != null)
        {
            textContainer.transform.position = position;
        }
    }
    
    public void MoveTextBy(Vector3 offset)
    {
        if (textContainer != null)
        {
            textContainer.transform.position += offset;
        }
    }
    
    public void SetTextRotation(Quaternion rotation)
    {
        if (textContainer != null)
        {
            textContainer.transform.rotation = rotation;
        }
    }
    
    public void AttachToTarget(Transform target, Vector3 offset = default)
    {
        targetObject = target;
        offsetFromTarget = offset;
        followTarget = true;
        
        if (textContainer != null && target != null)
        {
            textContainer.transform.position = target.position + offset;
        }
    }
    
    public void DetachFromTarget()
    {
        followTarget = false;
        targetObject = null;
    }
    
    public GameObject GetTextContainer()
    {
        return textContainer;
    }
}
