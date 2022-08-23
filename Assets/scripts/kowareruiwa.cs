using UnityEngine;

public class kowareruiwa : MonoBehaviour
{
    [SerializeField]
    private GameObject BrokenStone;

    [SerializeField]
    private EnemySund SE;
    // Start is called before the first frame update
    void Start()
    {
        SE = GameObject.FindGameObjectWithTag("SEManager").GetComponent<EnemySund>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Attack3") || other.gameObject.CompareTag("SnowBall"))
        {
            Instantiate(BrokenStone, transform.position, transform.rotation);
            SE.PlayerEnemySound(3);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SnowBall"))
        {
            Instantiate(BrokenStone, transform.position, transform.rotation);
            SE.PlayerEnemySound(3);
            Destroy(this.gameObject);
        }
    }
}
