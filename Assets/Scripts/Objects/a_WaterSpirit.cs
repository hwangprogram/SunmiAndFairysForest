using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_WaterSpirit : MonoBehaviour
{
    public float amplitude = 0.5f; // 오브젝트가 상하로 이동할 최대 거리
    public float frequency = 3f; // 상하 이동의 속도

    private Vector3 startPosition;

    public float shrinkSpeed = 3f; // 오브젝트가 작아지는 속도
    private bool isShrinking = false;

    public RectTransform uiTarget; // UI 요소의 RectTransform

    public ParticleSystem particlePrefab; // 파티클 프리팹
    public float particleMoveSpeed = 10f; // 파티클 이동 속도

    private GameObject setInteraction;
    private ParticleSystem particleInstance;
    private ParticleSystem.Particle[] particles;
    private bool isInstantiate = false;

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
                Hub.PlayerStatus.IsWaterGet = true;
                Hub.SFXManager.objectGet.Play();
                Debug.Log(Hub.PlayerStatus.IsWaterGet);
                Destroy(this.gameObject);
                // isShrinking = true; // 작아지기 시작
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

    void Start()
    {
        // 초기 위치 저장
        startPosition = transform.position;

        if (uiTarget == null)
        {
            Debug.LogError("uiTarget이 설정되지 않았습니다. Inspector에서 설정해주세요.");
        }
    }

    void Update()
    {
        if (!isShrinking)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        }
        else
        {
            // 오브젝트가 점점 작아지기
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * shrinkSpeed);
            if (transform.localScale.magnitude < 0.3f)
            {
                if (particleInstance == null)
                {
                    particleInstance = Instantiate(particlePrefab, transform.position, Quaternion.identity);
                    particleInstance.Emit(50);
                    particles = new ParticleSystem.Particle[particleInstance.main.maxParticles];

                    // 2초 후에 MoveParticlesToTarget 호출
                    StartCoroutine(DelayedMoveParticlesToTarget(1.0f));
                }
            }
        }

        // 파티클이 존재하면 파티클 이동 업데이트
        if (particleInstance != null && particleInstance.IsAlive() && isInstantiate)
        {
            Vector3 worldPosition = GetWorldPositionFromUI(uiTarget);
            MoveParticlesToTarget(particleInstance, worldPosition);

            // 모든 파티클이 제거되었는지 확인
            if (particleInstance.particleCount == 0)
            {
                Destroy(particleInstance.gameObject); // 파티클 시스템 제거
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator DelayedMoveParticlesToTarget(float delay)
    {
        yield return new WaitForSeconds(delay);
        isInstantiate = true;
        Vector3 worldPosition = GetWorldPositionFromUI(uiTarget);
        MoveParticlesToTarget(particleInstance, worldPosition);
    }

    private Vector3 GetWorldPositionFromUI(RectTransform uiTransform)
    {
        // Screen Space - Overlay 모드에서는 RectTransform의 position 자체가 화면 좌표를 사용합니다.
        Vector3 screenPoint = uiTransform.position;
        screenPoint.z = 0; // UI는 2D 평면에 있으므로 z 값을 0으로 설정

        // 화면 좌표를 월드 좌표로 변환 (카메라 필요 없음)
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPoint);
        worldPosition.z = 0; // 월드 좌표의 z 값을 0으로 설정
        return worldPosition;
    }

    private void MoveParticlesToTarget(ParticleSystem ps, Vector3 targetPosition)
    {
        if (ps == null) return;

        ParticleSystem.MainModule mainModule = ps.main;
        mainModule.simulationSpace = ParticleSystemSimulationSpace.World;

        int numParticlesAlive = ps.GetParticles(particles);

        for (int i = 0; i < numParticlesAlive; i++)
        {
            Vector3 direction = (targetPosition - particles[i].position).normalized;
            particles[i].velocity = direction * particleMoveSpeed; // 파티클 이동 속도 설정

            if (Vector3.Distance(particles[i].position, targetPosition) < 0.1f)
            {
                particles[i].remainingLifetime = 0; // 목표에 도달하면 파티클 제거
            }
        }

        ps.SetParticles(particles, numParticlesAlive);
    }
}
