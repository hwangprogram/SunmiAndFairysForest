using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public int currentStage;
    public int[] RequiredKeyAmount;
    private Color tmpColor;

    public void Awake()
    {
        tmpColor = Hub.UIManager.transition.GetComponent<Image>().color;
    }


    /// <summary>
    /// 1. ToMenu
    /// 2. GameOn
    /// 3. ToScene
    /// 4. ResetScene
    /// 5. GameOff
    /// </summary>
        

    public void ToMenu()
    {
        // Pause���� ToMenu() ����� ��쿣 Pause Ǯ���ֱ��
        Time.timeScale = 1f;
        Hub.BGMManager.ChangeBgm(Hub.BGMManager.menu);
        StartCoroutine(ToMenuCor());
    }
    IEnumerator ToMenuCor()
    {
        Hub.UIManager.transition.SetActive(true);
        for (float i = 0f; i < 1f; i += 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 1f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return new WaitForSeconds(1.7f);

        ///////////////////////////////////////////////////////////////////////////
        SceneManager.LoadScene("0. Menu");
        Hub.UIManager.statusUI.SetActive(false);
        Hub.GameManager.CurrentGameState = GameState.menu;
        tmpColor.a = 0f;
        Hub.UIManager.faintUI.GetComponent<Image>().color = tmpColor;
        Hub.UIManager.faintUI.SetActive(false);
        Hub.UIManager.Unpause();        
        ///////////////////////////////////////////////////////////////////////////

        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 0f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        Hub.UIManager.transition.SetActive(false);
        yield return null;
    }
    

    public void GameOn(string sceneName)
    {
        // ���⼭ �ʱ�ȭ�� �����. 
        Hub.PlayerStatus.isInPortal = false;
        Hub.PlayerStatus.isKeyAll = false;
        Hub.PlayerStatus.KeyCount = 0;
        Hub.PlayerStatus.isDamageFree = false;
        Hub.PlayerStatus.isFainting = false;
        Hub.PlayerStatus.DestroyFullHP();
        Hub.PlayerStatus.CurrentMP = 0;
        Hub.PlayerStatus.IsFireGet = false;
        Hub.PlayerStatus.IsWaterGet = false;
        Hub.PlayerStatus.IsWindGet = false;
        Hub.PlayerStatus.IsStoneGet = false;
        Hub.PlayerStatus.GetFullHP(2);
        Hub.PlayerStatus.SetCurrentHP(2);
        Hub.PlayerStatus.currentPlayerState = PlayerState.free;
        Hub.BGMManager.ChangeBgm(Hub.BGMManager.inGame);
        // ���⼭ Ű ����Ʈ�� ���ְ� currentStage�� �ʱ�ȭ
        Hub.PlayerStatus.DestroyFullKey(RequiredKeyAmount[currentStage]);
        currentStage = 0;

        Hub.GameManager.TimeAttackReset();
        Hub.GameManager.TimeAttackOn();

        StartCoroutine(GameOnCor(sceneName));
    }
    IEnumerator GameOnCor(string sceneName)
    {
        Hub.UIManager.transition.SetActive(true);
        for (float i = 0f; i < 1f; i += 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 1f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return new WaitForSeconds(1f);

        ///////////////////////////////////////////////////////////////////////////
        //���⼭ key list�� �߰���. �ʱ�ȭ�� GameOff()���� ��.
        Hub.PlayerStatus.GetFullKey(RequiredKeyAmount[currentStage]);
        SceneManager.LoadScene(sceneName);
        Hub.PlayerStatus.SaveCurrentState();
        // dialog�� �� player�� �������� �ʰ� �Ϸ��� pause�ΰ���.
        Hub.GameManager.CurrentGameState = GameState.pause;
        Hub.UIManager.statusUI.SetActive(true);
        Hub.UIManager.dialogUI.SetActive(true);
        ///////////////////////////////////////////////////////////////////////////

        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 0f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return null;
        Hub.UIManager.transition.SetActive(false);
        Hub.CVManager.cv[0].Play();
    }

       
    public void ToScene(string sceneName)
    {   
        Hub.PlayerStatus.isInPortal = false;
        Hub.PlayerStatus.isKeyAll = false;
        Hub.PlayerStatus.KeyCount = 0;
        Hub.PlayerStatus.isDamageFree = false;
        StartCoroutine(ToSceneCor(sceneName));        
    }
    IEnumerator ToSceneCor(string sceneName)
    {
        Hub.UIManager.transition.SetActive(true);
        for (float i = 0f; i < 1f; i += 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 1f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return new WaitForSeconds(0.7f);

        ///////////////////////////////////////////////////////////////////////////
        SceneManager.LoadScene(sceneName);
        // sceneName ����� ����ϰ� �־ string�� �Ľ��ϱ⺸�ٴ�
        // �ϴ� currentStage�� �Ź� +1 �ϴ� ����� �����. 
        // ������ �� ó���� GameOn()���� �ϰ�, ������ GameOff()���� �ϴϱ�
        // GameOn������ 0�� ä�� �״��, GameOff������ 0���� �ʱ�ȭ�ϴ� ������� ���.
        // ���߿� �ٽ� �����ϱ⳪ �̷� �� �߰��ϰ� �� ��� �̰� �ݵ�� �ٸ� ������� �ٲ����!! 
        Hub.PlayerStatus.DestroyFullKey(RequiredKeyAmount[currentStage]);
        currentStage += 1;
        Hub.PlayerStatus.GetFullKey(RequiredKeyAmount[currentStage]);
        // �ٽ� �� �� Ȯ���ϴ� ����.
        Hub.PlayerStatus.KeyCount = 0;
        Hub.PlayerStatus.SaveCurrentState();
        // ��Ż ��� ���߿� Pause������ �װ� Ǯ��
        Hub.UIManager.Unpause();
        Hub.PlayerStatus.currentPlayerState = PlayerState.free;
        ///////////////////////////////////////////////////////////////////////////

        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 0f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return null;
        Hub.UIManager.transition.SetActive(false);
    }


    public void ResetScene()
    {
        Time.timeScale = 1f;
        Hub.PlayerStatus.isInPortal = false;
        Hub.PlayerStatus.isKeyAll = false;
        Hub.PlayerStatus.KeyCount = 0;
        Hub.PlayerStatus.isDamageFree = false;        
        StartCoroutine(ResetSceneCor());
    }
    IEnumerator ResetSceneCor()
    {
        Hub.UIManager.transition.SetActive(true);
        for (float i = 0f; i < 1f; i += 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 1f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return new WaitForSeconds(1.7f);

        ///////////////////////////////////////////////////////////////////////////
        int tmpScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(tmpScene, LoadSceneMode.Single);
        Hub.PlayerStatus.LoadCurrentState();
        tmpColor.a = 0f;
        Hub.UIManager.faintUI.GetComponent<Image>().color = tmpColor;
        Hub.UIManager.faintUI.SetActive(false);
        Hub.UIManager.Unpause();
        // �̰� scene �ٲ�� ���� Ǯ������� ���� ȭ�鿡�� �ڲ� enemy �� �����ؼ� �̰͸� ��� �� ��ġ�� ��.
        Hub.PlayerStatus.isFainting = false;
        Hub.PlayerStatus.currentPlayerState = PlayerState.free;
        ///////////////////////////////////////////////////////////////////////////

        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 0f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return null;
        Hub.UIManager.transition.SetActive(false);
    }
       

    public void GameOff(string sceneName)
    {
        Hub.PlayerStatus.isInPortal = false;
        Hub.PlayerStatus.isKeyAll = false;
        Hub.PlayerStatus.KeyCount = 0;
        Hub.BGMManager.ChangeBgm(Hub.BGMManager.gameOff);
        StartCoroutine(GameOffCor(sceneName));

        Hub.GameManager.TimeAttackOff();
        PlayerPrefs.SetInt("IsClear", 1);
        PlayerPrefs.Save();
    }
    IEnumerator GameOffCor(string sceneName)
    {
        Hub.UIManager.transition.SetActive(true);
        for (float i = 0f; i < 1f; i += 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 1f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return new WaitForSeconds(2.3f);

        ///////////////////////////////////////////////////////////////////////////
        SceneManager.LoadScene(sceneName);
        Hub.GameManager.CurrentGameState = GameState.menu;
        Hub.UIManager.dialogUI.SetActive(false);
        Hub.UIManager.statusUI.SetActive(false);        
        ///////////////////////////////////////////////////////////////////////////

        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            tmpColor.a = i;
            Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
            yield return new WaitForSeconds(0.015f);
        }
        tmpColor.a = 0f;
        Hub.UIManager.transition.GetComponent<Image>().color = tmpColor;
        yield return null;
        Hub.UIManager.transition.SetActive(false);
    }









}
