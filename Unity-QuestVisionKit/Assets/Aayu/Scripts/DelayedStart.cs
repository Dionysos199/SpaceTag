using UnityEngine;
using System.Collections;

public class DelayedStart : MonoBehaviour
{
    void Start()
    {
        // Start the coroutine to delay the initialization
        StartCoroutine(DelayedInitialization());
    }

    IEnumerator DelayedInitialization()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Place your logic here that should happen after 2 seconds
        Debug.Log("Scene loaded. Initialization starts now!");

        // Example: Load UI, start animations, enable player, etc.
        // YourCustomInitFunction();
    }
}
