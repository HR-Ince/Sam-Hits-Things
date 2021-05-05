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
    [SerializeField] LayerMask interactableLayerMask, playerLayerMask;

    public bool IsBurdened { get { return isBurdened; } }
    public GameObject HeldObject { get { return heldObject; } }

    Collider[] collidersInReach;
    List<TMP_Text> labels = new List<TMP_Text>();
    List<Collider> collectableObjects = new List<Collider>();

    private bool isBurdened;
    private GameObject heldObject;

    private Camera cam;
    private Canvas canvas;
    private PlayerInput input;
    private PlayerController state;

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
        state = GetComponentInParent<PlayerController>();
        if (state == null) { Debug.LogError("State manager missing from player"); }

        canvas = FindObjectOfType<Canvas>();
    }
    private void ManageCollectableObjects(Collider[] colliders)
    {
        foreach(Collider collider in colliders)
        {
            Vector3 playerDirection = (transform.parent.position - collider.transform.position).normalized;
            Physics.Raycast(collider.transform.position, playerDirection, out RaycastHit hit, reach);

            if (hit.transform == transform.parent && !collectableObjects.Contains(collider))
            {
                collectableObjects.Add(collider);
            }
            else if(hit.transform != transform.parent && collectableObjects.Contains(collider))
            {
                collectableObjects.Remove(collider);
            }
        }

        if(collectableObjects.Count <= 0) { return; }
        
        if (colliders.Length <= 0)
        {
            collectableObjects.Clear();
            return;
        }
            
        
        for(int i = 0; i < collectableObjects.Count; i++)
        {
            bool contains = false;
            for (int j = 0; j < colliders.Length; j++)
            {
                if (collectableObjects[i] == colliders[j])
                {
                    contains = true;
                    break;
                }                    
            }

            if (!contains)
            {
                collectableObjects.Remove(collectableObjects[i]);
            }
        }
    }
    private void LabelObjects(string text)
    {
        if (labels.Count < collectableObjects.Count)
        {
            TMP_Text TMP = Instantiate(interactionText, canvas.transform);
            labels.Add(TMP);
        }
        if(labels.Count > collectableObjects.Count)
        {
            for (int i = labels.Count - 1; i >= collectableObjects.Count; i--)
            {
                labels[i].enabled = false;
            }
        }
        for (int i = 0; i < collectableObjects.Count; i++)
        {
            labels[i].transform.position = cam.WorldToScreenPoint(collectableObjects[i].transform.position + interactionTextOffset);
            labels[i].text = text;
            labels[i].enabled = true;
        }
    }
    private void Update()
    {
        if (heldObject != null)
            isBurdened = true;
        else
        {
            isBurdened = false;
        }

        if (state.IsGrounded)
        {
            if (!isBurdened)
            {
                collidersInReach = Physics.OverlapBox(transform.position, Vector3.one * reach, Quaternion.identity, interactableLayerMask);

                ManageCollectableObjects(collidersInReach);
                LabelObjects("Pick up");
            }
            else
            {
                for (int i = 0; i < labels.Count; i++)
                {
                    labels[i].enabled = false;
                }
            }

            if (input.Pressed)
            {
                if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f, interactableLayerMask))
                {
                    if (!isBurdened)
                    {
                        foreach (Collider collider in collectableObjects)
                            if (collider == hit.collider)
                            {
                                Grab(hit.transform.gameObject);
                            }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < labels.Count; i++)
            {
                labels[i].enabled = false;
            }
        }
    }
    private void Grab(GameObject obj)
    {
        heldObject = obj;
        heldObject.transform.position = transform.position;
        heldObject.transform.rotation = Quaternion.identity;
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    public void ReleaseHeldObject()
    {
        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject = null;
        interactionText.enabled = false;
    }
}
