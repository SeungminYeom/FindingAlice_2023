using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ===================================================================================================
// 게임이 실행되었을 때 Title 씬에서 동작하는 스크립트
// ===================================================================================================

public class Initialization : MonoBehaviour
{
    private bool gameStartReady;

    //private void Awake()
    //{
    //    Camera camera = GetComponent<Camera>();
    //    Rect rect = camera.rect;
    //    float scaleheight = ((float)Screen.width / Screen.height) / ((float)20 / 9); // (가로 / 세로)
    //    float scalewidth = 1f / scaleheight;
    //    if (scaleheight < 1)
    //    {
    //        rect.height = scaleheight;
    //        rect.y = (1f - scaleheight) / 2f;
    //    }
    //    else
    //    {
    //        rect.width = scalewidth;
    //        rect.x = (1f - scalewidth) / 2f;
    //    }
    //    camera.rect = rect;
    //}

    //void OnPreCull() => GL.Clear(true, true, Color.black);

    private void Start()
    {
        gameStartReady = true;
    }

    public void StartGame()
    {
        if (gameStartReady)
        {
            AsyncLoading.LoadScene("ChapterSelect");
        }
    }
}
