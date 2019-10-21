using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour {
    public Transform shotposition;
    public float Range = 50f;
    public Camera camera;
    private WaitForSeconds duration = new WaitForSeconds(1.0f);
    private LineRenderer laserline;
    private float next;
    // Use this for initialization
    void Start () {
        laserline = GetComponent<LineRenderer>();

    }

    //private Vector3 GetWorldPotitionFromMouse()
    //{
    //    Vector3 mousePosition = Input.mousePosition;
    //    Plane plane = new Plane(Vector3.up, new Vector3(0f, 0f, 0f));
    //    Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(mousePosition);

    //    // 求出上面二者的交点
    //    float depth;

    //    plane.Raycast(ray, out depth);

    //    Vector3 worldPosition;

    //    worldPosition = ray.origin + ray.direction * depth;

    //    // Y坐标和玩家保持一致
    //    worldPosition.y = 0;

    //    return worldPosition;
    //}

    //private void SetPlayerDirection()
    //{
    //    // 求出指向鼠标光标位置的角度
    //    Vector3 mousePos = GetWorldPotitionFromMouse();
    //    Vector3 relativePos = mousePos - transform.position;
    //    Quaternion tmpRotation = Quaternion.LookRotation(relativePos);
    //    transform.rotation = tmpRotation;
    //    float targetRotationAngle = player.transform.eulerAngles.y;
    //    float currentRotationAngle = transform.eulerAngles.y;

    //    currentRotationAngle = Mathf.LerpAngle(
    //        currentRotationAngle,
    //        targetRotationAngle,
    //        ScoutingLaserTurnRate * Time.deltaTime);

    //    Quaternion tiltedRotation = Quaternion.Euler(0, currentRotationAngle, 0);


    //}


    public void spray_water(){
        print("w");

        StartCoroutine(shotEffect());
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit, Range))
        {
            //Instantiate(particle, transform.position, transform.rotation);
            laserline.SetPosition(0, hit.point);
            laserline.SetPosition(1, shotposition.transform.position);
            print(hit);
        }
        else
        {
            laserline.SetPosition(0, Input.mousePosition);
            laserline.SetPosition(1, shotposition.transform.position);
        }
    }


    private IEnumerator shotEffect()
    {
        laserline.enabled = true;
        yield return duration;
        laserline.enabled = false;
    }
}
