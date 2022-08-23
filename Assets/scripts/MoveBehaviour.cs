using System.Collections;
using UnityEngine;

public class MoveBehaviour : MonoBehaviour
{
    // 移動の速さを指定します。
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float JumpSpeed = 5;

    // 回転の速さを取得します。

    public CharacterController charController;
    public float RotationSpeed
    {
        get => rotationSpeed;
        private set => rotationSpeed = value;
    }
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField]
    private LayerMask Ground;

    new Rigidbody rigidbody;

    [SerializeField]
    private GameObject CameraObj;

    private float turnSmoothVelocity;
    private float turnSmoothTime = 0.1f;

    private Transform mainCam;


    void Start()
    {
        mainCam = Camera.main.transform;
        rigidbody = GetComponent<Rigidbody>();
    }
    public void GravityAdder(float gravityForce,bool isDragDebuff)
    {
        rigidbody.AddForce(0, -gravityForce, 0);
    }

    public void ChangeDrag(bool onair)
    {
        if (onair)
        {
            rigidbody.drag = 4f;
        }
        else
        {
            rigidbody.drag = 0.4f;
        }
    }

    public bool IsDeath = false;
    public void StopMovement()
    {
        rigidbody.velocity = new Vector3(0,rigidbody.velocity.y,0);

    }
    // 指定した方角へ平行移動します。
    public void Move(Vector2 normalizedSpeed)
    {

        if (!IsDeath)
        {
            Vector3 MovePos = new Vector3(normalizedSpeed.x, 0, normalizedSpeed.y);
            if (MovePos.magnitude >= 0.1f)
            {
                float targetangle = Mathf.Atan2(normalizedSpeed.x, normalizedSpeed.y) * Mathf.Rad2Deg + mainCam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0, angle, 0);
                if (normalizedSpeed.x < 0) { normalizedSpeed.x *= -1; }
                if (normalizedSpeed.y < 0) { normalizedSpeed.y *= -1; }

                float MovementX = normalizedSpeed.x * moveSpeed;
                float MovementY = normalizedSpeed.y * moveSpeed;
                float finalMovement = MovementX + MovementY;
                if(normalizedSpeed.x + normalizedSpeed.y >= 1.3f)
                {
                    finalMovement /=1.5f;
                }

                Vector3 moveDir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
                Vector3 MoveToo = (moveDir * finalMovement)
                    + new Vector3(0, rigidbody.velocity.y, 0);
                Vector3 FinalVelocity = new Vector3(MoveToo.x * normalizedSpeed.x, MoveToo.y, MoveToo.z * normalizedSpeed.y);
                rigidbody.velocity = MoveToo;

            }
            else
            {

            }
        }
    }


    public IEnumerator ChangeSpeed(int speed)
    {
        float normalspeed = moveSpeed;
        moveSpeed = moveSpeed * 4;
        yield return new WaitForSeconds(5);
        moveSpeed = normalspeed;
    }

    public void Jump()
    {
        rigidbody.velocity = new Vector3(0, JumpSpeed, 0);
    }


        // このキャラクターを指定した速度で回転させます。
    public void Rotate(float normalizedSpeed)
    {
        transform.Rotate(0, RotationSpeed * normalizedSpeed, 0);
    }
}
