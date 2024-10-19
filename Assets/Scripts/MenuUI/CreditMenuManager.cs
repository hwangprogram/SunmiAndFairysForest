using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditMenuManager : MonoBehaviour
{    
    public Image cutGraphic;
    public Text textLocation;
    public Text clearTextLocation;
    public float duration;
    [TextArea]
    public string[] dialog;
    

    private Color tmpColor;


    // Start is called before the first frame update
    void Awake()
    {
        //GameObject dontDestroyThese2 = FindObjectOfType<DontDestroyThese2>().gameObject;
        //Destroy(dontDestroyThese2);
        clearTextLocation.text = Hub.GameManager.stopwatch.Elapsed.ToString("hh\\:mm\\:ss\\.ff");
    }

    void Start()
    {
        StartCoroutine(Cor1());
    }

    IEnumerator Cor1()
    {
        for (int i = 0; i < dialog.Length; i++)
        {
            textLocation.text = dialog[i];
            yield return new WaitForSeconds(duration);
        }
        yield return new WaitForSeconds(7f);
        Hub.StageManager.ToMenu();
        yield return null;
    }

































    /*
    IEnumerator CreditSequence()
    {
        // ���� �Ϸ���Ʈ�� Ȱ��ȭ
        endingIllustration.SetActive(true);

        // �ؽ�Ʈ�� ������ �����ϰ� ����
        StartCoroutine(FadeOutText(congratulationsText));


        // 'Congratulations!' �ؽ�Ʈ�� ������
        congratulationsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(fadeOutDuration); // 5�� ���� ���

        

        // ũ���� �ؽ�Ʈ�� �ʱ� ��ġ�� ����
        creditText.transform.localPosition = new Vector2(creditText.transform.localPosition.x, 0);

        // ũ���� �ؽ�Ʈ�� ���� ��ũ��
        while (creditText.transform.localPosition.y < scrollHeight)
        {
            creditText.transform.localPosition += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
            yield return null;
        }

        // ��ũ���� �Ϸ�Ǹ� ���� �Ϸ���Ʈ ��Ȱ��ȭ
        endingIllustration.SetActive(false);
    }

    IEnumerator FadeOutText(Text text)
    {
        float startOpacity = text.color.a;
        float rate = 1.0f / fadeOutDuration;
        float progress = 0.0f;

        while (progress < 1.0)
        {
            Color tmpColor = text.color;
            tmpColor.a = Mathf.Lerp(startOpacity, 0, progress);
            text.color = tmpColor;
            progress += rate * Time.deltaTime;
            yield return null;
        }

        text.gameObject.SetActive(false); // ���̵� �ƿ� �� �ؽ�Ʈ ��Ȱ��ȭ
    }

    public void ToMainMenu()
    {
        Hub.StageManager.ToScene("Menu");
    }
    */










}
