using UnityEngine;

public class DestroyInSeconds : MonoBehaviour
{[SerializeField]
    private float Seconds = 3;

    void Start()
    {
        Destroy(gameObject, Seconds);
    }

}
