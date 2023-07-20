using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ===================================================================================================
// 플레이어가 발사하는 시계 오브젝트에 Attach되는 스크립트
// ===================================================================================================

public class Clock : MonoBehaviour
{
    private GameObject  player;
    private GameObject  clockBg;
    private Color       clockBgMatColor;
    private Transform   playerTrans;

    private Vector3     vecToClock;
    private Coroutine   clockShoot;

    [Header("Clock Value")]
    [SerializeField] private float timeScaleValue       = 0.05f;
    [SerializeField] private float clockIncreasableTime = 2f;
    [SerializeField] private float clockMaxDistanceTime = 3f;
    [SerializeField] private float clockStartTime       = 0f;
    [SerializeField] private float clockMaxDistance     = 8f;
    [SerializeField] private float clockCurDistance     = 0f;
    [SerializeField] private float clockShootPower      = 20f;

    // 시네머신 카메라 변수
    private CinemachineVirtualCamera        virtureCam;
    private CinemachineFramingTransposer    virtureCamFT;
    private float cameraOriginDistance  = 10f;
    private float cameraOriginDampingXY = 1f;
    private float cameraOriginDampingZ  = 0.5f;
    private float cameraZoomDampingXY   = 0.1f;
    private float cameraZoomDampingZ    = 0.1f;

    // *****
    public bool usingClock = false;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        virtureCam = GameObject.Find("Cam_Idle").GetComponent<CinemachineVirtualCamera>();
        virtureCamFT = virtureCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // ===============================================================================================
    // 시계가 활성화될 때의 행동
    // ===============================================================================================
    private void OnEnable()
    {
        playerTrans = player.transform;

        clockStartTime = Time.unscaledTime;
        Time.timeScale = timeScaleValue;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        player.GetComponent<Movement>().StateBeginShoot();
        usingClock = true;

        //카메라 조작
        virtureCam.m_Follow = transform;
        CamSettings(true);

        //코루틴 초기화
        if (clockShoot != null) clockShoot = null;
        clockShoot = StartCoroutine(ClockShoot());
    }

    // ===============================================================================================
    // 시계가 발사되고 있는 상태일 때 실행되는 코루틴
    // ===============================================================================================
    private IEnumerator ClockShoot()
    {
        while (Time.unscaledTime - clockStartTime < clockIncreasableTime + clockMaxDistanceTime)
        {
            // 시계가 플레이어로부터 멀어지고 있는 상태
            if (clockCurDistance < clockMaxDistance)
            {
                clockCurDistance += (clockMaxDistance / clockIncreasableTime) * Time.unscaledDeltaTime;

                //카메라 조작
                virtureCamFT.m_CameraDistance = (Time.unscaledTime - clockStartTime) / clockIncreasableTime * 7 + cameraOriginDistance;

                clockBgMatColor.a = (Time.unscaledTime - clockStartTime) / clockIncreasableTime * 0.8f;
                clockBg.GetComponent<MeshRenderer>().material.color = clockBgMatColor;
            }

            Vector3 clockPos = ClockTouchZone.toDragedPos;
            vecToClock = (clockPos - player.transform.position).normalized;

            // 시계가 플레이어의 자식으로 있으므로, 플레이어의 Flip에 따라 시계도 Flip
            if ((vecToClock.x < 0 && playerTrans.localScale.x > 0) || (vecToClock.x > 0 && playerTrans.localScale.x < 0))
            {
                Vector3 sightDir = playerTrans.localScale;
                sightDir.x *= -1;
                playerTrans.localScale = sightDir;
            }

            transform.localPosition = new Vector3(vecToClock.x * clockCurDistance * (playerTrans.localScale.x / Mathf.Abs(playerTrans.localScale.x)),
                                                  vecToClock.y * clockCurDistance,
                                                  0);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -Mathf.Atan2(vecToClock.x, vecToClock.y) * Mathf.Rad2Deg * playerTrans.localScale.x));
            clockBg.transform.localPosition = new Vector3(clockBg.transform.localPosition.x, clockBg.transform.localPosition.y, -virtureCam.transform.position.z + 1f);

            yield return null;
        }

        ClockReturnIdle();
        player.GetComponent<Movement>().ClockStateEnd();
    }

    // ===============================================================================================
    // 시계의 동작이 완전히 종료되었을 때 최종 호출될 함수
    // ===============================================================================================
    public void ClockReturnIdle()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        //카메라 조작
        virtureCam.m_Follow = player.transform;
        CamSettings(false);

        clockCurDistance = 0;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;
        clockBg.SetActive(false);
        gameObject.SetActive(false);
        usingClock = false;
    }

    // ===============================================================================================
    // 플레이어가 시계를 따라갈 때 호출되는 함수
    // ===============================================================================================
    public void ClockFollow()
    {
        StopCoroutine(clockShoot);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        transform.SetParent(null);
        player.GetComponent<Movement>().StateBeginFollow(vecToClock * clockShootPower);

        //카메라 조작
        CamSettings(false);
    }

    // ===============================================================================================
    // Cinemachine Virture Camera 값 초기화
    // ===============================================================================================
    public void CamSettings(bool isZooming)
    {
        if (isZooming)
        {
            virtureCam.m_Follow = transform;
            virtureCamFT.m_XDamping = cameraZoomDampingXY;
            virtureCamFT.m_YDamping = cameraZoomDampingXY;
            virtureCamFT.m_ZDamping = cameraZoomDampingZ;
            clockBg = virtureCam.transform.GetChild(1).gameObject;
            clockBg.SetActive(true);
            clockBgMatColor = clockBg.GetComponent<MeshRenderer>().material.color;
        }
        else
        {
            virtureCamFT.m_XDamping = cameraOriginDampingXY;
            virtureCamFT.m_YDamping = cameraOriginDampingXY;
            virtureCamFT.m_ZDamping = cameraOriginDampingZ;
            virtureCamFT.m_CameraDistance = cameraOriginDistance;

            clockBgMatColor.a = 0;
            clockBg.GetComponent<MeshRenderer>().material.color = clockBgMatColor;
        }
    }

    // ===============================================================================================
    // 시계가 Player와 충돌했다면 vecToClock(플레이어 위치에서 시계의 위치로 향하는 벡터)에
    // clockCurDistance(플레이어와 시계 사이의 거리)를 곱한 값을 인자로 함수 호출
    // ===============================================================================================
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.GetComponent<Movement>().StateCollsionWithClock(vecToClock * clockCurDistance);
            ClockReturnIdle();
        }
    }
}