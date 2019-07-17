﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldInteractions : MonoBehaviour
{
    public GameObject player;
    public GameObject npc;

    /* NavMesh_ allows the player to walk on a path */
    [HideInInspector] public Rigidbody playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
<<<<<<< HEAD
        /* Checks if the mouse is hovering over the object. */
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())  
        {
            GetInteractions();
=======
        // distance = Vector3.Distance(player.transform.position, npc.transform.position);

        if ( /*distance >= 5 && */ Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            
            if (distance > 5)
            {
                // Debug.Log("You must get closer to speak with this NPC.");
            }
            else
            {
                GetInteractions();
            }
>>>>>>> 0d010d75a250b66a92269173638113bd8962d523
        }
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red);
    }

    void GetInteractions()
    { 
        /* Takes the coordinate of the mouse, sends out a ray in the screen space of the viewport.*/
        Ray interactionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        /* Store the information we get back. */
        RaycastHit interactionInfo;
        /* Pass the camera, pass the raycast hit and output the information of the raycast object, how far does the ray go? inifnite). */
        if (Physics.Raycast(interactionRay, out interactionInfo, Mathf.Infinity))
        {
            /* Stores the object we hit: goes through the component collider. */
            GameObject interactedObject = interactionInfo.collider.gameObject;

            if (interactedObject.tag == "Enemy")
            {
                /* Component derived from interactable. */
                interactedObject.GetComponent<Interactable>().GetContact(playerRigidbody);
            }
            else if (interactedObject.tag == "Interactable Object")
            {
                interactedObject.GetComponent<Interactable>().GetContact(playerRigidbody);
            }
            else
            {
                 //Debug.Log("Not clicking interactables..");
                 //playerAgent.stoppingDistance = 0;
                 //playerAgent.destination = interactionInfo.point;           
            }
        }
    }
}
