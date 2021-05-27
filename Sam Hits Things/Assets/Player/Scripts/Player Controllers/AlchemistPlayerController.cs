using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemistPlayerController : MonoBehaviour
{
    [SerializeField] float throwStrength;
    [SerializeField] GameObject sphere;
    [SerializeField] PlayerStateRegister register;

    private bool goodPress = false;
    private bool sphereThrown = false;
    
    private float sphereMass;
    private Rigidbody sphereBody;
    
    private LaunchModule launcher;
    private LineDrawer line;
    private PlayerInput input;
    private PlayerTargettingManager targeter;

    private void Awake()
    {
        FetchExternalVariables();

        if (sphere.activeInHierarchy)
        {
            sphere.SetActive(false);
            sphere.transform.position = transform.position;
        }

        register.PlayerOne = gameObject;
    }
    private void FetchExternalVariables()
    {
        sphereBody = sphere.GetComponent<Rigidbody>();
        sphereMass = sphereBody.mass;

        input = GetComponent<PlayerInput>();

        launcher = GetComponentInChildren<LaunchModule>();
        line = GetComponentInChildren<LineDrawer>();
        targeter = GetComponentInChildren<PlayerTargettingManager>();
    }

    private void Update()
    {
        if(!sphereThrown && input.Pressed)
        {
            goodPress = true;
            targeter.SetupTargetting(transform.position, input.PressPos);
        }
        if(goodPress && input.PressHeld)
        {
            targeter.AdjustTargetting(input.PressPos);
            line.ManageTrajectoryLine(throwStrength, 0, sphereMass, ForceMode.Impulse);
        }
        if(goodPress && input.PressReleased)
        {            
            line.DisableLine();
            if (targeter.DrawIsSufficient())
            {
                sphere.SetActive(true);
                launcher.Launch(sphereBody, targeter.DirectionVector, targeter.DrawPercentage, throwStrength, ForceMode.Impulse);
            }

            goodPress = false;
        }
    }
}
