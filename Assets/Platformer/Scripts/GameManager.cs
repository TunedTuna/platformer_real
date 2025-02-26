using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinTracker;
    public TextMeshProUGUI scoreText;
    public Camera cam;
    public Transform debugCubeTransform;
    public GameObject player;
    public Transform spawnLocation;
    private int coins;
    private int score;
    private bool gameOver;

    private LevelParser levelParser;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coins = 0;
        score = 0;
        gameOver = false;

    }

    // Update is called once per frame
    void Update()
    {
        int timeLeft= 100-(int)(Time.time);

        if(timeLeft > 0)
        {
            timerText.text = $"Time:{timeLeft}";
        }
        else
        {
            timerText.text = $"Time:000";
            if (!gameOver)
            {
                //disable player
                player.SetActive(false);
                Debug.Log("GameOver!");
                gameOver = true;
            }


        }
      

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            Ray cursorRay = cam.ScreenPointToRay(screenPos);

            bool rayHitSomething = Physics.Raycast(cursorRay,out RaycastHit hitInfo);

            if (rayHitSomething && hitInfo.transform.gameObject.CompareTag("Brick")) 
            {
                Debug.Log("ouch!");
                Destroy(hitInfo.transform.gameObject);
                updateScore();
                

            }
            if(rayHitSomething && hitInfo.transform.gameObject.CompareTag("Question"))
            {
                coins++;
                Debug.Log("ding!");
                StartCoroutine(questionAnimation(hitInfo.transform));
                coinTracker.text = $"\nx{coins}";
                updateScore();
            }
            if(rayHitSomething && hitInfo.transform.gameObject.CompareTag("Water"))
            {
                player.SetActive(false);
                Debug.Log("Oops!");
                gameOver = true;
            }

        }

        if(Input.GetKeyDown(KeyCode.R) && !player.activeSelf)
        {
            resetLevel();
        }
    }

    IEnumerator questionAnimation(Transform questionObj)
    {
        Vector3 originalPosition = questionObj.position;
        float moveAmount = 0.1f;
        float moveSpeed = 0.01f; 
        for (int i = 0; i < 5; i++)
        {
            questionObj.position += new Vector3(0, moveAmount, 0);
            yield return new WaitForSeconds(moveSpeed);
        }

        
        for (int i = 0; i < 5; i++)
        {
            questionObj.position -= new Vector3(0, moveAmount, 0);
            yield return new WaitForSeconds(moveSpeed);
        }

        
        questionObj.position = originalPosition;
    }

    void updateScore()
    {
        score += 100;
        string tempScore = score.ToString("D8");
        scoreText.text = $"Score\n{tempScore}";
    }

    void resetLevel()
    {
        levelParser = FindFirstObjectByType<LevelParser>();
        player.transform.position = levelParser.spawnLoc.transform.position;
        player.SetActive(true);
        score = 0;
        coins = 0;

        string tempScore = score.ToString("D8");
        scoreText.text = $"Score\n{tempScore}";
        coinTracker.text = $"\nx{coins}";
        gameOver = false;


    }
}
