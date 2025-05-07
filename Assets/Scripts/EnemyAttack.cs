using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    GameObject player;
    float time;
    bool bInRange;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            bInRange = true;
        }     
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject == player)
        {
            bInRange = false;
        }     
    }
    void Update()
    {
        time += Time.deltaTime;
        if(time >= 0.5f && bInRange)
        {
            time = 0;
            player.GetComponent<PlayerHealth>().Damage(50);
            if(player.GetComponent<PlayerHealth>().hp <= 0)
            {
                GetComponent<Animator>().SetTrigger("PlayerDeath");
            }
        }
    }
}
