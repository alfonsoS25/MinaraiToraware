using UnityEngine;
using UnityEngine.UI;

public class PowerGage : MonoBehaviour
{

    [SerializeField]
    private GameObject Arrow;

    [SerializeField]
    private GameObject gage;

    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float usedSpeed;

    [SerializeField]
    private float Z;

    [SerializeField]
    private int actualside = 0;

    [Header ("Patterns")]
    [SerializeField]
    private int patternNum;


    //simplify to array and use with for in the function 
    [SerializeField]
    private int[] Patter1;
    [SerializeField]
    private int[] Patter2;
    [SerializeField]
    private int[] Patter3;


    [SerializeField]
    private Sprite[] LoopDifficultySp;

    [SerializeField]
    private Image GageSprite;

    private bool IsLoop;
    private int LoopCount = 0;

    private float Offset = 10;

    void Start()
    {
        GageSprite.sprite = LoopDifficultySp[LoopCount];
    }

    public void ChangeArrowSpeed(float debuff1 = 1,float debuff2= 1,float debuff3=1) 
    {                                   //sets the speed of the arrow
        var Equalspeed = rotationSpeed;
        Equalspeed *= debuff1;
        Equalspeed *= debuff2;
        Equalspeed *= debuff3;
        usedSpeed = Equalspeed;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Arrow.transform.Rotate(0, 0, usedSpeed);    //rotates the arrow by the determinated speed
        Z = Arrow.transform.eulerAngles.z;          

        if(Z > 350)     //if by the finish of the cirle, sets the rullet ready  to level up
        {
            IsLoop = true;
        }
        if(Z < 15)
        {
            if(IsLoop ==true)
            {
                if (LoopCount < 2)
                {
                    LoopCount++;
                }
                switch(LoopCount)
                {
                    case 0:Offset = 10; break;      //sets the area of success

                    case 1: Offset = 30; break;

                    case 2: Offset = 60; break;
                }
                GageSprite.sprite = LoopDifficultySp[LoopCount];    //changes the sprite to the level
            }
            IsLoop = false;
        }
    }

    public int AreaCheck()      //if the arrow rotation its inside the are of any pattern, return the designed attack, else, set a random debuff
    {
        if (Z <= Patter1[patternNum] + Offset && Z > Patter1[patternNum] - Offset)
        {
            actualside = 0;
        }
        else if (Z <= Patter2[patternNum] + Offset && Z > Patter2[patternNum] - Offset)
        {
            actualside = 1;
        }
        else if (Z <= Patter3[patternNum] + Offset && Z > Patter3[patternNum] - Offset)
        {
            actualside = 2;
        }
        else
        {
            actualside = Random.Range(100, 110);
        }
        return actualside;
    }
}
