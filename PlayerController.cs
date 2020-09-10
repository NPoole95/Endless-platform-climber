using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public Rigidbody2D playerRB;

    [SerializeField]
    AudioSource jumpSound;
    [SerializeField]
    AudioSource brokenBranchSound;
    [SerializeField]
    ParticleSystem particleSystem;

    Animator jumpAnimator;

    [SerializeField]
    GameController gameController;

    const float Gravity = 2.0f;
    const float JumpForce = 100.0f;
    const float StrafeSpeed = 4.0f;

    public int score = 0;

    GameObject[] branches;

    Vector3 cameraViewPos;

    // Start is called before the first frame update
    void Awake()
    {
        cameraViewPos = Camera.main.WorldToViewportPoint(transform.position);
        jumpAnimator = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        cameraViewPos = Camera.main.WorldToViewportPoint(transform.position);

        Vector3 leftOfCamera = Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
        Vector3 rightOfCamera = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f));

        if (cameraViewPos.x < 0.0)
        {
            cameraViewPos.x = 0.0f; transform.position = new Vector3(rightOfCamera.x, transform.position.y, transform.position.z);
        }
        else if (1.0 < cameraViewPos.x)
        {
            cameraViewPos.x = 1.0f; transform.position = new Vector3(leftOfCamera.x, transform.position.y, transform.position.z);
        }


        if (score < (int)transform.position.y)
        {
            score = (int)transform.position.y;
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerRB.AddForce(Vector3.left * StrafeSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            playerRB.AddForce(Vector3.right * StrafeSpeed);
        }

        if (playerRB.velocity.y < 0) // if player is falling, reset animation
        {
            jumpAnimator.SetBool("Jumping", false);
        }

        playerRB.AddForce(Vector3.down * Gravity * Time.deltaTime);
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // collision code for when using tilemaps 
        if (playerRB.velocity.y < 0 && collision.gameObject.name == "branchTilemap")
        {
            playerRB.velocity = new Vector2(0.0f, 10.0f);
            jumpAnimator.SetBool("Jumping", true);
            jumpSound.Play();
        }
        else if (playerRB.velocity.y < 0 && collision.gameObject.CompareTag("BrokenBranch"))
        {
            brokenBranchSound.Play();
            particleSystem.Play();
            Rigidbody2D[] branchParts = collision.gameObject.GetComponentsInChildren<Rigidbody2D>();

            foreach (Rigidbody2D branch in branchParts)
            {
                branch.gravityScale = 1;

                if (branch.name == ("brokenBranchLeft"))
                {
                    branch.velocity = new Vector2(-0.2f, 0.0f);
                }
                else if (branch.name == ("brokenBranchRight"))
                {
                    branch.velocity = new Vector2(0.2f, 0.0f);
                }
            }
        }
        else  if (collision.gameObject.CompareTag("Branch") && playerRB.transform.position.y > collision.transform.position.y)
        {
            playerRB.velocity = new Vector2(0.0f, 10.0f);
            jumpAnimator.SetBool("Jumping", true);
            jumpSound.Play();
        }



        // collision code for when using prefab instantiation 
        //if (collision.gameObject.CompareTag("Branch") && playerRB.transform.position.y > collision.transform.position.y)
        //{
        //    playerRB.velocity = new Vector2(0.0f, 10.0f);
        //    jumpAnimator.SetBool("Jumping", true);
        //    jumpSound.Play();
        //}
        //else if (collision.gameObject.CompareTag("BrokenBranch") && playerRB.transform.position.y > collision.transform.position.y)
        //{
        //    brokenBranchSound.Play();
        //    Rigidbody2D[] branchParts = collision.gameObject.GetComponentsInChildren<Rigidbody2D>();

        //    foreach (Rigidbody2D branch in branchParts)
        //    {
        //        branch.gravityScale = 1;

        //        if (branch.name == ("brokenBranchLeft"))
        //        {
        //            branch.velocity = new Vector2(-0.2f, 0.0f);
        //        }
        //        else if (branch.name == ("brokenBranchRight"))
        //        {
        //            branch.velocity = new Vector2(0.2f, 0.0f);
        //        }
        //    }
        //}
        //else
        //{
        //   // jumpAnimator.ResetTrigger("Jump");
        //}
    }
}





