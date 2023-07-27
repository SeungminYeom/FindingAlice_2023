using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [System.Serializable]
    private enum FishType
    {
        FollowingFish
    }

    [SerializeField] private FishType fishType;

    [Header("FollowingFish")]
    [SerializeField] private float followSpeed = 3.0f; 
    [SerializeField] private float returnSpeed = 6.0f;
    private Vector3 originalPosition;
    private GameObject playerObject;
    private Transform playerTransform;

    private void Start()
    {
        originalPosition = transform.position;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;

        switch (fishType)
        {
            case FishType.FollowingFish:
                GetComponent<Collider>().isTrigger = true;
                break;  
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (fishType)
            {
                case FishType.FollowingFish:
                    transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, followSpeed * Time.deltaTime);
                    break;
            }
         
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (fishType)
            {
                case FishType.FollowingFish:
                    StartCoroutine(ReturnOriginalPos());
                    break;
            }
        }
    }

    private IEnumerator ReturnOriginalPos()
    {
        while (transform.position != originalPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }
    }
    
}