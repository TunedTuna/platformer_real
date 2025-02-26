using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject player;
    private Vector3 offset;
    private float fixedY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = transform.position - player.transform.position;
        fixedY = transform.position.y;
    }

    // Update is called once per frame
    void LateUpdate()//takes place after all other scripts, in event scripts move player obj
    {
        transform.position = new Vector3(player.transform.position.x + offset.x, fixedY, player.transform.position.z + offset.z);
    }
}
