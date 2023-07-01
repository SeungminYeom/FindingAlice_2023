using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Versioning;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public static ClockManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private GameObject          clock;
    private List<GameObject>    clockUI;
    private Coroutine           clockReload;
    private int clockMaxCount = 2;
    [SerializeField] private int _clockCounter;
    [SerializeField] private float clockReloadTime = 3f;

    [SerializeField] private bool _clockShootable = true;


    // ===============================================================================================
    // 시계 발사 횟수를 관리하는 프로퍼티
    //
    // 증감 연산자에 따라 시계를 활성/비활성화
    // ===============================================================================================
    public int clockCounter
    {
        get
        {
            return _clockCounter;
        }
        set
        {
            int temp = _clockCounter;
            _clockCounter = Mathf.Clamp(value, 0, clockMaxCount);

            // clockCounter++
            if (_clockCounter > temp)
            {
                for (int i = 0; i < _clockCounter; i++)
                {
                    clockUI[i].SetActive(true);
                }
            }
            // clockCounter--
            else if (_clockCounter < temp)
            {
                clockUI[_clockCounter].SetActive(false);

                if (clockReload != null)
                {
                    StopCoroutine(clockReload);
                }
                clockReload = StartCoroutine(ClockReload());
            }
        }
    }

    public bool clockShootable
    {
        get
        {
            return _clockShootable;
        }
        set
        {
            _clockShootable = value;
        }
    }

    private void Start()
    {
        clock = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject;
        clockUI = new List<GameObject>();
        clockCounter = clockMaxCount;
        for (int i = 0; i < clockMaxCount; i++)
        {
            clockUI.Add(GameObject.Find("ClockCounter").transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {

        }
        else
        {
            if (clockCounter > 0 && clockShootable)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    clock.SetActive(true);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (clock.activeInHierarchy)
                    {
                        clock.GetComponent<Clock>().ClockFollow();
                        clockCounter--;
                    }
                }
            }
        }
    }

    // ===============================================================================================
    // 일정 시간 시간 후에 시계를 발사할 수 있는 횟수를 증가시키는 함수
    //
    // 실행 후 시계가 최대가 아닐 경우 재실행
    // ===============================================================================================
    private IEnumerator ClockReload()
    {
        yield return new WaitForSeconds(clockReloadTime);

        clockCounter++;

        if (clockCounter < clockMaxCount)
        {
            clockReload = StartCoroutine(ClockReload());
        }
    }

    public void ClockReturnIdle()
    {
        if (clock.activeSelf)
        {
            clock.GetComponent<Clock>().ClockReturnIdle();
        }
    }
}
