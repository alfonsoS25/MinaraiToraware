using UnityEngine;

public class GenerateIce : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject Ice;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Attack1"))
        {
            Instantiate(Ice,new Vector3( other.transform.position.x,transform.position.y-0.65f,other.transform.position.z), other.transform.rotation);
        }
    }
}
