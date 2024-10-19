using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    free,
    move,
    gathering,
    skill,
    gotHit,
    crouch
}


// ���� ���ϰ� �ִ��� ������ ���ϰ� �ִ��� Ȯ���ؼ� 
// ��ų�̳� ���� ��ȯ�� ������ �ο��ϴ� ����.
public enum Direct
{
    left,
    right
}

public class PlayerStatus : MonoBehaviour
{
    [Header("SkillCost part")]
    public float fire1Amount;
    public float fire2Amount;
    public float water1Amount;
    public float wind1Amount;

    [Header("Status part")]
    public List<GameObject> fullHP = new List<GameObject>();
    public List<GameObject> fullKey = new List<GameObject>();
    private double _CurrentHP = 0;
    public double CurrentHP
    {
        get { return _CurrentHP; }
        set
        {
            // ü���� �������� ���� ���ظ� �Դ� ���� Ȯ�� 
            if (_CurrentHP > value)
            {
                //Ÿ�� ����Ʈ 
                //Ÿ�� ����
                foreach (GameObject heart in fullHP)
                {
                    heart.GetComponent<a_HPIcon>().half1.SetActive(false);
                    heart.GetComponent<a_HPIcon>().half2.SetActive(false);
                }
                try { Hub.DamageReceiver.GotHit(); }
                catch { } //GameOn�̸� catch�� �Ǿ� ����.
            }
            else //ü�� ȸ�� ���� 


            // �ʰ����� �ʵ��� ����
            if (value > fullHP.Count) value = fullHP.Count;
            _CurrentHP = value;

            // �������� ����
            int tmpCount = (int)value;
            double tmpExtra = value - (double)tmpCount;
            for (int i = 0; i < tmpCount; i++)
            {
                fullHP[i].GetComponent<a_HPIcon>().half1.SetActive(true);
                fullHP[i].GetComponent<a_HPIcon>().half2.SetActive(true);
            }
            if (tmpExtra > 0)
            {
                fullHP[tmpCount].GetComponent<a_HPIcon>().half1.SetActive(true);
            }

            if (_CurrentHP <= 0)
            {
                // ���� �� ó�� 
                // �ð� �� ó���� ���� ���⼭�� bool�� true�� �ٲٰ�, 
                // �������� DamageReceiver.cs���� ó����.
                isFainting = true;
                print("isFainting");
                
            }
        }
    }
    // �򰥸��� �׸� �̰� +=�� �����
    // Current += 5�� �ƴ϶� 
    // Current = 5�� �ؾ� +=5�� �Ǵ� ����
    [Range(0, 100)]
    private float _CurrentMP = 0;
    public float CurrentMP
    {
        get { return _CurrentMP; }
        set
        {
            _CurrentMP = value;
            if (_CurrentMP > 100) _CurrentMP = 100;
            else if (_CurrentMP < 0) _CurrentMP = 0;

            //Debug.Log(_CurrentMP);
            //Hub.UIManager.mpSlider.value = _CurrentMP / 100;
            Hub.UIManager.mpSlider.fillAmount = _CurrentMP / 100;
        }
    }

    private bool _IsFireGet = false;
    public bool IsFireGet
    {
        get { return _IsFireGet; }
        set
        {
            if (value) Hub.UIManager.spiritFireCover.SetActive(false);
            else Hub.UIManager.spiritFireCover.SetActive(true);
            _IsFireGet = value;
        }
    }
    private bool _IsWaterGet = false;
    public bool IsWaterGet
    {
        get { return _IsWaterGet; }
        set
        {
            if (value) Hub.UIManager.spiritWaterCover.SetActive(false);
            else Hub.UIManager.spiritWaterCover.SetActive(true);
            _IsWaterGet = value;
        }
    }
    private bool _IsWindGet = false;
    public bool IsWindGet
    {
        get { return _IsWindGet; }
        set
        {
            if (value) Hub.UIManager.spiritWindCover.SetActive(false);
            else Hub.UIManager.spiritWindCover.SetActive(true);
            _IsWindGet = value;
        }
    }
    private bool _IsStoneGet = false;
    public bool IsStoneGet
    {
        get { return _IsStoneGet; }
        set
        {
            if (value) Hub.UIManager.spiritStoneCover.SetActive(false);
            else Hub.UIManager.spiritStoneCover.SetActive(true);
            _IsStoneGet = value;
        }
    }

    [Header("Current State")]
    public PlayerState currentPlayerState;
    //�������� ���������� Ȯ��
    public Direct currentDirection;
    public bool isDamageFree = false;
    public bool isFainting = false;
    


    [Header("UI part")]
    public GameObject spiritFire;
    public GameObject spiritWater;
    public GameObject spiritWind;
    public GameObject spiritStone;
    public GameObject hpObject;    

    [Header("CurrentPosition")]
    public bool isInPortal;
    public bool isKeyAll;
    private int _KeyCount = 0;
    public int KeyCount
    {
        get { return _KeyCount; }
        set
        {
            _KeyCount = value;
            for (int i = 0; i < Hub.StageManager.RequiredKeyAmount[Hub.StageManager.currentStage]; i++) fullKey[i].GetComponent<Image>().sprite = Hub.UIManager.keyObject_not_found;
            for (int i = 0; i < _KeyCount; i++) fullKey[i].GetComponent<Image>().sprite = Hub.UIManager.keyObject_found;

            // ������ ���� �� �־ fullKey.Length�� �ƴ� Hub.StageManager.RequiredKeyAmount[Hub.StageManager.currentStage] �̰� ����Ѵ�.
            if (_KeyCount >= Hub.StageManager.RequiredKeyAmount[Hub.StageManager.currentStage]) isKeyAll = true;
            else isKeyAll = false;
        }

    }

    [HideInInspector]    
    float currentMPSaved;
    bool isFireGetSaved;
    bool isWaterGetSaved;
    bool isWindGetSaved;
    bool isStoneGetSaved;
    int fullHPSaved; 
    double currentHPSaved;
    




    // �߰� 
    public bool isGetHeart;
    public float effectDuration = 4f;
    public GameObject heartEffectPrefab;





    public void Awake()
    {        
        currentPlayerState = PlayerState.free;
    }


    public void Update()
    {
        /*
        if (Hub.InputManager.isGaugeFill == true):
            currentMP += 5;
        */
        //�����̰� �߰��� ��
        if (isGetHeart == true)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(ShowHeartEffect(playerObject.transform));
            isGetHeart = false;
        }
    }

    //�����̰� �߰��� �� 
    IEnumerator ShowHeartEffect(Transform playerTransform)
    {
        // ��Ʈ ����Ʈ�� �÷��̾� ��ġ�� ����
        Debug.Log("Heart effect coroutine started");
        GameObject heartEffectInstance = Instantiate(heartEffectPrefab, playerTransform.position + new Vector3(0, 3, 0), Quaternion.identity);
        heartEffectInstance.transform.SetParent(playerTransform); // ����Ʈ�� �÷��̾��� �ڽ����� �����Ͽ� �Բ� �����̵��� ��

        // ������ �ð���ŭ ���
        float rotationSpeed = 180f; // �ʴ� ȸ�� �ӵ� (��)
        float elapsedTime = 0f; // ��� �ð�

        // ������ �ð���ŭ ����ϸ鼭 ȸ��
        while (elapsedTime < effectDuration)
        {
            heartEffectInstance.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0); // z�� �������� ȸ��
            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }
        // yield return new Wait    ForSeconds(effectDuration);

        // �ð��� ������ ����Ʈ ����
        Debug.Log("Destroying heart effect");
        Destroy(heartEffectInstance);
    }





    public void GetFullKey(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject thisKey = Instantiate(Hub.UIManager.keyObject);
            fullKey.Add(thisKey);
            thisKey.transform.SetParent(Hub.UIManager.keyBarObject.transform);
            thisKey.transform.localScale = new Vector3(1, 1, 1);
            //Hub.UIManager.GetFullHP(amount);
        }
    }

    public void DestroyFullKey(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject.Destroy(fullKey[0]);
            fullKey.Remove(fullKey[0]);            
        }
        fullKey.Clear();
    }


    public void GetFullHP(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject thisHP = Instantiate(hpObject);
            fullHP.Add(thisHP);
            thisHP.transform.SetParent(Hub.UIManager.hpBarObject.transform);
            thisHP.transform.localScale = new Vector3(1, 1, 1);
            //Hub.UIManager.GetFullHP(amount);
        }

    }
    
    public void DestroyFullHP()
    {
        try
        {
            while (fullHP[0] != null)
            {
                Destroy(fullHP[0]);
                fullHP.Remove(fullHP[0]);
            }
        }
        catch { }
    }


    public void SetCurrentHP(double amount)
    {
        CurrentHP = amount;
    }

    // 0.5 ������ ������Ű�ų� ���ҽ�Ű�� 
    public void AddCurrentHP(double amount)
    {
        double tmp = CurrentHP + amount;
        Debug.Log(tmp);
        CurrentHP = tmp;


    }

    
    public void SaveCurrentState()
    {
        isFireGetSaved = IsFireGet;
        isWaterGetSaved = IsWaterGet;
        isWindGetSaved = IsWindGet;
        isStoneGetSaved = IsStoneGet;
        currentMPSaved = CurrentMP;
        fullHPSaved = fullHP.Count;
        currentHPSaved = CurrentHP;       
    }

    public void LoadCurrentState()
    {
        IsFireGet = isFireGetSaved;
        IsWaterGet = isWaterGetSaved;
        IsWindGet = isWindGetSaved;
        IsStoneGet = isStoneGetSaved;
        CurrentMP = currentMPSaved;
        DestroyFullHP();
        GetFullHP(fullHPSaved);
        SetCurrentHP(currentHPSaved);
    }











    /*
    public void SetCoolTime(int sprNum, float coolTime)
    {
        switch (sprNum)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                break;
        }
    }*/







    /* ������ �ٳన 
    
    




    */







}
