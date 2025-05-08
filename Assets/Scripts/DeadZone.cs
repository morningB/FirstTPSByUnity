using UnityEngine;

public class DeadZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerHealth>().Respawn();
                
            }
        }
}
