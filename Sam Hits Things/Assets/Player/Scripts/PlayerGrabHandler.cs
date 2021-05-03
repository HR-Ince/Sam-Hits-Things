using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabHandler : MonoBehaviour
{
    [SerializeField] float reach;
    [SerializeField] Vector2 discardForce;
    [SerializeField] TMP_Text interactionText;
    [SerializeField] Vector3 interactionTextOffset;
    [SerializeField] LayerMask interactableLayerMask;

    Collider[] collidersInReach;
    List<TMP_Text> labels = new List<TMP_Text>();

    private GameObject heldObject;

    private Camera cam;
    private Canvas canvas;
    private PlayerInput input;
    private PlayerStateManager state;

    private void Awake()
    {
        FetchExternalReferences();

        labels.Add(interactionText);
        interactionText.enabled = false;
    }
    private void FetchExternalReferences()
    {
        cam = Camera.main;
        input = GetComponentInParent<PlayerInput>();
        state = GetComponentInParent<PlayerStateManager>();
        if (state == null) { Debug.LogError("State manager missing from player"); }

        canvas = FindObjectOfType<Canvas>();
    }
    private void LabelObjects(Collider[] colliders, String text)
    {
        if (labels.Count < colliders.Length)
        {
            TMP_Text TMP = Instantiate(interactionText, canvas.transform);
            labels.Add(TMP);
        }
        else if(labels.Count > colliders.Length)
        {
            for (int i = labels.Count - 1; i >= collidersInReach.Length; i--)
            {
                labels[i].enabled = false;
            }
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            labels[i].transform.position = cam.WorldToScreenPoint(colliders[i].transform.position + interactionTextOffset);
            labels[i].text = text;
            labels[i].enabled = true;
        }
    }
    private void Update()
    {
        if (state.IsGrounded)
        {
            if (!state.IsBurdened)
            {
                collidersInReach = Physics.OverlapBox(transform.position, Vector3.one * reach, Quaternion.identity, interactableLayerMask);

                LabelObjects(collidersInReach, "Pick up");                        
            }

            if (input.Pressed)
            {
                if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f, interactableLayerMask))
                {
                    if (!state.IsBurdened)
                    {
                        foreach (Collider collider in collidersInReach)
                            if (collider == hit.collider)
                                Grab(hit.transform.gameObject);
                    }
                    else
                    {
                        if (hit.transform.gameObject == heldObject)
                            ReleaseHeldObject();
                    }
                }
            }
        }
        else
        {
            int switchStart;
            if (state.IsBurdened)
                switchStart = 1;
            else
                switchStart = 0;
            for (int i = switchStart; i < labels.Count; i++)
            {
                labels[i].enabled = false;
            }
        }

        if (state.IsBurdened)
        {
            heldObject.transform.position = transform.position;
            interactionText.transform.position = cam.WorldToScreenPoint(heldObject.transform.position + interactionTextOffset);
        }
    }
    private void Grab(GameObject obj)
    {
        heldObject = obj;
        heldObject.transform.parent = transform;
        state.SetIsBurdened(true);

        for (int i = 1; i < labels.Count; i++)
        {
            labels[i].enabled = false;
        }

        if (interactionText.enabled == false)
        {
            interactionText.enabled = true;
        }
        
        interactionText.text = "Discard";
    }
    private void ReleaseHeldObject()
    {
        print("Release");
        heldObject.transform.parent = null;
        heldObject.transform.position = transform.TransformPoint(Vector3.right * reach);
        heldObject = null;
        state.SetIsBurdened(false);
        interactionText.enabled = false;
    }
}
