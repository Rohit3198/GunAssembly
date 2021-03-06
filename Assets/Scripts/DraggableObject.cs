using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    [SerializeField]
    private float requiredHoldTime = 0.1f;

    float dragTimer;
    public int offset = 10;
    bool dragTry;

    Vector3 mousePos;
    Vector3 initPos;

    [SerializeField]
    bool correctDirection;
    bool duringCoroutine;
    bool isGreen;
    
    [SerializeField]
    Material green;
    [SerializeField]
    Material red;
    [SerializeField]
    Material correctGreen;

    public Transform endSnapper;
    int count = 0;
    private void Start()
    {
        dragTimer = requiredHoldTime;
    }

    private void OnMouseDown()
    {
        dragTry = true;

        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, offset);
        initPos = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
    }

    private void OnMouseDrag()
    {
        if (dragTimer < 0)
        {
            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, offset));
            //transform.position = Vector3.Lerp(transform.position, mousePos-initPos, 2f);
            transform.position = mousePos - initPos;

        }
    }

    private void Update()
    {
        if (dragTry)
        {
            dragTimer -= Time.deltaTime;
            count++;

            if (count %20 == 0)
            {
                if (Vector3.Distance(transform.position, endSnapper.position) > .5f && correctDirection)
                {
                    if (isGreen)
                    {
                        isGreen = false;
                        foreach (Material m in endSnapper.GetComponent<Renderer>().materials)
                            m.color = red.color;
                    }
                    else
                    {
                        isGreen = true;
                        foreach (Material m in endSnapper.GetComponent<Renderer>().materials)
                            m.color = green.color;
                    }
                }
                else
                {
                    if (isGreen)
                    {
                        isGreen = false;
                        foreach (Material m in endSnapper.GetComponent<Renderer>().materials)
                            m.color = correctGreen.color;
                    }
                    else
                    {
                        isGreen = true;
                        foreach (Material m in endSnapper.GetComponent<Renderer>().materials)
                            m.color = green.color;
                    }
                }
            }
        }

    }
    private void OnMouseUp()
    {
        if (!duringCoroutine)
        {
            if (dragTimer > 0)
            {
                //StartCoroutine(Rotater());
                //correctDirection = !correctDirection;
            }
            else
            {
                if (Vector3.Distance(transform.position, endSnapper.position) < .2f && correctDirection)
                {
                    transform.position = endSnapper.position;
                    endSnapper.gameObject.SetActive(false);
                    GetComponent<BoxCollider>().enabled = false;
                    GameManager.instance.PartPlaced();
                    this.enabled = false;
                }
            }
        }
        if (endSnapper.gameObject.activeSelf)
        {
            isGreen = true;
            foreach (Material m in endSnapper.GetComponent<Renderer>().materials)
            m.color = green.color;
        }
        dragTry = false;
        count = 0;
        dragTimer = requiredHoldTime;
    }
    IEnumerator Rotater()
    {
        duringCoroutine = true;
        for (int i = 0; i < 36; i++)
        {
            transform.Rotate(0, 5, 0);
            yield return new WaitForSeconds(.01f);
        }
        duringCoroutine = false;
    }
}