using UnityEngine;

public class GoalRules : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameManager gm;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.gameObject.name); // Check what is triggering the event

    
            Debug.Log("Goal hit!"); // Check if it's the goal
            gm.winner = true;
            gm.player.SetActive(false);
            Debug.Log("Yippie!");
        
    }

}
