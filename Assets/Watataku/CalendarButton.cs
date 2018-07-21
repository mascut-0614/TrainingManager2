using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


//カレンダーのボタンのレイアウトを変更するスクリプト
public class CalendarButton : MonoBehaviour
{
	enum csvData { date, pushUp };  //enumによるインデックスの宣言(記録用)
	public CalendarManager calendar;    //操作するカレンダー
    public CalendarDetail detail;
	public GameObject button;   //マス
	public GameObject text;   //日付け
    public int count;   //トレーンングの回数

    //マスの日時
	[HideInInspector] public DateTime dateValue;
    //ボタン番号
    [HideInInspector] public int index;

    // Use this for initialization
    void Start()
    {
        //ボタンのコンポーネント化、およびテキストサイズなど
        calendar = FindObjectOfType<CalendarManager>();
		//detail = FindObjectOfType<CalendarDetail>();
		text = this.transform.GetChild(0).gameObject;
		text.GetComponent<TextMesh>().fontSize = 20;

        //カレンダーのマスのレイアウト変更
        this.ObserveEveryValueChanged(dateValue => dateValue.dateValue)
            .Subscribe(_ =>
            {
			text.GetComponent<TextMesh>().text = dateValue.isDefault() ? "" : dateValue.Day.ToString();
			text.GetComponent<TextMesh>().color = GetColorDayOfWeek(dateValue.DayOfWeek);
				//今日の日付を目立たせる
			if (dateValue == DateTime.Today) {
				this.GetComponent<Renderer>().material.color = Color.yellow;
				this.transform.tag = "Today";
			}else {
				this.GetComponent<Renderer>().material.color = Color.white;
				this.transform.tag = "Untagged";
			}
            });
    }
    //岩瀬　追加分
	public void DetailActivate(){
		detail.Activate(dateValue, count);
	}

    //土日の色を変える関数
    Color GetColorDayOfWeek(DayOfWeek dayOfWeek){
		if(dateValue.Month == calendar.current.Month){
			switch(dayOfWeek){
				case DayOfWeek.Saturday:
					return Color.blue;
				case DayOfWeek.Sunday:
					return Color.red;
				default:
					return Color.black;
			}
		} else {
			//先月、来月の日付を灰色で表示
			return Color.gray;
		}
	}
}

//DateTimeの拡張
public static class DateTimeExtension{
	//デフォルトとして、0001/01/01を入れる
	static DateTime Default = new DateTime();
    //デフォルトの値と比較
	public static bool isDefault(this DateTime d){
    	return d.Equals(Default);
	}
}
