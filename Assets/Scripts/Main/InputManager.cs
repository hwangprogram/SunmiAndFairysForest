using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Key Configure")]
    public string keyA = "z";
    public string keyB = "x";
    public string keyC = "c";
    //public string 

    // ��ȣ�ۿ� Ű
    public string keyInteraction = "f";

    public bool isUP = false;
    public bool isDown = false;
    public bool isLeft = false;
    public bool isRight = false;
    public float horizontalInput;
    public float verticalInput;

    public bool isA = false;
    public bool isB = false;
    public bool isC = false;

    // ��ȣ�ۿ� Ű
    public bool isInteraction = false;

    public bool isAOnce = false;
    public bool isBOnce = false;
    public bool isCOnce = false;

    // ��ȣ�ۿ� Ű
    public bool isInteractionOnce = false;

    public bool isESC = false;
    public bool isESCOnce = false;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");



        isA = false;
        isB = false;
        isC = false;
        isESC = false;

        // ��ȣ�ۿ� Ű
        isInteraction = false;

        isAOnce = false;
        isBOnce = false;
        isCOnce = false;
        isESCOnce = false;

        isInteractionOnce = false;

        // ���� ������
        if (horizontalInput > 0f)
        {
            // Debug.Log("���������� �̵� ��");
            isLeft = false;
            isRight = true;
        }
        else if (horizontalInput < 0f)
        {
            // Debug.Log("�������� �̵� ��");
            isLeft = true;
            isRight = false;
        }
        else { isLeft = false; isRight = false; }

        // �� �Ʒ�
        if (verticalInput > 0f)
        {
            // Debug.Log("���� �̵� ��");
            isDown = false;
            isUP = true;

        }
        else if (verticalInput < 0f)
        {
            // Debug.Log("�Ʒ��� ���̱�");
            isUP = false;
            isDown = true;

        }
        else
        {
            isUP = false;
            isDown = false;
        }



        if (Input.GetKey(keyA))
        {
            isA = true;
            if (Input.GetKeyDown(keyA)) isAOnce = true;
        }
        if (Input.GetKey(keyB))
        {
            isB = true;
            if (Input.GetKeyDown(keyB)) isBOnce = true;
        }
        if (Input.GetKey(keyC))
        {
            isC = true;
            if (Input.GetKeyDown(keyC)) isCOnce = true;
        }
        if (Input.GetKey(keyInteraction))
        {
            isInteraction = true;
            if (Input.GetKeyDown(keyInteraction)) isInteractionOnce = true;
        }
        if (Input.GetKey(KeyCode.Escape))
        {
            isESC = true;
            if (Input.GetKeyDown(KeyCode.Escape)) isESCOnce = true;
        }
        //else isA = false;



    }
}
