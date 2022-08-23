using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Options : MonoBehaviour
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
    private GameObject PauseSC;

    [SerializeField]
    private GameObject[] things;

    [SerializeField]
    private GameObject setsumei;

    bool Onsettsumei = false;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private RectTransform BGMManager;
    [SerializeField]
    private RectTransform SEManager;

    static float BGMSizeOPtion = 10;

    static float SESizeOption = 10;

    public AudioMixer mixer;

    void Start()
    {
        var distance = 0;
        var distance2 = 0;

        for (int i = 0; i < BGMSizeOPtion; i++)
        {
            distance += 90;
        }
        BGMManager.transform.position = new Vector3(690 + distance, BGMManager.transform.position.y, BGMManager.transform.position.z);


        PauseSC = GameObject.FindGameObjectWithTag("Pause").gameObject;
        PauseSC.SetActive(false);
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.Options = this.gameObject.GetComponent<Options>();
        player.IsOnOptions = true;
        player.IsPaused = false;

        for (int i = 0; i < SESizeOption; i++)
        {
            distance2 += 90;
        }
        SEManager.transform.position = new Vector3(600 + distance, SEManager.transform.position.y, SEManager.transform.position.z);


        gameManager = GameObject.FindGameObjectWithTag("GameController").gameObject.GetComponent<GameManager>();

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

        var distance3 = 0;

        for (int i = 0; i < SESizeOption; i++)
        {
            distance3 += 90;
        }
        SEManager.transform.position = new Vector3(690 + distance3, SEManager.transform.position.y, SEManager.transform.position.z);

    }
    private void FixedUpdate()
    {
        Time.timeScale = 0;
    }

    public void MoveButton(float UpDown,float LeftRight)
    {
        if (UpDown > 0)
        {
            buttonIndex--;
        }
        else if (UpDown < 0 )
        {
            buttonIndex++;
        }

        if (buttonIndex <= -1)
        {
            buttonIndex = 3;
        }
        if (buttonIndex >= 4)
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
                Sankakkei.transform.localPosition = new Vector3(-690, 55, 0);
                if (LeftRight > 0.5f && BGMSizeOPtion < 9.9f)
                {
                    var distance = 0;
                    BGMSizeOPtion++;
                    for (int i = 0; i < BGMSizeOPtion; i++)
                    {
                        distance += 90;
                    }

                    BGMManager.transform.position = new Vector3(690 + distance, BGMManager.transform.position.y, BGMManager.transform.position.z);
                }
                else if (LeftRight < -0.5f && BGMSizeOPtion > 0.9f)
                {
                    {
                        var distance = 0;
                        BGMSizeOPtion--;
                        for (int i = 0; i < BGMSizeOPtion; i++)
                        {
                            distance += 90;
                        }
                        BGMManager.transform.position = new Vector3(690 + distance, BGMManager.transform.position.y, BGMManager.transform.position.z);
                    }
                }
                if(BGMSizeOPtion==0)
                {
                    BGMSizeOPtion = 0.00001f;
                }
                if(BGMSizeOPtion== 9.00001f)
                {
                    BGMSizeOPtion = 9;
                }
                mixer.SetFloat("BGM", Mathf.Log(BGMSizeOPtion/10)*20);

                break;

            case 1:
                Sankakkei.transform.localPosition = new Vector3(-677, -86, 0);
                if (LeftRight > 0.5f && SESizeOption < 10)
                {
                    SESizeOption++;
                    var distance = 0;
                    for (int i = 0; i < SESizeOption; i++)
                    {
                        distance += 90;
                    }
                    SEManager.transform.position = new Vector3(690 + distance, SEManager.transform.position.y, SEManager.transform.position.z);
                }
                else if (LeftRight < -0.5f && SESizeOption > 0)
                {
                    {
                        var distance = 0;
                        SESizeOption--;
                        for (int i = 0; i < SESizeOption; i++)
                        {
                            distance += 90;
                        }
                        SEManager.transform.position = new Vector3(690 + distance, SEManager.transform.position.y, SEManager.transform.position.z);
                    }
                }
                if (SESizeOption == 0)
                {
                    SESizeOption = 0.00001f;
                }
                if (SESizeOption == 9.00001f)
                {
                    SESizeOption = 9;
                }
                mixer.SetFloat("SE", Mathf.Log(SESizeOption / 10) * 20);
                gameManager.UpdateVolumeSE(SESizeOption);
                break; 

            case 2:
                Sankakkei.transform.localPosition = new Vector3(-300, -239, 0);
                break;
            case 3:
                Sankakkei.transform.localPosition = new Vector3(-190, -362, 0);
                break;
        }

    }

    public void SumbitButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!Onsettsumei)
            {
                if (gameManager.IsPaused) MainMenuSwitch();
            }
            else
            {
                foreach (GameObject objectToOff in things)
                {
                    objectToOff.SetActive(true);
                }
                Onsettsumei = false;
                setsumei.SetActive(false);

            }
        }
    }

    public void MainMenuSwitch()
    {
        switch (buttonIndex)
        {
            case 2:
                foreach(GameObject objectToOff in things)
                {
                    objectToOff.SetActive(false);
                }
                Onsettsumei = true;
                setsumei.SetActive(true);
                break;

            case 3:
                var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                player.Options = null;
                player.IsOnOptions = false;
                player.IsPaused = true;
                PauseSC.SetActive(true);
                PauseSC.GetComponent<Pause>().AbleToStop();
                this.gameObject.SetActive(false);
                break;
        }
    }
}
