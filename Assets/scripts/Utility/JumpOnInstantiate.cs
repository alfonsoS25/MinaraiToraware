using UnityEngine;

public class JumpOnInstantiate : MonoBehaviour
{
    private Rigidbody rigid;

    [Range(0, 250)] [SerializeField]
    private float ForceMinY;

    [Range(250, 500)] [SerializeField]
    private float ForceMaxY;



    void Start()
    {
        rigid = GetComponent<Rigidbody>();
         rigid.AddForce(0, Random.Range(ForceMinY, ForceMaxY),0);
    }

    //this script makes an object jump ith the attached force when its instantiated
}
