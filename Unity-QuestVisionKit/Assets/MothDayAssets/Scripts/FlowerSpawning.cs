using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawning : MonoBehaviour
{
        [SerializeField] private GameObject flowerPrefab;
        [SerializeField] private float letterWidth = 2f;
        [SerializeField] private float letterHeight = 3f;
        [SerializeField] private float letterSpacing = 0.5f;
        [SerializeField] private float flowerDensity = 0.3f; // Distance between flowers
        [SerializeField] private string textToSpell = "HELLO";
        [SerializeField] private int flowersPerPoint = 3;
        [SerializeField] private float flowerScatter = 0.2f;


        void Start()
        {
            SpellWithFlowers(textToSpell);
        }
        
        public void SpellWithFlowers(string text) 
        {
            float currentX = 0f;
            
            foreach (char c in text) 
            {
                if (c == ' ') 
                {
                    currentX += letterWidth * 0.6f; // Space width
                    continue;
                }
                
                SpawnFlowersForLetter(c, new Vector3(currentX, 0, 0));
                currentX += letterWidth + letterSpacing;
            }
        }
        
        void SpawnFlowersForLetter(char letter, Vector3 basePosition) 
        {
            List<Vector2> letterPoints = GetLetterPoints(letter);
    
            foreach (Vector2 point in letterPoints) 
            {
                Vector3 worldPos = basePosition + new Vector3(point.x, 0, point.y);
        
                // Spawn multiple flowers at each point
                for (int i = 0; i < flowersPerPoint; i++) 
                {
                    Vector3 scatteredPos = worldPos;
            
                    // Add random scatter for extra flowers (except the first one)
                    if (i > 0) 
                    {
                        scatteredPos += new Vector3(
                            Random.Range(-flowerScatter, flowerScatter), 
                            0, 
                            Random.Range(-flowerScatter, flowerScatter)
                        );
                    }
            
                    // All flowers face up, just random Y rotation for variety
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
                    // Left diagonal line (from bottom-left to top-center)
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                        points.Add(new Vector2(letterWidth * 0.1f + letterWidth * 0.4f * t, letterHeight * t));
                    // Right diagonal line (from bottom-right to top-center)
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                        points.Add(new Vector2(letterWidth * 0.9f - letterWidth * 0.4f * t, letterHeight * t));
                    // Crossbar (horizontal line in the middle)
                    for (float x = letterWidth * 0.3f; x <= letterWidth * 0.7f; x += flowerDensity) 
                        points.Add(new Vector2(x, letterHeight * 0.4f));
                    break;
                    
                case "B":
                    // Left vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity) 
                        points.Add(new Vector2(0, y));
                    // Top horizontal
                    for (float x = 0; x <= letterWidth * 0.6f; x += flowerDensity) 
                        points.Add(new Vector2(x, letterHeight));
                    // Middle horizontal
                    for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity) 
                        points.Add(new Vector2(x, letterHeight * 0.5f));
                    // Bottom horizontal
                    for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity) 
                        points.Add(new Vector2(x, 0));
                    // Top right curve (semicircle for upper bump)
                    for (float angle = -90; angle <= 90; angle += 180 * flowerDensity / (Mathf.PI * letterWidth * 0.2f)) 
                    {
                        float x = letterWidth * 0.6f + letterWidth * 0.2f * Mathf.Cos(angle * Mathf.Deg2Rad);
                        float y = letterHeight * 0.75f + letterHeight * 0.25f * Mathf.Sin(angle * Mathf.Deg2Rad);
                        points.Add(new Vector2(x, y));
                    }
                    // Bottom right curve (semicircle for lower bump)
                    for (float angle = -90; angle <= 90; angle += 180 * flowerDensity / (Mathf.PI * letterWidth * 0.2f)) 
                    {
                        float x = letterWidth * 0.7f + letterWidth * 0.2f * Mathf.Cos(angle * Mathf.Deg2Rad);
                        float y = letterHeight * 0.25f + letterHeight * 0.25f * Mathf.Sin(angle * Mathf.Deg2Rad);
                        points.Add(new Vector2(x, y));
                    }
                    break;
                    
                case "C":
                    // Left vertical line
                    for (float y = 0.2f; y <= letterHeight - 0.2f; y += flowerDensity) 
                        points.Add(new Vector2(0, y));
                    // Top horizontal - extended longer
                    for (float x = 0; x <= letterWidth * 0.9f; x += flowerDensity) 
                        points.Add(new Vector2(x, letterHeight));
                    // Bottom horizontal - extended longer
                    for (float x = 0; x <= letterWidth * 0.9f; x += flowerDensity) 
                        points.Add(new Vector2(x, 0));
                    break;

                case "D":
                    // Left vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(0, y));
                    // Top horizontal
                    for (float x = 0; x <= letterWidth * 0.6f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight));
                    // Bottom horizontal
                    for (float x = 0; x <= letterWidth * 0.6f; x += flowerDensity)
                        points.Add(new Vector2(x, 0));
                    // Right curve
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(
                            letterWidth * 0.6f + letterWidth * 0.3f * Mathf.Sin(y / letterHeight * Mathf.PI), y));
                    break;

                case "E":
                    // Left vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(0, y));
                    // Top horizontal
                    for (float x = 0; x <= letterWidth; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight));
                    // Middle horizontal
                    for (float x = 0; x <= letterWidth * 0.8f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight * 0.5f));
                    // Bottom horizontal
                    for (float x = 0; x <= letterWidth; x += flowerDensity)
                        points.Add(new Vector2(x, 0));
                    break;

                case "F":
                    // Left vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(0, y));
                    // Top horizontal
                    for (float x = 0; x <= letterWidth; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight));
                    // Middle horizontal
                    for (float x = 0; x <= letterWidth * 0.8f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight * 0.5f));
                    break;

                case "G":
                    // Left vertical line
                    for (float y = letterHeight * 0.15f; y <= letterHeight * 0.85f; y += flowerDensity)
                        points.Add(new Vector2(0, y));
                    // Top horizontal
                    for (float x = 0; x <= letterWidth * 0.8f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight));
                    // Bottom horizontal
                    for (float x = 0; x <= letterWidth * 0.8f; x += flowerDensity)
                        points.Add(new Vector2(x, 0));
                    // Right vertical (bottom half)
                    for (float y = 0; y <= letterHeight * 0.5f; y += flowerDensity)
                        points.Add(new Vector2(letterWidth * 0.8f, y));
                    // Middle horizontal (right side)
                    for (float x = letterWidth * 0.5f; x <= letterWidth * 0.8f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight * 0.5f));
                    break;

                case "H":
                    // Left vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(0, y));
                    // Right vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(letterWidth, y));
                    // Cross bar
                    for (float x = 0; x <= letterWidth; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight * 0.5f));
                    break;

                case "I":
                    // Center vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(letterWidth * 0.5f, y));
                    // Top horizontal
                    for (float x = letterWidth * 0.2f; x <= letterWidth * 0.8f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight));
                    // Bottom horizontal
                    for (float x = letterWidth * 0.2f; x <= letterWidth * 0.8f; x += flowerDensity)
                        points.Add(new Vector2(x, 0));
                    break;

                case "J":
                    // Right vertical line
                    for (float y = letterHeight * 0.3f; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(letterWidth * 0.7f, y));
                    // Top horizontal
                    for (float x = letterWidth * 0.3f; x <= letterWidth; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight));
                    // Bottom curve
                    for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight * 0.3f));
                    // Left bottom vertical
                    for (float y = 0; y <= letterHeight * 0.3f; y += flowerDensity)
                        points.Add(new Vector2(0, y));
                    break;

                case "K":
                    // Left vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(0, y));
                    // Upper diagonal
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight)
                        points.Add(new Vector2(letterWidth * 0.5f * t, letterHeight * 0.5f + letterHeight * 0.5f * t));
                    // Lower diagonal
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight)
                        points.Add(new Vector2(letterWidth * 0.5f * t, letterHeight * 0.5f - letterHeight * 0.5f * t));
                    break;

                case "L":
                    // Left vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(0, y));
                    // Bottom horizontal
                    for (float x = 0; x <= letterWidth; x += flowerDensity)
                        points.Add(new Vector2(x, 0));
                    break;

                case "M":
                    // Left vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(0, y));
                    // Right vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(letterWidth, y));
                    // Left diagonal to center
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight)
                        points.Add(new Vector2(letterWidth * 0.5f * t, letterHeight - letterHeight * 0.6f * t));
                    // Right diagonal to center
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight)
                        points.Add(new Vector2(letterWidth - letterWidth * 0.5f * t,
                            letterHeight - letterHeight * 0.6f * t));
                    break;

                case "N":
                    // Left vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity) 
                        points.Add(new Vector2(0, y));
                    // Right vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity) 
                        points.Add(new Vector2(letterWidth, y));
                    // Diagonal (from top-left to bottom-right)
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                        points.Add(new Vector2(letterWidth * t, letterHeight - letterHeight * t));
                    break;

                case "O":
                    // Circle/oval shape - made even larger
                    for (float angle = 0; angle < 360; angle += 360 * flowerDensity / (2 * Mathf.PI * letterWidth * 0.48f)) 
                    {
                        float x = letterWidth * 0.5f + letterWidth * 0.48f * Mathf.Cos(angle * Mathf.Deg2Rad);
                        float y = letterHeight * 0.5f + letterHeight * 0.48f * Mathf.Sin(angle * Mathf.Deg2Rad);
                        points.Add(new Vector2(x, y));
                    }
                    break;

                case "P":
                    // Left vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(0, y));
                    // Top horizontal
                    for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight));
                    // Middle horizontal
                    for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight * 0.5f));
                    // Right vertical (top half)
                    for (float y = letterHeight * 0.5f; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(letterWidth * 0.7f, y));
                    break;

                case "Q":
                    // Circle/oval shape
                    for (float angle = 0;
                         angle < 360;
                         angle += 360 * flowerDensity / (2 * Mathf.PI * letterWidth * 0.4f))
                    {
                        float x = letterWidth * 0.5f + letterWidth * 0.4f * Mathf.Cos(angle * Mathf.Deg2Rad);
                        float y = letterHeight * 0.5f + letterHeight * 0.4f * Mathf.Sin(angle * Mathf.Deg2Rad);
                        points.Add(new Vector2(x, y));
                    }
                    // Tail diagonal
                    for (float t = 0; t <= 1; t += flowerDensity / letterWidth)
                        points.Add(new Vector2(letterWidth * 0.6f + letterWidth * 0.3f * t,
                            letterHeight * 0.3f - letterHeight * 0.3f * t));
                    break;
                
                case "R":
                    // Left vertical line
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(0, y));
                    // Top horizontal
                    for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight));
                    // Middle horizontal
                    for (float x = 0; x <= letterWidth * 0.7f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight * 0.5f));
                    // Right vertical (top half)
                    for (float y = letterHeight * 0.5f; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(letterWidth * 0.7f, y));
                    // Diagonal leg
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight)
                        points.Add(new Vector2(letterWidth * 0.4f + letterWidth * 0.4f * t,
                            letterHeight * 0.5f - letterHeight * 0.5f * t));
                    break;

                case "S":
                    // Top horizontal
                    for (float x = letterWidth * 0.2f; x <= letterWidth * 0.8f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight));
                    // Top left vertical
                    for (float y = letterHeight * 0.5f; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(letterWidth * 0.2f, y));
                    // Middle horizontal
                    for (float x = letterWidth * 0.2f; x <= letterWidth * 0.8f; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight * 0.5f));
                    // Bottom right vertical
                    for (float y = 0; y <= letterHeight * 0.5f; y += flowerDensity)
                        points.Add(new Vector2(letterWidth * 0.8f, y));
                    // Bottom horizontal
                    for (float x = letterWidth * 0.2f; x <= letterWidth * 0.8f; x += flowerDensity)
                        points.Add(new Vector2(x, 0));
                    break;

                case "T":
                    // Top horizontal
                    for (float x = 0; x <= letterWidth; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight));
                    // Center vertical
                    for (float y = 0; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(letterWidth * 0.5f, y));
                    break;

                case "U":
                    // Left vertical line
                    for (float y = letterHeight * 0.25f; y <= letterHeight; y += flowerDensity) 
                        points.Add(new Vector2(letterWidth * 0.15f, y));
                    // Right vertical line
                    for (float y = letterHeight * 0.25f; y <= letterHeight; y += flowerDensity) 
                        points.Add(new Vector2(letterWidth * 0.85f, y));
                    // Bottom curve
                    for (float x = letterWidth * 0.15f; x <= letterWidth * 0.85f; x += flowerDensity) 
                    {
                        float normalizedX = (x - letterWidth * 0.15f) / (letterWidth * 0.7f);
                        float curveY = letterHeight * 0.25f * Mathf.Sin(normalizedX * Mathf.PI);
                        points.Add(new Vector2(x, curveY));
                    }
                    break;

                case "V":
                    // Left diagonal
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight)
                        points.Add(new Vector2(letterWidth * 0.5f * t, letterHeight - letterHeight * t));
                    // Right diagonal
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight)
                        points.Add(new Vector2(letterWidth - letterWidth * 0.5f * t, letterHeight - letterHeight * t));
                    break;

                case "W":
                    // Left vertical line
                    for (float y = letterHeight * 0.3f; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(0, y));
                    // Right vertical line
                    for (float y = letterHeight * 0.3f; y <= letterHeight; y += flowerDensity)
                        points.Add(new Vector2(letterWidth, y));
                    // Left diagonal to center
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight)
                        points.Add(new Vector2(letterWidth * 0.25f * (1 + t),
                            letterHeight * 0.3f + letterHeight * 0.4f * t));
                    // Right diagonal to center
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight)
                        points.Add(new Vector2(letterWidth * 0.75f * (1 + (1 - t) / 3),
                            letterHeight * 0.3f + letterHeight * 0.4f * t));
                    // Bottom points
                    points.Add(new Vector2(letterWidth * 0.25f, 0));
                    points.Add(new Vector2(letterWidth * 0.75f, 0));
                    break;

                case "X":
                    // Main diagonal (top-left to bottom-right)
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight)
                        points.Add(new Vector2(letterWidth * t, letterHeight - letterHeight * t));
                    // Cross diagonal (top-right to bottom-left)
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight)
                        points.Add(new Vector2(letterWidth - letterWidth * t, letterHeight - letterHeight * t));
                    break;

                case "Y":
                    // Left diagonal to center
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                        points.Add(new Vector2(letterWidth * 0.5f * t, letterHeight - letterHeight * 0.5f * t));
                    // Right diagonal to center
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight) 
                        points.Add(new Vector2(letterWidth - letterWidth * 0.5f * t, letterHeight - letterHeight * 0.5f * t));
                    // Center vertical (bottom half)
                    for (float y = 0; y <= letterHeight * 0.5f; y += flowerDensity) 
                        points.Add(new Vector2(letterWidth * 0.5f, y));
                    break;

                case "Z":
                    // Top horizontal
                    for (float x = 0; x <= letterWidth; x += flowerDensity)
                        points.Add(new Vector2(x, letterHeight));
                    // Diagonal
                    for (float t = 0; t <= 1; t += flowerDensity / letterHeight)
                        points.Add(new Vector2(letterWidth - letterWidth * t, letterHeight - letterHeight * t));
                    // Bottom horizontal
                    for (float x = 0; x <= letterWidth; x += flowerDensity)
                        points.Add(new Vector2(x, 0));
                    break;

                case "!":
                    // Main vertical rectangle (top part) - make it thicker like a rectangle
                    for (float y = letterHeight * 0.4f; y <= letterHeight; y += flowerDensity) 
                    {
                        points.Add(new Vector2(letterWidth * 0.45f, y)); // Left edge
                        points.Add(new Vector2(letterWidth * 0.5f, y));  // Center
                        points.Add(new Vector2(letterWidth * 0.55f, y)); // Right edge
                    }
                    // Top and bottom edges of rectangle
                    for (float x = letterWidth * 0.45f; x <= letterWidth * 0.55f; x += flowerDensity) 
                    {
                        points.Add(new Vector2(x, letterHeight));         // Top edge
                        points.Add(new Vector2(x, letterHeight * 0.4f));  // Bottom edge
                    }
    
                    // Empty space gap here (letterHeight * 0.4f to letterHeight * 0.2f)
    
                    // Bottom square dot
                    for (float x = letterWidth * 0.45f; x <= letterWidth * 0.55f; x += flowerDensity) 
                    {
                        points.Add(new Vector2(x, letterHeight * 0.2f));  // Top of square
                        points.Add(new Vector2(x, letterHeight * 0.1f));  // Bottom of square
                    }
                    for (float y = letterHeight * 0.1f; y <= letterHeight * 0.2f; y += flowerDensity) 
                    {
                        points.Add(new Vector2(letterWidth * 0.45f, y));  // Left edge of square
                        points.Add(new Vector2(letterWidth * 0.55f, y));  // Right edge of square
                    }
                    break;
                
                default:
                    // Default: just fill a rectangle outline
                    for (float x = 0; x <= letterWidth; x += flowerDensity) 
                    {
                        points.Add(new Vector2(x, 0)); // Bottom
                        points.Add(new Vector2(x, letterHeight)); // Top
                    }
                    for (float y = 0; y <= letterHeight; y += flowerDensity) 
                    {
                        points.Add(new Vector2(0, y)); // Left
                        points.Add(new Vector2(letterWidth, y)); // Right
                    }
                    break;
            }
            return points;
        }
}
