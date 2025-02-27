using System.Collections;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinTracker;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI fin;
    public Camera cam;
    public Transform debugCubeTransform;
    public GameObject player;
    public Transform spawnLocation;
    private int coins;
    private int score;

    private bool done;  //stop update from repeating
    private bool gameOver; //lose
    public bool winner;    //win
    private int doneTime;

    public float tempTime;

    private LevelParser levelParser;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coins = 0;
        score = 0;
        gameOver = false;
        winner = false;
        done = false;
        fin.enabled = false;
        tempTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        int timeLeft= 100-(int)(Time.time- tempTime);

        if (timeLeft > 0 && !gameOver && !winner)//alive n not dead
        {
            timerText.text = $"Time:{timeLeft}";
        }
        else if (timeLeft<=0|| gameOver)//outta time or died
        {
            
            fin.enabled = true;
            if (!done)
            {
                //disable player
                player.SetActive(false);
                Debug.Log("GameOver!");
                if (gameOver) 
                {
                    doneTime = timeLeft;
                    fin.text = "Game Over!\nPress 'R' to restart";

                }
                else
                {
                    fin.text = "Outta time! Game over.\nPress 'R' to restart";
                }

                gameOver = true;
                done = true;
                timerText.text = $"Time:{timeLeft}";

            }


        }
        else if (winner) //ya won
        {
            if (!done)
            {

                doneTime=timeLeft ;
                done = true ;
                fin.text = "Winner winner chicken dinner!\nPress 'R' to restart";
                fin.enabled = true;
            }
            
            timerText.text = $"Time:{doneTime}";
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
            if (rayHitSomething && hitInfo.transform.gameObject.CompareTag("Question"))
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
            //if (rayHitSomething && hitInfo.transform.gameObject.CompareTag("Coin")) //edited for CoinRules
            //{
            //    coins++;
            //    Debug.Log("ding!");
            //    coinTracker.text = $"\nx{coins}";
            //    Destroy(hitInfo.transform.gameObject);
            //    updateScore();

            //}
            //if (rayHitSomething && hitInfo.transform.gameObject.CompareTag("Goal")) //moved to GoalRules
            //{
            //    winner = true;
            //    player.SetActive(false);
            //    Debug.Log("Yippie!");
            //}


        }

        if(Input.GetKeyDown(KeyCode.R) && !player.activeSelf)
        {
            resetLevel();
            tempTime = Time.time;
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


    public void updateScore()
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
        winner = false;
        done = false;
        fin.enabled = false;

    }

    public void updateCoin(GameObject thisOne)
    {
        coins++;
        Debug.Log("ding!");
        coinTracker.text = $"\nx{coins}";
        Destroy(thisOne);
        updateScore();
    }
    public void breakBrick(GameObject obj)
    {
        //should have a paramter that is passed to destroy correct object
       
            Debug.Log("ouch!");
            obj.SetActive(false);
            updateScore();
        

    }
    public void exhaustQuestion(Transform xx)
    {
        //should have a paramter that is passed to destroy correct object
        //use same info for animation
        coins++;
        Debug.Log("ding!");
        StartCoroutine(questionAnimation(xx.transform));
        coinTracker.text = $"\nx{coins}";
        updateScore();
    }
    public void setGameOver()
    {
        player.SetActive(false);
        gameOver = true;

    }

}
