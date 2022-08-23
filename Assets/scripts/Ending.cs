using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    void Start()
    {
        StartCoroutine(EndingPhase());
    }

    IEnumerator EndingPhase()
    {
        yield return new WaitForSeconds(10);
        anim.SetTrigger("End");
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Title");
    }
}
