using System.Collections.Generic;
using UnityEngine;

public class FlowerTestCube : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private GameObject boundingCube; // Cube that defines spawn boundaries
    
    [Header("Text Settings")]
    [SerializeField] private string textToSpell = "HAPPY MOTHERS DAY";
    [SerializeField] private float letterSpacing = 1.2f;
    
    [Header("Flower Settings")]
    [SerializeField] private float flowerDensity = 0.3f;
    [SerializeField] private int flowersPerPoint = 1;
    [SerializeField] private float flowerScatter = 0.1f;
    [SerializeField] private Vector2 scaleRange = new Vector2(0.8f, 1.2f);
    
    private Vector3 cubeSize;
    private Vector3 cubeCenter;
    
    void Start()
    {
        if (boundingCube != null)
        {
            // Get cube bounds
            Renderer cubeRenderer = boundingCube.GetComponent<Renderer>();
            if (cubeRenderer != null)
            {
                cubeSize = cubeRenderer.bounds.size;
                cubeCenter = cubeRenderer.bounds.center;
            }
            else
            {
                cubeSize = boundingCube.transform.localScale;
                cubeCenter = boundingCube.transform.position;
            }
        }
        else
        {
            cubeSize = Vector3.one * 10f; // Default size
            cubeCenter = transform.position;
        }
        
        GenerateFlowerText();
    }
    
    [ContextMenu("Generate Flower Text")]
    public void GenerateFlowerText()
    {
        // Clear existing flowers
        foreach (Transform child in transform)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
        
        // Split text into words for better wrapping
        string[] words = textToSpell.Split(' ');
        
        // Calculate base scale for letters
        float letterHeight = 4f;
        float maxScale = Mathf.Min(cubeSize.x / (letterSpacing * 10), cubeSize.z / letterHeight) * 0.8f;
        
        float currentX = -cubeSize.x * 0.4f; // Start from left edge with padding
        float currentZ = cubeSize.z * 0.4f; // Start from top with padding
        float rowHeight = letterHeight * maxScale + 1f; // Space between rows
        
        foreach (string word in words)
        {
            // Calculate word width
            float wordWidth = (word.Length * letterSpacing + 1f) * maxScale; // +1 for space after word
            
            // Check if word fits on current row
            if (currentX + wordWidth > cubeSize.x * 0.4f && currentX > -cubeSize.x * 0.4f)
            {
                // Move to next row
                currentX = -cubeSize.x * 0.4f;
                currentZ -= rowHeight;
                
                // Check if we've run out of vertical space
                if (currentZ - letterHeight * maxScale < -cubeSize.z * 0.4f)
                {
                    Debug.LogWarning("Text is too long to fit in the cube boundaries!");
                    break;
                }
            }
            
            // Place each letter of the word
            for (int i = 0; i < word.Length; i++)
            {
                char c = word[i];
                Vector3 letterPos = cubeCenter + new Vector3(currentX, 0, currentZ);
                
                GenerateFlowersForLetter(c, letterPos, maxScale);
                currentX += letterSpacing * maxScale;
            }
            
            // Add space between words
            currentX += letterSpacing * maxScale * 0.5f;
        }
    }
    
    void GenerateFlowersForLetter(char letter, Vector3 basePosition, float scale)
    {
        List<Vector2> points = GetLetterPoints(letter);
        
        foreach (Vector2 point in points)
        {
            Vector3 worldPos = basePosition + new Vector3(point.x * scale, 0, point.y * scale);
            
            // Check if position is within cube bounds
            if (IsWithinCubeBounds(worldPos))
            {
                for (int i = 0; i < flowersPerPoint; i++)
                {
                    Vector3 flowerPos = worldPos;
                    if (i > 0)
                    {
                        flowerPos += new Vector3(
                            Random.Range(-flowerScatter, flowerScatter) * scale,
                            0,
                            Random.Range(-flowerScatter, flowerScatter) * scale
                        );
                    }
                    
                    // Double-check bounds after scatter
                    if (IsWithinCubeBounds(flowerPos))
                    {
                        SpawnFlower(flowerPos, scale);
                    }
                }
            }
        }
    }
    
    bool IsWithinCubeBounds(Vector3 position)
    {
        Vector3 localPos = position - cubeCenter;
        return Mathf.Abs(localPos.x) <= cubeSize.x * 0.5f &&
               Mathf.Abs(localPos.y) <= cubeSize.y * 0.5f &&
               Mathf.Abs(localPos.z) <= cubeSize.z * 0.5f;
    }
    
    void SpawnFlower(Vector3 position, float scale)
    {
        GameObject flower = Instantiate(flowerPrefab, position, Quaternion.Euler(0, Random.Range(0, 360), 0), transform);
        
        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        flower.transform.localScale = flowerPrefab.transform.localScale * scale * randomScale;
    }
    
    List<Vector2> GetLetterPoints(char letter)
    {
        List<Vector2> points = new List<Vector2>();
        
        switch (letter.ToString().ToUpper())
        {
            case "A":
                AddLine(points, Vector2.zero, new Vector2(0.5f, 4f));
                AddLine(points, new Vector2(1f, 0), new Vector2(0.5f, 4f));
                AddLine(points, new Vector2(0.25f, 1.6f), new Vector2(0.75f, 1.6f));
                break;
                
            case "B":
                AddLine(points, Vector2.zero, new Vector2(0, 4f));
                AddLine(points, new Vector2(0, 4f), new Vector2(0.7f, 4f));
                AddLine(points, new Vector2(0, 2f), new Vector2(0.6f, 2f));
                AddLine(points, Vector2.zero, new Vector2(0.7f, 0));
                AddLine(points, new Vector2(0.7f, 4f), new Vector2(0.7f, 2f));
                AddLine(points, new Vector2(0.6f, 2f), new Vector2(0.7f, 0));
                break;
                
            case "C":
                AddLine(points, new Vector2(0, 0.8f), new Vector2(0, 3.2f));
                AddLine(points, new Vector2(0, 4f), new Vector2(0.8f, 4f));
                AddLine(points, Vector2.zero, new Vector2(0.8f, 0));
                break;
                
            case "D":
                AddLine(points, Vector2.zero, new Vector2(0, 4f));
                AddLine(points, new Vector2(0, 4f), new Vector2(0.6f, 4f));
                AddLine(points, Vector2.zero, new Vector2(0.6f, 0));
                AddLine(points, new Vector2(0.6f, 4f), new Vector2(1f, 2.8f));
                AddLine(points, new Vector2(1f, 2.8f), new Vector2(1f, 1.2f));
                AddLine(points, new Vector2(1f, 1.2f), new Vector2(0.6f, 0));
                break;
                
            case "E":
                AddLine(points, Vector2.zero, new Vector2(0, 4f));
                AddLine(points, new Vector2(0, 4f), new Vector2(1f, 4f));
                AddLine(points, new Vector2(0, 2f), new Vector2(0.8f, 2f));
                AddLine(points, Vector2.zero, new Vector2(1f, 0));
                break;
                
            case "F":
                AddLine(points, Vector2.zero, new Vector2(0, 4f));
                AddLine(points, new Vector2(0, 4f), new Vector2(1f, 4f));
                AddLine(points, new Vector2(0, 2f), new Vector2(0.8f, 2f));
                break;
                
            case "G":
                AddLine(points, new Vector2(0, 0.8f), new Vector2(0, 3.2f));
                AddLine(points, new Vector2(0, 4f), new Vector2(0.8f, 4f));
                AddLine(points, Vector2.zero, new Vector2(0.8f, 0));
                AddLine(points, new Vector2(0.8f, 0), new Vector2(0.8f, 2f));
                AddLine(points, new Vector2(0.5f, 2f), new Vector2(0.8f, 2f));
                break;
                
            case "H":
                AddLine(points, Vector2.zero, new Vector2(0, 4f));
                AddLine(points, new Vector2(1f, 0), new Vector2(1f, 4f));
                AddLine(points, new Vector2(0, 2f), new Vector2(1f, 2f));
                break;
                
            case "I":
                AddLine(points, new Vector2(0.5f, 0), new Vector2(0.5f, 4f));
                AddLine(points, new Vector2(0.2f, 0), new Vector2(0.8f, 0));
                AddLine(points, new Vector2(0.2f, 4f), new Vector2(0.8f, 4f));
                break;
                
            case "L":
                AddLine(points, Vector2.zero, new Vector2(0, 4f));
                AddLine(points, Vector2.zero, new Vector2(1f, 0));
                break;
                
            case "M":
                AddLine(points, Vector2.zero, new Vector2(0, 4f));
                AddLine(points, new Vector2(1f, 0), new Vector2(1f, 4f));
                AddLine(points, new Vector2(0, 4f), new Vector2(0.5f, 2f));
                AddLine(points, new Vector2(1f, 4f), new Vector2(0.5f, 2f));
                break;
                
            case "N":
                AddLine(points, Vector2.zero, new Vector2(0, 4f));
                AddLine(points, new Vector2(1f, 0), new Vector2(1f, 4f));
                AddLine(points, new Vector2(0, 4f), new Vector2(1f, 0));
                break;
                
            case "O":
                AddLine(points, Vector2.zero, new Vector2(0, 4f));
                AddLine(points, new Vector2(1f, 0), new Vector2(1f, 4f));
                AddLine(points, new Vector2(0, 4f), new Vector2(1f, 4f));
                AddLine(points, Vector2.zero, new Vector2(1f, 0));
                break;
                
            case "P":
                AddLine(points, Vector2.zero, new Vector2(0, 4f));
                AddLine(points, new Vector2(0, 4f), new Vector2(0.7f, 4f));
                AddLine(points, new Vector2(0, 2f), new Vector2(0.7f, 2f));
                AddLine(points, new Vector2(0.7f, 4f), new Vector2(0.7f, 2f));
                break;
                
            case "R":
                AddLine(points, Vector2.zero, new Vector2(0, 4f));
                AddLine(points, new Vector2(0, 4f), new Vector2(0.7f, 4f));
                AddLine(points, new Vector2(0, 2f), new Vector2(0.7f, 2f));
                AddLine(points, new Vector2(0.7f, 4f), new Vector2(0.7f, 2f));
                AddLine(points, new Vector2(0.4f, 2f), new Vector2(1f, 0));
                break;
                
            case "S":
                AddLine(points, new Vector2(0.2f, 4f), new Vector2(0.8f, 4f));
                AddLine(points, new Vector2(0.2f, 4f), new Vector2(0.2f, 2f));
                AddLine(points, new Vector2(0.2f, 2f), new Vector2(0.8f, 2f));
                AddLine(points, new Vector2(0.8f, 2f), new Vector2(0.8f, 0));
                AddLine(points, new Vector2(0.2f, 0), new Vector2(0.8f, 0));
                break;
                
            case "T":
                AddLine(points, new Vector2(0, 4f), new Vector2(1f, 4f));
                AddLine(points, new Vector2(0.5f, 0), new Vector2(0.5f, 4f));
                break;
                
            case "U":
                AddLine(points, new Vector2(0, 1.2f), new Vector2(0, 4f));
                AddLine(points, new Vector2(1f, 1.2f), new Vector2(1f, 4f));
                AddLine(points, Vector2.zero, new Vector2(0.2f, 1.2f));
                AddLine(points, new Vector2(0.2f, 1.2f), new Vector2(0.8f, 1.2f));
                AddLine(points, new Vector2(0.8f, 1.2f), new Vector2(1f, 0));
                break;
                
            case "V":
                AddLine(points, new Vector2(0, 4f), new Vector2(0.5f, 0));
                AddLine(points, new Vector2(1f, 4f), new Vector2(0.5f, 0));
                break;
                
            case "W":
                AddLine(points, new Vector2(0, 4f), new Vector2(0.25f, 0));
                AddLine(points, new Vector2(0.25f, 0), new Vector2(0.5f, 2.4f));
                AddLine(points, new Vector2(0.5f, 2.4f), new Vector2(0.75f, 0));
                AddLine(points, new Vector2(0.75f, 0), new Vector2(1f, 4f));
                break;
                
            case "X":
                AddLine(points, Vector2.zero, new Vector2(1f, 4f));
                AddLine(points, new Vector2(0, 4f), new Vector2(1f, 0));
                break;
                
            case "Y":
                AddLine(points, new Vector2(0, 4f), new Vector2(0.5f, 2f));
                AddLine(points, new Vector2(1f, 4f), new Vector2(0.5f, 2f));
                AddLine(points, new Vector2(0.5f, 2f), new Vector2(0.5f, 0));
                break;
                
            case "!":
                AddLine(points, new Vector2(0.5f, 1.2f), new Vector2(0.5f, 4f));
                points.Add(new Vector2(0.5f, 0.4f));
                break;
        }
        
        return points;
    }
    
    void AddLine(List<Vector2> points, Vector2 start, Vector2 end)
    {
        float distance = Vector2.Distance(start, end);
        int stepCount = Mathf.Max(1, Mathf.RoundToInt(distance / flowerDensity));
        
        for (int i = 0; i <= stepCount; i++)
        {
            float t = (float)i / stepCount;
            Vector2 point = Vector2.Lerp(start, end, t);
            points.Add(point);
        }
    }
}
