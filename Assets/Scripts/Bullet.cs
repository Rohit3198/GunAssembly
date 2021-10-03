using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            Debug.Log("Hit");
            GameManager.instance.DeathCam();
            collision.gameObject.GetComponent<Animator>().SetTrigger("Die");
            collision.gameObject.transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        }
        moveSpeed = 0;
        GetComponent<Renderer>().enabled = false;
        
    }
}
