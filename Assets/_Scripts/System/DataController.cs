using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


// ===================================================================================================
// Save 데이터를 관리하는 스크립트
//
// 반드시 게임이 최초 실행되는 씬에 존재해야 한다
//
// bit 플래그 연산을 통해 Save 데이터를 관리한다
// ===================================================================================================

public class DataController : MonoBehaviour
{
    public static DataController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        gameData     = JsonUtility.FromJson<GameData>(Resources.Load<TextAsset>("Json/GameData").text);
        loadingTexts = JsonUtility.FromJson<LoadingTexts>(Resources.Load<TextAsset>("Json/Script").text);
    }

    // 로딩문구를 저장하는 클래스
    [System.Serializable]
    public class TextData
    {
        public string text;
    }

    // 로딩문구를 저장하는 클래스의 List 클래스
    [System.Serializable]
    public class LoadingTexts
    {
        public List<TextData> textData;
    }

    // 게임 데이터를 저장하는 클래스
    [System.Serializable]
    public class GameData
    {
        public int      totalProgressFlag;      // 전체 게임 진행도

        public int      chapterFlag_CT;         // 튜토리얼 챕터 진행도
        public int      chapterFlag_C1;         // 챕터1 진행도
        public int      chapterFlag_C2;         // 챕터2 진행도
        public int      chapterFlag_C3;         // 챕터3 진행도

        public int      collectionFlag_CT;
        public int      collectionFlag_C1;
        public int      collectionFlag_C2;
        public int      collectionFlag_C3;
                        
        public bool     joystickFixed;          // 조이스틱 Fixed/Floating 저장
        public float    bgSoundValue;           // 배경음 크기 저장
        public float    fxSoundValue;           // 효과음 크기 저장
    }

    [SerializeField] private GameData gameData;
    [SerializeField] public LoadingTexts loadingTexts;

    public bool joystickFixed
    {
        get { return gameData.joystickFixed; }
        set { gameData.joystickFixed = value; }
    }

    public float bgSoundValue
    {
        get { return gameData.bgSoundValue; }
        set { gameData.bgSoundValue = value; }
    }

    public float fxSoundValue
    {
        get { return gameData.fxSoundValue; }
        set { gameData.fxSoundValue = value; }
    }

    // ===============================================================================================
    // 플레이 기록이 있는 챕터면(해당 챕터의 bit가 0이 아니면) true 반환
    // ===============================================================================================
    public bool IsChapterPlayedBefore(string sceneName)
    {
        switch (sceneName)
        {
            case "Chapter_T":
                if (gameData.chapterFlag_CT == 0)
                    return false;
                return true;

            case "Chapter_1":
                if (gameData.chapterFlag_C1 == 0)
                    return false;
                return true;

            case "Chapter_2":
                if (gameData.chapterFlag_C2 == 0)
                    return false;
                return true;

            case "Chapter_3":
                if (gameData.chapterFlag_C3 == 0)
                    return false;
                return true;
        }

        return false;
    }

    // ===============================================================================================
    // 게임 플레이 기록 초기화 (bit and(&) 연산)
    // ===============================================================================================
    public void ClearData(string sceneName)
    {
        switch (sceneName)
        {
            case "Chapter_T":
                gameData.chapterFlag_CT &= 0;
                break;

            case "Chapter_1":
                gameData.chapterFlag_C1 &= 0;
                break;

            case "Chapter_2":
                gameData.chapterFlag_C2 &= 0;
                break;

            case "Chapter_3":
                gameData.chapterFlag_C3 &= 0;
                break;
        }

        //SaveData(0);
    }

    // ===============================================================================================
    // 게임 플레이 기록 저장(bit or(|) 연산)
    // ===============================================================================================
    public void SaveData(int bitPlace)
    {
        // 현재 저장 위치가 이미 저장되어 있다면 return
        switch (SceneManager.GetActiveScene().name)
        {
            case "Chapter_T":
                if ((gameData.chapterFlag_CT & bitPlace) == bitPlace)
                    return;

                gameData.chapterFlag_CT |= bitPlace;
                break;


            case "Chapter_1":
                if ((gameData.chapterFlag_C1 & bitPlace) == bitPlace)
                    return;

                gameData.chapterFlag_C1 |= bitPlace;
                break;


            case "Chapter_2":
                if ((gameData.chapterFlag_C2 & bitPlace) == bitPlace)
                    return;

                gameData.chapterFlag_C2 |= bitPlace;
                break;


            case "Chapter_3":
                if ((gameData.chapterFlag_C3 & bitPlace) == bitPlace)
                    return;

                gameData.chapterFlag_C3 |= bitPlace;
                break;


            default:
                break;
        }
    }

    // ===============================================================================================
    // 현재 챕터의 마지막 저장 위치의 SavePoint를 찾아 플레이어의 위치 이동
    // ===============================================================================================
    public void LoadSavePoint()
    {
        // 저장된 데이터가 없을 때 플레이어가 기본 위치에서 시작하도록 지정
        Vector3 position = GameObject.FindGameObjectWithTag("Player").transform.position;

        switch (SceneManager.GetActiveScene().name)
        {
            case "Chapter_T":
                if (gameData.chapterFlag_CT > 0)
                {
                    position = GameObject.Find("SavePoint_" + (Convert.ToString(gameData.chapterFlag_CT, 2).Length - 1).ToString()).transform.position;
                }
                break;

            case "Chapter_1":
                if (gameData.chapterFlag_C1 > 0)
                {
                    position = GameObject.Find("SavePoint_" + (Convert.ToString(gameData.chapterFlag_C1, 2).Length - 1).ToString()).transform.position;
                }
                break;

            case "Chapter_2":
                if (gameData.chapterFlag_C2 > 0)
                {
                    position = GameObject.Find("SavePoint_" + (Convert.ToString(gameData.chapterFlag_C2, 2).Length - 1).ToString()).transform.position;
                }
                break;

            case "Chapter_3":
                if (gameData.chapterFlag_C3 > 0)
                {
                    position = GameObject.Find("SavePoint_" + (Convert.ToString(gameData.chapterFlag_C3, 2).Length - 1).ToString()).transform.position;
                }
                break;
        }

        GameObject.FindGameObjectWithTag("Player").transform.position = position;
    }

    // ===============================================================================================
    // 게임의 전체 진행도를 확인
    // ===============================================================================================
    public int CheckProcress()
    {
        if (gameData.totalProgressFlag == 0)
        {
            return 0;
        }
        else
        {
            return Convert.ToString(gameData.totalProgressFlag, 2).Length - 1;
        }
    }

    // ===============================================================================================
    // 다이얼로그 및 패턴이 정해진 Area(게임 내 진행도에 따른 위치)를 벗어났을 때 다시 실행하지 않도록 체크
    // ===============================================================================================
    public bool CheckArea(int area)
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Chapter_T":
                if (gameData.chapterFlag_CT > 0)
                {
                    if (Convert.ToString(gameData.chapterFlag_CT, 2).Length > area)
                    {
                        return false;
                    }
                }
                break;

            case "Chapter_1":
                if (gameData.chapterFlag_C1 > 0)
                {
                    if (Convert.ToString(gameData.chapterFlag_C1, 2).Length > area)
                    {
                        return false;
                    }
                }
                break;

            case "Chapter_2":
                if (gameData.chapterFlag_C2 > 0)
                {
                    if (Convert.ToString(gameData.chapterFlag_C2, 2).Length > area)
                    {
                        return false;
                    }
                }
                break;

            case "Chapter_3":
                if (gameData.chapterFlag_C3 > 0)
                {
                    if (Convert.ToString(gameData.chapterFlag_C3, 2).Length > area)
                    {
                        return false;
                    }
                }
                break;
        }

        return true;
    }

    public void GetCollection()
    {

    }

    // ===============================================================================================
    // 게임이 종료될 때 게임 데이터를 저장하고 게임 종료
    // ===============================================================================================
    private void OnApplicationQuit()
    {
        string toJsonData = JsonUtility.ToJson(gameData, true);
        string filePath = Application.dataPath + "/Resources/Json/GameData.json";
        File.WriteAllText(filePath, toJsonData);
        Debug.Log("Save Complete");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
