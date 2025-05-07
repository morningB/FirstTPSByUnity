using UnityEngine;
using UnityEngine.AI;

public class DogMoveFollowReturn : MonoBehaviour
{
    Transform player;
    NavMeshAgent nav;
    Vector3 posReturn;

    public float maxDistance = 6;
    public float minDistance = 2;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        nav = GetComponent<NavMeshAgent>();

        posReturn = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if(dist > maxDistance)
        {
            if(Vector3.Distance(transform.position, posReturn) > 1)
            {
                GetComponent<Animator>().SetBool("bMove", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("bMove", false);
            }
        }
        else if (dist > minDistance)
        {
            nav.SetDestination(player.position);
            GetComponent<Animator>().SetBool("bMove", true);
        }
        else
        {
            nav.SetDestination(transform.position);
            GetComponent<Animator>().SetBool("bMove", false);
        }
        
    }
}
