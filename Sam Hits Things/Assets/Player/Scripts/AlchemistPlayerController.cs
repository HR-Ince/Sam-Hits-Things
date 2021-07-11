using UnityEngine;

public class AlchemistPlayerController : MonoBehaviour
{
    [SerializeField] float throwStrength;
    [SerializeField] GameObject demon;
    [SerializeField] GameEvent onSphereRetrieval;
    [SerializeField] PlayerStateRegister register;

    private bool goodPress = false;
    private bool sphereThrown = false;

    private LaunchData demonLaunchData;

    private Animator anim;
    private LaunchModule launcher;
    private LineDrawer line;
    private PlayerInput input;
    private PlayerTargettingManager targeter;

    private void Awake()
    {
        FetchExternalVariables();

        register.PlayerOne = gameObject;
    }
    private void Start()
    {
        if (demon.activeInHierarchy)
        {
            demon.SetActive(false);
            demon.transform.position = transform.position;
        }
    }
    private void FetchExternalVariables()
    {
        demonLaunchData = demon.GetComponent<LaunchData>();

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
            targeter.SetupTargetting(input.PressPos);
            anim.SetTrigger("Readying");
        }
        if(goodPress && input.PressHeld)
        {
            targeter.AdjustTargetting(input.PressPos);
            if (targeter.DrawIsSufficient())
            {
                line.ManageTrajectoryLine(demonLaunchData, throwStrength, ForceMode.Impulse);
            }
            else
            {
                line.DisableLine();
            }
                
        }
        if(goodPress && input.PressReleased)
        {            
            line.DisableLine();
            anim.SetTrigger("Throwing");
            if (targeter.DrawIsSufficient())
            {
                demon.SetActive(true);
                launcher.Launch(demonLaunchData.Rigidbody, targeter, throwStrength, ForceMode.Impulse);
                sphereThrown = true;
            }            
            goodPress = false;
        }

        if(sphereThrown && input.Pressed)
        {
            anim.SetTrigger("Recalling");
            RetrieveDemon();
        }
    }

    private void RetrieveDemon()
    {
        demon.SetActive(false);
        demon.transform.position = transform.position;
        if (onSphereRetrieval != null)
            onSphereRetrieval.Invoke();
        sphereThrown = false;
    }
}
