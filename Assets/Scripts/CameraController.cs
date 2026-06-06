using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI.Table;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private float lookSpeed;

    private InputAction _lookInput;
    private Vector2 _lookInputValue;

    float _verticalRotation = 0f;

    private Camera _camera;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void Awake()
    {
        _lookInput = InputSystem.actions.FindAction("Look");
        _camera = Camera.main;
    }

    private void Update()
    {
        _lookInputValue = _lookInput.ReadValue<Vector2>();
        HandleLook();
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void HandleLook()
    {
        _verticalRotation -= _lookInputValue.y * lookSpeed * Time.deltaTime;
        float horizontalDelta = _lookInputValue.x * lookSpeed * Time.deltaTime;

        _verticalRotation = Mathf.Clamp(_verticalRotation, -90f, 90f);

        transform.localEulerAngles = new Vector3(_verticalRotation, transform.localEulerAngles.y + horizontalDelta, transform.localEulerAngles.z);
    }
}
