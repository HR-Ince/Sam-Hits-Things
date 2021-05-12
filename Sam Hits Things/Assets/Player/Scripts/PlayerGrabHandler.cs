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

    public GameObject HeldObject { get { return heldObject; } }

    Collider[] collidersInReach;
    List<TMP_Text> labels = new List<TMP_Text>();
    List<Collider> collectableObjects = new List<Collider>();

    private GameObject objToGrab;
    private GameObject heldObject;

    private Camera cam;
    private Canvas canvas;

    private void Awake()
    {
        FetchExternalReferences();

        labels.Add(interactionText);
        interactionText.enabled = false;
    }
    private void FetchExternalReferences()
    {
        cam = Camera.main;

        canvas = FindObjectOfType<Canvas>();
    }
    private void Update()
    {
        if(heldObject != null)
            heldObject.transform.position = transform.position;
    }
    public void ManageCollectableObjects()
    {
        collidersInReach = Physics.OverlapBox(transform.position, Vector3.one * reach, Quaternion.identity, interactableLayerMask);

        foreach (Collider collider in collidersInReach)
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
        
        if (collidersInReach.Length <= 0)
        {
            collectableObjects.Clear();
            return;
        }
            
        
        for(int i = 0; i < collectableObjects.Count; i++)
        {
            bool contains = false;
            for (int j = 0; j < collidersInReach.Length; j++)
            {
                if (collectableObjects[i] == collidersInReach[j])
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

        LabelCollectableObjects("Pick up");
    }
    private void LabelCollectableObjects(string text)
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
    public void DisableLabels()
    {
        for (int i = 0; i < labels.Count; i++)
        {
            labels[i].enabled = false;
        }
    }
    public bool CollectableObjectPressed()
    {
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f, interactableLayerMask))
        {
            foreach (Collider collider in collectableObjects)
                if (collider == hit.collider)
                {
                    objToGrab = collider.gameObject;
                    return true;
                }
                else
                {
                    return false;
                }
        }
        return false;
    }
    public void Grab()
    {
        heldObject = objToGrab;
        heldObject.transform.rotation = Quaternion.identity;
    }
    public void ReleaseHeldObject()
    {
        heldObject = null;
        interactionText.enabled = false;
    }
}
