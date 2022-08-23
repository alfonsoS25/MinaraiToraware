using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform UI;

    [SerializeField]
    private GameObject PauseGamen;

    private float fixedDeltaTime;

    [Header("Checkers")]

    [SerializeField]
    public bool IsPaused = false;

    [SerializeField]
    public bool IsCompleted = false;

    [SerializeField]
    public bool isOnOptions = false;

    [SerializeField]
    public bool IsGameOver = false;

    [SerializeField]
    private Player player;

    [SerializeField]
    private StageScene stsc;

    [SerializeField]
    private StageScene stage;

    [SerializeField]
    private Animator fader;

    [SerializeField]
    private AudioSource[] BGM;

    [SerializeField]
    private AudioSource[] SE;

    [SerializeField]
    private AudioSource[] enemiesSE;

    static float GameManagerBGM=10;
    static float GameManagerSE=10;

    [SerializeField]
    private bool IsOnTitle = false;


    public AudioMixer mixer;
    private void Start()
    {
        if (!IsOnTitle)
        {
            Time.timeScale = 1;

            EnemySund enemyse = GameObject.FindGameObjectWithTag("SEManager").GetComponent<EnemySund>();


            BGM[0] = GameObject.FindGameObjectWithTag("BGM1").GetComponent<AudioSource>();
            BGM[1] = GameObject.FindGameObjectWithTag("BGM2").GetComponent<AudioSource>();
            BGM[2] = GameObject.FindGameObjectWithTag("BGM3").GetComponent<AudioSource>();

            int t = 0;
            foreach (AudioSource audio in enemyse.EnemySounds)
            {
                enemiesSE[t] = enemyse.EnemySounds[t];
                t++;
            }
            t = 0;
            foreach (AudioSource audio in player.PlayerSE)
            {
                if (t < 6)
                {
                    SE[t] = player.PlayerSE[t];
                }
                t++;
            }

            foreach (AudioSource audio in SE)
            {
                audio.volume = GameManagerSE / 10;
            }
            foreach (AudioSource audio in enemiesSE)
            {
                audio.volume = GameManagerSE / 10;
            }

            for (int f = 0; f < BGM.Length; f++)
            {

                BGM[f].volume = GameManagerBGM / 10;
            }
        }
        UpdateVolumeBGM(GameManagerSE);
    }

    public void UpdateVolumeBGM(float Add)
    {
        GameManagerBGM = Add;
        for (int f = 0; f < BGM.Length; f++)
        {
            BGM[f].volume = GameManagerBGM / 10;
        }
    }

    public void UpdateVolumeSE(float Add)
    {
        GameManagerSE = Add;
        foreach (AudioSource audio in SE)
        {
            audio.volume = GameManagerSE / 10;
        }
        foreach (AudioSource audio in enemiesSE)
        {
            audio.volume = GameManagerSE / 10;
            //Debug.Log(audio.volume);
        }
    }

    public void Resume()
    {
        if (!IsOnTitle)
        {
            Destroy(gameObject);
        }
    }

    public void Pause()
    {
        if (!IsOnTitle)
        {
            if (!isOnOptions && !IsCompleted)
            {

                if (!IsCompleted && !IsGameOver)
                {
                    if (IsPaused)
                    {
                        IsPaused = false;
                        PauseGamen.SetActive(false);
                        player.IsPaused = false;
                        Time.timeScale = 1;

                    }
                    else
                    {
                        if (player.IsOnRoullete)
                        {

                            var Clone = GameObject.FindGameObjectWithTag("PowerGage");
                            player.PlayerSE[2].Stop();

                            Destroy(Clone);
                        }
                        IsPaused = true;
                        PauseGamen.SetActive(true);
                        player.IsPaused = true;
                        Time.timeScale = 0;
                    }
                }

                //    Destroy(GameObject.FindGameObjectWithTag("Options").gameObject);
                isOnOptions = false;
            }
        }
    }

    public void Complete()
    {
        if (!IsOnTitle)
        {
            if (!IsPaused)
            {
                if (stsc.number < 2)
                {
                    stsc.LoadNextScene();
                    StartCoroutine(LoadStage("Main"));
                    fader.SetTrigger("End");
                }
                else
                {
                    fader.SetTrigger("good");
                    StartCoroutine(LoadStage("Clear"));
                }

                //IsCompleted = true;
            }
        }
    }

    private IEnumerator LoadStage(string level)
    {
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene(level);
    } 
}