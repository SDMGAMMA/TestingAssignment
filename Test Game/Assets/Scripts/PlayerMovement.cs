using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    // Other varibables
    private Rigidbody playerRigidbody;
    private HealthBar healthBar;

    [SerializeField] public bool dead;

    private float movementInput = 0;

    private float maxSpeedBoostMultiplier;
    private int speedBoostMultiplier;

    public Camera playerCamera;
    public float gravity;

    [Space]
    // Movement variables
    public float forwardSpeed;
    public float movementSpeed;

    public float dashSpeed;
    public float dashUnlockMovementSpeed;

    public int speedBoost;

    [Space]
    // Movement cap variables
    public int maxForwardSpeed;
    public int maxMovementSpeed;
    public float maxSpeedBoost;

    [Space]
    // Friction variables
    public float friction;
    public float slowDownFriction;    

    [Space]
    // Camera variables
    public float zoomMultiplier;
    public float cameraEase;

    [SerializeField] TextMeshProUGUI highScore;
    private float timeSpeedBoosted = 0;

    [SerializeField] ParticleSystem particles;

    bool jump;
    bool isGrounded;

    bool invurnable;

    void Start()
    {
        jump = false;
        isGrounded = true;
        invurnable = false;

        playerRigidbody = GetComponent<Rigidbody>();
        healthBar = GetComponent<HealthBar>();

        dead = false; 

        particles.Stop();

        // Multiply variables to have them work with time.delta time
        forwardSpeed *= 1000;
        movementSpeed *= 1000;
    }

    void Update()
    {
        // Get input for movement
        movementInput = Input.GetAxisRaw("Horizontal") * movementSpeed * Time.deltaTime;

        // Get if player is speeding up
        if (Input.GetKey("z"))
        {
            // Set variables to another value so that later the speed will be multiplied
            maxSpeedBoostMultiplier = maxSpeedBoost;
            speedBoostMultiplier = speedBoost;
        }
        else
        {
            // Else set variables to 1 to not speed up
            maxSpeedBoostMultiplier = 1;
            speedBoostMultiplier = 1;
        }

        if (Input.GetKeyDown("x"))
        {
            jump = true;
        }
    }

    void FixedUpdate()
    {
        // Move forward
        playerRigidbody.AddForce(transform.forward * forwardSpeed * Time.deltaTime * speedBoostMultiplier);

        // Slowdown if going too fast
        if (playerRigidbody.velocity.z > maxForwardSpeed * maxSpeedBoostMultiplier)
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y, maxForwardSpeed * maxSpeedBoostMultiplier);
        }

        // Add movement force
        playerRigidbody.AddForce(transform.right * movementInput);

        // If velocity is too high, cap it
        if (playerRigidbody.velocity.x < -maxMovementSpeed)
        {
            playerRigidbody.velocity = new Vector3(-maxMovementSpeed, playerRigidbody.velocity.y, playerRigidbody.velocity.z);
        }
        if (playerRigidbody.velocity.x > maxMovementSpeed)
        {
            playerRigidbody.velocity = new Vector3(maxMovementSpeed, playerRigidbody.velocity.y, playerRigidbody.velocity.z);
        }

        if (maxSpeedBoostMultiplier != 1)
        {
            timeSpeedBoosted += Time.deltaTime;
            healthBar.health += Time.deltaTime * 30;

            if (healthBar.health > 500)
            {
                healthBar.health = 500;
            }
        }

        // left and right friction
        playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x * friction, playerRigidbody.velocity.y, playerRigidbody.velocity.z);

        // Gravity
        if (jump)
        {
            jump = false;

            if (isGrounded)
            {
                isGrounded = false;
                playerRigidbody.AddForce(transform.up * 320);
            }
        }

        playerRigidbody.AddForce(-transform.up * gravity);

        if (dead)
        {
            return;
        }

        // Give a cool camera effect (Apparte script)
        Vector3 trajectory = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z + 2 - playerRigidbody.velocity.z / maxForwardSpeed * zoomMultiplier);
        playerCamera.transform.position += (trajectory - playerCamera.transform.position) * cameraEase;

        highScore.text = ((int)transform.position.z).ToString() + (int)(timeSpeedBoosted / 10);
    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;

        if (collision.gameObject.tag == "Obstacle")
        {
            if (!invurnable)
            {
                healthBar.Damage();

                StartCoroutine(ControlInvurnebility());
            }

            if (healthBar.health <= 0)
            {
                dead = true;

                particles.transform.position = this.transform.position;
                particles.Play();

                transform.position = new Vector3(0, -100, 0);

                StartCoroutine(Death());
            }

            collision.gameObject.SetActive(false);
        }
    }

    public IEnumerator Death()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator ControlInvurnebility()
    {
        invurnable = true;

        yield return new WaitForSeconds(1);

        invurnable = false;
    }
}
