using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0.0001f, 1f)]
    public float posSmoothness;

    public ParticleSystem blood;
    public ParticleSystem poof;

    public GameObject bulletProjectile;

    bool flag;

    Camera _targetCamera;
    public float _minXAngle, _maxXAngle, _minYAngle, _maxYAngle;
    public float sensitivityX, sensitivityY;

    public static float _targetFieldOfView, speed;
    public float rotationY, rotationX;
    Vector3 firstPoint, secondPoint, FinalRotWithLerp;
    public float extraXVal, extraYVal, xTemp = 0, yTemp = 0, downTime, fingerId, scopeVal, tempRotationY, tempRotationX;


    public float range = 1200f;

    bool isMove;
    // Start is called before the first frame update
    void Start()
    {
        flag = false;
        //Vector3 _new = new Vector3(-rotationY + extraXVal, rotationX + extraYVal, 0);
        //transform.localEulerAngles = _new;
        _targetFieldOfView = 60;
        _targetCamera = this.GetComponent<Camera>();

        speed = 80;
    }

    // Update is called once per frame
    void Update()
    {
        if (!flag)
        {
            flag = true;
            StartFps();
        }

        if (!isMove)
        {
            SetFieldOfView();
            FpsControl();
        }
    }

    void SetFieldOfView()
    {
        if (_targetFieldOfView != _targetCamera.fieldOfView)
        {
            if (_targetCamera.fieldOfView > _targetFieldOfView + 1)
            {
                _targetCamera.fieldOfView -= (Time.deltaTime * speed);
            }
            else if (_targetCamera.fieldOfView < _targetFieldOfView - 1)
            {
                _targetCamera.fieldOfView += (Time.deltaTime * speed);
            }
            else
            {
                _targetCamera.fieldOfView = _targetFieldOfView;
            }
        }
    }

    void FpsControl()
    {

        if (Input.GetMouseButton(0) && Input.mousePosition.x > Screen.width / 6)
        {
            if (Input.GetMouseButtonDown(0))
            {
                firstPoint = Input.mousePosition;
                xTemp = rotationX;
                yTemp = rotationY;
                _targetFieldOfView = 40;
                GameManager.instance.Aim();
            }
            if (Input.GetMouseButton(0))
            {
                secondPoint = Input.mousePosition;
                float _dividerVal = 50;

                float xVAL = xTemp + ((secondPoint.x - firstPoint.x) * 180 / Screen.width * (_targetFieldOfView / _dividerVal));
                float yVAL = yTemp + ((secondPoint.y - firstPoint.y) * 90 / Screen.height * (_targetFieldOfView / _dividerVal));
                rotationX = Mathf.Clamp(xVAL, _minXAngle + _targetFieldOfView / _dividerVal, _maxXAngle + _targetFieldOfView / _dividerVal);
                rotationY = Mathf.Clamp(yVAL, _minYAngle + _targetFieldOfView / _dividerVal, _maxYAngle + _targetFieldOfView / _dividerVal);
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            Shoot();
            GameManager.instance.StopAim();
            _targetFieldOfView = 60;
        }
        extraXVal = ((Mathf.Sin(Time.time)) / (100 - 0));
        extraYVal = ((Mathf.Cos(Time.time)) / (100 - 0));

        FinalRotWithLerp.x = Mathf.Lerp(FinalRotWithLerp.x, rotationX, Time.deltaTime * 4);

        FinalRotWithLerp.y = Mathf.Lerp(FinalRotWithLerp.y, rotationY, Time.deltaTime * 4);
        FinalRotWithLerp.z = 0;

        Vector3 _new = new Vector3(-FinalRotWithLerp.y + extraXVal, FinalRotWithLerp.x + extraYVal, 0);
        transform.localEulerAngles = _new;

    }
    void StartFps()
    {


        rotationX = 0;
        rotationY = -90;
        FinalRotWithLerp = new Vector3(0, -90f, 0);
        
        rotationY = 0; ;
        xTemp = rotationX;
        yTemp = rotationY;


    }

    void Shoot()
    {
        Debug.Log("Fire");
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {

            if (hit.transform.CompareTag("Villian"))
            {
                Debug.Log("Shot");
                
                GameObject g = Instantiate(bulletProjectile, GameManager.instance.Muzzle().position, Quaternion.identity);
                g.transform.LookAt(hit.point);
                
                GameManager.instance.GunShot();
                StartCoroutine(SlowDeath(hit));
                //GameManager.instance.SetShotCam(g); 
            }

        }
    }
    
   IEnumerator SlowDeath(RaycastHit hit)

    {
        yield return new WaitForSeconds(2f);
        hit.transform.GetComponent<Animator>().SetTrigger("Die");
        Instantiate(blood, hit.point, Quaternion.identity);
        poof.gameObject.SetActive(false);
    }
        


}
