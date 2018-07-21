using UnityEngine;

public class EyeControllerAR : MonoBehaviour
{
	RaycastHit hit;
	public GameObject target;
	public GameObject cam;
	public static bool ARon=false;
	public static bool distCal = false;
    
	void Update(){
		if(ARon){
			if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
				Debug.Log("In The Sight");
				target.GetComponent<Renderer>().material.color = Color.magenta;
				distCal = true;
            }
            else
            {
				target.GetComponent<Renderer>().material.color= Color.green;
				distCal = false;
            }
		}
	}
}