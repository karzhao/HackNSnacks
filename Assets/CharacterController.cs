using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.SceneManagement;


public class CharacterController : MonoBehaviour
{
    
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 1f;
    public LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    public int calories;
    public int protein;
    public int fat;
    public int carbs;
    public int maxCalories = 2000;
    public TMP_Text myText;
    // public HealthSystem healthGameObject;
    public List<string> names;

    public Sprite appleSprite;
    public Sprite breadSprite;
    public Sprite ChickenSprite;

    public TMP_Text popupText;
    public float displayDuration = 4f;
    private Coroutine currentCoroutine;
    
    void Start()
    {
        string character = PlayerPrefs.GetString("Character");
        SpriteRenderer spriteRenderder = GetComponent<SpriteRenderer>();
        if(character == "Bread"){
            spriteRenderder.sprite = breadSprite;
        }
        else if(character == "Chicken"){
            spriteRenderder.sprite = ChickenSprite;
        }
        
        names = new List<string>(); 
        rb = GetComponent<Rigidbody2D>();
        // healthGameObject.TakeDamage(10);
        myText.text = "Calories: " + calories;

        popupText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Get player input
        moveInput = Input.GetAxisRaw("Horizontal");

        // Jumping
        
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if(transform.position.y < -7){
            Die();
        }
        
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Check if the player is on the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log(calories);
        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

        if (enemy != null)
        {
            this.calories += enemy.calories; 
            this.protein += enemy.protein;
            this.carbs += enemy.carbs;
            this.fat += enemy.fat;

            collision.gameObject.SetActive(false);
            string name = collision.gameObject.GetComponent<SpriteRenderer>().sprite.name;
            ShowPopUp("You ate a " + name + "!\n" + "Calories: " + enemy.calories + "\nProtein: " + enemy.protein + "\nCarbohydrates: " + enemy.carbs + "\nFats: " + enemy.fat);
            names.Add(name);

            transform.localScale = new Vector3(
            Mathf.Min(0.4f + transform.localScale.x, 2.5f),
            Mathf.Min(0.4f + transform.localScale.y, 2.5f),
            Mathf.Min(0.4f + transform.localScale.z, 2.5f)
            );
        }
        // Debug.Log(calories);
        myText.text = "Calories: " + calories + "cal\n" + "Protein: " + this.protein + "g\n" + "Fats: " + this.fat + "g\n" + "Carbs: " + this.carbs + "g\n";
        if(calories >= maxCalories){
            Die();
        }
    }

    void Die()
    {
        LoadSceneWithData(names);
        SceneManager.LoadScene("deathSummary");
    }

    public void LoadSceneWithData(List<string> data)
    {
        string dataString = string.Join(",", data);
        PlayerPrefs.SetString("MyData", dataString);
        PlayerPrefs.SetInt("TotalProtein", protein);
        PlayerPrefs.SetInt("TotalCarbs", carbs);
        PlayerPrefs.SetInt("TotalFat", fat);
        PlayerPrefs.Save();
    }

     public void ShowPopUp(string info)
    {
        // If a pop-up is already showing, stop its timer so we can reset it
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Update the text and make it visible
        popupText.text = info;
        popupText.gameObject.SetActive(true);

        // Start a coroutine to hide the text after a delay
        currentCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        popupText.gameObject.SetActive(false);
    }
    
}
