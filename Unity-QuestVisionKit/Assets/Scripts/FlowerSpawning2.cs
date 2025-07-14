using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawning2 : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject flowerPrefab;
    
    [Header("Spawn Settings")]
    [SerializeField] private Transform originPoint; // Reference to spawn point
    
    [Header("Text Settings")]
    [SerializeField] private string textToSpell = "HAPPY MOTHERS DAY!!";
    [SerializeField] private float letterWidth = 3f;
    [SerializeField] private float letterHeight = 4f;
    [SerializeField] private float letterSpacing = 1f;
    
    [Header("Flower Settings")]
    [SerializeField] private float flowerDensity = 0.5f;
    [SerializeField] private int flowersPerPoint = 2;
    [SerializeField] private float flowerScatter = 0.1f;
    
    [Header("Scale Settings")]
    [SerializeField] private float globalScale = 0.1f; // Scale everything down more for table size
    [SerializeField] private bool useRandomScale = true;
    [SerializeField] private Vector2 scaleRange = new Vector2(0.8f, 1.2f);
    
    [Header("Density Settings")]
    [SerializeField] private float densityMultiplier = 2f; // Increase flower density independently
    
    [Header("Wave Animation Settings")]
    [SerializeField] private float waveSpeed = 0.1f; // Time between each flower appearing
    [SerializeField] private float growDuration = 0.5f; // How long each flower takes to grow
    [SerializeField] private bool playWaveOnStart = true;
    
    // Store flowers grouped by letter for wave animation
    private List<LetterData> allLetters = new List<LetterData>();
    
    [System.Serializable]
    private class LetterData
    {
        public char letter;
        public float xPosition;
        public List<FlowerData> flowers;
        
        public LetterData(char letter, float xPosition)
        {
            this.letter = letter;
            this.xPosition = xPosition;
            this.flowers = new List<FlowerData>();
        }
    }
    
    [System.Serializable]
    private class FlowerData
    {
        public GameObject flower;
        public Vector3 targetScale;
        
        public FlowerData(GameObject flower, Vector3 targetScale)
        {
            this.flower = flower;
            this.targetScale = targetScale;
        }
    }
    
    void Start()
    {
        GenerateFlowerText();
        
        if (playWaveOnStart)
        {
            StartCoroutine(PlayWaveAnimationCoroutine());
        }
    }
    
    [ContextMenu("Generate Flower Text")]
    public void GenerateFlowerText()
    {
        // Clear existing flowers
        allLetters.Clear();
        
        // Use origin point if assigned, otherwise use this transform
        Vector3 startPosition = originPoint != null ? originPoint.position : transform.position;
        
        float currentX = 0f;
        
        foreach (char c in textToSpell)
        {
            if (c == ' ')
            {
                currentX += letterWidth * globalScale * 0.6f; // Apply global scale to spacing
                continue;
            }
            
            // Create a new letter data entry
            LetterData letterData = new LetterData(c, currentX * globalScale);
            GenerateFlowersForLetter(c, startPosition + new Vector3(currentX * globalScale, 0, 0), letterData);
            allLetters.Add(letterData);
            
            currentX += letterWidth + letterSpacing;
        }
    }
    
    void GenerateFlowersForLetter(char letter, Vector3 basePosition, LetterData letterData)
    {
        List<Vector2> points = GetSimpleLetterPoints(letter);
        
        foreach (Vector2 point in points)
        {
            Vector3 worldPos = basePosition + new Vector3(point.x * globalScale, 0, point.y * globalScale);
            
            // Use density multiplier to add more flowers per point
            int totalFlowers = Mathf.RoundToInt(flowersPerPoint * densityMultiplier);
            
            for (int i = 0; i < totalFlowers; i++)
            {
                Vector3 flowerPos = worldPos;
                if (i > 0)
                {
                    // Scale scatter based on global scale but allow more spread for density
                    float scatterAmount = flowerScatter * globalScale * densityMultiplier * 0.5f;
                    flowerPos += new Vector3(
                        Random.Range(-scatterAmount, scatterAmount),
                        0,
                        Random.Range(-scatterAmount, scatterAmount)
                    );
                }
                
                GameObject flower = Instantiate(flowerPrefab, flowerPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
                
                // Calculate target scale
                Vector3 targetScale;
                if (useRandomScale)
                {
                    float randomScale = Random.Range(scaleRange.x, scaleRange.y);
                    targetScale = flowerPrefab.transform.localScale * globalScale * randomScale;
                }
                else
                {
                    targetScale = flowerPrefab.transform.localScale * globalScale;
                }
                
                // Start flowers at scale 0 for animation
                flower.transform.localScale = Vector3.zero;
                
                // Add flower to this letter's collection
                letterData.flowers.Add(new FlowerData(flower, targetScale));
            }
        }
    }
    
    [ContextMenu("Play Wave Animation")]
    public void PlayWaveAnimation()
    {
        StartCoroutine(PlayWaveAnimationCoroutine());
    }
    
    private IEnumerator PlayWaveAnimationCoroutine()
    {
        // Sort letters by X position (left to right)
        allLetters.Sort((a, b) => a.xPosition.CompareTo(b.xPosition));
        
        // Animate each letter in sequence
        foreach (LetterData letterData in allLetters)
        {
            // Start growing all flowers in this letter simultaneously
            foreach (FlowerData flowerData in letterData.flowers)
            {
                if (flowerData.flower != null)
                {
                    StartCoroutine(GrowFlower(flowerData.flower, flowerData.targetScale));
                }
            }
            
            // Wait before starting the next letter
            yield return new WaitForSeconds(waveSpeed);
        }
    }
    
    private IEnumerator GrowFlower(GameObject flower, Vector3 targetScale)
    {
        Vector3 startScale = Vector3.zero;
        float elapsedTime = 0f;
        
        while (elapsedTime < growDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / growDuration;
            
            // Use ease-out curve for natural growth
            t = 1f - Mathf.Pow(1f - t, 3f);
            
            flower.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        
        // Ensure final scale is exact
        flower.transform.localScale = targetScale;
    }
    
    // Rest of your existing code remains the same...
    
    List<Vector2> GetSimpleLetterPoints(char letter)
    {
        List<Vector2> points = new List<Vector2>();
        
        switch (letter.ToString().ToUpper())
        {
            case "A":
                AddLine(points, Vector2.zero, new Vector2(letterWidth * 0.5f, letterHeight));
                AddLine(points, new Vector2(letterWidth, 0), new Vector2(letterWidth * 0.5f, letterHeight));
                AddLine(points, new Vector2(letterWidth * 0.25f, letterHeight * 0.4f), new Vector2(letterWidth * 0.75f, letterHeight * 0.4f));
                break;
                
            case "B":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth * 0.7f, letterHeight));
                AddLine(points, new Vector2(0, letterHeight * 0.5f), new Vector2(letterWidth * 0.6f, letterHeight * 0.5f));
                AddLine(points, Vector2.zero, new Vector2(letterWidth * 0.7f, 0));
                AddLine(points, new Vector2(letterWidth * 0.7f, letterHeight), new Vector2(letterWidth * 0.7f, letterHeight * 0.5f));
                AddLine(points, new Vector2(letterWidth * 0.6f, letterHeight * 0.5f), new Vector2(letterWidth * 0.7f, 0));
                break;
                
            case "C":
                AddLine(points, new Vector2(0, letterHeight * 0.2f), new Vector2(0, letterHeight * 0.8f));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth * 0.8f, letterHeight));
                AddLine(points, Vector2.zero, new Vector2(letterWidth * 0.8f, 0));
                break;
                
            case "D":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth * 0.6f, letterHeight));
                AddLine(points, Vector2.zero, new Vector2(letterWidth * 0.6f, 0));
                AddLine(points, new Vector2(letterWidth * 0.6f, letterHeight), new Vector2(letterWidth, letterHeight * 0.7f));
                AddLine(points, new Vector2(letterWidth, letterHeight * 0.7f), new Vector2(letterWidth, letterHeight * 0.3f));
                AddLine(points, new Vector2(letterWidth, letterHeight * 0.3f), new Vector2(letterWidth * 0.6f, 0));
                break;
                
            case "E":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth, letterHeight));
                AddLine(points, new Vector2(0, letterHeight * 0.5f), new Vector2(letterWidth * 0.8f, letterHeight * 0.5f));
                AddLine(points, Vector2.zero, new Vector2(letterWidth, 0));
                break;
                
            case "F":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth, letterHeight));
                AddLine(points, new Vector2(0, letterHeight * 0.5f), new Vector2(letterWidth * 0.8f, letterHeight * 0.5f));
                break;
                
            case "G":
                AddLine(points, new Vector2(0, letterHeight * 0.2f), new Vector2(0, letterHeight * 0.8f));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth * 0.8f, letterHeight));
                AddLine(points, Vector2.zero, new Vector2(letterWidth * 0.8f, 0));
                AddLine(points, new Vector2(letterWidth * 0.8f, 0), new Vector2(letterWidth * 0.8f, letterHeight * 0.5f));
                AddLine(points, new Vector2(letterWidth * 0.5f, letterHeight * 0.5f), new Vector2(letterWidth * 0.8f, letterHeight * 0.5f));
                break;
                
            case "H":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, new Vector2(letterWidth, 0), new Vector2(letterWidth, letterHeight));
                AddLine(points, new Vector2(0, letterHeight * 0.5f), new Vector2(letterWidth, letterHeight * 0.5f));
                break;
                
            case "I":
                AddLine(points, new Vector2(letterWidth * 0.5f, 0), new Vector2(letterWidth * 0.5f, letterHeight));
                AddLine(points, new Vector2(letterWidth * 0.2f, 0), new Vector2(letterWidth * 0.8f, 0));
                AddLine(points, new Vector2(letterWidth * 0.2f, letterHeight), new Vector2(letterWidth * 0.8f, letterHeight));
                break;
                
            case "J":
                AddLine(points, new Vector2(letterWidth * 0.7f, letterHeight * 0.3f), new Vector2(letterWidth * 0.7f, letterHeight));
                AddLine(points, new Vector2(letterWidth * 0.3f, letterHeight), new Vector2(letterWidth, letterHeight));
                AddLine(points, Vector2.zero, new Vector2(letterWidth * 0.7f, letterHeight * 0.3f));
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight * 0.3f));
                break;
                
            case "K":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, new Vector2(0, letterHeight * 0.5f), new Vector2(letterWidth, letterHeight));
                AddLine(points, new Vector2(0, letterHeight * 0.5f), new Vector2(letterWidth, 0));
                break;
                
            case "L":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, Vector2.zero, new Vector2(letterWidth, 0));
                break;
                
            case "M":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, new Vector2(letterWidth, 0), new Vector2(letterWidth, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth * 0.5f, letterHeight * 0.5f));
                AddLine(points, new Vector2(letterWidth, letterHeight), new Vector2(letterWidth * 0.5f, letterHeight * 0.5f));
                break;
                
            case "N":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, new Vector2(letterWidth, 0), new Vector2(letterWidth, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth, 0));
                break;
                
            case "O":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, new Vector2(letterWidth, 0), new Vector2(letterWidth, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth, letterHeight));
                AddLine(points, Vector2.zero, new Vector2(letterWidth, 0));
                break;
                
            case "P":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth * 0.7f, letterHeight));
                AddLine(points, new Vector2(0, letterHeight * 0.5f), new Vector2(letterWidth * 0.7f, letterHeight * 0.5f));
                AddLine(points, new Vector2(letterWidth * 0.7f, letterHeight), new Vector2(letterWidth * 0.7f, letterHeight * 0.5f));
                break;
                
            case "Q":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, new Vector2(letterWidth, 0), new Vector2(letterWidth, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth, letterHeight));
                AddLine(points, Vector2.zero, new Vector2(letterWidth, 0));
                AddLine(points, new Vector2(letterWidth * 0.6f, letterHeight * 0.3f), new Vector2(letterWidth, 0));
                break;
                
            case "R":
                AddLine(points, Vector2.zero, new Vector2(0, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth * 0.7f, letterHeight));
                AddLine(points, new Vector2(0, letterHeight * 0.5f), new Vector2(letterWidth * 0.7f, letterHeight * 0.5f));
                AddLine(points, new Vector2(letterWidth * 0.7f, letterHeight), new Vector2(letterWidth * 0.7f, letterHeight * 0.5f));
                AddLine(points, new Vector2(letterWidth * 0.4f, letterHeight * 0.5f), new Vector2(letterWidth, 0));
                break;
                
            case "S":
                AddLine(points, new Vector2(letterWidth * 0.2f, letterHeight), new Vector2(letterWidth * 0.8f, letterHeight));
                AddLine(points, new Vector2(letterWidth * 0.2f, letterHeight), new Vector2(letterWidth * 0.2f, letterHeight * 0.5f));
                AddLine(points, new Vector2(letterWidth * 0.2f, letterHeight * 0.5f), new Vector2(letterWidth * 0.8f, letterHeight * 0.5f));
                AddLine(points, new Vector2(letterWidth * 0.8f, letterHeight * 0.5f), new Vector2(letterWidth * 0.8f, 0));
                AddLine(points, new Vector2(letterWidth * 0.2f, 0), new Vector2(letterWidth * 0.8f, 0));
                break;
                
            case "T":
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth, letterHeight));
                AddLine(points, new Vector2(letterWidth * 0.5f, 0), new Vector2(letterWidth * 0.5f, letterHeight));
                break;
                
            case "U":
                AddLine(points, new Vector2(0, letterHeight * 0.3f), new Vector2(0, letterHeight));
                AddLine(points, new Vector2(letterWidth, letterHeight * 0.3f), new Vector2(letterWidth, letterHeight));
                AddLine(points, Vector2.zero, new Vector2(letterWidth * 0.2f, letterHeight * 0.3f));
                AddLine(points, new Vector2(letterWidth * 0.2f, letterHeight * 0.3f), new Vector2(letterWidth * 0.8f, letterHeight * 0.3f));
                AddLine(points, new Vector2(letterWidth * 0.8f, letterHeight * 0.3f), new Vector2(letterWidth, 0));
                break;
                
            case "V":
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth * 0.5f, 0));
                AddLine(points, new Vector2(letterWidth, letterHeight), new Vector2(letterWidth * 0.5f, 0));
                break;
                
            case "W":
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth * 0.25f, 0));
                AddLine(points, new Vector2(letterWidth * 0.25f, 0), new Vector2(letterWidth * 0.5f, letterHeight * 0.6f));
                AddLine(points, new Vector2(letterWidth * 0.5f, letterHeight * 0.6f), new Vector2(letterWidth * 0.75f, 0));
                AddLine(points, new Vector2(letterWidth * 0.75f, 0), new Vector2(letterWidth, letterHeight));
                break;
                
            case "X":
                AddLine(points, Vector2.zero, new Vector2(letterWidth, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth, 0));
                break;
                
            case "Y":
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth * 0.5f, letterHeight * 0.5f));
                AddLine(points, new Vector2(letterWidth, letterHeight), new Vector2(letterWidth * 0.5f, letterHeight * 0.5f));
                AddLine(points, new Vector2(letterWidth * 0.5f, letterHeight * 0.5f), new Vector2(letterWidth * 0.5f, 0));
                break;
                
            case "Z":
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), new Vector2(letterWidth, 0));
                AddLine(points, Vector2.zero, new Vector2(letterWidth, 0));
                break;
                
            case "!":
                AddLine(points, new Vector2(letterWidth * 0.5f, letterHeight * 0.3f), new Vector2(letterWidth * 0.5f, letterHeight));
                points.Add(new Vector2(letterWidth * 0.5f, letterHeight * 0.1f));
                points.Add(new Vector2(letterWidth * 0.45f, letterHeight * 0.15f));
                points.Add(new Vector2(letterWidth * 0.55f, letterHeight * 0.15f));
                points.Add(new Vector2(letterWidth * 0.45f, letterHeight * 0.05f));
                points.Add(new Vector2(letterWidth * 0.55f, letterHeight * 0.05f));
                break;
                
            default:
                // Simple rectangle for unknown characters
                AddLine(points, Vector2.zero, new Vector2(letterWidth, 0));
                AddLine(points, new Vector2(letterWidth, 0), new Vector2(letterWidth, letterHeight));
                AddLine(points, new Vector2(letterWidth, letterHeight), new Vector2(0, letterHeight));
                AddLine(points, new Vector2(0, letterHeight), Vector2.zero);
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
