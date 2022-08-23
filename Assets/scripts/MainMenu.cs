using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private RectTransform[] buttons = null;
    // 現在選択されているボタンID
    public int buttonIndex = 0;

    [SerializeField]
    private int MenuNum;

    [SerializeField]
    private Transform Sankakkei = null;

    [SerializeField]
    private GameObject OptionsUI;

    [SerializeField]
    private Transform uiRoot;

    [SerializeField]
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        MoveButton(0);
        Debug.Log(MenuNum);
        gameManager = GameObject.FindGameObjectWithTag("GameController").gameObject.GetComponent<GameManager>();
        uiRoot = GameObject.FindGameObjectWithTag("UiRoot").gameObject.GetComponent<Transform>();
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
    private void Update()
    {
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
            case 0:
                Sankakkei.transform.localPosition = new Vector3(-244, -7, 0);
                break;

            case 1:
                Sankakkei.transform.localPosition = new Vector3(-244, -155, 0);
                break;

            case 2:
                Sankakkei.transform.localPosition = new Vector3(-244, -323, 0);
                break;
        }
    }

    public void SumbitButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (gameManager.IsPaused) MainMenuSwitch();
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
        Time.timeScale = 1;
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

