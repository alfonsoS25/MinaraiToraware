using System.Collections;
using UnityEngine;

public class IceSpikes : MonoBehaviour
{
    [SerializeField]
    private LayerMask Player;
    [SerializeField]
    private LayerMask Tsurara;

    private Rigidbody rigid;

    [SerializeField]
    private float frozetime;

    private bool freezeable = false;


    bool Ischasing = false;

    [SerializeField]
    private GameObject BrokenIce;
    
    [SerializeField]
    private GameObject ActovatorObj;

    [SerializeField]
    private EnemySund SE;
    // Start is called before the first frame update
    void Start()
    {
        SE = GameObject.FindGameObjectWithTag("SEManager").GetComponent<EnemySund>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y-0.5f, transform.position.z-1f), Vector3.down * 20f, Color.red);
        Debug.DrawRay(new Vector3(transform.position.x-(transform.localScale.x/4), transform.position.y-0.5f, transform.position.z-1f), Vector3.down * 20f, Color.red);
        Debug.DrawRay(new Vector3(transform.position.x+ (transform.localScale.x / 4), transform.position.y-0.5f, transform.position.z-1f), Vector3.down * 20f, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z - 1f), Vector3.down * 20, out hit, 20f,Player) && !Ischasing)
        {
            Activate();
        }
        if (Physics.Raycast(new Vector3(transform.position.x - (transform.localScale.x / 4), transform.position.y - 0.5f, transform.position.z - 1f), Vector3.down * 20, out hit, 20f,Player) && !Ischasing)
        {
            Activate();
        }
        if (Physics.Raycast(new Vector3(transform.position.x+(transform.localScale.x / 4), transform.position.y - 0.5f, transform.position.z - 1f), Vector3.down * 20, out hit, 20f,Player) && !Ischasing)
        {
            Activate();
        }
        if(Ischasing)
        {
            rigid.AddForce(0, -10, 0);
        }
        if(freezeable == true)
        {
            frozetime += Time.deltaTime;
            if(frozetime > 1)
            {
                rigid.isKinematic = true;
                freezeable = false;
            }
            if(rigid.velocity != new Vector3(0,0,0))
            {
                frozetime = 0;
            }
        }
    }
    public void Activate()
    {
        SE.PlayerEnemySound(2);
        Ischasing = true;
        rigid.isKinematic = false;
        ActovatorObj.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("awebo");
            StartCoroutine(FreezeRig());
            this.tag = "Untagged";
        }
        if (collision.gameObject.CompareTag("SnowBall"))
        {
            SE.PlayerEnemySound(0);
            Instantiate(BrokenIce, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Activator"))
        {
            Activate();
        }
    }

    IEnumerator FreezeRig()
    {
        yield return new WaitForSeconds(1);
        freezeable = true;
    }
}

