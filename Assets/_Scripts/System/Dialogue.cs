using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    // 클래스 및 멤버의 접근지정자 public이어야 Parsing 됨
    [System.Serializable]
    public class ScriptData
    {
        public string   spriteName;
        public bool     spriteLeft;
        public string   actorName;
        public int      num;
        public string   line;
    }

    // 클래스 및 멤버의 접근지정자 public이어야 Parsing 됨
    [System.Serializable]
    public class Script{
        public List<ScriptData> scriptDatas;
    }

    [Header("Dialogue")]
    [SerializeField] private Script script;
    [SerializeField] private int    BeginNum;
    [SerializeField] private int    EndNum;

    [Header("Bounds")]
    [SerializeField] private float  dialogueBoxCenterX;
    [SerializeField] private float  dialogueBoxCenterY;
    [SerializeField] private float  dialogueBoxCenterZ;
    [SerializeField] private float  dialogueBoxScaleX;
    [SerializeField] private float  dialogueBoxScaleY;
    [SerializeField] private float  dialogueBoxScaleZ;

    private GameObject dialogueUI;

    void Start()
    {
        script = JsonUtility.FromJson<Script>(Resources.Load<TextAsset>("Json/Script").text);
        GetComponent<BoxCollider>().center  = new Vector3(dialogueBoxCenterX, dialogueBoxCenterY, dialogueBoxCenterZ);
        GetComponent<BoxCollider>().size    = new Vector3(dialogueBoxScaleX, dialogueBoxScaleY, dialogueBoxScaleZ);
        dialogueUI = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Movement>().StateDialogueBegin(new Vector3(transform.position.x + dialogueBoxCenterX,
                                                                          transform.position.y + dialogueBoxCenterY,
                                                                          transform.position.z + dialogueBoxCenterZ));
            dialogueUI.SetActive(true);
            ClockManager.instance.clockShootable = false;
            StartCoroutine(PrintDialogue(other.gameObject));
        }
    }

    IEnumerator PrintDialogue(GameObject player)
    {
        while (BeginNum < EndNum)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (script.scriptDatas[BeginNum].spriteLeft)
                {
                    dialogueUI.transform.GetChild(0).GetComponent<Image>().sprite
                        = Resources.Load<Sprite>("Sprites/" + script.scriptDatas[BeginNum].spriteName);
                }
                else
                {
                    dialogueUI.transform.GetChild(1).GetComponent<Image>().sprite
                        = Resources.Load<Sprite>("Sprites/" + script.scriptDatas[BeginNum].spriteName);
                }
                dialogueUI.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text
                    = script.scriptDatas[BeginNum].actorName;
                dialogueUI.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text
                    = script.scriptDatas[BeginNum].line;


                BeginNum++;
            }

            yield return null;
        }

        player.GetComponent<Movement>().StateDialogueEnd();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x + dialogueBoxCenterX,
                                        transform.position.y + dialogueBoxCenterY,
                                        transform.position.z + dialogueBoxCenterZ),
                            new Vector3(dialogueBoxScaleX, dialogueBoxScaleY, dialogueBoxScaleZ));
    }
}
