using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceDebug : MonoBehaviour {
	public Text Dist;
	public Text Count;
	// Update is called once per frame
	void Update () {
		if(EyeControllerAR.ARon){
			if (EyeControllerAR.distCal)
            {
                Dist.text = TrainingChecker.dis.ToString();
                Count.text = TrainingChecker.count.ToString();
            }
            else
            {
				Dist.text = TrainingChecker.xsensor.ToString();
            }
		}else{
			Dist.text = "";
			Count.text = "";
		}

	}
}
