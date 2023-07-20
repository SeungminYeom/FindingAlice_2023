using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// ===================================================================================================
// 챕터 선택 씬에서 사용되는 스크립트
// ===================================================================================================

public class ChapterSelect : MonoBehaviour
{
    [SerializeField] private GameObject chapters;
    [SerializeField] private GameObject chapterInfo;
    [SerializeField] private GameObject chapterInfoSprite;
    [SerializeField] private GameObject continueGame;
    [SerializeField] private GameObject setting;
    [SerializeField] private GameObject settingBGSound;
    [SerializeField] private GameObject settingFXSound;
    [SerializeField] private GameObject settingFixedJoystick;
    private string targetScene;

    private void Awake()
    {
        // 이전 챕터 플레이 기록이 없으면 챕터 잠구기
        int range = DataController.instance.CheckProcress();
        for (int i = 0; i <= range; i++)
        {
            chapters.transform.GetChild(i + 1).GetComponent<Button>().interactable = true;
        }
    }

    void Start()
    {
        // 배경음 슬라이더 조작 시 슬라이더 값에 따라 음량을 조절하기 위한 델리게이트 지정
        Slider BGSound_Slider = settingBGSound.GetComponent<Slider>();
        BGSound_Slider.onValueChanged.AddListener(delegate
                                                        {
                                                            DataController.instance.bgSoundValue
                                                                = settingBGSound.GetComponent<Slider>().value;
                                                        });

        // 효과음 슬라이더 조작 시 슬라이더 값에 따라 음량을 조절하기 위한 델리게이트 지정
        Slider FXSound_Slider = settingFXSound.GetComponent<Slider>();
        FXSound_Slider.onValueChanged.AddListener(delegate
                                                        {
                                                            DataController.instance.fxSoundValue
                                                                = settingFXSound.GetComponent<Slider>().value;
                                                        });
    }

    // ===============================================================================================
    // 챕터를 클릭했을 때 챕터 정보를 열기 (UI에 연결)
    // ===============================================================================================
    public void OpenInfo(string sceneName)
    {
        chapterInfoSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Chapter/" + sceneName);
        continueGame.GetComponent<Button>().interactable = DataController.instance.IsChapterPlayedBefore(sceneName);

        chapterInfo.SetActive(true);
        targetScene = sceneName;
    }

    // ===============================================================================================
    // 챕터 정보 닫기 (UI에 연결)
    // ===============================================================================================
    public void CloseInfo()
    {
        targetScene = null;
        chapterInfo.SetActive(false);
    }

    // ===============================================================================================
    // 설정 열기 (UI에 연결)
    // ===============================================================================================
    public void OpenSetting()
    {
        if (DataController.instance.joystickFixed)
            settingFixedJoystick.GetComponent<Toggle>().isOn = true;
        else
            settingFixedJoystick.GetComponent<Toggle>().isOn = false;

        settingBGSound.GetComponent<Slider>().value = DataController.instance.bgSoundValue;
        settingFXSound.GetComponent<Slider>().value = DataController.instance.fxSoundValue;

        setting.SetActive(true);
    }

    // ===============================================================================================
    // 설정 닫기 (UI에 연결)
    // ===============================================================================================
    public void CloseSetting()
    {
        setting.SetActive(false);
    }

    // ===============================================================================================
    // 새 게임 시작 (UI에 연결)
    // ===============================================================================================
    public void StartNewGame()
    {
        DataController.instance.ClearData(targetScene);
        AsyncLoading.LoadScene(targetScene);
    }

    // ===============================================================================================
    // 이전 게임 이어하기 (UI에 연결)
    // ===============================================================================================
    public void ContinueGame()
    {
        AsyncLoading.LoadScene(targetScene);
    }

    // ===============================================================================================
    // 조이스틱 설정 변경에 따라 게임 데이터 변경 (UI에 연결)
    // ===============================================================================================
    public void ChangeJoystick()
    {
        // 설정 창 열릴 때 isOn 조작으로 인해 호출되는 것 방지
        if (!settingFixedJoystick.activeInHierarchy) return;

        if (DataController.instance.joystickFixed)
        {
            DataController.instance.joystickFixed = false;
            return;
        }
        DataController.instance.joystickFixed = true;
    }
 }
