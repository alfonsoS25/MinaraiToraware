using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageScene : MonoBehaviour
{ 

    // UI表示用のRootオブジェクトを指定します。
    [SerializeField]
    private Transform uiRoot = null;

    // ステージ数を指定します。

    // 現在プレイしているステージ番号を取得または設定します。
    public static int StageNo
    {
        get { return stageNo; }
        set { stageNo = value; }
    }

    public static int stageNo =1;

    // このクラスのインスタンスを取得します。
    public static StageScene Instance
    {
        get { return instance; }
        set { instance = value; }
    }
    private static StageScene instance = null;

    public int number = 0;

    [SerializeField]
    private GameObject[] Stages;


    // AwakeメソッドはStartメソッドよりも先に実行したい初期化処理を記述します。
    void Awake()
    {if (stageNo <= 2)
        {
            //InstantiateLevel(stageNo);
        }
     if(stageNo==2)
        {
            Debug.Log("A");
        }
        // 生成されたインスタンス（自分自身）をstaticな変数に保存
        instance = this;


        // Resourcesフォルダーからステージのプレハブを読み込む

    }

    public void InstantiateLevel(int stageNumber)
    {
        var prefabName = string.Format("stage{0}", stageNumber);
        var stagePrefab = Resources.Load<GameObject>(prefabName);
        Instantiate(stagePrefab, transform);
    }


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Stages.Length;i++)
        {
            if(i != stageNo)
            {
                Stages[i].SetActive(false);
            }
            else
            {
                Stages[i].SetActive(true);
            }
        }
        number = stageNo;
        if (GameObject.Find("UIRoot"))
        {
            uiRoot = GameObject.Find("UIRoot").GetComponent<Transform>();
        }
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    #region ポーズ

    // ポーズ状態を解除します。
    public void Resume()
    {

    }
    #endregion

    #region ステージクリアー
    // このステージをクリアーさせます。

    // 次のシーンを読み込みます。
    public void LoadNextScene()
    {
        StageNo += 1;
    }

    public void ResetStage()
    {

        Debug.Log("reset");
        StageNo = 0;
        stageNo = 0;
    }
    #endregion

    #region ゲームオーバー
    // このステージをゲームオーバーで終了させます。

    public void GameOver()
    {
        StartCoroutine(Death());
    }
    public IEnumerator Death()
    {
        yield return new WaitForSeconds(0.4f);
    }

    public void ReturnMainTitle()
    {
        SceneManager.LoadScene("AnyButtonScene");
    }
    public void finishThis()
    {
        StartCoroutine(FinishedGame());
    }
    public IEnumerator FinishedGame()
    {
        yield return new WaitForSeconds(1.5f);
    }
    public void Retry()
    {
        Resume();
    }

    public void GiveUp()
    {
        Resume();
        StageNo = 0;
    }
    #endregion

    public void LoadSceneWithFadeOut(string sceneName, Sprite sprite)
    {
        StartCoroutine(OnLoadSceneWithFadeOut(sceneName, sprite));
    }

    IEnumerator OnLoadSceneWithFadeOut(string sceneName, Sprite sprite)
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneName);
    }
}