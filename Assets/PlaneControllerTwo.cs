using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlaneControllerTwo : MonoBehaviour
{
    public float force = 0;
    public int rotation = 0;
    public float maxRotation = 0;

    public Transform camT;

    Rigidbody2D rb;

    public Transform steeringKnob;
    public Transform forceKnob;

    bool isUsing = false;

    public ActiveAction action = ActiveAction.NONE;

    public TextMeshPro powerText;
    public Text heightText;
    public Text speedText;

    public enum ActiveAction
    {
        NONE,
        STEERING,
        FORCE
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        force = 0;
        rotation = 0;
    }

    void FixedUpdate()
    {
        UpdateHeightAndSpeed();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.transform == forceKnob)
                {
                    action = ActiveAction.FORCE;
                }
                else if (hit.collider.transform == steeringKnob)
                {
                    action = ActiveAction.STEERING;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
        {
            action = ActiveAction.NONE;
        }

        switch (action)
        {
            case ActiveAction.FORCE:
                force = Mathf.Clamp(forceKnob.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x, -2, 2) - 2;
                powerText.text = "Poder: " + (int)(force * -25f) + "%";
                forceKnob.localPosition = new Vector3(force + 2, 0, -0.1f);
                break;
            case ActiveAction.STEERING:
                rotation = Mathf.RoundToInt(Mathf.Clamp(steeringKnob.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x, -2, 2));
                steeringKnob.localPosition = new Vector3(rotation, 0, -0.1f);
                break;
        }

        camT.position = new Vector3(transform.position.x, transform.position.y, camT.position.z);

        if (rotation > 0 && transform.rotation.eulerAngles.z < 180 + maxRotation)
        {
            rb.AddTorque((float)rotation / 5);
        }
        if (rotation < 0 && transform.rotation.eulerAngles.z > 180 - maxRotation)
        {
            rb.AddTorque((float)rotation / 5);
        }
        if (transform.rotation.eulerAngles.z > 180 + maxRotation)
        {
            rb.angularVelocity = 0;
        }
        if (transform.rotation.eulerAngles.z < 180 - maxRotation)
        {
            rb.angularVelocity = 0;
        }

        rb.AddForce(transform.right * -force * 6);
        //rb.AddForce(transform.up * transform.rotation.eulerAngles.z * (-rb.velocity.x) / 450f);

        rb.gravityScale = Mathf.Lerp(1, 0f, -rb.velocity.x / 25f);
        //Physics2D.gravity = new Vector3(0, Mathf.Lerp(-9.8f, 0f, -rb.velocity.x/25f), 0);


        rb.velocity *= new Vector2(0.995f, 1);

        if (rb.velocity.y > 0)
        {
            rb.velocity *= new Vector2(1, 0.995f);
        }
        else
        {
            rb.velocity *= new Vector2(1, 1.0001f);
        }
    }

    void UpdateHeightAndSpeed()
    {
        heightText.text = "Altura\n - Actual:" + (int)transform.position.y + "mts.\n - Objetivo: 500 ~ 700 mts.";
        speedText.text = "Velocidad\n - Actual:" + (int)(rb.velocity.magnitude * 14) + "kms/h.\n - Objetivo: 1040km/h";
    }
}