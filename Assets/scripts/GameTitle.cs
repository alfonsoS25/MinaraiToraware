using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameTitle : MonoBehaviour
{
    [SerializeField]
    private string TitleName;

    public void GoingTitle()
    {
        SceneManager.LoadScene(TitleName);
    }
    void Update()
    {
        InputSystem.Update();
    }
}
