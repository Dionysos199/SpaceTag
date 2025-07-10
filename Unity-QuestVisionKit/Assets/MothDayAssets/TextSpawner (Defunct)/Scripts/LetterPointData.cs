using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LetterPointData", menuName = "Text System/Letter Point Data")]
public class LetterPointData : ScriptableObject
{
    [System.Serializable]
    public class LetterPoints
    {
        public char character;
        public Vector2[] points;
        public float width = 1f; // Width of this letter for spacing
    }

    public List<LetterPoints> letters = new List<LetterPoints>();
    public float defaultSpacing = 1.2f; // Default spacing between letters
    public float defaultLineHeight = 1.5f; // Height between lines

    // Dictionary for fast lookup
    private Dictionary<char, LetterPoints> letterDict;

    void OnEnable()
    {
        BuildDictionary();
    }

    void BuildDictionary()
    {
        letterDict = new Dictionary<char, LetterPoints>();
        foreach (var letter in letters)
        {
            letterDict[char.ToUpper(letter.character)] = letter;
        }
    }

    public LetterPoints GetLetterPoints(char character)
    {
        if (letterDict == null)
            BuildDictionary();

        char upperChar = char.ToUpper(character);
        return letterDict.ContainsKey(upperChar) ? letterDict[upperChar] : null;
    }
}
