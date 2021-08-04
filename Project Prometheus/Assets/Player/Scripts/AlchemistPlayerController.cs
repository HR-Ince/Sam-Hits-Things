using UnityEngine;

public class AlchemistPlayerController : MonoBehaviour
{
    [SerializeField] float throwStrength;
    [SerializeField] GameObject demon;
    [SerializeField] Vector3 throwOriginOffset, summonOriginOffset;
    [SerializeField] PlayerStateRegister register;

    private bool goodPress = false;
    private bool demonThrown = false;

    private LaunchData demonLaunchData;
    private Rigidbody demonBody;
    private SpriteRenderer demonSprite;

    private PlayerAnimationController anim;
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
            demonSprite.enabled = false;
            demon.transform.position = transform.position + throwOriginOffset;
        }
    }
    private void FetchExternalVariables()
    {
        demonLaunchData = demon.GetComponent<LaunchData>();
        demonBody = demon.GetComponent<Rigidbody>();
        demonSprite = demon.GetComponent<SpriteRenderer>();

        anim = GetComponent<PlayerAnimationController>();
        input = GetComponent<PlayerInput>();

        launcher = GetComponentInChildren<LaunchModule>();
        line = GetComponentInChildren<LineDrawer>();
        targeter = GetComponentInChildren<PlayerTargettingManager>();
    }

    private void Update()
    {
        if(!demonThrown && input.GamePressed)
        {
            goodPress = true;
            targeter.SetupTargetting(input.PressPos);
            demon.transform.position = transform.position + throwOriginOffset;
            line.SetLaunchObjectVariables(demonLaunchData);
        }
        if(goodPress && input.PressHeld)
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
        if(goodPress && input.PressReleased)
        {            
            line.DisableLine();
            anim.PlayThrow();
            if (targeter.DrawIsSufficient())
            {
                demonSprite.enabled = true;
                launcher.Launch(demonLaunchData.Rigidbody, targeter, throwStrength, ForceMode.Impulse);
                demonThrown = true;
            }

            goodPress = false;
        }

        if(demonThrown && input.GamePressed)
        {
            anim.PlayRecall();
        }
    }

    public void RetrieveDemon()
    {
        demonBody.velocity = Vector3.zero;
        demonBody.angularVelocity = Vector3.zero;
        demon.transform.position = transform.position + summonOriginOffset;
        anim.PlaySummon();
        demonThrown = false;
    }
}
