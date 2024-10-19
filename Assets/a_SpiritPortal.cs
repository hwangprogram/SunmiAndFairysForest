using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class a_SpiritPortal : MonoBehaviour
{
    /// <summary>
    /// �ϴ� Stone �κп��� ����� �����̶� �ű⿡ �°� �Ǿ� ����. 
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
        // �̰� isGateUsing ����ؼ� ���� �� �ݺ����� �ʰ� �ϱ�
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
