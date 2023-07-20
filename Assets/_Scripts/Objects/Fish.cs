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
    [SerializeField] private bool isFollowing = false;
    private Vector3 originalPosition;
    private GameObject playerObject;
    private Transform playerTransform;

    private void Start()
    {
        originalPosition = transform.position;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObject.transform;
        StartCoroutine(FishUpdate());
    }

    private IEnumerator FishUpdate()
    {
        switch (fishType)
        {
            case FishType.FollowingFish:
                while (true)
                {
                    if (!isFollowing)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
                    }

                    yield return new WaitForFixedUpdate();
                }
            default:
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (fishType)
            {
                case FishType.FollowingFish:
                    isFollowing = true;
                    transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, followSpeed * Time.deltaTime);
                    break;
                default:
                    break;
            }
         
        }

    }

    private void OnTriggerExit(Collider other)
    {
        switch (fishType)
        {
            case FishType.FollowingFish:
                isFollowing = false;
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, returnSpeed * Time.deltaTime);
                break;
            default:
                break;
        }
    }
}