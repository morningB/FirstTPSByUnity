using System.Transactions;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    float timer;
    LineRenderer line;
    Transform shootPoint;
    public AudioClip clipGunShot;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        shootPoint = transform.Find("ShootPoint");
    }
    public void Fire()
    {
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        RaycastHit hit;

        timer = 0;
        

        if(Physics.Raycast(ray,out hit, 100, LayerMask.GetMask("Shootable")))
        {
            EnemyHealth e = hit.collider.GetComponent<EnemyHealth>();
            if(e != null)
            {
                e.Damage(50);
                Score.score += 10;
            }

                line.enabled = true;
                line.SetPosition(0, shootPoint.position);
                line.SetPosition(1,hit.point);
        }
        else
            {
                line.enabled = true;
                line.SetPosition(0, shootPoint.position);
                line.SetPosition(1,shootPoint.position + ray.direction * 100);
            }
    }
    // Update is called once per frame
    void Update()
    {
        
        if(line.enabled)
        {
            timer += Time.deltaTime;
            if(timer > 0.05f)
            {
                line.enabled =false;
                GetComponent<AudioSource>().PlayOneShot(clipGunShot);
                GetComponent<Light>().enabled = true;
            }
        }
        
        GetComponent<Light>().enabled = false;
    }
}
