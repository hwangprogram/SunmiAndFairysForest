using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public GameObject platform;
    [Range(0.0f, 20.0f)]
    public float moveSpeed = 3.0f;
    public Transform currentPoint;
    public Transform[] points;
    public int pointSelection;
    public Rigidbody2D rb;
    [HideInInspector]
    public bool isPause;
    // 이 플랫폼에 타고 나서야 움직이는지 여부 
    public bool isModeHopOn;
    public bool isHopOnReusable = false;            //
    public float pauseDuration;

    
    public bool isOneWaylock;       // 이게 켜져 있으면 한 번 도착하면 고정되어 버림. 

    // player의 오브젝트를 찾아서 해당 오브젝트의 y값보다 낮을 때만 collider가 활성화되도록 
    // 이를 위해 player의 groundCheck 오브젝트에 Player'sGroundPos 태그를 넣음.
    private Transform playerPos;
    public Transform posOn;
    public Transform posOff;

    



    void Awake()
    {
        currentPoint = points[pointSelection];
        rb = GetComponent<Rigidbody2D>();
        try { playerPos = GameObject.FindGameObjectWithTag("Player'sGroundPos").transform; }        
        catch { }
    }

    // Update is called once per frame
    void Update()
    {
        if (posOff.position.y > playerPos.position.y) rb.simulated = false;
        else if (posOn.position.y < playerPos.position.y) rb.simulated = true;
        if (!isPause & !isModeHopOn) MoveObj();

    }

    void MoveObj()
    {
        rb.MovePosition(transform.position + transform.forward * Time.deltaTime);
        platform.transform.position = Vector2.MoveTowards(platform.transform.position, currentPoint.position, Time.deltaTime * moveSpeed);
        if (platform.transform.position == currentPoint.position)
        {
            isPause = true;
            pointSelection++;
            if (pointSelection == points.Length)
            {
                pointSelection = 0;
            }
            currentPoint = points[pointSelection];
            if (!isOneWaylock) StartCoroutine(WaitForAWhile());
        }
    }

    IEnumerator WaitForAWhile()
    {
        yield return new WaitForSeconds(pauseDuration);
        isPause = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            other.transform.SetParent(transform);
            if (isModeHopOn) isModeHopOn = false;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            other.transform.SetParent(null);
            // 끝 부분에서 이 플랫폼에 내린 경우만 HopOn이 다시 되도록 isPause
            if (isHopOnReusable & isPause) isModeHopOn = true;
        }
    }



}
