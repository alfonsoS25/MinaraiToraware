using UnityEngine;

public class MeltBlock : MonoBehaviour
{

    // Start is called before the first frame update
    private bool attacked = false;
    float equisde;

    [SerializeField]
    float MeltTime;

    float MeltPart;

    [SerializeField]
    private GameObject BrokenIce;
    [SerializeField]
    private EnemySund SE;

    [SerializeField]
    private bool IsSound = false;

    void Start()
    {
        SE = GameObject.FindGameObjectWithTag("SEManager").GetComponent<EnemySund>();
        equisde = transform.localScale.y;

        MeltTime *= 50;

        MeltPart = equisde / MeltTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(attacked&&transform.localScale.y > 0.1f)
        {
            equisde -= MeltPart;
            transform.localScale = (new Vector3(transform.localScale.x,equisde, transform.localScale.z));
            transform.Translate(0, -(MeltPart/2), 0);
        }
        else if(transform.localScale.y <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SnowBall"))
        {
            if (IsSound)
            {
                SE.PlayerEnemySound(0);
            }
            Instantiate(BrokenIce, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack2"))
        {
            Debug.Log("attacked");
            attacked = true;
        }
    }

}
