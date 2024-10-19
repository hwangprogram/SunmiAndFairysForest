using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine;

public class a_Arrow : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D col;

    void Start()
    {
        StartCoroutine(ArrowCor());
    }

    IEnumerator ArrowCor()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WaterCircle")
        {
            this.gameObject.tag = "Untagged";
            col.isTrigger = false;
            rb.velocity = rb.velocity * 0.1f;
        }

        if (collision.tag == "ThrowEmOut")
        {
            this.gameObject.tag = "Untagged";
            //col.isTrigger = false;
            rb.velocity = Vector3.zero;
            Vector2 tmpDirectionThrowEmOut;
            if (collision.GetComponent<a_ThrowEmOut>().direct == "left") tmpDirectionThrowEmOut = new Vector2(-1, -0.2f);
            else tmpDirectionThrowEmOut = new Vector2(1, 0.2f);
            rb.AddForce(tmpDirectionThrowEmOut * 7f, ForceMode2D.Impulse);
            rb.AddTorque(tmpDirectionThrowEmOut.x * 125f, ForceMode2D.Force);
        }

        if (collision.tag == "Player")
        {
            print("Colllsion");
            Color tmpColor;
            tmpColor = this.gameObject.GetComponent<SpriteRenderer>().color;
            tmpColor.a = 0f;
            this.gameObject.GetComponent<SpriteRenderer>().color = tmpColor;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "WaterCircle")
        {
            Destroy(this.gameObject);
        }
    }
}
