using UnityEngine;

public class AlchemistPlayerController : MonoBehaviour
{
    [SerializeField] float throwStrength;
    [SerializeField] GameObject sphere;
    [SerializeField] GameEvent onSphereRetrieval;
    [SerializeField] PlayerStateRegister register;

    private bool goodPress = false;
    private bool sphereThrown = false;
    
    private float sphereMass;
    private Rigidbody sphereBody;

    private Animator anim;
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

        anim = GetComponent<Animator>();
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
            anim.SetTrigger("Readying");
        }
        if(goodPress && input.PressHeld)
        {
            targeter.AdjustTargetting(input.PressPos);
            line.ManageTrajectoryLine(throwStrength, 0, sphereMass, ForceMode.Impulse);
        }
        if(goodPress && input.PressReleased)
        {            
            line.DisableLine();
            anim.SetTrigger("Throwing");
            if (targeter.DrawIsSufficient())
            {
                sphere.SetActive(true);
                launcher.Launch(sphereBody, targeter.DirectionVector, targeter.DrawPercentage, throwStrength, ForceMode.Impulse);
            }
            sphereThrown = true;
            goodPress = false;
        }

        if(sphereThrown && input.Pressed)
        {
            anim.SetTrigger("Recalling");
        }
    }

    private void RetrieveSphere()
    {
        sphere.SetActive(false);
        sphere.transform.position = transform.position;
        if (onSphereRetrieval != null)
            onSphereRetrieval.Invoke();
        sphereThrown = false;
    }
}
