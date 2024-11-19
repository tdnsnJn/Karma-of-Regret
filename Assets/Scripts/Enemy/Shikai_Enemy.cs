using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class Shikai_Enemy : MonoBehaviour
{
    [SerializeField]
    private SphereCollider Search;
    [SerializeField]
    private float searchAnge = 90f;
    [SerializeField]
    private float searchDistance = 10f;
    [SerializeField]
    private float radius = 2f;
    public Transform EnemySt;
    NavMeshAgent nav;
    private Animator anim;

    // == ターゲット設定
    private Transform target;           // 追跡ターゲット
    private Vector3   targetDirection;  // ターゲットの向きベクトル
    private float     targetDistance;   // ターゲットの距離
    private float     targetAngle;      // ターゲットの角度

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        Search.radius = radius;
    }

    // Update is called once per frame
    void Update()
    {
    }

    
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log($"Hits");

            target = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {

        }
    }
    
    public void Reset()
    {
        transform.position = EnemySt.position;
    }
#if UNITY_EDITOR
    //角度表示
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAnge, 0f) * transform.forward, searchAnge * 2f, Search.radius);
    }
#endif

    public void ChaseMovement( Transform target )
    {
        //主人公の方向
        targetDirection = target.position - transform.position;
        //敵の前方からの方向
        targetDirection.y = 0;
        targetAngle = Vector3.Angle(transform.forward, targetDirection);
        targetDistance = Vector3.Distance(target.position, this.transform.position);

        //Debug.Log($"サーチ >> { playerDirection }, { angle }, { searchAnge }, { angle <= searchAnge }, { other.name }");
        //サーチする角度内だったら発見
        if (targetAngle <= searchAnge && targetDistance < searchDistance)
        {
            //Debug.Log("発見" + angle);
            nav.speed = 7.0f;
            anim.SetBool("isRunning", true);
            BGM_manager bgm = GameObject.Find("BGM").GetComponent<BGM_manager>();
            bgm.PlayBGM(1);

        }
        else
        {
            nav.speed = 2.5f;
            anim.SetBool("isRunning", false);
            BGM_manager bgm = GameObject.Find("BGM").GetComponent<BGM_manager>();
            bgm.PlayBGM(0);
        }
    }
}
