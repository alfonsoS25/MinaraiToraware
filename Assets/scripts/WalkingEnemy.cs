using System.Collections;
using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{

    [Header("Enemy Especifications")]

    [SerializeField]
    private float WalkingTime;

    [SerializeField]
    private float DistanceX;

    [SerializeField]
    private float DistanceY;

    [SerializeField]
    private float ChaseSpeed;

    [SerializeField]
    private GameObject BrokenState;

    [SerializeField]
    private GameObject SlimePrefab;

    [SerializeField]
    private GameObject Ice;

    [SerializeField]
    private GameObject Player;

    private Rigidbody Rigid;

    bool IsFrozen = false;

    [SerializeField]
    LayerMask Ground;

    [SerializeField]
    private bool IsRotable = true;
    
    [SerializeField]
    LayerMask IceGround;

    [SerializeField] private bool IsIntelligent = false;

    [SerializeField] private float TrackingDistance;

    [SerializeField]
    private bool isHit = false;


    [SerializeField]
    private EnemySund SE;

    [SerializeField]
    private bool IsFrozenened = false;

    [SerializeField]
    private bool IsBoss=false;

    [SerializeField]
    private GameObject Detector;

    void Start()
    {
        SE = GameObject.FindGameObjectWithTag("SEManager").GetComponent<EnemySund>();
        if(IsFrozenened)
        {
            this.gameObject.tag = "Untagged";
            Ice.SetActive(true);
            IsFrozen = true;
        }
        Rigid = GetComponent<Rigidbody>();
        if (IsIntelligent)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (IsIntelligent && !isHit)
        {
            Debug.DrawRay(new Vector3(Detector.transform.position.x, Detector.transform.position.y, Detector.transform.position.z), Vector3.down);
            if (Player.transform.position.x > transform.position.x - TrackingDistance && Player.transform.position.x < transform.position.x + TrackingDistance &&
                    Player.transform.position.z > transform.position.z - TrackingDistance && Player.transform.position.z < transform.position.z + TrackingDistance)
            {
                if (!IsFrozen)
                {
                    if (IsRotable)
                    {
                        transform.LookAt(Player.transform.position);
                    }
                }
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(Detector.transform.position.x, Detector.transform.position.y, Detector.transform.position.z), Vector3.down, out hit, 5f, Ground))
                {
                    Vector3 MovePos = Vector3.MoveTowards(transform.position, Player.transform.position, ChaseSpeed);
                    Rigid.MovePosition(MovePos);
                }
            }

        }
        if(isHit && !IsFrozenened)
        {
            Rigid.velocity = new Vector3(0, Rigid.velocity.y, 0);
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack1") && IsFrozen == false)        //if gets the player attack ice makes frozen sound and sets on the ice this enemy and frozen the rigidbody
        {
            SE.PlayerEnemySound(1);             
            this.gameObject.tag = "Untagged";
            Ice.SetActive(true);
            IsFrozen = true;
            Rigid.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }

        else if (other.gameObject.CompareTag("Attack2") && IsFrozen == true)     //  if the object is frozen, unfrozen the rigidbod ,the enemy and melts the ice
        {

            Ice.SetActive(false);
            IsFrozen = false;
            Rigid.constraints = RigidbodyConstraints.None;
            isHit = false;
            IsFrozenened = false;
            this.gameObject.tag ="Enemy";
            // StartCoroutine(CheckFrozen());
        }

        else if (other.gameObject.CompareTag("Attack3") && IsFrozen == true)       
        {
            SE.PlayerEnemySound(0);     
               isHit = true;
            var ice = Instantiate(BrokenState, transform.position,Quaternion.identity);     
            ice.transform.localScale = transform.localScale;
            Ice.SetActive(false);
            SlimePrefab.SetActive(false);
            if (!IsBoss)
            {
                this.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
            }
            else
            {
                this.gameObject.GetComponent<BoxCollider>().isTrigger = true;

            }
            Destroy(this.gameObject, 3);
        }
        else if(other.gameObject.CompareTag("Attack3") && IsFrozen == false)
        {
            var force = 500;
            Rigid.AddForce((Vector3.forward * (force*40)) + (Vector3.up * Rigid.mass * force/2));
        }
    }
}


