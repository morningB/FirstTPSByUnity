using UnityEngine;
using UnityEngine.SceneManagement;

public class MovetoScene : MonoBehaviour
{
    
    public void BossScene()
    {
        SceneManager.LoadScene("Boss");
    }
    // Update is called once per frame
    void Update()
    {
        if(Score.score == 100)
            BossScene();

    }
}
