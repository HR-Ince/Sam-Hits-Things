using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ChaplainPlayerController : MonoBehaviour
{
    // Public variables
    public Vector2 PointerPos { get { return _pointerPos; } }

    [Header("Human things")]
    [SerializeField] float _throwStrength;
    [Header("Demon things")]
    [SerializeField] int _noOfDemons;
    [SerializeField] GameObject _demonPrefab;
    [SerializeField] Vector3 _throwOriginOffset;
    [Header("Externals")]
    [SerializeField] PlayerStateRegister _register;

    // Private variables
    private List<GameObject> Vessels;
    private bool _launchReadied = false;
    private bool _launchValid = false;
    private GameObject _currentVessel;
    private Vector2 _pointerPos;

    // Component references
    private LineDrawer _throwVisualisation;
    private PlayerAnimationController _anim;
    private PlayerDrawHandler _drawHandler;
    private PlayerElementManager _elementManager;
    private Rigidbody _currentVesselRB;
    private VesselStateManager _currentVesselVSM;
    

    private void Awake()
    {
        FetchComponentReferences();
        _register.PlayerOne = gameObject;
        SetupDemons();
    }

    private void FetchComponentReferences()
    {
        _anim = GetComponent<PlayerAnimationController>();
        _elementManager = GetComponent<PlayerElementManager>();

        _throwVisualisation = GetComponentInChildren<LineDrawer>();
        _drawHandler = GetComponentInChildren<PlayerDrawHandler>();
    }

    private void SetupDemons()
    {
        Vessels = new List<GameObject>();
        while (Vessels.Count < _noOfDemons)
        {
            var temp = Instantiate(_demonPrefab, transform);
            Vessels.Add(temp);
            temp.name = "Demon " + Vessels.IndexOf(temp);
            temp.SetActive(false);
        }
        if (Vessels.Count > 0)
        {
            NewThrow();
        }
    }

    public void NewThrow()
    {
        if(_currentVesselVSM)
            _currentVesselVSM.OnInactive.RemoveListener(OnVesselInactive);
        if (Vessels.Count <= 0) return;
        _currentVessel = Vessels[0];
        _currentVesselRB = _currentVessel.GetComponent<Rigidbody>();
        _currentVesselVSM = _currentVessel.GetComponent<VesselStateManager>();
        _currentVessel.transform.position = transform.position + _throwOriginOffset;
        _elementManager.AbilityUsed = false;
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        OnPress();
    }

    public void OnPress()
    {

        if (_currentVessel.activeSelf)
        {
            _elementManager.OnAbilityActivated(_currentVessel);
            return;
        }

        PrepLaunch();
    }

    public void PauseVesselFlight()
    {
        _currentVesselRB.useGravity = false;
        _currentVesselRB.velocity = Vector3.zero;
        _currentVesselVSM.ResetActive();
    }

    public void PrepLaunch()
    {
        _launchReadied = true;
        _drawHandler.SetupTargetting(_pointerPos);
    }

    public void TrackPointer(InputAction.CallbackContext context)
    {
        _pointerPos = context.ReadValue<Vector2>();

        if (!_launchReadied) return;
        
        AdjustLaunch();
    }

    private void AdjustLaunch()
    {
        _anim.PlayReady();
        _drawHandler.AdjustTargetting(_pointerPos);
        if (_drawHandler.DrawAboveMin())
        {
            _throwVisualisation.ManageTrajectoryLine(_currentVesselRB, _throwStrength, ForceMode.Impulse);
            _launchValid = true;
        }
        else
        {
            _launchValid = false;
            _throwVisualisation.DisableLine();
            //anim.PlayWithdraw();
        }
    }

    public void OnDragReleased(InputAction.CallbackContext context)
    {
        if (!context.canceled)
        {
            return;
        }

        OnDragReleased();
    }

    public void OnDragReleased()
    {
        _launchReadied = false;
        _throwVisualisation.DisableLine();

        if (_launchValid)
        {
            PrepareVesselForLaunch();
            Launch(_currentVesselRB, ForceMode.Impulse);
            TrackVesselActiveState();
            _anim.PlayThrow();
        }
        _launchValid = false;
    }

    private void PrepareVesselForLaunch()
    {
        _currentVessel.SetActive(true);
        _currentVesselRB.useGravity = true;
    }

    private void TrackVesselActiveState()
    {
        _currentVesselVSM.OnInactive.AddListener(OnVesselInactive);
        _register.IsActiveVessel = true;
    }

    private void Launch(Rigidbody body, ForceMode forceMode)
    {
        Vector3 force = _drawHandler.DirectionVector * (_drawHandler.DrawPercentage * _throwStrength);
        body.AddForce(force, forceMode);
    }

    public void OnVesselInactive()
    {
        if (!_register.IsActiveVessel) return;

        _register.IsActiveVessel = false;
        Vessels.RemoveAt(0);
        NewThrow();
    }
}

