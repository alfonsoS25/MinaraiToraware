using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.InputSystem;

public class CameraSC : MonoBehaviour
{
    [SerializeField]
    private PostProcessVolume PPVolume;

    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject CameraHolder;
    [SerializeField]
    private GameObject CameraArm;

    [SerializeField]
    float cameraPitchX = 0;
    [SerializeField]
    float cameraPitchY = 0;

    [SerializeField]
    private MoveBehaviour moveBehaviour;

    float CameraXmove = 0;
    float CameraYmove = 0;

    float inputX = 0;

    [SerializeField]
    private LayerMask Ground;
    [SerializeField]
    private LayerMask Ground2;

    public bool IsGameOver = false;
    void Start()
    {
        transform.rotation = Quaternion.identity;
        cameraPitchX = 0;
    }

    public IEnumerator VignetteScreen(float Repeats, Color color)   //creates loop from an color and an intensitie its gave, until  the effect its faded
    {
        Vignette vignette;
        if (PPVolume.profile.TryGetSettings<Vignette>(out vignette))
        {
            vignette.intensity.value = Repeats;
            vignette.color.value = color;           
        }

        yield return new WaitForSeconds(0.02f);
        Repeats -= 0.01f;                           

        if (Repeats > 0)        //while the 
        {
            StartCoroutine(VignetteScreen(Repeats, color));
        }
    }

    public void ResetScreen()       //resets the vignette
    {
        Vignette vignette;
        if (PPVolume.profile.TryGetSettings<Vignette>(out vignette))
        {
            vignette.intensity.value = 0;
            vignette.intensity.value = 0;
            vignette.color.value = Color.white;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (IsGameOver == false)        
        {
            if (CameraXmove != 0)          //this is for the controllers, when player moves add the movement to the rotating camera
            {
                inputX += CameraXmove;
            }
            if (CameraYmove != 0)       
            {
                if (CameraYmove < 0 && cameraPitchX > -15)
                {
                    cameraPitchX += CameraYmove * 13;       //ads the movement to the rotation of the camera without getting out of the rotation box
                }
                else if (CameraYmove > 0 && cameraPitchX < 30)
                {
                    cameraPitchX += CameraYmove * 13;   
                }
            }
            CameraArm.transform.position = Player.transform.position;   //sets the center of rotation of the camera

            Vector3 CameraPos = new Vector3(CameraHolder.transform.position.x, CameraHolder.transform.position.y, CameraHolder.transform.position.z);       //sets the camera in the camera Rotated Position
            transform.position = CameraPos;
            transform.LookAt(new Vector3(CameraArm.transform.position.x, CameraArm.transform.position.y, CameraArm.transform.position.z));  //makes the camera look the player position

            CameraArm.transform.rotation = Quaternion.Euler(cameraPitchX + cameraPitchX, inputX + cameraPitchY, 0);

            transform.position = CalculateCameraPosition();//chekcs if there isnt any blocks between the camera and the player
        }
        else if(IsGameOver ==true)
        {

        }
    }
    public void OnMouseLook(InputAction.CallbackContext context)        //this function makes the camera moves with the mouse
    {

        var lookInput = context.ReadValue<Vector2>();       //gets the mouse movement

        moveBehaviour.Rotate(lookInput.x);      //moves the player Character to the moving position

        cameraPitchX += lookInput.y * moveBehaviour.RotationSpeed*30;       
        cameraPitchX = Mathf.Clamp(cameraPitchX, -15, 30);              //limits the movement of the camera not below -15 and not upper 30
        cameraPitchY += (lookInput.x * moveBehaviour.RotationSpeed) * 90;

    }
    public void OnControllerLook(InputAction.CallbackContext context)   //this function makes the camera moves with the controller
    {
        var lookInput = context.ReadValue<Vector2>();       //gets the controller movement
        //lookInput.y *= -1f;
        moveBehaviour.Rotate(lookInput.x);

        if (lookInput.x > 0.4|| lookInput.x < -0.4f)    //if its pressed more than the hslp of the button rotatesthe camera
        {
            CameraXmove = (lookInput.x * moveBehaviour.RotationSpeed) * 2500;   
        }
        if (lookInput.y > 0.4 || lookInput.y < -0.4f)
        {
            CameraYmove = (-lookInput.y * moveBehaviour.RotationSpeed) * 60;
           
        }
        CameraYmove = Mathf.Clamp(CameraYmove, -30, 30);
        if (context.canceled)   //if the controller button its not longer pressed, resets the camera movement
        {
            CameraXmove = 0;
            CameraYmove = 0;
        }
    }
    Vector3 CalculateCameraPosition(float maxLerpDist = -1f)        //checks if there is an ground object behind the camera
    {
        Vector3 result = transform.position;
        float ratio = 0.8f;                                                 //this is the percentage of the camera getting back from the object
        Ray ray = new Ray(Player.transform.position, -transform.forward);       //throws a ray behind the camera
        RaycastHit rayHit;
        Debug.DrawRay(Player.transform.position, -transform.forward*6, Color.red);
        if (Physics.Raycast(ray, out rayHit,9f,Ground | Ground2 ))
        {
            float distance = (Player.transform.position - rayHit.point).magnitude * ratio;      //calculates the distance between the player and the hitted object
            if (maxLerpDist < 0 || distance < maxLerpDist)
            {
                result = Vector3.Lerp(Player.transform.position, rayHit.point, ratio);  //gives the 80% of the distance between the player and the hit object  towards the player
            }
        }
        return result;  
    }
}
