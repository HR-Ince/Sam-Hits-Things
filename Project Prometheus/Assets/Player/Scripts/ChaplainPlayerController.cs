using System.Collections.Generic;
using UnityEngine;
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
    private GameObject _currentDemon;
    private Vector2 pointerPos;

    // Component references
    private LaunchData _currentDemonLaunchData;
    private LineDrawer _throwVisualisation;
    private PlayerAnimationController _anim;
    private PlayerDrawHandler _drawHandler;
    private PlayerElementManager _elementManager;
    

    private void Awake()
    {
        FetchComponentReferences();

        _register.PlayerOne = gameObject;
        SetupDemons();
    }

    private void FetchComponentReferences()
    {
        _anim = GetComponent<PlayerAnimationController>();
        _throwVisualisation = GetComponentInChildren<LineDrawer>();
        _drawHandler = GetComponentInChildren<PlayerDrawHandler>();
        _elementManager = GetComponentInChildren<PlayerElementManager>();
    }

    private void SetupDemons()
    {
        Demons = new List<GameObject>();
        while (Demons.Count < _noOfDemons)
        {
            var temp = Instantiate(_demonPrefab, transform);
            Demons.Add(temp);
            temp.name = "Demon " + Demons.IndexOf(temp);
        }
        if (Demons.Count > 0)
        {
            SetDemonVariables(Demons[0]);

            foreach (GameObject demon in Demons)
            {
                demon.SetActive(false);
            }
        }
    }

    private void SetDemonVariables(GameObject obj)
    {
        _currentDemon = obj;
        _currentDemonLaunchData = obj.GetComponent<LaunchData>();
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        if (Demons.Count <= 0 || !context.started ||
            EventSystem.current.IsPointerOverGameObject()) return;

        if (_currentDemon.activeSelf && _currentDemon.GetComponent<VesselStateManager>().IsActive)
        {
            _elementManager.OnAbilityActivated(_currentDemon);
            return;
        }

        _launchReadied = true;
        SetDemonVariables(Demons[0]);
        _drawHandler.SetupTargetting(pointerPos);
        _currentDemon.transform.position = transform.position + _throwOriginOffset;
        _throwVisualisation.SetLaunchObjectVariables(_currentDemonLaunchData);
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
            _throwVisualisation.ManageTrajectoryLine(_throwStrength, ForceMode.Impulse);
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
            Launch(_currentDemonLaunchData.Rigidbody, ForceMode.Impulse);
            _anim.PlayThrow();
            Demons.RemoveAt(0);
        }

        _launchReadied = false;
    }

    private void Launch(Rigidbody body, ForceMode forceMode)
    {
        Vector3 force = _drawHandler.DirectionVector * (_drawHandler.DrawPercentage * _throwStrength);
        body.AddForce(force, forceMode);
    }

    private void Update()
    {
        if (_currentDemon.GetComponent<VesselStateManager>().IsActive)
        {
            _register.IsActiveVessel = true;
            return;
        }

        _register.IsActiveVessel = false;
    }

    public void RetrieveDemon()
    {
        ManageDemon();
        Demons.Add(_currentDemon);
    }

    private void ManageDemon()
    {
        _currentDemonLaunchData.Rigidbody.drag = _currentDemonLaunchData.DefaultDrag;
        _currentDemon.SetActive(false);
    }
}

