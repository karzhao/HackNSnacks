using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;             // Speed of the enemy
    public Transform pointA;                 // First patrol point
    public Transform pointB;                 // Second patrol point

    public int calories;
    public int protein;
    public int fat;
    public int carbs;

    public bool isMoving = true;


    private Vector3 target;                  // Current target position
    private bool movingToPointA = true;      // Direction flag

    void Start()
    {
        // Set the initial target to point A
        target = pointA.position;
    }

    void Update()
    {
        if(!isMoving){
            return;
        }
        // Move the enemy towards the target position
        MoveEnemy();

        // Check if the enemy reached the target
        CheckTargetReached();
    }

    void MoveEnemy()
    {
        // Move the enemy towards the target position
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
    }

    void CheckTargetReached()
    {
        // Check if the enemy has reached the target position
        if (Vector3.Distance(transform.position, target) < 0.24f)
        {
            // Switch the target based on the current direction
            if (movingToPointA)
            {
                target = pointB.position; // Move towards point B
            }
            else
            {
                target = pointA.position; // Move towards point A
            }

            // Toggle the direction
            movingToPointA = !movingToPointA;
        }
    }
}
