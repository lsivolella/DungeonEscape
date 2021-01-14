using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVerticalScroll : MonoBehaviour
{
    // Configuration Parameters
    [SerializeField] float waterSpeed = 1f;
    [SerializeField] float waterSpeedIncrement = 1f;

    // Cached References
    private Rigidbody2D myRigidbody2D;
    private CompositeCollider2D myCompositeCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myCompositeCollider2D = GetComponent<CompositeCollider2D>();
    }

    private void FixedUpdate()
    {
        MoveVertically();
    }

    private void MoveVertically()
    {
        myRigidbody2D.transform.position += Vector3.up * waterSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IncreaseWaterSpeed(collision);
    }

    private void IncreaseWaterSpeed(Collider2D collision)
    {
        if (myCompositeCollider2D.IsTouchingLayers(LayerMask.GetMask("WaterTrigger")))
        {
            waterSpeed += waterSpeedIncrement;
            collision.gameObject.SetActive(false);
        }
    }
}
