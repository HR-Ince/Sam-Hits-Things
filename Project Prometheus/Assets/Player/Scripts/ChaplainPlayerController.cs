using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaplainPlayerController : MonoBehaviour
{
    [SerializeField] int noOfDemons;
    [SerializeField] float throwStrength;
    [SerializeField] GameObject demon;
    [SerializeField] Vector3 throwOriginOffset, summonOriginOffset;
    [SerializeField] PlayerStateRegister register;
    [SerializeField] ActiveObjects demonHolder;

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

        demons = new List<GameObject>();
        demons.Add(demon);
        while(demons.Count < noOfDemons)
        {
            var temp = Instantiate(demon);
            demons.Add(temp);
            temp.SetActive(false);
        }
        if (demons.Count > 0) { SetDemonVariables(demons[0]); }
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
        if (demons.Count > 0 && input.Pressed)
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
        SetDemonVariables(demonHolder.ActiveDemon);
        demon.SetActive(false);
        demons.Add(demon);
    }
}

