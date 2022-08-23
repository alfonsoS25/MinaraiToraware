using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class Pause : MonoBehaviour
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
    private Transform uiRoot;

    [SerializeField]
    private GameObject OptionsGamen;

    [SerializeField]
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        MoveButton(0);
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
        if (UpDown > 0)
        {
            buttonIndex--;
        }
        else if (UpDown < 0)
        {
            buttonIndex++;
        }
        if(buttonIndex <=-1)
        {
            buttonIndex = 2;
        }
        if(buttonIndex >=3)
        {
            buttonIndex = 0;
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

    public void AbleToStop()
    {
        gameManager.isOnOptions = false;
    }

    public void SumbitButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(gameManager.IsPaused)MainMenuSwitch();
        }
    }

    public void MainMenuSwitch()
    {
        switch (buttonIndex)
        {
            case 0:
                Time.timeScale = 1;
                resume();
                break;

            case 1:
                Option();
                break;
                 
            case 2:
                Time.timeScale = 1;
                BackToTitle();
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
        gameManager.isOnOptions = true;
        Instantiate(OptionsGamen, transform.position, transform.rotation, uiRoot);
    }

    public void BackToTitle()
    {
       SceneManager.LoadScene("Title");
    }
}
