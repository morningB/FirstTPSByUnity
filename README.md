> 에니매이션
> 
- Apply Root Motion : 해제 안하면 이상한 모션 가능
    
    ![스크린샷 2025-04-30 오전 11.31.45.png](attachment:4289fdee-1414-4cfa-8f32-0fafe74521d5:스크린샷_2025-04-30_오전_11.31.45.png)
    
- `PlayerMove.cs`
    
    ```csharp
    void Update()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            if (h != 0.0f || v != 0.0f)
            {
                Vector3 dir = h * Vector3.right + v* Vector3.forward;
                transform.rotation = Quaternion.LookRotation(-dir);
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
        }
    ```
    
    - if 조건 : 멈추지 않았다면
    - dir : 방향을 저장함
    - Quaternion.LookRotation(dir) : dir 방향으로 회전
    - Translate : 방향으로 이동.
- UI를 만들면 처음 설정
    - Canvas → Render Mode → Screen Space - Overlay
- Animator 설정.
    
    ![스크린샷 2025-04-30 오후 1.19.56.png](attachment:62bd09b9-cdd3-4670-8f80-db0db438259b:스크린샷_2025-04-30_오후_1.19.56.png)
    
- 삼각형 m_idle_A를 Controller에 넣기
- Player 클릭 후 Parameters에 bMove 추가
- 우클릭 후, Make Transition 하기
    
    ![스크린샷 2025-04-30 오후 1.20.37.png](attachment:92fa59c2-cbdc-4f77-a6e6-19a3438ba47e:스크린샷_2025-04-30_오후_1.20.37.png)
    
- 화살표 클릭 후, Has Exit Time 해제
    - Has Exit Time : 모든 애니메이션을 끝낸 후에 적용한다.
- Condition(조건문)에 bMove 추가하기
    
    ![스크린샷 2025-04-30 오후 1.22.43.png](attachment:3b85272d-7ad2-4930-ad5b-950ccc7c7e8b:스크린샷_2025-04-30_오후_1.22.43.png)
    
- `FollowCamera.cs`
    
    ```csharp
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
            transform.position = player.position + new Vector3(0,6,-8);
        }
    }
    
    ```
    
- FollowCamera 스크립트 개선
- 어떤 수치에서 어떤 수치로 값이 변경될 때, 한번에 바뀌지 않고 부드럽게 이동.

```
transform.position = Vector3.Lerp(transform.position,
player.position + new Vector3(0,6,-8),Time.deltaTime);
```

- Lerp(시작 위치, 목적지 위치, 0~1인 숫자)
    - 0이면 시작위치에 가깝고, 1이면 목적지에 가까움
- Enemy
    - Nav Mesh Agent
        
        ![스크린샷 2025-04-30 오후 2.12.06.png](attachment:890c5f39-a753-4bc4-b0f1-3b18f8e24a2d:스크린샷_2025-04-30_오후_2.12.06.png)
        
    - **Stopping Distance**: 1.3 (플레이어와 접촉 인식 거리)
- **배경 오브젝트(DemoObjects)**: Static 설정
    - NavMesh Surface를 추가 → Bake
    
    ![스크린샷 2025-04-30 오후 2.33.56.png](attachment:1b13c639-12d8-46f4-bcb8-9f0d21ffa85d:스크린샷_2025-04-30_오후_2.33.56.png)
    
- `EnemyMoveFollow.cs`
    
    ```csharp
    using UnityEngine;
    using UnityEngine.AI;
    
    public class EnemyMoveFollow : MonoBehaviour
    {
        Transform playerTrans;
        NavMeshAgent nav;
        
        void Start()
        {
            playerTrans = GameObject.Find("Player").transform;
            nav = GetComponent<NavMeshAgent>();
        }
    
        // Update is called once per frame
        void Update()
        {
            nav.SetDestination(playerTrans.position);
        }
    }
    
    ```
    
- FSM(finite state machine)
    - 상태를 기반으로 동작을 제어하는 방식을 구현하기 위한 디자인 패턴
    - 핵심은 단 하나의 상태만 활성화 한다.
        
        ![스크린샷 2025-04-30 오후 2.57.14.png](attachment:36e27b46-0ba1-4e06-b45e-0884df9f37dd:스크린샷_2025-04-30_오후_2.57.14.png)
        
- 애니메이션 블랜딩 : idle상태에서 walk를 갑자기 하면 부자연스러움.
    - idle 50%하다가 walk 50% 움직임을 하는 것이 자연스러움 → 블랜딩
        
        ![스크린샷 2025-04-30 오후 3.00.55.png](attachment:3a3a48c7-b928-4d67-a1b4-1c52851b7e4d:스크린샷_2025-04-30_오후_3.00.55.png)
        
    - Transition Duration : 바뀌는 시간
    
- 어떠한 상태든 죽는 것으로 함.
    
    ![스크린샷 2025-04-30 오후 3.02.24.png](attachment:2d9a2c24-ccb6-4495-b70c-92bbd36a07b7:스크린샷_2025-04-30_오후_3.02.24.png)
    
    - Any State를 안한다면 다 이어줘야됨
        
        ![스크린샷 2025-04-30 오후 3.03.26.png](attachment:fc418225-c89b-45f9-9200-9fc8d0964792:스크린샷_2025-04-30_오후_3.03.26.png)
        
- `EnemyHealth.cs` (적 사망)
    
    ```csharp
    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.LightTransport;
    
    public class EnemyHealth : MonoBehaviour
    {
        int hp = 100;
        public void Damage(int amount)
        {
            if(hp <= 0)return;
    
            hp -= amount;
            if(hp <= 0 )
            {
                GetComponent<Animator>().SetTrigger("Death");
                GetComponent<NavMeshAgent>().enabled = false;
    
                Destroy(gameObject,2);
            }
        }
    }
    
    ```
    
    - 2초 뒤에 삭제하는 이유 : Enemy의 에니매이션이나 사운드를 보기 위해
- Player에 빈Object 생성
    - 이름은 ShootPoint
- Player에 LineRenderer 추가
- `PlayerAttack.cs`
    
    ### ✅ 요점
    
    - **Raycast**로 적 히트 판정.
    - 맞으면 데미지 주고 **LineRenderer**로 레이저 발사선 표시.
    - **레이어 마스크** 사용으로 필요 없는 물체 무시.
    - 사격 후 0.05초 동안만 선 표시.
    
    ```cpp
    using System.Transactions;
    using UnityEngine;
    
    public class PlayerAttack : MonoBehaviour
    {
        float timer;
        LineRenderer line;
        Transform shootPoint;
    
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
    
        void Update()
        {
            
            if(line.enabled)
            {
                timer += Time.deltaTime;
                if(timer > 0.05f)
                {
                    line.enabled =false;
    
                }
            }
        }
    }
    
    ```
    
- pos0 ~ pos2 : 좀비 생성 위치
- `Spawn.cs` 생성 후 빈 GameManager에 할당
    - 스크립트
        
        ### ✅ 요점
        
        - **InvokeRepeating**으로 자동 소환.
        - 최대 소환 수 넘으면 중지.
        - 소환 위치 랜덤.
        
        ```csharp
        using Unity.VisualScripting;
        using UnityEngine;
        
        public class Spawn : MonoBehaviour
        {
            public GameObject prefab;
            public float time;
            public Transform[] point;
        
            public int max;
            public int count;
        
            void Start()
            {
                InvokeRepeating("Create", time, time);
            }
        
            void Create()
            {
                if(count >= max)
                {
                    return;
                }
        
                count++;
                int i = Random.Range(0, point.Length);
                Instantiate(prefab, point[i]);
            }
        }
        
        ```
        
    
    ![스크린샷 2025-05-07 오후 1.26.19.png](attachment:5a13c225-5812-4d44-a531-d4bf65c27c3f:스크린샷_2025-05-07_오후_1.26.19.png)
    
- Player animation 추가
    
    ![스크린샷 2025-05-07 오후 1.26.48.png](attachment:8376e03d-8d4c-4ea8-aad0-785ab15f451c:스크린샷_2025-05-07_오후_1.26.48.png)
    
- `EnemyAttack.cs`
    
    ### ✅ 요점
    
    - **Trigger 범위 안**에 플레이어 있으면 공격.
    - 0.5초마다 데미지.
    - 플레이어 죽으면 Death 애니메이션 실행.
    
    ```
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
    
    ```
    
- PlayerHealth에 ReSpawn 생성
    
    ### ✅ 요점
    
    - **사망 후** 3초 뒤 초기 위치로 부활.
    - 체력/HP바 초기화.
    - 이동/공격 스크립트 재활성화.
    
    ```csharp
    void Start()
        {
            posRespawn = transform.position;
                   
        }
    ...
    public void Respawn()
        {
            hp = 100;
    
            transform.position = posRespawn;
            GetComponent<Animator>().SetTrigger("Respawn");
    
            GetComponent<PlayerMove>().enabled = true;
            GetComponent<PlayerAttack>().enabled = true;
    
            imgBar.transform.localScale = new Vector3(1,1,1);
            sliderHP.value = hp;
        }
    ```
    
- 강아지 follow
    
    ### ✅ 요점
    
    - **플레이어 가까우면** 정지.
    - **중거리면** 따라옴.
    - **멀어지면** 복귀 위치(posReturn)로 돌아감.
    - NavMeshAgent로 이동.
    - **Animator의 bMove**로 움직임 애니메이션 제어.
    
    ```csharp
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
    
    ```
    
- EnemyHPBar
    
    ![스크린샷 2025-05-07 오후 1.46.17.png](attachment:93306310-946d-415d-928c-ff5aa8df99ff:스크린샷_2025-05-07_오후_1.46.17.png)
    
    - `EnemyHealth.cs`
        
        ```csharp
        using UnityEngine;
        using UnityEngine.AI;
        using UnityEngine.LightTransport;
        using UnityEngine.UI;
        
        public class EnemyHealth : MonoBehaviour
        {
            int hp = 100;
            public RawImage imgBar;
            public AudioClip clipHurt;
            public AudioClip clipDeath;
        
            public void Damage(int amount)
            {
                if(hp <= 0)return;
        
                hp -= amount;
                imgBar.transform.localScale = new Vector3(hp/ 100.0f,1,1);
        
                GetComponent<AudioSource>().PlayOneShot(clipHurt);
                if(hp <= 0 )
                {
                    GetComponent<Animator>().SetTrigger("Death");
                    GetComponent<NavMeshAgent>().enabled = false;
        
                    Destroy(gameObject,2);
                    GameObject.Find("GameManager").GetComponent<Spawn>().count--;
                    GetComponent<AudioSource>().PlayOneShot(clipDeath);
                }
            }
        }
        
        ```
        
- 현재는 좀비가 돌면 HPBar도 같이 회전
- `BillBoard.cs` 추가
    
    ```
    using UnityEngine;
    
    public class BillBoard : MonoBehaviour
    {
        
        void Update()
        {
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
        }
    }
    ```
    
    - HPBar에 컴포넌트 추가
- PlayerHPBar 만들기
    - `PlayerHealth.cs` 수정
        
        ```csharp
        using UnityEngine;
        using UnityEngine.LightTransport;
        using UnityEngine.UI;
        
        public class PlayerHealth : MonoBehaviour
        {
            public int hp = 100;
            Vector3 posRespawn;
            bool bDamage;
            public RawImage imgDamage;
            public RawImage imgBar;
            public Slider sliderHP;
            
            public AudioClip clipHurt;
            public AudioClip clipDeath;
            public void Damage(int amount)
            {
                if(hp <= 0) return;
        
                hp -= amount;
        
                bDamage = true;
                imgBar.transform.localScale = new Vector3(hp / 100.0f, 1,1);
                sliderHP.value = hp;
                
                GetComponent<AudioSource>().PlayOneShot(clipHurt);
                
                if(hp <= 0)
                {
                    GetComponent<Animator>().SetTrigger("Death");
        
                    GetComponent<PlayerMove>().enabled = false;
                    GetComponent<PlayerAttack>().enabled = false;
                    GetComponent<AudioSource>().PlayOneShot(clipDeath);
                    Invoke("Respawn", 3);
                }
            }
            public void Respawn()
            {
                hp = 100;
        
                transform.position = posRespawn;
                GetComponent<Animator>().SetTrigger("Respawn");
        
                GetComponent<PlayerMove>().enabled = true;
                GetComponent<PlayerAttack>().enabled = true;
        
                imgBar.transform.localScale = new Vector3(1,1,1);
                sliderHP.value = hp;
            }
            public void SetHP(int value)
            {
                if(value <0 || value > 100)
                return;
                hp = value;
                imgBar.transform.localScale = new Vector3(hp/100.0f,1,1);
                sliderHP.value = hp;
            }
            void Start()
            {
                posRespawn = transform.position;
                       
            }
        
            // Update is called once per frame
            void Update()
            {
                if(bDamage)
                {
                    imgDamage.color = new Color(1,0,0,1);
                }
                else
                {
                    imgDamage.color = Color.Lerp(imgDamage.color, Color.clear, 5 * Time.deltaTime);
                }
                bDamage = false;
            }
        }
        
        ```
