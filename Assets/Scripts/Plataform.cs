using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataform : MonoBehaviour
{
    // Configuration Parameters
    [SerializeField] bool horizontalPlataform = false;
    [SerializeField] float blockSpeed = 1f;
    [SerializeField] float topLimit = 1f;
    [SerializeField] float bottomLimit = 1f;
    [SerializeField] float leftLimit = 1f;
    [SerializeField] float rightLimit = 1f;

    // Cached States
    private bool hasReachedTop;
    private bool hasReachedBottom;
    private bool hasReachedRight;
    private bool hasReachedLeft;

    // Cached References
    private Rigidbody2D myRigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!horizontalPlataform)
        {
            MonitorVerticalPosition();
            MoveVertically();
        }

        else if (horizontalPlataform)
        {
            MonitorHorizontalPosition();
            MoveHorizontally();
        }
    }

    private void MoveVertically()
    {
        var blockAceleration = blockSpeed * Time.deltaTime;

        if (!hasReachedTop)
        {
            myRigidbody2D.transform.position += Vector3.up * blockAceleration;
        }
        else if (!hasReachedBottom)
        {
            myRigidbody2D.transform.position += Vector3.down * blockAceleration;
        }
    }

    private void MonitorVerticalPosition()
    {
        if (transform.position.y >= topLimit)
        {
            hasReachedTop = true;
            hasReachedBottom = false;
        }
        else if (transform.position.y <= bottomLimit)
        {
            hasReachedTop = false;
            hasReachedBottom = true;
        }
    }

    private void MoveHorizontally()
    {
        var blockAceleration = blockSpeed * Time.deltaTime;

        if (!hasReachedRight)
        {
            myRigidbody2D.transform.position += Vector3.right * blockAceleration;
        }
        else if (!hasReachedLeft)
        {
            myRigidbody2D.transform.position += Vector3.left * blockAceleration;
        }
    }

    private void MonitorHorizontalPosition()
    {
        if (transform.position.x >= rightLimit)
        {
            hasReachedRight = true;
            hasReachedLeft = false;
        }
        else if (transform.position.x <= leftLimit)
        {
            hasReachedRight = false;
            hasReachedLeft = true;
        }
    }
}
