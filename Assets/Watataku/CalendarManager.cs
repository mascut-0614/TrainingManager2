using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class CalendarManager : MonoBehaviour
{
    enum csvData { date, pushUp };  //enumによるインデックスの宣言(記録用)
	public GameObject calendarParent;   //親オブジェクト
	public GameObject nextButton;   //来月
	public GameObject prevButton;   //先月
    //public Button backToHome;   //メインメニュに戻る
	public DateTime current;    //カレンダーの日時
	GameObject[] objDays = new GameObject[42];  //Buttonオブジェクト
	CalendarButton[] Days = new CalendarButton[42]; //カレンダーの日付マス
	//public Button toMonth;  //カレンダーの月のボタン
	public GameObject monthText;  //カレンダーの月テキスト


	// Use this for initialization
	void Start()
	{
		//現在の日付を取得
		current = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

		//初期設定
		initCalendarComponent();
        //カレンダーの更新
		setCalendar();
        
	}

    //コンポーネントの取得・設定
    void initCalendarComponent()
	{
		//行
		for (int i = 0; i < calendarParent.transform.childCount; i++)
		{
			//子オブジェクトの保存
			objDays[i] = calendarParent.transform.GetChild(i).gameObject;
			//コンポーネントを設定、取得
			Days[i] = objDays[i].AddComponent<CalendarButton>();
			Days[i].index = i + 1;
		}
	}

	//カレンダーに日付をセット
	void setCalendar()
	{
		int day = 1;
		//今月の1日目
		var first = new DateTime(current.Year, current.Month, day);


		//来月
		var nextMonth = current.AddMonths(1);
		int nextMonthDay = 1;
		//先月
		var prevMonth = current.AddMonths(-1);
		//先月の場合は後ろから数える
		int prevMonthDay = DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month) - (int)first.DayOfWeek + 1;

		//カレンダーのマスに日付を割り当てる
		foreach (var cDay in Days)
        {
            //カウントを一旦初期化
            cDay.count = 0;
			//今月の1日より前は先月の日にちを入れる
			if (cDay.index <= (int)first.DayOfWeek)
			{
				cDay.dateValue = new DateTime(prevMonth.Year, prevMonth.Month, prevMonthDay);
				prevMonthDay++;
			}
			//今月の最終日以降は来月の日にちを入れる
			else if (day > DateTime.DaysInMonth(current.Year, current.Month))
			{
				cDay.dateValue = new DateTime(nextMonth.Year, nextMonth.Month, nextMonthDay);
				nextMonthDay++;
			}
			//今月の日にちを入れる
			else
			{
				cDay.dateValue = new DateTime(current.Year, current.Month, day);
				day++;
			}
		}

        //読み込み用のstring
/*        string line;
        //ストリームリーダを呼び出す
        StreamReader sr = new StreamReader(@"saveData.csv", Encoding.GetEncoding("Shift_JIS"));
        //ストリームの読み込み
        while ((line = sr.ReadLine()) != null)
        {
            string[] splitedLine = line.Split(',');
            //シリアルがカレンダーの範囲内にあるか確かめる
            if(isInRange(splitedLine[(int)csvData.date])){
                //あればその日を探してカウントに入れる
                foreach (var cDay in Days){
                    if(MakeDate(cDay.dateValue) == splitedLine[(int)csvData.date]){
                        cDay.count = Int32.Parse(splitedLine[(int)csvData.pushUp]);
                        break;
                    }
                }
            }
        }
        //ストリームを閉じる
        sr.Close();
*/
        //カレンダーの月を更新する
		monthText.GetComponent<TextMesh>().text = current.Year.ToString() + "年 " + current.Month.ToString() + "月";

	}

    //csvに回数を記録する
/*    public void CallRecorder(){
        StreamReader sr = new StreamReader(@"saveData.csv", Encoding.GetEncoding("Shift_JIS"));
        string readLine;    //読み込む文字列
        string writeLine = "";   //書き込む文字列
        //csvの読み込み
        while ((readLine = sr.ReadLine()) != null)
        {
            string[] splitedLine = readLine.Split(',');
            //今月の成果でなければそのまま記録
            if (!isInRange(splitedLine[(int)csvData.date])){
                writeLine += string.Join(",", splitedLine) + "\r";
            }
        }
        sr.Close();

        StreamWriter sw = new StreamWriter(@"saveData.csv", false, Encoding.GetEncoding("Shift_JIS"));
        sw.Write(writeLine);
        //今月の成果ならば改めて保存する
        foreach(var cDay in Days){
            if (cDay.count == 0) continue;
            writeLine = MakeDate(cDay.dateValue) + "," + cDay.count.ToString() + "\r";
            sw.Write(writeLine);
        }
        sw.Close();
    }
*/
    //日付でシリアルナンバーを作成するメソッド
    string MakeDate(DateTime tm)
    {
        return tm.Year.ToString() + (((int)tm.Month < 10) ? "0" : "")
               + tm.Month.ToString() + (((int)tm.Day < 10) ? "0" : "") + tm.Day.ToString();
    }

    //シリアルが今月のカレンダーに入ってるかを判断する
    bool isInRange(string date){
        int headDate = Int32.Parse(MakeDate(Days[0].dateValue));    //カレンダーの日付の最初のシリアル
        int tailDate = Int32.Parse(MakeDate(Days[Days.Length - 1].dateValue));  //カレンダーの日付の最後のシリアル
        int readDate = Int32.Parse(date); //調べるカレンダーの日付のシリアル
        return (readDate >= headDate && readDate <= tailDate) ? true : false;
    }

	public void goNext(){
		//CallRecorder();
        current = current.AddMonths(1); //月を進める
        setCalendar();
	}
	public void goPrev(){
		//CallRecorder();
        current = current.AddMonths(-1);//月を戻す
        setCalendar();
	}
}