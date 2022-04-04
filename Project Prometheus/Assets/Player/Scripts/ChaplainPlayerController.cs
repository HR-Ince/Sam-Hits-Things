using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ChaplainPlayerController : MonoBehaviour
{
    [Header("Human things")]
    [SerializeField] float _throwStrength;
    [Header("Demon things")]
    [SerializeField] int _noOfDemons;
    [SerializeField] GameObject _demonPrefab;
    [SerializeField] Vector3 _throwOriginOffset;
    [Header("Externals")]
    [SerializeField] PlayerStateRegister _register;

    // Private variables
    private List<GameObject> Demons;
    private bool _launchReadied = false;
    private bool _isWindLaunch = false;
    private GameObject _currentDemon;
    private Vector2 pointerPos;

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
        Demons = new List<GameObject>();
        while (Demons.Count < _noOfDemons)
        {
            var temp = Instantiate(_demonPrefab, transform);
            Demons.Add(temp);
            temp.name = "Demon " + Demons.IndexOf(temp);
            temp.SetActive(false);
        }
        if (Demons.Count > 0)
        {
            NewThrow();
        }
    }

    public void NewThrow()
    {
        print(Demons.Count);
        if(_currentVesselVSM)
            _currentVesselVSM.OnInactive.RemoveListener(OnVesselInactive);
        Demons.RemoveAt(0);
        _currentDemon = Demons[0];
        _currentVesselRB = _currentDemon.GetComponent<Rigidbody>();
        _currentVesselVSM = _currentDemon.GetComponent<VesselStateManager>();
        _currentDemon.transform.position = transform.position + _throwOriginOffset;
        _elementManager.AbilityUsed = false;
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        if (Demons.Count <= 0 || !context.started ||
            EventSystem.current.IsPointerOverGameObject()) return;

        if (_currentDemon.activeSelf)
        {
            _elementManager.OnAbilityActivated(_currentDemon);
            return;
        }

        PrepLaunch();
    }

    public void WindLaunchPrep()
    {
        _isWindLaunch = true;
        _currentVesselRB.useGravity = false;
        _currentVesselRB.velocity = Vector3.zero;
        _currentVesselVSM.ResetActive();
        PrepLaunch();
    }

    private void PrepLaunch()
    {
        _launchReadied = true;
        _drawHandler.SetupTargetting(pointerPos);
    }

    public void TrackPointer(InputAction.CallbackContext context)
    {
        pointerPos = context.ReadValue<Vector2>();

        if (_launchReadied)
        {
            AdjustLaunch();
        }
    }

    private void AdjustLaunch()
    {
        _anim.PlayReady();
        _drawHandler.AdjustTargetting(pointerPos);
        if (_drawHandler.DrawIsSufficient())
        {
            _throwVisualisation.ManageTrajectoryLine(_currentVesselRB, _throwStrength, ForceMode.Impulse);
        }
        else
        {
            _throwVisualisation.DisableLine();
            //anim.PlayWithdraw();
        }
    }

    public void OnDragReleased(InputAction.CallbackContext context)
    {
        if(!context.canceled || !_launchReadied) return;

        _throwVisualisation.DisableLine();
        if (_drawHandler.DrawIsSufficient())
        {
            _currentDemon.SetActive(true);
            _currentVesselVSM.OnInactive.AddListener(OnVesselInactive);
            Launch(_currentVesselRB, ForceMode.Impulse);
            _anim.PlayThrow();
        }

        if (_isWindLaunch)
        {
            _currentVesselRB.useGravity = true;
        } 
        _launchReadied = false;
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
        NewThrow();
    }
}

