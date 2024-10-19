using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_Heart : MonoBehaviour
{
    public double amount;
    private GameObject setInteraction;
    public float amplitude = 0.5f; // ������Ʈ�� ���Ϸ� �̵��� �ִ� �Ÿ�
    public float frequency = 3f; // ���� �̵��� �ӵ�

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
            // Interaction UI Ȱ��ȭ
            setInteraction.SetActive(true);

            // �÷��̾ ��ȣ�ۿ� Ű�� ������ ��
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
            // Interaction UI ��Ȱ��ȭ
            setInteraction.SetActive(false);
        }
    }





}
