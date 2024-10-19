using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_Key : MonoBehaviour
{
    public float amplitude = 0.15f; // 오브젝트가 상하로 이동할 최대 거리
    public float frequency = 3f; // 상하 이동의 속도
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
            // Interaction UI 활성화
            setInteraction.SetActive(true);

            // 플레이어가 상호작용 키를 눌렀을 때
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
            // Interaction UI 비활성화
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
