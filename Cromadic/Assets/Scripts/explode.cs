using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour {
    //public script e;
    public GameObject rock;
    public bool first;
    public bool iswaterfall;
    public bool iswellflow;
    public Vector3	blowout;				// 怪物飞散的方向（速度向量）
	public	Vector3	blowout_up;				// ↑的垂直分量
	public	Vector3	blowout_xz;				// ↑的水平分量
    public Vector3	angular_velocity;
	public	float	y_angle;
	public	float 	blowout_speed;
	public	float	blowout_speed_base;

	public	float	forward_back_angle;		// 圆锥的前后倾斜角度

	public	float   base_radius;			// 圆锥的地面半径

	public	float   y_angle_center;
	public	float	y_angle_swing;			// 圆弧的中心（根据动作左右决定该值）

	public	float	arc_length;				// 圆弧的长度（圆周）
	// Use this for initialization
	void Start () {
		base_radius = 0.3f;

				blowout_speed_base = 10.0f;

				forward_back_angle = 0.0f;

				y_angle_center = 0.0f;
				y_angle_swing = 60.0f;
	}
    private void onfire(){
        forward_back_angle += Random.Range(-5.0f, 5.0f);

		arc_length = 2*30.0f;
		arc_length = Mathf.Min(arc_length, 120.0f);
        y_angle = y_angle_center;
        y_angle += -arc_length/2.0f;
        y_angle += y_angle_swing;
        
        blowout_up = Vector3.up;
        blowout_xz = Vector3.right*base_radius;
        blowout_xz = Quaternion.AngleAxis(y_angle, Vector3.up)*blowout_xz;
        blowout = blowout_up + blowout_xz;
        blowout.Normalize();
        blowout = Quaternion.AngleAxis(forward_back_angle, Vector3.forward)*blowout;
        
        blowout_speed = blowout_speed_base*Random.Range(0.8f, 1.2f);
        blowout *= blowout_speed;
        
        
        angular_velocity = Vector3.Cross(Vector3.up, blowout);
        angular_velocity.Normalize();
        angular_velocity *= 3.14f*8.0f*blowout_speed/15.0f*Random.Range(0.5f, 1.5f);

    }
    
    void OnTriggerStay(Collider other){
    
        if(other.tag=="Player"){
            this.onfire();
            rock.GetComponent<Rigidbody>().isKinematic = false;
            rock.GetComponent<Rigidbody>().velocity = blowout;
            rock.GetComponent<Rigidbody>().angularVelocity = angular_velocity;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(first==false){
            this.onfire();
            rock.GetComponent<Rigidbody>().isKinematic = false;
            rock.GetComponent<Rigidbody>().velocity = blowout;
            rock.GetComponent<Rigidbody>().angularVelocity = angular_velocity;
            first=true;
        }
        if(iswaterfall){
            this.onfire();
            rock.GetComponent<Rigidbody>().isKinematic = false;
            rock.GetComponent<Rigidbody>().velocity = -blowout;
            rock.GetComponent<Rigidbody>().angularVelocity = -angular_velocity;
            iswaterfall=false;
        }
        
        if(iswellflow){
            this.onfire();
            rock.GetComponent<Rigidbody>().isKinematic = false;
            rock.GetComponent<Rigidbody>().velocity = blowout;
            rock.GetComponent<Rigidbody>().angularVelocity = -angular_velocity;
            iswaterfall=false;
        }
        if(transform.position.y< -6.0f){
            Destroy(gameObject);
        }
//        if(transform.localPosition.y<10.0f){
//            Destroy(gameObject);
//        }
//        if(transform.localPosition.y> 25.0f){
//            Destroy(gameObject);
//        }
//        if(transform.localPosition.x> 150.0f){
//            Destroy(gameObject);
//        }
//        this.onfire();
//            rock.GetComponent<Rigidbody>().isKinematic = false;
//            rock.GetComponent<Rigidbody>().velocity = blowout;
//            rock.GetComponent<Rigidbody>().angularVelocity = angular_velocity;
//		if(hold_lid){
//            
//        }
	}
}
