using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceFloor : MonoBehaviour
{
    [SerializeField]
    private float FirstDelay;
    [SerializeField]
    private float SecondDelay;
    [SerializeField]
    private float ThirdDelay;

    [SerializeField]
    private Material this_Material;

    [SerializeField]
    private Texture texturas, textura2, textura3;
    void Start()
    {
       // StartCoroutine(FirstPhase());
    }

    private IEnumerator FirstPhase()
    {
        Debug.Log("sisirve1");
        this_Material.mainTexture = texturas;
        yield return new WaitForSeconds(FirstDelay);
        StartCoroutine(SecondPhase());
    }
    private IEnumerator SecondPhase()
    {
        Debug.Log("sisirve2");
        this_Material.mainTexture= textura2;
        yield return new WaitForSeconds(SecondDelay);
        StartCoroutine(ThirdPhase());
    }
    private IEnumerator ThirdPhase()
    {
        Debug.Log("sisirve3");
        this_Material.mainTexture = textura3;
        yield return new WaitForSeconds(ThirdDelay);
    }
    
}
