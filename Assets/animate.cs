using UnityEngine;
using TMPro;

public class animate : MonoBehaviour
{
    public TextMeshProUGUI textMesh;  // Assign your TextMeshPro object in Inspector
    public float floatSpeed = 1f;     // Speed of the floating effect
    public float floatHeight = 4f;   // How much the text moves up and down

    private Vector3 startPosition;

    void Start()
    {
        startPosition = textMesh.transform.position; // Store the original position
    }

    void Update()
    {
        // Create a smooth up-and-down motion using Mathf.Sin()
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        textMesh.transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}