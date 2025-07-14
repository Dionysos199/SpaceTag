using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GlyphSpawner : MonoBehaviour
{
    private TMP_Text textMeshPro;
    public GameObject prefabToSpawn;
    public int prefabsPerCharacter = 20;
    public Vector3 offset = Vector3.zero;

    void Start()
    {
        StartCoroutine(SpawnInsideGlyphs());
    }

    IEnumerator SpawnInsideGlyphs()
    {
        textMeshPro = gameObject.AddComponent<TextMeshPro>();
        textMeshPro.text = "Hello";
        textMeshPro.ForceMeshUpdate();
        TMP_TextInfo textInfo = textMeshPro.textInfo;
        Mesh mesh = textMeshPro.mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;

            // Each character is a quad (2 triangles, 4 vertices)
            Vector3 v0 = textMeshPro.transform.TransformPoint(vertices[vertexIndex + 0]);
            Vector3 v1 = textMeshPro.transform.TransformPoint(vertices[vertexIndex + 1]);
            Vector3 v2 = textMeshPro.transform.TransformPoint(vertices[vertexIndex + 2]);
            Vector3 v3 = textMeshPro.transform.TransformPoint(vertices[vertexIndex + 3]);

            // Triangle 1: v0, v1, v2 | Triangle 2: v2, v3, v0
            for (int j = 0; j < prefabsPerCharacter; j++)
            {
                bool useFirstTriangle = Random.value > 0.5f;
                Vector3 a = useFirstTriangle ? v0 : v2;
                Vector3 b = useFirstTriangle ? v1 : v3;
                Vector3 c = useFirstTriangle ? v2 : v0;

                // Barycentric coordinate sampling
                Vector3 point = RandomPointInTriangle(a, b, c);
                Instantiate(prefabToSpawn, point + offset, Quaternion.identity, this.transform);
                yield return null;
            }
        }
    }

    Vector3 RandomPointInTriangle(Vector3 a, Vector3 b, Vector3 c)
    {
        float r1 = Random.value;
        float r2 = Random.value;

        if (r1 + r2 > 1f)
        {
            r1 = 1f - r1;
            r2 = 1f - r2;
        }

        return a + r1 * (b - a) + r2 * (c - a);
    }
}
