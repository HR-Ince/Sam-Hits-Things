using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChaplainPlayerController : MonoBehaviour
{
    [Header("Human things")]
    [SerializeField] float throwStrength;
    [Header("Demon things")]
    [SerializeField] int noOfDemons;
    [SerializeField] GameObject demon;
    [SerializeField] GameObject demonContextMenu;
    [SerializeField] Vector3 throwOriginOffset;
    [Header("Externals")]
    [SerializeField] PlayerStateRegister register;

    // Public variables
    public GameObject[] PlayerDemons { get { return Demons.ToArray(); } }

    // Private variables
    private List<GameObject> Demons;
    private bool _launchReadied = false;
    private Vector2 pointerPos;
    
    // Component references
    private PlayerAnimationController _anim;
    private LineDrawer _line;
    private PlayerDrawHandler _drawHandler;
    private PlayerInput _input;
    private Pointer _pointer;
    private LaunchData demonLaunchData;

    private void Awake()
    {
        FetchComponentReferences();

        register.PlayerOne = gameObject;
        SetupDemons();
    }

    private void FetchComponentReferences()
    {
        _anim = GetComponent<PlayerAnimationController>();
        _input = new PlayerInput();
        _pointer = Pointer.current;
        _line = GetComponentInChildren<LineDrawer>();
        _drawHandler = GetComponentInChildren<PlayerDrawHandler>();
    }

    private void SetupDemons()
    {
        Demons = new List<GameObject>();
        Demons.Add(demon);
        while (Demons.Count < noOfDemons)
        {
            var temp = Instantiate(demon, transform);
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

    private void Start()
    {
        if (demon.activeInHierarchy)
        {
            demon.SetActive(false);
            demon.transform.position = transform.position + throwOriginOffset;
        }
    }

    private void SetDemonVariables(GameObject obj)
    {
        demon = obj;
        demonLaunchData = demon.GetComponent<LaunchData>();
    }

    public void OnPress(InputAction.CallbackContext context)
    {
        if (Demons.Count <= 0 || !context.started) return;

        _launchReadied = true;
        SetDemonVariables(Demons[0]);
        _drawHandler.SetupTargetting(pointerPos);
        demon.transform.position = transform.position + throwOriginOffset;
        _line.SetLaunchObjectVariables(demonLaunchData);
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
            _line.ManageTrajectoryLine(throwStrength, ForceMode.Impulse);
        }
        else
        {
            _line.DisableLine();
            //anim.PlayWithdraw();
        }
    }

    public void OnDragReleased(InputAction.CallbackContext context)
    {
        if(!context.canceled) return;

        _line.DisableLine();
        if (_drawHandler.DrawIsSufficient())
        {
            demon.SetActive(true);
            Launch(demonLaunchData.Rigidbody, ForceMode.Impulse);
            _anim.PlayThrow();
            Demons.RemoveAt(0);
        }

        _launchReadied = false;
    }

    private void Launch(Rigidbody body, ForceMode forceMode)
    {
        Vector3 force = _drawHandler.DirectionVector * (_drawHandler.DrawPercentage * throwStrength);
        body.AddForce(force, forceMode);
    }

    public void RetrieveDemon()
    {
        ManageDemon();
        Demons.Add(demon);
    }

    private void ManageDemon()
    {
        demonLaunchData.Rigidbody.drag = demonLaunchData.DefaultDrag;
        demon.SetActive(false);
    }
}

