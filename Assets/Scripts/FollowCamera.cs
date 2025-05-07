using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform player;
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {   
        transform.position = Vector3.Lerp(transform.position,
        player.position + new Vector3(0,6,-8),Time.deltaTime);
        // transform.position = player.position + new Vector3(0,6,-8);
    }
}
