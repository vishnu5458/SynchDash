using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5.5f;
    [SerializeField] float jumpForce = 3f;
    [SerializeField] float groundCheckDistance = 0.5f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] CameraShake cameraShake;
    [SerializeField] GameObject obstaclesVFX;

    private Rigidbody rb;
    private bool isGrounded;

    public delegate void RemoteHandler(float xValue, float yValue, float zValue);
    public static RemoteHandler remoteHandler; //Delegate used to subscribe method from remote 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsGameStart)
            return;
        ForwardMovement();
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began && CheckGrounded())
                {
                    Jump();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && CheckGrounded())
        {
            Jump();
        }

        SendDataToServer();
    }

    void ForwardMovement()
    {
        Vector3 movement = transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void Jump()
    {
        Debug.Log("Jumped");
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset Y velocity
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
        return isGrounded;
    }

    void SendDataToServer()
    {
        remoteHandler?.Invoke(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }

    void GameOver() => GameManager.Instance.OnGameOver();

    void PlayParticle(Vector3 vect)
    {
        ParticleSystem ps = GameManager.Instance.GetCollectableVFX();
        ps.transform.position = vect;
        ps.Stop();
        ps.Play();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checker"))
        {
            GameManager.Instance.ResetPlatforms();
        }
        else if(other.CompareTag("Collectables"))
        {
            other.gameObject.SetActive(false);
            speed += 0.5f;
            PlayParticle(other.transform.position);
            GameManager.Instance.UpdateScore(10);
        }
        else if (other.CompareTag("Obstacles"))
        {
            other.gameObject.SetActive(false);
            obstaclesVFX.SetActive(true);
            cameraShake.start = true;
            Invoke(nameof(GameOver), 1.0f);
        }
    }

}
