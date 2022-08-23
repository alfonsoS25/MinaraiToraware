using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MoveBehaviour), typeof(PlayerInput))]

public class Player : MonoBehaviour
{// ƒJƒƒ‰‚ÌˆÊ’u‚ðŽw’è‚µ‚Ü‚·B
    [Header("Movement")]

    MoveBehaviour moveBehaviour;

    Vector2 moveInput = Vector2.zero;

    [SerializeField]
    private int AvaliableJump = 0;

    [SerializeField]
    private int MaxJumps;

    [SerializeField]
    private float OffsetJump = 0;

    [SerializeField]
    private float gravity = 20;

    [SerializeField]
    private float JumpDelay;

    [SerializeField]
    private Transform GroundChecker;

    [SerializeField]
    private LayerMask Ground;

    [SerializeField]
    private LayerMask Ice;


    [SerializeField]
    private bool OnAir = false;
    [Header("Camara")]

    [SerializeField]
    private CameraSC camerasc;

    [SerializeField]
    private Transform cameraPos;

    [Header("hp")]
    [SerializeField]
    private int HP;

    private HPHandler hpController;

    [SerializeField]
    private GameObject[] HitEffect;

    [SerializeField]
    private float InvisibleTime;

    [SerializeField]
    private float hitDelay = 0;

    [SerializeField]
    public bool WasHitByIwa = false;


    [Header("Attack")]

    [SerializeField]
    private GameObject[] AttackParticles;

    [SerializeField]
    private GameObject PowerGage = null;

    [SerializeField]
    private GameObject Attack1;

    [SerializeField]
    private GameObject Attack2;

    [SerializeField]
    private GameObject attack2anim = null;

    [SerializeField]
    private GameObject Attack3;

    [SerializeField]
    private float attackDelay = 0;


    [Header("AttackSpeed")]

    [SerializeField]
    private float AttackSpeed1;
    [SerializeField]
    private float AttackSpeed2;
    [SerializeField]
    private float AttackSpeed3;

    [SerializeField]
    private Transform UI;
    [SerializeField]
    private Transform UIDebuffT;

    [SerializeField]
    private GameObject Finish;

    [SerializeField]
    private Pause pauseMenu;


    [Header("Debuff")]

    private PlayerDebuff playerdebuffsc;

    public bool isDebuffedDrag = false;

    public bool RulletSpeedUp = false, RulletSpeedDown = false, RulletGyaku = false;

    public bool RulletOff = false;


    [Header("Checkers")]

    public bool IsPaused;

    [SerializeField]
    private int IsGround;

    [SerializeField]
    private bool IsCompleted = false;

    [SerializeField]
    private float timer;

    [SerializeField]
    private Pause pause;

    [SerializeField]
    private Gameover gameoversc = null;

    [SerializeField]
    private bool Lost = false;

    [SerializeField]
    public AudioSource[] PlayerSE;

    [SerializeField]
    private GameObject Douga;

    [SerializeField]
    private GameObject UIDebuff;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject GameOverUI;

    public bool IsOnOptions;

    public Options Options;

    private float DeathAnim;

    [SerializeField]
    private GameObject UnasableRullet;

    public bool isCinematic = false;

    float buttonDelay=0;

    bool OnGrounded = false;

    public bool IsOnRoullete = false;

    bool IsWalking = false;

    bool ToStop = false;

    [Header("Animator")]

    [SerializeField]
    private Animator animChar;

    [SerializeField]
    private Animator fader;


    // Start is called before the first frame update

    void Start()
    {
        playerdebuffsc = GetComponent<PlayerDebuff>();
        hpController = GetComponent<HPHandler>();
        moveBehaviour = GetComponent<MoveBehaviour>();
        moveInput = Vector2.zero;

        hpController.SetHP(HP);
    }

    void WalkingSound()
    {
        if(!PlayerSE[0].isPlaying)
        {
            PlayerSE[0].loop = true;
            PlayerSE[0].Play();
        }
    }
    // MoveƒAƒNƒVƒ‡ƒ“‚ðˆ—‚µ‚Ü‚·B

    public void OnClick(InputAction.CallbackContext context)
    {
        
        if (context.performed && !IsCompleted && HP > 0 && !RulletOff && attackDelay < 0&& gameManager.IsPaused ==false &&　!Lost && !isCinematic)
        {
            StartRoullete();
        }
        else if (context.performed && !IsCompleted && HP > 0 && RulletOff && attackDelay < 0 && gameManager.IsPaused == false && !Lost && !isCinematic) //spawns unusable rullete
        {
            var cloneX = Instantiate(UnasableRullet, UI.transform.position, Quaternion.identity, UIDebuffT);
            Destroy(cloneX, 1.2f);
            attackDelay = 1f;
        }
        else if (context.canceled && !IsCompleted && HP > 0 && !RulletOff && gameManager.IsPaused == false && attackDelay <0 && !isCinematic)
        {
            StopRoullete();
        }
    }

    void StartRoullete()
    {
        IsOnRoullete = true;
        PlayerSE[2].Play();
        var clone = Instantiate(PowerGage, UI.transform.position, Quaternion.identity, UI);   //instantiate the roullete

        if (RulletSpeedDown || RulletSpeedUp || RulletGyaku)
        {
            float speed1 = 1, speed2 = 1, speed3 = 1;   //sets the velocity of spin of the roullete if there is any debuff
            if (RulletSpeedDown)
            {
                speed1 = 0.7f;
            }
            if (RulletSpeedUp)
            {
                speed2 = 1.5f;
            }
            if (RulletGyaku)
            {
                speed3 = -1f;
            }
            clone.GetComponent<PowerGage>().ChangeArrowSpeed(speed1, speed2, speed3);
        }
        else
        {
            clone.GetComponent<PowerGage>().ChangeArrowSpeed(1, 1, 1);
        }

    }
    public void StopRoullete()
    {
        if (GameObject.FindGameObjectWithTag("PowerGage").gameObject.activeSelf)
        {
            //gets the actual side of the PowerGage and activates of the side 
            //adds the delay for next Power
            PlayerSE[2].Stop();
            PlayerSE[3].Play();
            var Clone = GameObject.FindGameObjectWithTag("PowerGage");  
            Power(Clone.GetComponent<PowerGage>().AreaCheck());
            attackDelay = 1f;                                   
            IsOnRoullete = false;
            Destroy(Clone);
        }

    }

    public void OnSpace(InputAction.CallbackContext context)   //this Function makes the player Jump if the player isnt on Pause, win, death or in cinematic,
    {
        if (context.performed && !IsCompleted && HP > 0 && AvaliableJump > 0 && !isCinematic && OnGrounded && !Lost)
        {   
            PlayerSE[0].Stop();
            PlayerSE[0].loop = false;
            JumpDelay = 0;
            animChar.SetBool("Jump", true);
            PlayerSE[1].Play();
            moveBehaviour.Jump();
            AvaliableJump--;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Finish") && !IsCompleted)
        {
            var stageScene = GameObject.FindGameObjectWithTag("stageManager").GetComponent<StageScene>();
            if (stageScene.number != 2)
            {
                if (IsOnRoullete)
                {
                    StopRoullete();
                }
                StartCoroutine(Finished());
            }
            else
            {
                if (IsOnRoullete)
                {
                    StopRoullete();
                }
                PlayerSE[8].Play();
                PlayerSE[7].Stop();
                IsCompleted = true;
                animChar.SetTrigger("Finish");
                stageScene.ResetStage();
                gameManager.Complete();
            }
        }
        if (collision.gameObject.CompareTag("Enemy") && timer > InvisibleTime && !isCinematic)
        {
            TakeDamage(1);
        }
        else if(collision.gameObject.CompareTag("SnowBall") && timer > InvisibleTime)
        {
            HitByIwa();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("SnowBallHit"))
        {
            HitByIwa();
        }
    }

    public void HitByIwa()
    {
        PlayerSE[0].Stop();
        PlayerSE[0].loop = false;
        animChar.SetBool("Walk", false);
        moveBehaviour.StopMovement();
        moveInput = Vector2.zero;

        if (timer > InvisibleTime)
        {
            TakeDamage(2);
        }
        GameObject.FindGameObjectWithTag("SnowBall").GetComponent<SnowBall>().ResetSpeed();
        WasHitByIwa = true;
        var rig = gameObject.GetComponent<Rigidbody>();
        rig.velocity = (new Vector3(0, 20, 35));
    }

    IEnumerator Finished()
    {
        PlayerSE[8].Play();
        PlayerSE[7].Stop();
        IsCompleted = true;
        //gameManager.IsCompleted = true;
        animChar.SetTrigger("Finish");
        yield return new WaitForSeconds(1.5f);
        Instantiate(Finish, UI);
    }

    private void TakeDamage(int Damage)
    {
        hitDelay = 0;
        var ParticleClone = Instantiate(HitEffect[1], transform.position, transform.rotation);
        HP -= Damage;
        hpController.TakeDamage(Damage);
        if (HP <=0 && Lost ==false)
        {
            GameOVerAction();
        }
        animChar.SetTrigger("Hurt");
        PlayerSE[5].Play();
        timer = 0;
    }

    // Update is called once per frame

    void Update()
    {
        InputSystem.Update();
        JumpDelay = 100;
        if(IsCompleted)
        {
            Vector3 positionsToLook = new Vector3(cameraPos.gameObject.transform.position.x, transform.position.y, cameraPos.gameObject.transform.position.z);
            transform.LookAt(positionsToLook);
        }
        if(attackDelay >=0)
        {
            attackDelay -= Time.deltaTime;
        }
        moveBehaviour.Move(moveInput);

        if (timer < InvisibleTime)
        {
            timer += Time.deltaTime;
        }
        if(Lost && DeathAnim < 1.5f)
        {
            DeathAnim += Time.deltaTime+1.5f;
            animChar.SetFloat("death", DeathAnim);
        }
        if (IsGround > 0)
        {
            OffsetJump = 0;
            OnAir = false;
            AvaliableJump = MaxJumps;
        }
        else if(IsGround ==0)
        {
            OffsetJump += Time.deltaTime;
            if(OffsetJump >0.25f) OnAir = true;
        }
        IsGround = 0;
        bool isonair = false;
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), Vector3.down, out hit, 0.8f, Ground) && hitDelay>1)
        {
            if (ToStop)
            {
                ToStop = false; 
                moveBehaviour.StopMovement();
            }
            OnGrounded = true;
            IsGround++;
            isonair = true;
            WasHitByIwa = false;
            if(IsWalking)
            {
                WalkingSound();
            }
        }
        else
        {
            PlayerSE[0].Stop();
            PlayerSE[0].loop = false;
            
            OnGrounded = false;
        }
        if(hitDelay <= 1f)
        {
            hitDelay += Time.deltaTime;
        }
        if (isonair)
        {
            if (JumpDelay > 0.2f)
            {
                animChar.SetBool("Jump", false);
            }
        }
        //OffsetJump += Time.deltaTime;

        if (!isDebuffedDrag)
        {
            moveBehaviour.ChangeDrag(isonair);
        }
        else
        {
            moveBehaviour.ChangeDrag(false);
        }

        if (buttonDelay <= 1.2f)
        {
            buttonDelay += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (OnAir == true)
        {
            moveBehaviour.GravityAdder(gravity, isDebuffedDrag);
        }

        if (transform.position.y < -10 && Lost == false)
        {
            GameOVerAction();
            this.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isCinematic )
        {
            if (context.performed && IsPaused && buttonDelay > 1 && !Lost)
            {
                moveInput = context.ReadValue<Vector2>().normalized;
                pause.MoveButton(moveInput.y);
                buttonDelay = 0;
            }
            else if (context.performed && IsOnOptions && buttonDelay > 1 && !Lost)
            {
                moveInput = context.ReadValue<Vector2>().normalized;
                Options.MoveButton(moveInput.y, moveInput.x);
                buttonDelay = 0;
            }
            else if (context.performed && !IsCompleted && HP > 0 && OnAir == false && WasHitByIwa == false && !isCinematic && !Lost ||
                context.performed && !IsCompleted && HP > 0 && OnAir == true && WasHitByIwa == false && !isCinematic && !Lost)//&& HitBoxes == 0)
            {
                IsWalking = true;
                if (OnGrounded) { 
                PlayerSE[0].Play();
                PlayerSE[0].loop = true;}
                moveInput = context.ReadValue<Vector2>().normalized;
                Vector2 MoveTowards = new Vector2(0, 0);
                MoveTowards.y = moveInput.y;
                MoveTowards.x = moveInput.x;
                moveBehaviour.Move(MoveTowards);
                animChar.SetBool("Walk", true);
                animChar.SetFloat("WalkSpeed", moveInput.y);
            }
            if (Lost && buttonDelay > 1)
            {
                buttonDelay = 0;
                moveInput = context.ReadValue<Vector2>().normalized;
                gameoversc.MoveButton(-moveInput.x);
            }
            if (context.canceled && !isDebuffedDrag)
            {
                StopPlayerMove();
            }
        }
        else
        {
            animChar.SetBool("Walk", false);
            moveBehaviour.StopMovement();
            moveInput = Vector2.zero;
            PlayerSE[0].Stop();
            PlayerSE[0].loop = false;
        }
    }

    public void StopPlayerMove()
    {
        IsWalking = false;
        buttonDelay = 2;
        PlayerSE[0].Stop();
        PlayerSE[0].loop = false;
        animChar.SetBool("Walk", false);
        if (!OnAir)
        {
            moveBehaviour.StopMovement();
        }
        else
        {
            ToStop = true;
        }
        moveInput = Vector2.zero;
    }
    private void Power(int PowerNum)   
    {
        if (PowerNum < 100)
        {
            PlayerSE[4].Play();
            var ParticleClone = Instantiate(HitEffect[0], transform.position, transform.rotation);  //instantiate success effect
            switch (PowerNum)
            {
                case 0:
                    StartCoroutine(Attack1Enum());  //starts attack 1
                    break;

                case 1:
                    StartCoroutine(Attack2Enum());  //starts attack 2
                    break;

                case 2:
                    StartCoroutine(Attack3Enum());  //starts attack 3
                    break;

            }
            animChar.SetTrigger("Attack");
        }
        else if (PowerNum > 100)
        {
            //sets the fail effect and starts debuff action
            var ParticleClone = Instantiate(HitEffect[2], new Vector3(transform.position.x,transform.position.y-(transform.localScale.y/2),transform.position.z), Quaternion.Euler(90,0,0));
            StartCoroutine(Debuffer(PowerNum));
        }
    }

    private IEnumerator Attack1Enum()
    {
        yield return new  WaitForSeconds(AttackSpeed1);     
        Attack1.SetActive(true);                      //sets the hitbox of the attack on true
        var ParticleClone =Instantiate(AttackParticles[0], transform.position, transform.rotation);  //generates the ice attack effect
        StartCoroutine(camerasc.VignetteScreen(0.20f,new Color(85, 125,180)));      //activates the camera effect
        PlayerSE[4].Play();
        yield return new WaitForSeconds(0.5f);
        Attack1.SetActive(false);                   
        ParticleClone.GetComponent<ParticleSystem>().Stop();    //stop and destroy the attack effect
        Destroy(ParticleClone,2);
    }
    private IEnumerator Attack2Enum()
    {
        var equisde = Instantiate(HitEffect[3], transform.position, transform.rotation);//generates the 1 fire attack effect
        equisde.transform.SetParent(this.transform);
        yield return new  WaitForSeconds(AttackSpeed2); 
        Attack2.SetActive(true);                      //sets the hitbox of the attack on true
        var ParticleClone = Instantiate(AttackParticles[1], transform.position, transform.rotation); //generates the 2 fire attack effect
        StartCoroutine(camerasc.VignetteScreen(0.50f,new Color(15, 6,0)));//activates the camera effect
        yield return new WaitForSeconds(0.5f);
        Attack2.SetActive(false);   //stop and destroy the attack effect
    }
    private IEnumerator Attack3Enum()
    {
        yield return new  WaitForSeconds(AttackSpeed3);
        Instantiate(attack2anim, new Vector3(transform.position.x,transform.position.y-0.5f,transform.position.z), transform.rotation); //generates the ice stones attack effect
        StartCoroutine(camerasc.VignetteScreen(0.20f, new Color(85, 125, 180)));//activates the camera effect
        yield return new WaitForSeconds(0.5f);
        Attack3.SetActive(false);   //stop and destroy the attack effect
    }
    private IEnumerator Debuffer(int DebuffNum)     //sets the player debuffs
    {
        StartCoroutine(camerasc.VignetteScreen(0.75f, new Color(0.15f, 0.35f, 1f)));//sets the camera failure effect
        animChar.SetTrigger("Hurt");

        var UiDebuffClonne = UIDebuff;             //gets the ui debuff text sample 
        var schanger = UIDebuff.GetComponent<DebuffUI>();     //gets the player debuff script
        switch (DebuffNum)  //sets the player debuff and the ui sprite
        {
            case 101:   StartCoroutine(playerdebuffsc.HeadDebuffer()); schanger.SpriteSet(4);
                break;
            case 102:   StartCoroutine(playerdebuffsc.GenerateEnemy()); schanger.SpriteSet(6);
                break;
            case 103:   StartCoroutine(playerdebuffsc.SkinChange()); schanger.SpriteSet(7);
                break;
            case 104:   StartCoroutine(playerdebuffsc.RulleteSpeed(0)); schanger.SpriteSet(0);
                break;
            case 105:   StartCoroutine(playerdebuffsc.RulleteSpeed(1)); schanger.SpriteSet(2);
                break;
            case 106:   StartCoroutine(playerdebuffsc.RulleteSpeed(2)); schanger.SpriteSet(1);
                break;
            case 107:   StartCoroutine(playerdebuffsc.GenerateFrozenEnemys()); schanger.SpriteSet(5);
                break;
            case 108:   StartCoroutine(playerdebuffsc.MakeSlippery()); schanger.SpriteSet(8);
                break;
            case 109:   StartCoroutine(playerdebuffsc.RulletDissable()); schanger.SpriteSet(3);
                break;
            default:
                break;
        }
        Instantiate(UiDebuffClonne, UI.transform.position, Quaternion.identity, UIDebuffT);  //generates an ui text of the debuff

        yield return new WaitForSeconds(5);
    }
    private void GameOVerAction()
    {
        if(Lost ==false)
        {
            PlayerSE[6].Play();
            PlayerSE[7].Stop();
            gameManager.IsGameOver = true;
            camerasc.IsGameOver = true;
            Lost = true;
            animChar.SetFloat("death", 1f);
            fader.SetTrigger("died");
            GameObject.FindGameObjectWithTag("Canvas").gameObject.SetActive(false);
            StartCoroutine(instantiateDeath());
            Time.timeScale = 0.2f;
            moveBehaviour.IsDeath = true;
        }
    }

     IEnumerator instantiateDeath() {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1.2f);
        Instantiate(Douga, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        var clone = Instantiate(GameOverUI, UI);
        gameoversc = clone.GetComponent<Gameover>();
    }

    public void Enter(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Lost)
            {
                gameoversc.SumbitButton(context);
            }
            if (IsOnOptions)
            {
                Options.SumbitButton(context);
            }
            else if (IsPaused)
            {
                GameObject.FindGameObjectWithTag("Pause").GetComponent<Pause>().MainMenuSwitch();
            }
            if(IsCompleted)
            {
                gameManager.Complete();
            }
            
        }
    }
}

