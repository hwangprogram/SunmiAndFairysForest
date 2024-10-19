using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_Key : MonoBehaviour
{
    public float amplitude = 0.15f; // ������Ʈ�� ���Ϸ� �̵��� �ִ� �Ÿ�
    public float frequency = 3f; // ���� �̵��� �ӵ�
    private Vector3 startPosition;

    public ParticleSystem ps;
    private GameObject setInteraction;

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
        if (other.tag == "Player")
        {
            // Interaction UI Ȱ��ȭ
            setInteraction.SetActive(true);

            // �÷��̾ ��ȣ�ۿ� Ű�� ������ ��
            if (Hub.InputManager.isInteraction)
            {
                /*ParticleSystem newPs = Instantiate(ps, transform.position, transform.rotation);
                newPs.Emit(30);

                Destroy(newPs.gameObject, newPs.main.duration + newPs.main.startLifetime.constantMax);*/
                Hub.SFXManager.keyGet.Play();
                Hub.PlayerStatus.KeyCount += 1;
                Destroy(gameObject);
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

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
