using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_WaterSpirit : MonoBehaviour
{
    public float amplitude = 0.5f; // ������Ʈ�� ���Ϸ� �̵��� �ִ� �Ÿ�
    public float frequency = 3f; // ���� �̵��� �ӵ�

    private Vector3 startPosition;

    public float shrinkSpeed = 3f; // ������Ʈ�� �۾����� �ӵ�
    private bool isShrinking = false;

    public RectTransform uiTarget; // UI ����� RectTransform

    public ParticleSystem particlePrefab; // ��ƼŬ ������
    public float particleMoveSpeed = 10f; // ��ƼŬ �̵� �ӵ�

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
            // Interaction UI Ȱ��ȭ
            setInteraction.SetActive(true);

            // �÷��̾ ��ȣ�ۿ� Ű�� ������ ��
            if (Hub.InputManager.isInteraction)
            {
                Hub.PlayerStatus.IsWaterGet = true;
                Hub.SFXManager.objectGet.Play();
                Debug.Log(Hub.PlayerStatus.IsWaterGet);
                Destroy(this.gameObject);
                // isShrinking = true; // �۾����� ����
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

    void Start()
    {
        // �ʱ� ��ġ ����
        startPosition = transform.position;

        if (uiTarget == null)
        {
            Debug.LogError("uiTarget�� �������� �ʾҽ��ϴ�. Inspector���� �������ּ���.");
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
            // ������Ʈ�� ���� �۾�����
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * shrinkSpeed);
            if (transform.localScale.magnitude < 0.3f)
            {
                if (particleInstance == null)
                {
                    particleInstance = Instantiate(particlePrefab, transform.position, Quaternion.identity);
                    particleInstance.Emit(50);
                    particles = new ParticleSystem.Particle[particleInstance.main.maxParticles];

                    // 2�� �Ŀ� MoveParticlesToTarget ȣ��
                    StartCoroutine(DelayedMoveParticlesToTarget(1.0f));
                }
            }
        }

        // ��ƼŬ�� �����ϸ� ��ƼŬ �̵� ������Ʈ
        if (particleInstance != null && particleInstance.IsAlive() && isInstantiate)
        {
            Vector3 worldPosition = GetWorldPositionFromUI(uiTarget);
            MoveParticlesToTarget(particleInstance, worldPosition);

            // ��� ��ƼŬ�� ���ŵǾ����� Ȯ��
            if (particleInstance.particleCount == 0)
            {
                Destroy(particleInstance.gameObject); // ��ƼŬ �ý��� ����
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
        // Screen Space - Overlay ��忡���� RectTransform�� position ��ü�� ȭ�� ��ǥ�� ����մϴ�.
        Vector3 screenPoint = uiTransform.position;
        screenPoint.z = 0; // UI�� 2D ��鿡 �����Ƿ� z ���� 0���� ����

        // ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ (ī�޶� �ʿ� ����)
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPoint);
        worldPosition.z = 0; // ���� ��ǥ�� z ���� 0���� ����
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
            particles[i].velocity = direction * particleMoveSpeed; // ��ƼŬ �̵� �ӵ� ����

            if (Vector3.Distance(particles[i].position, targetPosition) < 0.1f)
            {
                particles[i].remainingLifetime = 0; // ��ǥ�� �����ϸ� ��ƼŬ ����
            }
        }

        ps.SetParticles(particles, numParticlesAlive);
    }
}
