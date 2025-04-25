using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class handMovement : MonoBehaviour
{
    [SerializeField] private InputActionAsset InputActions;

    private InputAction m_woobleAction;
    private InputAction k_pauseAction;

    private InputAction m_cameraAction;

    private Vector2 m_mouseDelta;

    private Vector2 m_cameraDelta;
    private bool delayStart = true;
    private bool lockRotate = false;

    [SerializeField] private float changeSpeed = 3;
    [SerializeField] private float x;
    [SerializeField] private float y;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rotationX, rotationY, lookSpeed, lookXLimit;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    void Start()
    {
        m_woobleAction = InputSystem.actions.FindAction("Rotate");
        k_pauseAction = InputSystem.actions.FindAction("Jump");
        m_cameraAction = InputSystem.actions.FindAction("CameraRotate");
        Cursor.lockState = CursorLockMode.Locked;
        Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
        m_mouseDelta = new Vector2(0,0);
        StartCoroutine(Delay());
    }

    void Update()
    {
        if(k_pauseAction.WasPressedThisFrame())
        {
            lockRotate = !lockRotate;
        }

        if(m_cameraAction.WasPerformedThisFrame())
        {
            lockRotate = true;
            Debug.Log("Camera to be moving");
            CameraMovement();
        }

        if(m_cameraAction.WasReleasedThisFrame())
        {
            lockRotate = false;
        }

        if(delayStart == false){

            if(lockRotate == true){
                Debug.Log("plate no moving");
                return;
            }
            else{
                m_mouseDelta += m_woobleAction.ReadValue<Vector2>();

                x = m_mouseDelta.x;
                y = m_mouseDelta.y;
                transform.localRotation = Quaternion.Euler(m_mouseDelta.y, m_mouseDelta.x + 180f, 0);
                Debug.Log("plate moving");
            }

        }

        
    }

    void CameraMovement()
    {
        rotationX += -m_cameraAction.ReadValue<Vector2>().x * lookSpeed;
        rotationY += -m_cameraAction.ReadValue<Vector2>().y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        rotationY = Mathf.Clamp(rotationY, -lookXLimit, lookXLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        //transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    private IEnumerator Delay()
    {
	    yield return new WaitForSeconds(1f);
        delayStart = false;
    }
}
