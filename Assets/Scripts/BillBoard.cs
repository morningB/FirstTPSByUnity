using UnityEngine;

public class BillBoard : MonoBehaviour
{
    
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
