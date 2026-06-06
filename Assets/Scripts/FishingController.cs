using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class FishingController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private InputActionAsset inputActions;

    // Bobber
    [Header("Hook")]
    [SerializeField] private GameObject _hookObject;
    [SerializeField] private float _hookSendPower;

    // VFX
    [Header("VFX")]
    [SerializeField] private VisualEffect _castEffect;
    [SerializeField] private VisualEffect _lineEffect;
    [SerializeField] private VisualEffect _splashEffect;

    // SFX
    // SFX goes here

    // Events
    public static event Action PlayerCasted;
    public static event Action PlayerReeled;

    private InputAction _fishInput;
    private Rigidbody _rbHook;
    private FishingState _fishingState;
    private Camera _camera;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void Awake()
    {
        _fishInput = InputSystem.actions.FindAction("Fish");
        _camera = Camera.main;
        _rbHook = _hookObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_fishInput.ReadValue<bool>())
        {
            FishAction();
        }
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void FishAction()
    {
        switch (_fishingState)
        {
            case FishingState.Idle:
                CastLine();
                break;
            case FishingState.Casted:
                ReelEmpty();
                break;
            case FishingState.Hooked:
                ReelSomething();
                break;
        }
    }

    private void CastLine()
    {
        SetFishState(FishingState.Casted);

        Vector3 forceForHook = _camera.transform.forward * _hookSendPower;
        _rbHook.AddForce(forceForHook, ForceMode.Impulse);

        PlayerCasted.Invoke();
    }

    private void ReelEmpty()
    {
        SetFishState(FishingState.Idle);
        PlayerReeled.Invoke();
    }

    private void ReelSomething()
    {
        SetFishState(FishingState.Caught);

        // Logic for picking up a message in a bottle

        PlayerReeled.Invoke();
    }

    public void SetFishState(FishingState NewState)
    {
        if (_fishingState == NewState) return;
        _fishingState = NewState;
    }
}
