using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class SpiderPlayerController : PlayerControllerBase, IPerishable
{
    private TargettingWithOrbit orbitTargetting;
 
    private void Awake()
    {
        FetchExternalVariables();
    }
    private new void FetchExternalVariables()
    {
        base.FetchExternalVariables();
        orbitTargetting = targetting as TargettingWithOrbit;
        if(orbitTargetting == null) { Debug.LogError("Targetting manager missing from children of player"); }

    }

    private void Update()
    {
        GetIsGrounded();

        if (isGrounded)
        {
            if (input.Pressed)
            {
                orbitTargetting.SetupTargetting(transform.position, input.PressPos);
            }

            if (input.PressHeld)
            {
                orbitTargetting.AdjustTargetting(input.PressPos);
                if (orbitTargetting.DrawIsSufficient())
                {
                    line.ManageTrajectoryLine(orbitTargetting.DirectionVector, orbitTargetting.DrawPercentage, launcher.ThrustModifier, _rigidbody.mass);
                    line.enabled = true;
                }
                else
                    line.enabled = false;
            }
            if (input.PressReleased)
            {
                line.DisableLine();
                if (orbitTargetting.DrawIsSufficient())
                {
                    launcher.Launch(_rigidbody, orbitTargetting.DirectionVector, orbitTargetting.DrawPercentage);
                }
            }
        }
        else
        {
            if (orbitTargetting.HasOrbitTarget())
            {
                if (input.Pressed && orbitTargetting.HasOrbitTarget())
                {
                    _rigidbody.velocity = Vector3.zero;
                    _rigidbody.useGravity = false;

                    orbitTargetting.GetOrbitCentre();
                }
                if (input.PressHeld)
                {
                    orbitTargetting.AdjustTargetting(input.PressPos);
                }
            }
        }
    }
}
