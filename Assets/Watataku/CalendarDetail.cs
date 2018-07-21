using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CalendarDetail : MonoBehaviour {
    public CalendarManager calendar;    //操作するカレンダー
    public GameObject panel;    //詳細を表示するボード
    public Button Exit; //終了
    public Text Date;   //日付
    public Text pushUp; //腕立てのカウント
    public Button forDebugButton; //カウンタ

    void Start() {
        panel.SetActive(false); //初めに詳細を非表示する
        if(Exit != null){
            Exit.onClick.AsObservable()
                .Subscribe(_ =>
                {
                    panel.SetActive(false);
                });
        }

        if(forDebugButton != null){
            forDebugButton.onClick.AsObservable()
                          .Subscribe(_ =>
            {
                
            });
        }
	}
	
    public void Activate(DateTime dateValue, int count){
        panel.SetActive(true);
        Date.text = dateValue.Year.ToString() + "年"
                  + dateValue.Month.ToString() + "月"
                  + dateValue.Day.ToString() + "日";
        pushUp.text = count.ToString();
    }

}
