using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    
    private Rigidbody2D _rb; 
    private float _velocity;
    private bool _isGrounded;
    private Vector2 _speedHorizontal;
    private bool _isMoving;
    private Vector2 _canMove;

    public float speedModifier = 100f;
    public float maxSpeed = 10f;
    public float jumpForce = 100f;
    public float raycastLenght = 0.2f;
    public LayerMask groundLayer;
    public float slowFactor = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
       // coll = GetComponent<floorDetection>();
       // Vector2 velocity = _rb.linearVelocity;
    }

    void Update()
    {
        _canMove = new Vector2(Input.GetAxis("Horizontal"), 0);
        _speedHorizontal = new Vector2(Input.GetAxis("Horizontal")  * speedModifier * 10 * Time.deltaTime, 0);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (_canMove != Vector2.zero)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }

        Debug.Log(_isMoving);

        switch (_isMoving)
        {
            case true:
                _rb.linearVelocity -= speedModifier + slowFactor;
                break;
            
        }

        if (_isGrounded)
        {
            _rb.linearVelocity += _speedHorizontal;
        } 
        else if (_isGrounded == false)
        {
            _rb.linearVelocity += _speedHorizontal / 4;
        }

        if (_rb.linearVelocity.x>= maxSpeed)
        {
            _rb.linearVelocity = new Vector2(maxSpeed, _rb.linearVelocity.y); 
        } 
        else if (_rb.linearVelocity.x <= -maxSpeed)
        {
            _rb.linearVelocity = new Vector2(-maxSpeed, _rb.linearVelocity.y);
        }
            
        Vector2 position = transform.position;
        Vector2 leftPoint = position + new Vector2(-0.45f, -0.5f);
        Vector2 rightPoint = position + new Vector2(0.45f, -0.5f);

        bool leftGrounded = Physics2D.Raycast(leftPoint, Vector2.down, raycastLenght, groundLayer);
        bool rightGrounded = Physics2D.Raycast(rightPoint, Vector2.down, raycastLenght, groundLayer);

        _isGrounded = leftGrounded || rightGrounded;
        
        if (Input.GetButton("Jump") && _isGrounded)
        {
            _rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            Debug.Log("Jump");
        }
        
        Debug.DrawRay(leftPoint, Vector2.down * raycastLenght, Color.red);
        Debug.DrawRay(rightPoint, Vector2.down * raycastLenght, Color.red);

       // Debug.Log(_rb.linearVelocity);
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("levelExit"))
        {
            SceneManager.LoadScene(1);
        }
        
        if (collision.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}

