using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public GameObject heldObject;
    public GameObject heldObject2;
    public GameObject fullGun;
    public GameObject muzzle;
    public GameObject bulletPref;
    public GameObject player;
    public ParticleSystem flash;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Aim", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Aim()
    {
        heldObject.SetActive(false);
        heldObject2.SetActive(false);
        fullGun.SetActive(true);
        Camera.main.GetComponent<CameraController>().poof.gameObject.SetActive(false);
        GetComponent<Animator>().SetTrigger("Aim");
        GameManager.instance.LookEnemy();
        Invoke("Shoot", 2f);

    }

    void Shoot()
    {
        flash.Play();
        GameObject g= Instantiate(bulletPref, muzzle.transform.position, Quaternion.identity);
        g.transform.LookAt(player.transform.position);
    }
}
