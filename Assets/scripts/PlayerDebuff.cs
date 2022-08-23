using System.Collections;
using UnityEngine;
//this script is the handler of all the player debuffs
public class PlayerDebuff : MonoBehaviour
{
    [Header("Debuff Head")]         //required things for the Debuff "Big Head"

    [SerializeField]
    private Transform CharHead;

    [SerializeField]
    private float headSize;

    [SerializeField]
    private float actuaHeadSize = 1f;

    bool HeadActivation;

    [Header("Debuffs Enemy")]   //required things for the Debuff "Spawn Enemies"

    [SerializeField]
    private GameObject Enemy;

    [SerializeField]
    private GameObject FrozenEnemy;


    [Header("Debuff SkinChanger")]   //required things for the Debuff "Skin Changer"

    [SerializeField]
    private Texture[] Skins;
    [SerializeField]
    private Texture defaultSkin;

    [SerializeField]
    private Material PlayerMaterial;

    [SerializeField]
    private Player player;

    [Header("Slipery")]   //required things for the Debuff "Slippery Feet"
    [SerializeField]
    private float slipperyFloat;


    // Start is called before the first frame update
    private void Start()
    {
        player = GetComponent<Player>();
        PlayerMaterial.mainTexture = defaultSkin;
    }


    // Update is called once per frame
    void Update()
    {
        if(HeadActivation && actuaHeadSize < headSize)
        {
            actuaHeadSize += 0.05f;
            CharHead.transform.localScale = new Vector3(actuaHeadSize, actuaHeadSize, actuaHeadSize);
        }
    }

    public IEnumerator HeadDebuffer()       //makes the player head big
    {
        HeadActivation = true;
        yield return new WaitForSeconds(3);
        HeadActivation = false;
        CharHead.transform.localScale = new Vector3(1, 1, 1);
        actuaHeadSize = 1;
    }   

    public IEnumerator GenerateEnemy()  //generates an enemy 
    {
        Vector2 EnemyPos;


        EnemyPos.x = Random.Range(-2, 2);
        EnemyPos.y = Random.Range(-2, 2);

        yield return new WaitForSeconds(0.5f);
        Instantiate(Enemy, new Vector3(transform.position.x + EnemyPos.x, transform.position.y, transform.position.z + EnemyPos.y), Quaternion.identity);
    }

    public IEnumerator SkinChange()    //changes the player skin
    {
        int Randomizer = Random.Range(0, 7);
        PlayerMaterial.mainTexture = Skins[Randomizer];
        yield return new WaitForSeconds(2);
    }

    public IEnumerator RulleteSpeed(int SpeedEffect)    //changes the rullet effect on fast, slower or backwards
    {
       switch(SpeedEffect)
        {
            case 0: player.RulletSpeedUp = true; break;
            case 1: player.RulletSpeedDown = true;    break;
            case 2: player.RulletGyaku = true; break;
            default: break;
        }
        yield return new WaitForSeconds(5);
        switch (SpeedEffect)
        {
            case 0: player.RulletSpeedUp = false; break;
            case 1: player.RulletSpeedDown = false; break;
            case 2: player.RulletGyaku = false; break;
            default: break;
        }
    }

    public IEnumerator GenerateFrozenEnemys()  //Generates 3 Frozen enemies
    {
        Vector2 EnemyPos;

        for(int i = 0;i < 3;i++)
        {

            EnemyPos.x = Random.Range(-2, 2);
            EnemyPos.y = Random.Range(-2, 2);
            yield return new WaitForSeconds(0.5f);
            Instantiate(FrozenEnemy, new Vector3(transform.position.x + EnemyPos.x, transform.position.y, transform.position.z + EnemyPos.y), Quaternion.identity);
        }
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator MakeSlippery()       //makes the player cannot have any drag on the physics
    {
        var rig = GetComponent<Rigidbody>();
        var dragg = rig.drag;
        rig.drag = slipperyFloat;
        player.isDebuffedDrag = true;
        yield return new WaitForSeconds(5);
        rig.drag = dragg;
        player.isDebuffedDrag = false;
    }

    public IEnumerator RulletDissable()     //makes the player rullet disable
    {
        player.RulletOff = true;
        yield return new WaitForSeconds(5);
        player.RulletOff = false;
    }

}
