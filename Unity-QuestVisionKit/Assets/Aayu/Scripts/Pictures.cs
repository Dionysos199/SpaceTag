using UnityEngine;
using UnityEngine.UI;

public class Pictures : MonoBehaviour
{
    [SerializeField] GameObject image;
    public Sprite polaroid {  get; set; }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image.GetComponent<Image>().sprite = polaroid;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
