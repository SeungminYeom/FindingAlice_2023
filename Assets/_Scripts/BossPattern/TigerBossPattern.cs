using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerBossPattern : MonoBehaviour
{
    Coroutine pattern;

    float patternCooldown = 5f;
    float patternWarningTime = 3f;

    private void OnEnable()
    {
        if (pattern != null) pattern = null;
        pattern = StartCoroutine(Pattern());
    }

    private IEnumerator Pattern()
    {
        int patternCase = Random.Range(0, 2);
        float patternStartTime = Time.time;

        switch (patternCase)
        {
            // 할퀴기
            case 0:
                // 패턴 예고
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).rotation = Quaternion.Euler(0, 0, Random.Range(0f, 180f));
                Color color = GetComponent<MeshRenderer>().material.color;
                while (Time.time - patternStartTime < patternWarningTime)
                {
                    color.a = (Time.time - patternStartTime) / patternWarningTime;

                    yield return null;
                }
                break;

            // 돌 던지기
            case 1:
                break;
        }

        yield return new WaitForSeconds(patternCooldown);

        pattern = StartCoroutine(Pattern());
    }
}
