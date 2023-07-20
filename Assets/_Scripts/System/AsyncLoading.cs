using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


// ===================================================================================================
// 비동기 로딩을 위한 로딩씬에 사용되는 스크립트
//
// 씬 전환 시 아래와 같이 사용한다.
// AsyncLoading.LoadScene("전환될 씬 이름");
// ===================================================================================================

public class AsyncLoading : MonoBehaviour
{
    public static string nextScene;

    void Start()
    {
        // 로딩 문구
        GameObject.Find("Comment").GetComponent<TextMeshProUGUI>().text
            = DataController.instance.loadingTexts.textData[Random.Range(0, DataController.instance.loadingTexts.textData.Count)].text;

        StartCoroutine(LoadScene());
    }

    // ===============================================================================================
    // 다른 씬에서 접근하는 함수
    // ===============================================================================================
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    // ===============================================================================================
    // 비동기 로드
    // ===============================================================================================
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
            if (op.progress >= 0.9f)
            {
                // 로딩이 끝나도 약간의 여유를 주기 위한 가짜 로딩 시간
                yield return new WaitForSeconds(1f);

                op.allowSceneActivation = true;

                yield break;
            }
        }
    }
}