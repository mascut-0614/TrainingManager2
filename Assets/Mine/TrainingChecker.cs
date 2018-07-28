using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingChecker : MonoBehaviour {
	public static int count = 0;
	bool countOn = false;
	public GameObject target;
	public static float dis;//距離計測
	public AudioClip ok;
	public AudioClip clear;
	private AudioSource audioSource;
	public static float xsensor = 0;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();

	}
	private void Update()
	{
		switch(EyeControllerVR.course){
			case "Push":
				//Targetが視界にある時のみ計測可能
				if(EyeControllerAR.distCal){
					dis = Vector3.Distance(this.transform.position, target.transform.position);
					Debug.Log(dis);
                    if (dis <20f)
                    {
                        countOn = true;
                    }
                    if (dis >30f)
                    {
						if(countOn){
							count++;
                            countOn = false;
                            if (count < 10)
                            {
                                audioSource.clip = ok;
                                audioSource.Play();
                            }
                            if (count == 10)
                            {
                                //レコーダを呼び出して書き込む
                                FindObjectOfType<CalendarRecorder>().CallRecorder(count, 1);
								count = 0;
                                audioSource.clip = clear;
                                audioSource.Play();
                                EyeControllerAR.ARon = false;
                                EyeControllerAR.distCal = false;
                                FindObjectOfType<FadeControl>().FadeOut();
                            }
						}
                    }
				}
				break;
			case "Crunch":
				if (EyeControllerAR.distCal)
				{
					dis = Vector3.Distance(this.transform.position, target.transform.position);
					if(dis<30f){
						countOn = true;
					}
				}else{
					var rotRH = Input.gyro.attitude;
					var rot = new Quaternion(-rotRH.x, -rotRH.z, -rotRH.y, rotRH.w) * Quaternion.Euler(90f, 0f, 0f);
					xsensor = rot.x;
					if(countOn&&xsensor>0.4f){
						count++;
                        countOn = false;
                        if (count < 10)
                        {
                            audioSource.clip = ok;
                            audioSource.Play();
                        }
                        if (count == 10)
                        {
                            //レコーダを呼び出して書き込む
                            FindObjectOfType<CalendarRecorder>().CallRecorder(count, 2);
							count = 0;
                            audioSource.clip = clear;
                            audioSource.Play();
                            EyeControllerAR.ARon = false;
                            EyeControllerAR.distCal = false;
                            FindObjectOfType<FadeControl>().FadeOut();
                        }
					}
				}
				break;
			default:
				break;
		}
	}

}
