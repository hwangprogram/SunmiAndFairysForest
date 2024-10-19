using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_ThrowEmOut : MonoBehaviour
{
    public int count;
    public Transform pos;
    [HideInInspector] public string direct = "none";

    void Start()
    {
        if (count == 3) Hub.SFXManager.throwEmOut.Play();
        if (direct == "none")
        {
            if (Hub.PlayerStatus.currentDirection == Direct.left) direct = "left";            
            else direct = "right";
        }
        if (direct == "left") this.transform.localScale = new Vector3(-1.5f, 1.5f, 1);               
        StartCoroutine(Cor());
    }

    IEnumerator Cor()
    {
        yield return new WaitForSeconds(0.2f);        
        if (count != 0)
        {
            count = count - 1;
            GameObject tmpObject = Instantiate(this.gameObject, pos.position, pos.rotation);
            tmpObject.GetComponent<a_ThrowEmOut>().direct = direct;
        }
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.75f);
        Destroy(this.gameObject);
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
