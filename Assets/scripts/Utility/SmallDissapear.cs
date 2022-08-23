using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallDissapear : MonoBehaviour
{
    private Vector3 Size = Vector3.one;

     bool BecomingSmaller = false;
    void Start()
    {
        Size = this.gameObject.transform.localScale;    //gets his objects, size
        StartCoroutine(Smaller());                      //sets a 3 second timer to start getting smaller
    }

    // Update is called once per frame
    void Update()               
    {
        if(BecomingSmaller ==true)      //after 3 secodns start making the object 0.05 times smaller
        {
            Size *= 0.95f;
            this.transform.localScale = Size;                       
            if(Size.x < 0.1f && Size.y < 0.1f && Size.z < 0.1f)     //when the object size is smaller than 0.1, destory it
            {
                Destroy(gameObject);    
            }
        }
    }

    private IEnumerator Smaller()
    {
        yield return new WaitForSeconds(3);
        BecomingSmaller = true;
    }
}
