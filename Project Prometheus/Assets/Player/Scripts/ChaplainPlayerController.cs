using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] ActiveObjects actives;

    public GameObject[] Demons { get { return demons.ToArray(); } }

    private bool goodPress = false;

    private List<GameObject> demons;
    private LaunchData demonLaunchData;

    private PlayerAnimationController anim;
    private LaunchModule launcher;
    private LineDrawer line;
    private PlayerInput input;
    private PlayerTargettingManager targeter;


    private void Awake()
    {
        FetchExternalVariables();

        register.PlayerOne = gameObject;
        SetupDemons();
    }
    private void SetupDemons()
    {
        demons = new List<GameObject>();
        demons.Add(demon);
        while (demons.Count < noOfDemons)
        {
            var temp = Instantiate(demon);
            demons.Add(temp);
            temp.name = "Demon " + demons.IndexOf(temp);
        }
        if (demons.Count > 0)
        {
            SetDemonVariables(demons[0]);

            foreach (GameObject demon in demons)
            {
                if (demon.TryGetComponent(out DemonController controller))
                {
                    controller.SetContextMenu(demonContextMenu);
                }

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
    private void FetchExternalVariables()
    {
        anim = GetComponent<PlayerAnimationController>();
        input = GetComponent<PlayerInput>();

        launcher = GetComponentInChildren<LaunchModule>();
        line = GetComponentInChildren<LineDrawer>();
        targeter = GetComponentInChildren<PlayerTargettingManager>();
    }

    private void SetDemonVariables(GameObject obj)
    {
        demon = obj;
        demonLaunchData = demon.GetComponent<LaunchData>();
    }

    private void Update()
    {
        if (demons.Count > 0 && input.GamePressed)
        {
            SetDemonVariables(demons[0]);
            goodPress = true;
            targeter.SetupTargetting(input.PressPos);
            demon.transform.position = transform.position + throwOriginOffset;
            line.SetLaunchObjectVariables(demonLaunchData);
        }
        if (goodPress && input.PressHeld)
        {
            anim.PlayReady();

            targeter.AdjustTargetting(input.PressPos);
            if (targeter.DrawIsSufficient())
            {
                line.ManageTrajectoryLine(throwStrength, ForceMode.Impulse);
            }
            else
            {
                line.DisableLine();
                //anim.PlayWithdraw();
            }

        }
        if (goodPress && input.PressReleased)
        {
            line.DisableLine();
            if (targeter.DrawIsSufficient())
            {
                demon.SetActive(true);
                launcher.Launch(demonLaunchData.Rigidbody, targeter, throwStrength, ForceMode.Impulse);
                anim.PlayThrow();
                demons.RemoveAt(0);
            }
            goodPress = false;
        }
    }

    public void RetrieveDemon()
    {
        ManageDemon();
        demons.Add(demon);
    }

    private void ManageDemon()
    {
        SetDemonVariables(actives.ActiveDemon);
        demon.GetComponent<DemonController>().ResetAssociations();
        actives.SetActiveShrine(null);
        demonLaunchData.Rigidbody.drag = demonLaunchData.DefaultDrag;
        demon.SetActive(false);
    }
}

