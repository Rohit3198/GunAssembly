using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [SerializeField]
    int totalPartCount=0;
    int partCount=0;

    [SerializeField]
    GameObject parts=null;
    [SerializeField]
    GameObject mainGun=null;
    [SerializeField]
    GameObject secondCamera=null;
    [SerializeField]
    GameObject deathCam=null;
    [SerializeField]
    GameObject wanted=null;
    [SerializeField]
    GameObject deadPanel=null;
    [SerializeField]
    Transform gunEndPos=null;
    bool flag;

    [SerializeField]
    GameObject crossHair = null;

    Vector3 camEndPos;
    Vector3 camStartPos;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        camEndPos = new Vector3(0,5,0);
        camStartPos = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (flag)
        {
            mainGun.transform.position = Vector3.Lerp(mainGun.transform.position, gunEndPos.position, .05f);
            mainGun.transform.rotation = gunEndPos.transform.rotation;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,camEndPos,.05f);
        }
    }

    public void PartPlaced()
    {
        partCount++;
        if(partCount==totalPartCount)
        {
            Debug.Log("Aim");
            parts.SetActive(false);
            mainGun.transform.parent = Camera.main.transform;
            
            mainGun.SetActive(true);
            secondCamera.SetActive(false);
            wanted.SetActive(false);
            flag = true;
            Invoke("DeFlag", .75f);
            
        }
    }

    void DeFlag()
    {
        flag = false;
        mainGun.transform.position = gunEndPos.position;
        mainGun.GetComponent<Animator>().enabled = true;
        Camera.main.GetComponent<CameraController>().enabled = true;
    }

    public void LookEnemy()
    {
        secondCamera.SetActive(false);
        wanted.SetActive(false);
        Invoke("DFlag", .75f);
        flag = true;
    }

    void DFlag()
    {
        flag = false;
        Camera.main.GetComponent<CameraController>().enabled = true;
    }
    public void Aim()
    {
        
        crossHair.SetActive(true);
        mainGun.GetComponent<Renderer>().enabled = false;
    }
    public void StopAim()
    {
        
        crossHair.SetActive(false);
    }
    public void GunShot()
    {
        mainGun.GetComponent<Animator>().SetTrigger("Shot");
    }

    public Transform Muzzle()
    {
        return mainGun.transform.GetChild(0);
    }
    public void DeathCam()
    {
        Camera.main.gameObject.SetActive(false);
        secondCamera.SetActive(false);
        wanted.SetActive(false);
        deathCam.SetActive(true);
        Invoke("Dead", 0.5f);
    }

    void Dead()
    {
        deadPanel.SetActive(true);
    }
    //public void SetShotCam(GameObject g)
    //{
    //   Destroy(Camera.main.GetComponent<CameraController>());
    //    Camera.main.transform.parent = g.transform;
    //    Camera.main.transform.localPosition = g.transform.GetChild(0).localPosition;
    //    Debug.Log(Camera.main.transform.rotation);
    //    Debug.Log(g.transform.GetChild(0).rotation);
    //    Camera.main.transform.rotation = g.transform.GetChild(0).rotation;
    //    Debug.Log(Camera.main.transform.rotation);
    //    Debug.Log(g.transform.GetChild(0).rotation);


    //}    
}
