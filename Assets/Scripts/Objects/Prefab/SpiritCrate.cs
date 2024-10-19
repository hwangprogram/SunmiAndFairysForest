using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritCrate : MonoBehaviour
{

    public GameObject[] objectsToSpawn; // 생성할 오브젝트 배열
    public int hitCount = 5; // 현재 부딪힌 횟수
    private bool isCooldown = false; // 쿨다운 중인지 확인하는 플래그

    public ParticleSystem ps;

    private void OnTriggerStay2D(Collider2D other)
    {
        // 특정 태그와 부딪혔는지 확인
        if (other.CompareTag("Player") && !isCooldown)
        {
            Debug.Log("Triggered with: " + other.tag);
            hitCount--; // 부딪힌 횟수 감소
            
            ParticleSystem newPs = Instantiate(ps, transform.position, transform.rotation);
            newPs.Emit(100);

            Destroy(newPs.gameObject, newPs.main.duration + newPs.main.startLifetime.constantMax);

            StartCoroutine(Cooldown()); // 쿨다운 시작

            if (hitCount <= 0) // 5회 이상 부딪혔는지 확인
            {
                Break(); // 깨지는 함수 호출
            }
        }
    }

    private IEnumerator Cooldown()
    {
        isCooldown = true; // 쿨다운 시작
        yield return new WaitForSeconds(1); // 1초 대기
        isCooldown = false; // 쿨다운 종료
    }

    private void Break()
    {
        Destroy(gameObject); // 현재 오브젝트 파괴

        // 랜덤한 오브젝트 생성
        int randomIndex = Random.Range(0, objectsToSpawn.Length); // 랜덤 인덱스 선택
        Instantiate(objectsToSpawn[randomIndex], transform.position, Quaternion.identity); // 오브젝트 생성
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
