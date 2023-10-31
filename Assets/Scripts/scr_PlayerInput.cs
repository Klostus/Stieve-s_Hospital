using Cinemachine;
using UnityEngine;

public class scr_PlayerInput : MonoBehaviour
{
    private scr_PlayerMovement playerMovement;
    [SerializeField] private scr_CameraMovement cameraMovement;
    private scr_PauseController pauseControllerScript;
    [SerializeField] private GameObject pausePanel;
    //private scr_AnimationsManager animManager;

    [HideInInspector]
    public GameObject interactiveObject;

    [SerializeField] private GameObject virtualCameraZoom;
    private bool zoom;

    private bool cameraReset;

    private void Awake()
    {
        playerMovement = GetComponent<scr_PlayerMovement>();
        pauseControllerScript = GetComponent<scr_PauseController>();
        //animManager = GetComponent<scr_AnimationsManager>();
    }

    private void Update()
    {
        float horizontalDirection = Input.GetAxisRaw(scr_GlobalStringVars.HORIZONTAL_AXIS);
        float verticalDirection = Input.GetAxisRaw(scr_GlobalStringVars.VERTICAL_AXIS);
        cameraMovement.CameraMovement(horizontalDirection, verticalDirection);

        if (Input.GetButtonDown(scr_GlobalStringVars.B2P))
        {
            cameraReset = true;
            cameraMovement.CameraReset(cameraReset);
        }
        else cameraReset = false;

        if (Input.GetButtonDown(scr_GlobalStringVars.INTERACT))
        {
            bool click = Input.GetButtonDown(scr_GlobalStringVars.INTERACT);
            Vector2 mousePosition = Input.mousePosition;
            playerMovement.SetTargetPosition(click, mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(interactiveObject != null)
            {
                if (interactiveObject.CompareTag("WorkPlace"))
                {
                    interactiveObject.GetComponent<scr_PC>().interact = true;
                }
                else 
                {
                    interactiveObject.GetComponent<scr_PatientStatus>().interact = true;
                }                
            }
        }

        if (Input.GetButtonDown(scr_GlobalStringVars.ZOOM))
        {
            zoom = !zoom;
            if (zoom)
            {
                virtualCameraZoom.SetActive(true);
            }
            else virtualCameraZoom.SetActive(false);
        }

        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("Escape pressed!");
            pausePanel.SetActive(true);
            pauseControllerScript.PauseGame();
        }
    }
}