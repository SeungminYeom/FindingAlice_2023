using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ===================================================================================================
// 게임에서 사용되는 Item 타입의 오브젝트에 Attach되는 스크립트
//
// Inspector 창에서 어떤 아이템인지 ItemType을 지정하여 사용한다.
// ===================================================================================================

public class Item : MonoBehaviour
{
    [System.Serializable]
    private enum ItemType
    {
        Clock,      // 시계 횟수를 증가시키는 아이템
        Oxygen      // 산소 게이지를 충전시키는 아이템
    }

    [SerializeField] private ItemType itemType;
    [SerializeField] private float respawnTime;

    private Mesh objMesh;


    void Start()
    {
        objMesh = GetComponent<MeshFilter>().mesh;
    }

    // ===============================================================================================
    // 플레이어와 충돌 시 Mesh와 Collider를 끄고, 아이템의 역할에 맞는 기능 실행
    // ===============================================================================================
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<MeshFilter>().mesh = null;
            GetComponent<SphereCollider>().enabled = false;
            StartCoroutine(Respawn());

            switch(itemType)
            {
                case ItemType.Clock:
                    ClockManager.instance.clockCounter++;
                    break;

                case ItemType.Oxygen:
                    OxygenBar.instance.GetOxygenItem();
                    break;
            }
        }
    }

    // ===============================================================================================
    // 정해진 시간이 지난 후 아이템 재생성
    // ===============================================================================================
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        GetComponent<MeshFilter>().mesh = objMesh;
        GetComponent<SphereCollider>().enabled = true;
    }
}
