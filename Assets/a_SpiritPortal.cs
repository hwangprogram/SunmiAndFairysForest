using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class a_SpiritPortal : MonoBehaviour
{
    /// <summary>
    /// 일단 Stone 부분에만 사용할 생각이라서 거기에 맞게 되어 있음. 
    /// </summary>


    public string stageName;
    public bool isGateForClear = false;
    private bool isGateUsing = false;
    public Sprite gateNo;
    public Sprite gateYes;
    [HideInInspector] public GameObject setInteraction;

    /* public GameObject isFireGetCover;
    public GameObject isWaterGetCover;
    public GameObject isWindGetCover; */
    public GameObject isStoneGetCover;

    private void FixedUpdate()
    {
        if (Hub.PlayerStatus.IsStoneGet) 
        {
            isStoneGetCover.SetActive(false);
            this.GetComponent<SpriteRenderer>().sprite = gateYes;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" &
            Hub.PlayerStatus.IsStoneGet)
        {
            setInteraction = FindObjectOfType<a_PlayerUI>().gameObject.transform.Find("Interaction").gameObject;
            if (setInteraction != null) setInteraction.SetActive(true);
            Hub.PlayerStatus.isInPortal = true;
        }
        // 이거 isGateUsing 사용해서 여러 번 반복되지 않게 하기
        if (!isGateUsing &
           other.tag == "Player" &
           Hub.InputManager.isInteraction == true &
           Hub.PlayerStatus.IsStoneGet &
           Hub.PlayerStatus.currentPlayerState != PlayerState.skill)
        {
            isGateUsing = true;
            if (isGateForClear) Hub.StageManager.GameOff(stageName);
            else Hub.StageManager.ToScene(stageName);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Hub.PlayerStatus.isInPortal = false;
            setInteraction.SetActive(false);
        }
    }
}
