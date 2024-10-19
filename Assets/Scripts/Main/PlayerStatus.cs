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


// 왼쪽 향하고 있는지 오른쪽 향하고 있는지 확인해서 
// 스킬이나 정령 소환의 방향을 부여하는 역할.
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
            // 체력이 더해지는 건지 피해를 입는 건지 확인 
            if (_CurrentHP > value)
            {
                //타격 이펙트 
                //타격 사운드
                foreach (GameObject heart in fullHP)
                {
                    heart.GetComponent<a_HPIcon>().half1.SetActive(false);
                    heart.GetComponent<a_HPIcon>().half2.SetActive(false);
                }
                try { Hub.DamageReceiver.GotHit(); }
                catch { } //GameOn이면 catch가 되어 버림.
            }
            else //체력 회복 사운드 


            // 초과되지 않도록 보정
            if (value > fullHP.Count) value = fullHP.Count;
            _CurrentHP = value;

            // 실제적인 보정
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
                // 게임 셋 처리 
                // 시간 차 처리를 위해 여기서는 bool만 true로 바꾸고, 
                // 나머지는 DamageReceiver.cs에서 처리함.
                isFainting = true;
                print("isFainting");
                
            }
        }
    }
    // 헷갈리게 그만 이건 +=로 사용함
    // Current += 5가 아니라 
    // Current = 5를 해야 +=5가 되는 구조
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
    //왼쪽인지 오른쪽인지 확인
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

            // 오류가 있을 수 있어서 fullKey.Length가 아닌 Hub.StageManager.RequiredKeyAmount[Hub.StageManager.currentStage] 이걸 사용한다.
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
    




    // 추가 
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
        //현성이가 추가한 거
        if (isGetHeart == true)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(ShowHeartEffect(playerObject.transform));
            isGetHeart = false;
        }
    }

    //현성이가 추가한 거 
    IEnumerator ShowHeartEffect(Transform playerTransform)
    {
        // 하트 이펙트를 플레이어 위치에 생성
        Debug.Log("Heart effect coroutine started");
        GameObject heartEffectInstance = Instantiate(heartEffectPrefab, playerTransform.position + new Vector3(0, 3, 0), Quaternion.identity);
        heartEffectInstance.transform.SetParent(playerTransform); // 이펙트를 플레이어의 자식으로 설정하여 함께 움직이도록 함

        // 설정된 시간만큼 대기
        float rotationSpeed = 180f; // 초당 회전 속도 (도)
        float elapsedTime = 0f; // 경과 시간

        // 설정된 시간만큼 대기하면서 회전
        while (elapsedTime < effectDuration)
        {
            heartEffectInstance.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0); // z축 방향으로 회전
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }
        // yield return new Wait    ForSeconds(effectDuration);

        // 시간이 지나면 이펙트 제거
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

    // 0.5 단위로 증가시키거나 감소시키기 
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







    /* 최현성 다녀감 
    
    




    */







}
