using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_Heart : MonoBehaviour
{
    public double amount;
    private GameObject setInteraction;
    public float amplitude = 0.5f; // 오브젝트가 상하로 이동할 최대 거리
    public float frequency = 3f; // 상하 이동의 속도

    private Vector3 startPosition;


    public void Awake()
    {
        startPosition = transform.position;
    }

    public void FixedUpdate()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }






    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            setInteraction = FindObjectOfType<a_PlayerUI>().gameObject.transform.Find("Interaction").gameObject;

            if (setInteraction != null)
            {
                setInteraction.SetActive(true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Interaction UI 활성화
            setInteraction.SetActive(true);

            // 플레이어가 상호작용 키를 눌렀을 때
            if (Hub.InputManager.isInteraction)
            {
                Hub.PlayerStatus.AddCurrentHP(amount);
                Hub.SFXManager.objectGet.Play();
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Interaction UI 비활성화
            setInteraction.SetActive(false);
        }
    }





}
