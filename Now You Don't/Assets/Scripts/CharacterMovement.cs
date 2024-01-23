using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public float speed = 1.0f;
    private Rigidbody2D rb;
    public float jump;
    private int airJumpNum;
    public int airJumpMax;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        airJumpNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && airJumpNum <= airJumpMax) 
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump));
            airJumpNum++;
        }
        float xMove = Input.GetAxisRaw("Horizontal");

        xMove = xMove * speed;
        rb.velocity = (new Vector2(xMove, rb.velocity.y));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name=="Floor")
        {
            airJumpNum = 0;
        }
        else 
        {
            if(Mathf.Abs(rb.velocity.y) < 0.5) 
            {
                airJumpNum = 0;
            }
        }

        
    }

}
