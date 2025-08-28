using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zero : MonoBehaviour
{
    [Header("左相机箱子")]
    public GameObject leftBox; // 左边相机的对应箱子

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (leftBox != null)
            leftBox.SetActive(false);
    }

    public void Push(float direction, float speed)
    {
        if (rb == null) return;

        // 激活左相机箱子
        if (leftBox != null && !leftBox.activeSelf)
        {
            leftBox.SetActive(true);
        }

        // 移动箱子
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;

            if (leftBox != null && !leftBox.activeSelf)
            {
                leftBox.SetActive(true);
            }
        }
    }
}
