using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum SnakePlayer
{
    Snake1,
    Snake2
    
}
public class Snake : MonoBehaviour
{
    [SerializeField]
    public SnakePlayer player;
    [SerializeField]
    private TextMeshProUGUI scoreTextSnake1;
    [SerializeField]
    private TextMeshProUGUI scoreTextSnake2;
    [SerializeField]
    private float textToDestroy = 3f;
    public GameObject speedUp;
    public GameObject scoreBoost;
    public float speed = 1f;
    public float speedMultiplier = 1f;
    public GameObject shieldEffect;
    private bool hasSpeedUp = false;
    public bool hasScoreboost = false;
    [SerializeField]
    private TextMeshProUGUI pickupText;

    private float PowerAcivated = 15f;
    public int snake1Score = 0;
    public int snake2Score = 0;
    public BoxCollider2D gridArea;
    private Bounds bounds;
    private string wallDirection = "right";

    private Vector2Int direction1 = Vector2Int.right;
    private Vector2Int direction2 = Vector2Int.left;
    private bool hasShield = false;
    private List<Transform> _segments;
    private int initialSize = 1;
    public Transform segmentPrefab;
    [SerializeField] private string gameOver;

    private void Start()
    {
        bounds = gridArea.bounds;
        hasShield = false;

        _segments = new List<Transform>();
        _segments.Add(this.transform);
        for (int i = 1; i < initialSize; i++)
        {
            _segments.Add(Instantiate(segmentPrefab, transform.position, Quaternion.identity));
        }
    }

    private void Update()
    {
        SnakeDirection();
       
    }

    private void FixedUpdate()
    {
         MovingSnake();

    }

    private void MovingSnake()
    {
        
        if (player == SnakePlayer.Snake1)
        {
            for (int i = _segments.Count - 1; i > 0; i--)
            {
                _segments[i].position = _segments[i - 1].position;
            }
            this.transform.position = new Vector3(
                Mathf.Round(this.transform.position.x  + direction1.x ),
                Mathf.Round(this.transform.position.y  + direction1.y ),
                0.0f);
        }
        else if (player == SnakePlayer.Snake2)
        {
            for (int i = _segments.Count - 1; i > 0; i--)
            {
                _segments[i].position = _segments[i - 1].position;
            }
            this.transform.position = new Vector3(
                Mathf.Round(this.transform.position.x  + direction2.x ),
                Mathf.Round(this.transform.position.y  + direction2.y ),
                0.0f);
        }

    }


    private void SnakeDirection()
    {
        if (player == SnakePlayer.Snake1)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) && (direction1.x != 1) && (direction1.y == 1 || direction1.y == -1))
            {
                direction1 = Vector2Int.left;
                wallDirection = "left";
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && (direction1.x != -1) && (direction1.y == 1 || direction1.y == -1))
            {
                direction1 = Vector2Int.right;
                wallDirection = "right";
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && (direction1.y != -1) && (direction1.x == 1 || direction1.x == -1))
            {
                direction1 = Vector2Int.up;
                wallDirection = "up";
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && (direction1.y != 1) && (direction1.x == 1 || direction1.x == -1))
            {
                direction1 = Vector2Int.down;
                wallDirection = "down";
            }
            RotationEffect();
        }

        else if (player == SnakePlayer.Snake2)
        {
            if (Input.GetKeyDown(KeyCode.A) && (direction2.x != 1) && (direction2.y == 1 || direction2.y == -1))
            {
                direction2 = Vector2Int.left;
                wallDirection = "left";
            }
            else if (Input.GetKeyDown(KeyCode.D) && (direction2.x != -1) && (direction2.y == 1 || direction2.y == -1))
            {
                direction2 = Vector2Int.right;
                wallDirection = "right";
            }
            else if (Input.GetKeyDown(KeyCode.W) && (direction2.y != -1) && (direction2.x == 1 || direction2.x == -1))
            {
                direction2 = Vector2Int.up;
                wallDirection = "up";
            }
            else if (Input.GetKeyDown(KeyCode.S) && (direction2.y != 1) && (direction2.x == 1 || direction2.x == -1))
            {
                direction2 = Vector2Int.down;
                wallDirection = "down";
            }
            RotationEffect();
        }

    }
    void RotationEffect()
    {
        shieldEffect.transform.rotation = Quaternion.Euler(0, 0, GetRotationAngleFromDirection(direction1));
        scoreBoost.transform.rotation = Quaternion.Euler(0, 0, GetRotationAngleFromDirection(direction1));
        speedUp.transform.rotation = Quaternion.Euler(0, 0, GetRotationAngleFromDirection(direction1));
    }

    private float GetRotationAngleFromDirection(Vector2Int directionx)
    {
        if (directionx == Vector2Int.up)
        {
            return 90f;
        }
        else if (directionx == Vector2Int.right)
        {
            return 0f; 
        }
        else if (directionx == Vector2Int.down)
        {
            return -90f; 
        }
        else if (directionx == Vector2Int.left)
        {
            return 180f; 
        }
        return 0f;
    }
    private void WrapSnakeMovement()
    {
        float xPosition = 1;
        float yPosition = 1;
        if (wallDirection == "left" || wallDirection == "right")
        {
           xPosition = -1;
        }
        else
        {
            
            yPosition = -1;
        }

        transform.position = new Vector3(
             xPosition * (float)(Mathf.Round(transform.position.x)),
             yPosition * (float)(Mathf.Round(transform.position.y)),
             0.0f
        );
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch (collision.tag)
        {
            case "GreenFood":
               GreenFoodGrow();
                break;
            case "RedFood":
               RedFoodShrink();
                break;
            case "boundary":
                WrapSnakeMovement();
                break;
            case "Shield":
                if (player == SnakePlayer.Snake1)
                    ShieldPowerEffect(SnakePlayer.Snake1);
                else if (player == SnakePlayer.Snake2)
                    ShieldPowerEffect(SnakePlayer.Snake2);
                break;
            case "ScoreBoost":
                if (player == SnakePlayer.Snake1)
                    ScorePowerEffect(SnakePlayer.Snake1);
                else if (player == SnakePlayer.Snake2)
                    ScorePowerEffect(SnakePlayer.Snake2);
                break;
            case "SpeedUp":
                if (player == SnakePlayer.Snake1)
                    SpeedPowerEffect(SnakePlayer.Snake1);
                else if (player == SnakePlayer.Snake2)
                    SpeedPowerEffect(SnakePlayer.Snake2);
                break;
            case "obstacle":
                if (hasShield)
                 {
                 
                    hasShield = false;
                   
                 }
                else
               {
                    if (player == SnakePlayer.Snake1 && SceneManager.GetActiveScene().buildIndex==1)
                    {

                        SceneManager.LoadScene(gameOver);
                    }
                    else if(SceneManager.GetActiveScene().buildIndex == 3)
                    {
                      
                        HandleSnakeCollision();
                    }
                }
                break;
        }
    }

    
    private void GreenFoodGrow()
    {
        if (player == SnakePlayer.Snake1)
        {
            SoundManager.Instance.PlaySound(Sounds.GreenFood);
            Grow();
            DisplayPickupText("GREEN FOOD PICKED UP BY SNAKE1 !");
            UpdateScore(true, SnakePlayer.Snake1);

        }
        else if (player == SnakePlayer.Snake2)
        {
            SoundManager.Instance.PlaySound(Sounds.GreenFood);
            Grow();
            DisplayPickupText("GREEN FOOD PICKED UP BY SNAKE2 !");
            UpdateScore(true, SnakePlayer.Snake2);
        }
    }

    private void RedFoodShrink()
    {
        if (player == SnakePlayer.Snake1)
        {
          
            Shrink();
            DisplayPickupText("RED FOOD PICKED UP BY SNAKE1 !");
            UpdateScore(false, SnakePlayer.Snake1); 

        }
        else if (player == SnakePlayer.Snake2)
        {
            SoundManager.Instance.PlaySound(Sounds.RedFood);
            Shrink();
            DisplayPickupText("RED FOOD PICKED UP BY SNAKE2 !");
            UpdateScore(false, SnakePlayer.Snake2);
        }
    }

    private void DisplayPickupText(string text)
    {
        pickupText.text = text;
        pickupText.gameObject.SetActive(true);
        StartCoroutine(HidePickupText());
    }

    private IEnumerator HidePickupText()
    {
        yield return new WaitForSeconds(2f); 
        pickupText.gameObject.SetActive(false);
    }

    private void HandleSnakeCollision()
    {
        
        if (snake1Score > snake2Score)
        {
          
            UIController.instance.SnakeWin(SnakePlayer.Snake1,snake1Score);
        }
        else if (snake2Score > snake1Score)
        {
           
            
            UIController.instance.SnakeWin(SnakePlayer.Snake2,snake2Score);
        }
        
    }
    private void ShieldPowerEffect(SnakePlayer snakePlayer)
    {
       
        if (!hasShield && snakePlayer==SnakePlayer.Snake1)

        {
            hasShield = true;
            DisplayPickupText("SHIELD ACTIVATED FOR SNAKE1!");
            StartCoroutine(PowerTimerRoutine());
        }
        else if (snakePlayer == SnakePlayer.Snake2 && !hasShield)
        {
          
            hasShield = true;
            DisplayPickupText("SHIELD ACTIVATED FOR SNAKE2!");
            StartCoroutine(PowerTimerRoutine());
        }
    }

    private void SpeedPowerEffect(SnakePlayer snakePlayer)
    {
        
        if (!hasSpeedUp && !hasShield && !hasScoreboost)
        {
            hasSpeedUp = true;
            speedMultiplier = 2f;
            Time.fixedDeltaTime = 0.04f;
            if (snakePlayer == SnakePlayer.Snake1)
            {
               
                DisplayPickupText("SPEED BOOST ACTIVATED FOR SNAKE1!");
            }
            else if (snakePlayer == SnakePlayer.Snake2)

            {
               
                DisplayPickupText("SPEED BOOST ACTIVATED FOR SNAKE2!");
            }

            StartCoroutine(PowerTimerRoutine());

        }
    }

   
    private void ScorePowerEffect(SnakePlayer snakePlayer)
    {
       
        if (!hasScoreboost && !hasShield && !hasSpeedUp)
        {
            SoundManager.Instance.PlaySound(Sounds.ScoreBoost);
            hasScoreboost = true;
            scoreBoost.SetActive(true);
            if (snakePlayer == SnakePlayer.Snake1)
            {
                
                DisplayPickupText("SCORE BOOST ACTIVATED FOR SNAKE1!");
            }
            else if (snakePlayer == SnakePlayer.Snake2)
            {
               
                DisplayPickupText("SCORE BOOST ACTIVATED FOR SNAKE2!");
            }

            StartCoroutine(PowerTimerRoutine());
        }
    }

    private IEnumerator PowerTimerRoutine()
    {
        yield return new WaitForSeconds(PowerAcivated);
     
        if (hasShield)
        {
            hasShield = false;
           
        }
        if (hasSpeedUp)
        {
            hasSpeedUp = false;
            speedMultiplier = 1f;
            Time.fixedDeltaTime = 0.09f;

        }
        if (hasScoreboost)
        {
            hasScoreboost = false;
            scoreBoost.SetActive(false);
        }
    }
    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

    private void Shrink()
    {
        if (_segments.Count > initialSize)
        {
            Transform lastSegment = _segments[_segments.Count - 1];
            _segments.RemoveAt(_segments.Count - 1);
            Destroy(lastSegment.gameObject);
        }
    }



    private void ResetState()
    {
        if (snake1Score > 0 && snake2Score > 0)
        {

            if (player == SnakePlayer.Snake1)
            {
                ResetSnake();
                new Vector3(0, 2, 0);
                direction1 = Vector2Int.right;
            }
            else if (player == SnakePlayer.Snake2)
            {
                ResetSnake();
                new Vector3(0, -2, 0);
                direction2 = Vector2Int.left;
            }
        }
    }
    public void ResetSnake()
    {
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();
        _segments.Add(transform);

        for (int i = 1; i < initialSize; i++)
        {
            Transform segment = Instantiate(segmentPrefab);
            _segments.Add(segment);
        }
        transform.position = Vector3.zero;
    }

    private void UpdateScore(bool isGreenFood,SnakePlayer player)
    {
        int scoreChange = isGreenFood ? 10 : -10;
        if(hasScoreboost)
        {
            scoreChange *= 2;
        }
        if(player==SnakePlayer.Snake1)
        {
            snake1Score += scoreChange;
            UIController.instance.UpdateScore(snake1Score, SnakePlayer.Snake1);

        }
        if (player == SnakePlayer.Snake2)
        {
           snake2Score += scoreChange;
            UIController.instance.UpdateScore(snake2Score,SnakePlayer.Snake2);

        }
        
    }
    
}