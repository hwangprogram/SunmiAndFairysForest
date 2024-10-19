using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritCrate : MonoBehaviour
{

    public GameObject[] objectsToSpawn; // ������ ������Ʈ �迭
    public int hitCount = 5; // ���� �ε��� Ƚ��
    private bool isCooldown = false; // ��ٿ� ������ Ȯ���ϴ� �÷���

    public ParticleSystem ps;

    private void OnTriggerStay2D(Collider2D other)
    {
        // Ư�� �±׿� �ε������� Ȯ��
        if (other.CompareTag("Player") && !isCooldown)
        {
            Debug.Log("Triggered with: " + other.tag);
            hitCount--; // �ε��� Ƚ�� ����
            
            ParticleSystem newPs = Instantiate(ps, transform.position, transform.rotation);
            newPs.Emit(100);

            Destroy(newPs.gameObject, newPs.main.duration + newPs.main.startLifetime.constantMax);

            StartCoroutine(Cooldown()); // ��ٿ� ����

            if (hitCount <= 0) // 5ȸ �̻� �ε������� Ȯ��
            {
                Break(); // ������ �Լ� ȣ��
            }
        }
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true; // ��ٿ� ����
        yield return new WaitForSeconds(1); // 1�� ���
        isCooldown = false; // ��ٿ� ����
    }

    private void Break()
    {
        Destroy(gameObject); // ���� ������Ʈ �ı�

        // ������ ������Ʈ ����
        int randomIndex = Random.Range(0, objectsToSpawn.Length); // ���� �ε��� ����
        Instantiate(objectsToSpawn[randomIndex], transform.position, Quaternion.identity); // ������Ʈ ����
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
