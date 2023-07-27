using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ===================================================================================================
// 상호작용할 수 있는 오브젝트에 Attach되는 스크립트
//
// Inspector 창에서 Type을 지정하여 사용한다.
// ===================================================================================================

public class Interactable : MonoBehaviour
{
    [System.Serializable]
    private enum InteractableType
    {
        Button,         // 플랫폼 생성 버튼
        Cave            // ch.2 작은 동굴 플랫폼
    }

    [Header("Button Value")]
    [SerializeField] private float disappearTime;

    [SerializeField] private InteractableType interactableType;

    private GameObject oxygenBar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (interactableType)
            {
                case InteractableType.Button:
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }

                    StartCoroutine(DisappearTimer(disappearTime));

                    break;

                case InteractableType.Cave:
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (interactableType)
            {
                case InteractableType.Button:
                    break;

                // 싱글톤으로 관리하는 것도 비용이 들기때문에 GetComponent로 작성
                case InteractableType.Cave:
                    oxygenBar = GameObject.Find("oxygenBar");
                    oxygenBar.GetComponent<OxygenBar>().EnterCave();
                    break;
            }
        }
    }

    private IEnumerator DisappearTimer(float time)
    {
        if (time <= 0) yield break;

        yield return new WaitForSeconds(time);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
