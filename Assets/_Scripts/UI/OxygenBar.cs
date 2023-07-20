using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OxygenBar : MonoBehaviour
{
    public static OxygenBar instance;

    private Slider oxygenBar;

    private float maxOxygen;
    private float minOxygen;

    private float depletionRate;
    private float oxygenItem;
    private float caveOxygenRate;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        maxOxygen = 1f;
        minOxygen = 0f;

        depletionRate = maxOxygen / 15f;
        oxygenItem = maxOxygen / 2f;
        caveOxygenRate = maxOxygen / 5f;

        oxygenBar = gameObject.GetComponent<Slider>();
        oxygenBar.value = maxOxygen;

        StartCoroutine(OxygenBarUpdate());
    }

    private IEnumerator OxygenBarUpdate()
    {        
        while (oxygenBar.value <= maxOxygen && oxygenBar.value > minOxygen)
        {
            oxygenBar.value -= depletionRate * Time.deltaTime;

            yield return null;
        }

        // oxygenBar의 값이 0이면, 플레이어 사망
        oxygenBar.value = 0f;
        GameManager.instance.PlayerDead();
    }

    public void GetOxygenItem()
    {
        oxygenBar.value += oxygenItem;
    }

    public void EnterCave()
    {
        oxygenBar.value += caveOxygenRate * Time.deltaTime;
    }
}
