using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public float verticalBallSpeed = 500.0f;
    public float horizontalBallSpeed = 800.0f;
    public float jumpForce = 500.0f;
    public Text speedText;
    public int maxSpeed = 60;
    public Text gameOverText;
    public GameObject restartButton;
    private Rigidbody _rigidBody;
    private ScoreMgr _scoreMgr;
    private bool _isOnGround = false;
    private bool _isGameOver = false;
    private bool _isWon = false;

    public int finishScore = 500;
    
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _scoreMgr = GetComponent<ScoreMgr>();
        Reset();
    }

    private void FixedUpdate()
    {
        if (_isGameOver || _isWon)
        {
            return;
        }

        MoveBall();

        if (!_isWon && !_isGameOver && transform.position.y < -100f)
        {
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            _isGameOver = true;
        }
    }

    private void MoveBall()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        int speed = (int) _rigidBody.velocity.z;
        float verticalSpeed = moveVertical * verticalBallSpeed;
        if (verticalSpeed > 0f && speed >= maxSpeed)
        {
            verticalSpeed = 0f;
        }
        Vector3 ballMovement = new Vector3(moveHorizontal * horizontalBallSpeed,
            0f,
            verticalSpeed);

        _rigidBody.AddForce(ballMovement * Time.deltaTime);

        speedText.text = $"Speed: {speed} KMH";

        if (Input.GetAxis("Jump") > 0f && _isOnGround)
        {
            _rigidBody.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
            _isOnGround = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckScoreCollectible(other);
        CheckFinish(other);
    }

    private void OnCollisionEnter(Collision other)
    {
        CheckIsOnGround(other);
    }

    private void CheckScoreCollectible(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            other.gameObject.SetActive(false);
        }

        CollectibleData collectibleData = other.gameObject.GetComponent<CollectibleData>();
        if (collectibleData != null)
        {
            _scoreMgr.AddScore(collectibleData.scoreValue);
        }
    }
    
    private void CheckFinish(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            _scoreMgr.AddScore(finishScore);
            _scoreMgr.HasFinished();
            _isWon = true;
        }
    }

    private void CheckIsOnGround(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isOnGround = true;
        }
    }

    public void Reset()
    {
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        _rigidBody.velocity = Vector3.zero;
        _isGameOver = false;
        _isOnGround = true;
        _isWon = false;
    }
}
