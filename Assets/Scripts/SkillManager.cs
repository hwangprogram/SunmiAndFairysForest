using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [Header("Amount")]
    public float increseMP;

    [Header("Position")]
    public Transform spiritPos;
    public Transform skillPos;

    [Header("SpiritList")]
    public GameObject fox;
    public GameObject turtle;
    public GameObject squirrel;

    [Header("SkillList")]
    public GameObject dontChaseFox;
    public GameObject dontRunFromFox;
    public GameObject waterCircle;
    public GameObject throwEmOut;

    [HideInInspector]
    private GameObject foxOfSkill;
    private GameObject fireA;
    private GameObject turtleOfSkill;
    private GameObject waterA;
    private GameObject squirrelOfSkill;

    [Space]
    public Text tmpTextField;

    public void Awake()
    {
        tmpTextField = Hub.UIManager.tmpTextField;
    }


    public void GatheringEnergy()
    {
        Hub.PlayerStatus.CurrentMP += increseMP;
        tmpTextField.text = "GatheringEngergy";
    }

    public void FireA()
    {
        tmpTextField.text = "FireA!";
        Destroy(foxOfSkill);
        Destroy(fireA);
        StopCoroutine(FireACor());
        StartCoroutine(FireACor());
        
    }
    IEnumerator FireACor()
    {
        GameObject foxOfSkill2;
        if (Hub.PlayerStatus.currentDirection == Direct.right)
        {
            foxOfSkill2 = Instantiate(fox, spiritPos.position + new Vector3(0f, 0.5f, 0), spiritPos.rotation);
            foxOfSkill = foxOfSkill2;
            fireA = Instantiate(dontChaseFox, skillPos.position + new Vector3(1f, 0.5f, 0), skillPos.rotation);
        }
        else
        {
            foxOfSkill2 = Instantiate(fox, spiritPos.position + new Vector3(0f, 0.5f, 0), spiritPos.rotation);
            foxOfSkill = foxOfSkill2;
            fireA = Instantiate(dontChaseFox, skillPos.position + new Vector3(-1f, 0.5f, 0), skillPos.rotation);
        }
        yield return new WaitForSeconds(3.3f);
        Destroy(foxOfSkill2);
        yield return null;
    }


    public void FireB()
    {
        tmpTextField.text = "FireB!";
        Destroy(foxOfSkill);
        Destroy(fireA);
        StopCoroutine(FireBCor());
        StartCoroutine(FireBCor());
    }
    IEnumerator FireBCor()
    {
        foxOfSkill = Instantiate(fox, spiritPos.position, spiritPos.rotation);
        Instantiate(dontRunFromFox, skillPos.position, skillPos.rotation);
        yield return new WaitForSeconds(0.6f);
        Destroy(foxOfSkill);
        yield return null;
    }




    public void WaterA()
    {
        tmpTextField.text = "WaterA!";
        Destroy(turtleOfSkill);
        Destroy(waterA);
        StopCoroutine(WaterACor());
        StartCoroutine(WaterACor());
    }

    IEnumerator WaterACor()
    {
        GameObject turtleOfSkill2;
        turtleOfSkill2 = Instantiate(turtle, skillPos.position + new Vector3(0, 1f, 0), skillPos.rotation);
        turtleOfSkill = turtleOfSkill2;
        waterA = Instantiate(waterCircle, skillPos.position + new Vector3(0, 1f, 0), skillPos.rotation);
        yield return new WaitForSeconds(3.5f);
        Destroy(turtleOfSkill2);
        yield return null;
    }

    public void WaterB()
    {
        tmpTextField.text = "WaterB";
    }
    public void WindA()
    {
        tmpTextField.text = "WindA!";
        Destroy(squirrelOfSkill);
        StopCoroutine(WindACor());
        StartCoroutine(WindACor());
    }
    IEnumerator WindACor()
    {
        squirrelOfSkill = Instantiate(squirrel, spiritPos.position + new Vector3(0, 0f, 0), spiritPos.rotation);
        if (Hub.PlayerStatus.currentDirection == Direct.right)
        {
            Instantiate(throwEmOut, skillPos.position + new Vector3(1f, 0f, 0), skillPos.rotation);
        }
        else
        {
            Instantiate(throwEmOut, skillPos.position + new Vector3(-1f, 0f, 0), skillPos.rotation);
        }
        yield return new WaitForSeconds(2.5f);
        Destroy(squirrelOfSkill);
        yield return null;
    }

    public void WindB()
    {
        tmpTextField.text = "WindB!";
    }
    public void WindC()
    {
        tmpTextField.text = "WindC!";
    }
}
