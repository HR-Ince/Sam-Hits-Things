using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Element { None, Wind, Earth, Fire, Water }

public class PlayerElementManager : MonoBehaviour
{
    // Public variables
    public bool AbilityUsed { get { return _abilityUsed; } set { _abilityUsed = value; } }
    public Element[] HeldElements { get { return UsableElements.ToArray(); } }

    // Private variables
    [SerializeField] private float _earthForceMultiplier;
    [SerializeField] private float _windJumpStrength;
    [SerializeField] private float _waterSwapRadius;
    [SerializeField] private LayerMask _waterSwapLayerMask;
    [SerializeField] private ElementButton[] ElementButtons;

    private List<Element> UsableElements = new List<Element>();
    private Collider[] _waterSwapColliders;
    private bool _abilityUsed;
    private bool _shouldWaitForAbility;
    private Vector3 _waterVelocityCache;

    private delegate void ActivateAbility(Rigidbody rb, VesselStateManager vesselState);
    private ActivateAbility activateAbility;

    // Component references
    private ChaplainPlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<ChaplainPlayerController>();
    }

    // Injections
    private void AddUsableElement(Element elementToAdd)
    {
        if (UsableElements.Contains(elementToAdd) ||
            elementToAdd == Element.None) return;

        UsableElements.Add(elementToAdd);
    }

    public void SetElementAbility(Element elementToActivate)
    {
        print("Setting element " + elementToActivate);
        UpdateAbility(elementToActivate);
    }

    public void AddUsableElementAndUpdate(Element elementToAdd)
    {
        AddUsableElement(elementToAdd);
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < ElementButtons.Length; i++)
        {
            if (i < UsableElements.Count)
            {
                ElementButtons[i].SetAssociatedElementAndUpdate(UsableElements[i]);
                continue;
            }

            ElementButtons[i].MarkEmpty();
        }
    }

    private void UpdateAbility(Element abilityElement)
    {
        if(abilityElement == Element.None)
        {
            activateAbility = ElementAbilityNone;
            return;
        }
        else if (abilityElement == Element.Earth)
        {
            activateAbility = ElementAbilityEarth;
            return;
        }
        else if (abilityElement == Element.Fire)
        {
            activateAbility = ElementAbilityFire;
            return;
        }
        else if (abilityElement == Element.Wind)
        {
            activateAbility = ElementAbilityWind;
            return;
        }
        else if (abilityElement == Element.Water)
        {
            activateAbility = ElementAbilityWater;
            return;
        }
    }

    public void OnAbilityActivated(GameObject vessel)
    {
        if (activateAbility == null || _abilityUsed == true) return;

        activateAbility(vessel.GetComponent<Rigidbody>(), vessel.GetComponent<VesselStateManager>());
        if(!_shouldWaitForAbility) _abilityUsed = true;
    }

    private void ElementAbilityNone(Rigidbody rb, VesselStateManager vesselState)
    {
        print("No ability");
    }

    private void ElementAbilityEarth(Rigidbody rb, VesselStateManager vesselState)
    {
        print("Earth ability");
        
        if (!vesselState.IsGrounded)
        {
            rb.velocity = Vector3.down * _earthForceMultiplier;
            return;
        }

        rb.velocity *= _earthForceMultiplier;
    }

    private void ElementAbilityWind(Rigidbody rb, VesselStateManager vesselState)
    {
        if (!vesselState.IsGrounded)
        {
            _playerController.PauseVesselFlight();
            _playerController.PrepLaunch();
            
            return;
        }

        rb.AddForce(Vector3.up * _windJumpStrength, ForceMode.VelocityChange);
    }

    private void ElementAbilityFire(Rigidbody rb, VesselStateManager vesselState)
    {
        print("Fire ability");
    }

    private void ElementAbilityWater(Rigidbody rb, VesselStateManager vesselState)
    {
        _waterVelocityCache = rb.velocity;
        _playerController.PauseVesselFlight();

        _waterSwapColliders = Physics.OverlapSphere(rb.transform.position, _waterSwapRadius, _waterSwapLayerMask);
        activateAbility = ElementAbilityWaterSwap;
        _shouldWaitForAbility = true;
        
    }

    private void ElementAbilityWaterSwap(Rigidbody rb, VesselStateManager vesselState)
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(_playerController.PointerPos), out RaycastHit hit, 100f, _waterSwapLayerMask);
        if (_waterSwapColliders.Contains(hit.collider))
        {
            // Swap positions
            Vector3 cachedPos = hit.transform.position;
            hit.transform.position = rb.transform.position;
            rb.transform.position = cachedPos;
            // Reset rigidbody
            rb.useGravity = true;
            rb.velocity = _waterVelocityCache;

            _shouldWaitForAbility = false;
        }
    }
}
