using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawning : MonoBehaviour
{
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private float letterWidth = 3f;
    [SerializeField] private float letterHeight = 4f;
    [SerializeField] private float flowerDensity = 0.3f;
    [SerializeField] private float letterSpacing = 1f;
    [SerializeField] private float lineSpacing = 6f;
    [SerializeField] private int flowersPerPoint = 4;
    [SerializeField] private float flowerScatter = 0f;
    [SerializeField] private string textToSpell = "HELLO\nWORLD";

    void Start()
    {
        SpellWithFlowers(textToSpell);
    }

    public void SpellWithFlowers(string text) 
    {
        string[] lines = text.Split('\n');
        
        for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            string line = lines[lineIndex];
            float currentX = 0f;
            float currentZ = -lineIndex * lineSpacing;
            
            foreach (char c in line) 
            {
                if (c == ' ') 
                {
                    currentX += letterWidth * 0.6f;
                    continue;
                }
                
                SpawnFlowersForLetter(c, new Vector3(currentX, 0, currentZ));
                currentX += letterWidth + letterSpacing;
            }
        }
    }
    
    void SpawnFlowersForLetter(char letter, Vector3 basePosition) 
    {
        List<Vector2> letterPoints = GetLetterPoints(letter);
        
        foreach (Vector2 point in letterPoints) 
        {
            Vector3 worldPos = basePosition + new Vector3(point.x, 0, point.y);
            
            if (grassPrefab != null)
            {
                Quaternion grassRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                Instantiate(grassPrefab, worldPos, grassRotation);
            }
            
            for (int i = 0; i < flowersPerPoint; i++) 
            {
                Vector3 scatteredPos = worldPos;
                
                if (i > 0) 
                {
                    scatteredPos += new Vector3(
                        Random.Range(-flowerScatter, flowerScatter), 
                        0, 
                        Random.Range(-flowerScatter, flowerScatter)
                    );
                }
                
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                Instantiate(flowerPrefab, scatteredPos, rotation);
            }
        }
    }

    List<Vector2> GetLetterPoints(char letter) 
    {
        List<Vector2> points = new List<Vector2>();
        
        switch (letter.ToString().ToUpper()) 
        {
            case "A":
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth * 0.1f + letterWidth * 0.4f * t, letterHeight * t));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth * 0.9f - letterWidth * 0.4f * t, letterHeight * t));
                for (float x = letterWidth * 0.3f; x <= letterWidth * 0.7f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight * 0.4f));
                break;

            case "B":
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float x = 0; x <= letterWidth * 0.6f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight * 0.5f));
                for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity) 
                    points.Add(new Vector2(x, 0));
                for (float angle = -90; angle <= 90; angle += 180 * flowerDensity / (Mathf.PI * letterWidth * 0.2f)) 
                {
                    float x = letterWidth * 0.6f + letterWidth * 0.2f * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float y = letterHeight * 0.75f + letterHeight * 0.25f * Mathf.Sin(angle * Mathf.Deg2Rad);
                    points.Add(new Vector2(x, y));
                }
                for (float angle = -90; angle <= 90; angle += 180 * flowerDensity / (Mathf.PI * letterWidth * 0.2f)) 
                {
                    float x = letterWidth * 0.7f + letterWidth * 0.2f * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float y = letterHeight * 0.25f + letterHeight * 0.25f * Mathf.Sin(angle * Mathf.Deg2Rad);
                    points.Add(new Vector2(x, y));
                }
                break;

            case "C":
                for (float y = 0.2f; y <= letterHeight - 0.2f; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float x = 0; x <= letterWidth * 0.9f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float x = 0; x <= letterWidth * 0.9f; x += flowerDensity) 
                    points.Add(new Vector2(x, 0));
                break;

            case "D":
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float x = 0; x <= letterWidth * 0.6f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float x = 0; x <= letterWidth * 0.6f; x += flowerDensity) 
                    points.Add(new Vector2(x, 0));
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth * 0.6f + letterWidth * 0.3f * Mathf.Sin(y / letterHeight * Mathf.PI), y));
                break;

            case "E":
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float x = 0; x <= letterWidth; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float x = 0; x <= letterWidth * 0.8f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight * 0.5f));
                for (float x = 0; x <= letterWidth; x += flowerDensity) 
                    points.Add(new Vector2(x, 0));
                break;

            case "F":
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float x = 0; x <= letterWidth; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float x = 0; x <= letterWidth * 0.8f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight * 0.5f));
                break;

            case "G":
                for (float y = letterHeight * 0.15f; y <= letterHeight * 0.85f; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float x = 0; x <= letterWidth * 0.8f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float x = 0; x <= letterWidth * 0.8f; x += flowerDensity) 
                    points.Add(new Vector2(x, 0));
                for (float y = 0; y <= letterHeight * 0.5f; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth * 0.8f, y));
                for (float x = letterWidth * 0.5f; x <= letterWidth * 0.8f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight * 0.5f));
                break;

            case "H":
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth, y));
                for (float x = 0; x <= letterWidth; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight * 0.5f));
                break;

            case "I":
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth * 0.5f, y));
                for (float x = letterWidth * 0.2f; x <= letterWidth * 0.8f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float x = letterWidth * 0.2f; x <= letterWidth * 0.8f; x += flowerDensity) 
                    points.Add(new Vector2(x, 0));
                break;

            case "J":
                for (float y = letterHeight * 0.3f; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth * 0.7f, y));
                for (float x = letterWidth * 0.3f; x <= letterWidth; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight * 0.3f));
                for (float y = 0; y <= letterHeight * 0.3f; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                break;

            case "K":
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth * 0.5f * t, letterHeight * 0.5f + letterHeight * 0.5f * t));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth * 0.5f * t, letterHeight * 0.5f - letterHeight * 0.5f * t));
                break;

            case "L":
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float x = 0; x <= letterWidth; x += flowerDensity) 
                    points.Add(new Vector2(x, 0));
                break;

            case "M":
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth, y));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth * 0.5f * t, letterHeight - letterHeight * 0.6f * t));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth - letterWidth * 0.5f * t, letterHeight - letterHeight * 0.6f * t));
                break;

            case "N":
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth, y));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth * t, letterHeight - letterHeight * t));
                break;

            case "O":
                for (float angle = 0; angle < 360; angle += 360 * flowerDensity / (2 * Mathf.PI * letterWidth * 0.48f)) 
                {
                    float x = letterWidth * 0.5f + letterWidth * 0.48f * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float y = letterHeight * 0.5f + letterHeight * 0.48f * Mathf.Sin(angle * Mathf.Deg2Rad);
                    points.Add(new Vector2(x, y));
                }
                break;

            case "P":
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight * 0.5f));
                for (float y = letterHeight * 0.5f; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth * 0.7f, y));
                break;

            case "Q":
                for (float angle = 0; angle < 360; angle += 360 * flowerDensity / (2 * Mathf.PI * letterWidth * 0.4f)) 
                {
                    float x = letterWidth * 0.5f + letterWidth * 0.4f * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float y = letterHeight * 0.5f + letterHeight * 0.4f * Mathf.Sin(angle * Mathf.Deg2Rad);
                    points.Add(new Vector2(x, y));
                }
                for (float t = 0; t <= 1; t += flowerDensity / letterWidth) 
                    points.Add(new Vector2(letterWidth * 0.6f + letterWidth * 0.3f * t, letterHeight * 0.3f - letterHeight * 0.3f * t));
                break;

            case "R":
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight * 0.5f));
                for (float y = letterHeight * 0.5f; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth * 0.7f, y));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth * 0.4f + letterWidth * 0.4f * t, letterHeight * 0.5f - letterHeight * 0.5f * t));
                break;

            case "S":
                for (float x = letterWidth * 0.2f; x <= letterWidth * 0.8f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float y = letterHeight * 0.5f; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth * 0.2f, y));
                for (float x = letterWidth * 0.2f; x <= letterWidth * 0.8f; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight * 0.5f));
                for (float y = 0; y <= letterHeight * 0.5f; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth * 0.8f, y));
                for (float x = letterWidth * 0.2f; x <= letterWidth * 0.8f; x += flowerDensity) 
                    points.Add(new Vector2(x, 0));
                break;

            case "T":
                for (float x = 0; x <= letterWidth; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth * 0.5f, y));
                break;

            case "U":
                for (float y = letterHeight * 0.3f; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth * 0.1f, y));
                for (float y = letterHeight * 0.3f; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth * 0.9f, y));
                for (float angle = 0; angle <= 180; angle += 180 * flowerDensity / (Mathf.PI * letterWidth * 0.4f)) 
                {
                    float x = letterWidth * 0.5f + letterWidth * 0.4f * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float y = letterHeight * 0.15f + letterHeight * 0.15f * Mathf.Sin(angle * Mathf.Deg2Rad);
                    points.Add(new Vector2(x, y));
                }
                break;

            case "V":
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth * 0.5f * t, letterHeight - letterHeight * t));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth - letterWidth * 0.5f * t, letterHeight - letterHeight * t));
                break;

            case "W":
                for (float y = letterHeight * 0.3f; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(0, y));
                for (float y = letterHeight * 0.3f; y <= letterHeight; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth, y));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth * 0.25f * (1 + t), letterHeight * 0.3f + letterHeight * 0.4f * t));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth * 0.75f * (1 + (1-t)/3), letterHeight * 0.3f + letterHeight * 0.4f * t));
                points.Add(new Vector2(letterWidth * 0.25f, 0));
                points.Add(new Vector2(letterWidth * 0.75f, 0));
                break;

            case "X":
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth * t, letterHeight - letterHeight * t));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth - letterWidth * t, letterHeight - letterHeight * t));
                break;

            case "Y":
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth * 0.5f * t, letterHeight - letterHeight * 0.5f * t));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth - letterWidth * 0.5f * t, letterHeight - letterHeight * 0.5f * t));
                for (float y = 0; y <= letterHeight * 0.5f; y += flowerDensity) 
                    points.Add(new Vector2(letterWidth * 0.5f, y));
                break;

            case "Z":
                for (float x = 0; x <= letterWidth; x += flowerDensity) 
                    points.Add(new Vector2(x, letterHeight));
                for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                    points.Add(new Vector2(letterWidth - letterWidth * t, letterHeight - letterHeight * t));
                for (float x = 0; x <= letterWidth; x += flowerDensity) 
                    points.Add(new Vector2(x, 0));
                break;

            case "!":
                for (float y = letterHeight * 0.4f; y <= letterHeight; y += flowerDensity) 
                {
                    points.Add(new Vector2(letterWidth * 0.45f, y));
                    points.Add(new Vector2(letterWidth * 0.5f, y));
                    points.Add(new Vector2(letterWidth * 0.55f, y));
                }
                for (float x = letterWidth * 0.45f; x <= letterWidth * 0.55f; x += flowerDensity) 
                {
                    points.Add(new Vector2(x, letterHeight));
                    points.Add(new Vector2(x, letterHeight * 0.4f));
                }
                for (float x = letterWidth * 0.45f; x <= letterWidth * 0.55f; x += flowerDensity) 
                {
                    points.Add(new Vector2(x, letterHeight * 0.2f));
                    points.Add(new Vector2(x, letterHeight * 0.1f));
                }
                for (float y = letterHeight * 0.1f; y <= letterHeight * 0.2f; y += flowerDensity) 
                {
                    points.Add(new Vector2(letterWidth * 0.45f, y));
                    points.Add(new Vector2(letterWidth * 0.55f, y));
                }
                break;

            default:
                for (float x = 0; x <= letterWidth; x += flowerDensity) 
                {
                    points.Add(new Vector2(x, 0));
                    points.Add(new Vector2(x, letterHeight));
                }
                for (float y = 0; y <= letterHeight; y += flowerDensity) 
                {
                    points.Add(new Vector2(0, y));
                    points.Add(new Vector2(letterWidth, y));
                }
                break;
        }
        
        return points;
    }
}
