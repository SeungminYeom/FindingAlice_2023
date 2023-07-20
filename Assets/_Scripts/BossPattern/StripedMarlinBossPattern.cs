using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripedMarlinBossPattern : MonoBehaviour
{
    private Coroutine pattern;

    private GameObject player;

    private MeshRenderer testWarning;

    private float patternCooldown = 4f;
    private float patternWarningTime = 3f;

    private void Awake()
    {
        testWarning = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    private void Start() 
    {
    }

    private void OnEnable()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        StartCoroutine(FollowPlayer());

        if (pattern != null) pattern = null;
        pattern = StartCoroutine(Pattern());
    }

    // 플레이어의 위치를 반영해 따라 다님
    private IEnumerator FollowPlayer()
    {
        while(true)
        {
            transform.position = new Vector3(player.transform.position.x,
                                             player.transform.position.y,
                                             gameObject.transform.position.z);
            yield return null;                                        
        }
    }

    private IEnumerator Pattern()
    {
        // 0, 2가 맞음, 테스트를 위해 1, 2로 변경한 것
        int patternCase = Random.Range(1, 2);
        float patternStartTime = Time.time;

        switch (patternCase)
        {
            // 1페이즈 : 청새치 돌진
            case 0:
                break;

            // 2페이즈 : 전원 돌격
            case 1:
                // 패턴 예고
                transform.GetChild(0).gameObject.SetActive(true);
                Color color = testWarning.material.color;


                while (Time.time - patternStartTime < patternWarningTime)
                {
                    float alpha = (Time.time - patternStartTime) / patternWarningTime;
                    color = new Color(color.r, color.g, color.b, alpha);
                    Debug.Log(alpha);

                    testWarning.material.color = color;
                    yield return null;
                }
                break;
        }
        yield return new WaitForSeconds(patternCooldown);

        pattern = StartCoroutine(Pattern());
    }

    // while (pattern2_duration <= 1f)
    // {
    //     pattern.transform.position = new Vector3(player.transform.position.x,
    //                                                 player.transform.position.y,
    //                                                 pattern.transform.position.z);
    //     pattern2_time += Time.deltaTime;
    //     pattern2_duration = pattern2_time / pattern2_launchTime;
    //     patternColor.material.color = new Color(1, 0, 0, Mathf.Lerp(0f, 0.8f, pattern2_duration));
    //     yield return null;
    // }
}
