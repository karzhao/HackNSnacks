using System.Collections;
using UnityEngine;

public class DancingFruit : MonoBehaviour
{
    public Sprite[] danceFrames; // Assign different sprite frames in Inspector
    public float frameRate = 0.2f; // Time per frame
    public float bounceHeight = 0.2f; // Bounce effect height
    public float bounceSpeed = 2f; // Speed of the bounce effect
    public float wiggleAmount = 5f; // Rotation wiggle angle
    public float wiggleSpeed = 5f; // Speed of the wiggle effect

    private SpriteRenderer spriteRenderer;
    private int frameIndex;
    private Vector3 startPos;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        StartCoroutine(AnimateSprite());
    }

    void Update()
    {
        // Apply bouncing effect
        float bounce = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = startPos + new Vector3(0, bounce, 0);

        // Apply wiggle effect (rotates left and right slightly)
        float wiggle = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAmount;
        transform.rotation = Quaternion.Euler(0, 0, wiggle);
    }

    IEnumerator AnimateSprite()
    {
        while (true)
        {
            if (danceFrames.Length > 0)
            {
                frameIndex = (frameIndex + 1) % danceFrames.Length;
                spriteRenderer.sprite = danceFrames[frameIndex];
            }
            yield return new WaitForSeconds(frameRate);
        }
    }
}
