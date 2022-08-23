using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
    [SerializeField]
    private float ZSpeed;
    [SerializeField]
    private float YGravity;

    [SerializeField]
    private LayerMask Ground;
    [SerializeField]
    private LayerMask PlayerLayer;

    private Rigidbody rig;

    [SerializeField]
    private GameObject Player;

    public float ola;

    [SerializeField]
    private bool IsActivated = false;
    [SerializeField]
    private bool MakeDestroy = false;

    [SerializeField]
    private Transform WallChecker;

    [SerializeField]
    private Camera CinematicCam;

    [SerializeField] 
    private Camera mainCam;

    public float speed = 0;

    [SerializeField]
    float DistanceBetween = 0;

    [SerializeField]
    GameObject CameraObj =null;

    bool triggers = false;

    private EnemySund enemySund;
    // Start is called before the first frame update
    void Start()
    {
        enemySund = GameObject.FindGameObjectWithTag("SEManager").GetComponent<EnemySund>();
        CameraObj = Camera.main.gameObject;
        mainCam = Camera.main;
        Player = GameObject.FindGameObjectWithTag("Player");
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (speed <ZSpeed) {
            speed += 0.05f; }
        if (IsActivated)
        {
            DistanceBetween = transform.position.z - Player.transform.position.z;
            if (DistanceBetween < 0)
            {
                rig.velocity = new Vector3(0, rig.velocity.y, speed);
            }
            else if(DistanceBetween < -25)
            {
                rig.velocity = new Vector3(0, rig.velocity.y, speed * 2);
            }
            else
            {
                Player.GetComponent<Rigidbody>().velocity = (new Vector3(0, 20, 35));
                Player.GetComponent<Player>().WasHitByIwa = true;
            }
            RaycastHit hit2;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), Vector3.down * 2, out hit2, 2f, Ground))
            {

            }
            else
            {
                rig.AddForce(0, -YGravity, 0);
            }

            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), transform.position.z), Vector3.down);
            ola = DistanceBetween;
        }
        else
        {
            int i =0;
            var resultWalls = Physics.OverlapBox(
                WallChecker.position,
                WallChecker.localScale / 2, 
                Quaternion.identity, PlayerLayer);
            while (i < resultWalls.Length)
            {
                i++;
                StartCoroutine(Cinematic());
            }
        }
    }

    public AudioClip test;
    IEnumerator Cinematic()
    {
        var audio = GameObject.FindGameObjectWithTag("BGM2").gameObject.GetComponent<AudioSource>();
        audio.Stop();

        CameraObj.GetComponent<CameraSC>().ResetScreen();
        var Clone = GameObject.FindGameObjectWithTag("PowerGage");
        Destroy(Clone);

        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.isCinematic = true;player.StopPlayerMove();
        if(player.IsOnRoullete)
        {
            player.PlayerSE[2].Stop();
        }
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        mainCam.gameObject.SetActive(false);
        CinematicCam.gameObject.SetActive(true);
        MakeDestroy = true;
        yield return new WaitForSeconds(1);

        MakeDestroy = false;
        audio.clip = test;

        audio.Play(); 
        yield return new WaitForSeconds(1.5f);

        yield return new WaitForSeconds(1.5f);

        player.isCinematic = false;
        IsActivated = true;
        mainCam.gameObject.SetActive(true);
        CinematicCam.gameObject.SetActive(false);
        enemySund.PlayerEnemySound(6);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tenjyou") && MakeDestroy == true)
        {
            enemySund.PlayerEnemySound(4);
            collision.gameObject.AddComponent<Rigidbody>();
            collision.gameObject.AddComponent<SmallDissapear>();
        }
        if (collision.gameObject.CompareTag("Ground") && triggers ==false)
        {
            enemySund.PlayerEnemySound(5);
            CinematicCam.GetComponent<Animator>().SetTrigger("Shake");
            triggers = true;
        }
    }
    public void ResetSpeed()
    {
        speed = 0;
    }
}
