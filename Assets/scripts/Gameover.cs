using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    [SerializeField]
    private RectTransform[] buttons = null;
    // 現在選択されているボタンID
    public int buttonIndex = 0;

    [SerializeField]
    private int MenuNum;

    [SerializeField]
    private GameObject OptionsUI;

    [SerializeField]
    private Transform uiRoot;

    [SerializeField]
    private Transform Sankakkei = null;
    [SerializeField]
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {   
        MoveButton(0);
        Debug.Log(MenuNum);
        gameManager = GameObject.FindGameObjectWithTag("GameController").gameObject.GetComponent<GameManager>();
        uiRoot = GameObject.FindGameObjectWithTag("UIRoot").gameObject.GetComponent<Transform>();
        transform.SetParent(uiRoot);

        for (int index = 0; index < buttons.Length; index++)
        {
            // 選択状態
            if (index == buttonIndex)
            {
                buttons[index].localScale = new Vector3(1, 1, 1);
            }
            // 非選択状態
            else
            {
                buttons[index].localScale = new Vector3(0.95f, 0.95f, 1);
            }
        }
    }

    public void MoveButton(float UpDown)
    {
        if (UpDown > 0 && buttonIndex > 0)
        {
            buttonIndex--;
        }
        else if (UpDown < 0 && buttonIndex < buttons.Length - 1)
        {
            buttonIndex++;
        }

        // ボタンの選択状態を更新
        for (int index = 0; index < buttons.Length; index++)
        {
            // 選択状態
            if (index == buttonIndex)
            {
                buttons[index].localScale = new Vector3(1, 1, 1);
            }
            // 非選択状態
            else
            {
                buttons[index].localScale = new Vector3(0.95f, 0.95f, 1);
            }
        }
        switch (buttonIndex)
        {
            case 0: Sankakkei.transform.localPosition = new Vector3(-700, -344,0);
                break;

            case 1: Sankakkei.transform.localPosition = new Vector3(183, -344, 0);
                break;
        }
    }

    public void SumbitButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (buttonIndex)
            {
                case 0:RetryScene();
                    break;

                case 1:BackToTitle();
                    break;
            }
        }
    }

    private void MainMenuSwitch()
    {
        switch (buttonIndex)
        {
            case 0:
                resume();
                break;

            case 1:
                break;

            case 2:
                BackToTitle();
                break;
        }
    }
    private void GameOverSwitch()
    {
        switch (buttonIndex)
        {
            case 0:RetryScene();
                break;
            case 1:BackToTitle();
                break;
        }
    }


    public void resume()
    {
        gameManager.Pause();
    }
    public void RetryScene()
    {
        SceneManager.LoadScene("Main");
    }
        public void Option()
    {
        Instantiate(OptionsUI, transform.position, transform.rotation, uiRoot);
        Destroy(this.gameObject);
    }

    public void BackToTitle()
    {
       SceneManager.LoadScene("Title");
    }
}
