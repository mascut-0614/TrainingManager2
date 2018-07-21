using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class FadeControl : MonoBehaviour {
	float speed = 0.01f;
    float red, green, blue, alpha;
	public GameObject ARmode;
	public GameObject VRmode;
	public static bool wait = false;
	// Use this for initialization
	void Start () {
		red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
        alpha = GetComponent<Image>().color.a;
	}
	public void FadeOut()
    {
        StartCoroutine("FadeChange");
    }
    IEnumerator FadeChange()
    {
		wait = true;
        while (alpha < 1)
        {
            GetComponent<Image>().color = new Color(red, green, blue, alpha);
            alpha += speed;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
		if(EyeControllerAR.ARon){
			VRmode.SetActive(false);
			XRSettings.enabled = false;
			StartCoroutine("LoadAR");
			Debug.Log("Coroutine End");
			ARmode.SetActive(true);
		}else{
			ARmode.SetActive(false);
			XRSettings.enabled = false;
			StartCoroutine("LoadVR");
			Debug.Log("Coroutine End");
			VRmode.SetActive(true);
			EyeControllerVR.rayOn = true;
		}
		while (alpha > 0)
        {
            GetComponent<Image>().color = new Color(red, green, blue, alpha);
            alpha -= speed;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        Debug.Log("Mode Change");
        wait = false;
    }
	IEnumerator LoadAR()
    {
        XRSettings.LoadDeviceByName("Vuforia");
        Debug.Log("Load Vuforia");
        yield return null;
		XRSettings.enabled = true;
    }
	IEnumerator LoadVR()
    {
        XRSettings.LoadDeviceByName("Cardboard");
        Debug.Log("Load Cardboard");
		yield return null;
		XRSettings.enabled = true;
    }
}
