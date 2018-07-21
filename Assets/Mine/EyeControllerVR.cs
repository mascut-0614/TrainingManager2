using UnityEngine;

public class EyeControllerVR : MonoBehaviour {
	//raycast関連
	RaycastHit hit;
	GameObject nowhit;
	GameObject prevhit;
    //rayをとばすVRカメラ
    public GameObject cam;
	//各ボタン作動までの時間計測
	float cooltime = 0;
	//raycastが可能か
	public static bool rayOn=true;
    //Trainingのコース
	public static string course;
	//各メニュー全体の空オブジェクト
	public GameObject MainMenu;
	public GameObject TrainingMenu;
	public GameObject CalenderMenu;
	//効果音
	public AudioClip decide;
	private AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = decide;
		prevhit = GameObject.Find("FirstPrev");
	}
	void Update()
    {
		if (!EyeControllerAR.ARon&&!FadeControl.wait)
		{
			if (rayOn && Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
			{
				Debug.Log("In The Sight");
				nowhit = hit.collider.gameObject;
				nowhit.GetComponent<Renderer>().material.color = Color.magenta;
				if (nowhit == prevhit)
				{
					cooltime += 0.1f;
					if (cooltime >= 10f)
					{
						rayOn = false;
						cooltime = 0;
						ColorRev(nowhit);
						string hittag = nowhit.transform.tag;
						ChangeMenu(hittag);
					}
				}
				else
				{
					cooltime = 0;
					ColorRev(prevhit);
				}
				prevhit = nowhit;
			}else{
				ColorRev(prevhit);
				cooltime = 0;
			}
        }
    }
	void ChangeMenu(string tagname){
		audioSource.Play();
		switch(tagname){
			case "Training":
				MainMenu.SetActive(false);
				TrainingMenu.SetActive(true);
				rayOn = true;
				break;
			case "Results":
				MainMenu.SetActive(false);
				CalenderMenu.SetActive(true);
				rayOn = true;
				break;
			case "Push":
				EyeControllerAR.ARon = true;
				course = "Push";
				FindObjectOfType<FadeControl>().FadeOut();
				break;
			case "Crunch":
				EyeControllerAR.ARon = true;
                course = "Crunch";
                FindObjectOfType<FadeControl>().FadeOut();
				break;
			case "Back":
				TrainingMenu.SetActive(false);
				CalenderMenu.SetActive(false);
				MainMenu.SetActive(true);
				rayOn = true;
				break;
			case "Next":
				FindObjectOfType<CalendarManager>().goNext();
				rayOn = true;
				break;
			case "Prev":
				FindObjectOfType<CalendarManager>().goPrev();
				rayOn = true;
				break;
			default:
				rayOn = true;
				break;
		}
	}
	void ColorRev(GameObject target){
		if (target.CompareTag("Today"))
        {
			target.GetComponent<Renderer>().material.color = Color.yellow;
		}else{
			target.GetComponent<Renderer>().material.color = Color.white;
		}
	}
}
