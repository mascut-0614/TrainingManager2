using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarManager : MonoBehaviour
{
    enum csvData { date, pushUp, crunch };  //enumによるインデックスの宣言(記録用)
    int csvSize = Enum.GetNames(typeof(csvData)).Length;    //enumの大きさ
	public GameObject calendarParent;   //親オブジェクト
	public GameObject nextButton;   //来月
	public GameObject prevButton;   //先月
	public DateTime current;    //カレンダーの日時
	GameObject[] objDays = new GameObject[42];  //Buttonオブジェクト
	CalendarButton[] Days = new CalendarButton[42]; //カレンダーの日付マス
	public GameObject monthText;  //カレンダーの月テキスト

    const string sumi = "Textures/sumi";    //スタンプの場所
    const string sumi2 = "Textures/sumi2";  //スタンプの場所2
    Texture2D stamp;                        //スタンプのテクスチャ
    Texture2D stamp2;                       //スタンプのテクスチャ2

	public GameObject Dlight;
	public static int total;
	CSVReader csvreader = new CSVReader();
	// Use this for initialization
	void Start()
	{
		//現在の日付を取得
		current = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

        //テクスチャのデータをロードする
        stamp = Resources.Load<Texture2D>(sumi);

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
        //月ごとの成果の合計
        total = 0;  

        //今月の1日と晦日を入れる
        DateTime headDate = new DateTime(current.Year, current.Month, 1);
        DateTime tailDate = new DateTime(current.Year, current.Month, DateTime.DaysInMonth(current.Year, current.Month));

		//csv読み込み用のストリームと文字列
		StreamReader sr;
        string readLine;

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
            //スタンプの初期化
            cDay.GetComponent<Renderer>().material.mainTexture = null;

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

            //今月の日にちでなければボタンを非表示にする
            if(isInRange(MakeDate(cDay.dateValue), headDate, tailDate)){
                
            }

			//その日トレーニングを行ったかどうかをカレンダーで表示する
			sr = new StreamReader(@"saveData.csv", Encoding.GetEncoding("Shift_JIS"));
            while ((readLine = sr.ReadLine()) != null)
            {
                string[] splitedLine = readLine.Split(',');
                //該当する日付があればスタンプを載せる
                if (MakeDate(cDay.dateValue) == splitedLine[(int)csvData.date] && isInRange(MakeDate(cDay.dateValue), headDate, tailDate)){
                    cDay.GetComponent<Renderer>().material.mainTexture = stamp;
                }
            }

		}
        //カレンダーの月を更新する
		monthText.GetComponent<TextMesh>().text = current.Year.ToString() + "年 " + current.Month.ToString() + "月";

        sr = new StreamReader(@"saveData.csv", Encoding.GetEncoding("Shift_JIS"));
        //今月の成果をtotalに加算する
        while((readLine = sr.ReadLine())!=null){
            string[] splitedLine = readLine.Split(',');
            if (isInRange(splitedLine[(int)csvData.date], headDate, tailDate))
            {
                //totalを合算する
                for (int i = 1; i < csvSize; i++)
                {
                    total += Int32.Parse(splitedLine[i]);
                }
            }
        }
        sr.Close();

		//合計値によって背景を変更する
		if (total >= 400)
        {
            Dlight.GetComponent<Light>().color = new Color(255f/255f, 164f/255f, 49f/255f);
        }
        else
        {
			float red = total / 400f;
			float green = 164f / 255f * total / 400f;
			float blue = 49f * total / 400f / 255f;
			Dlight.GetComponent<Light>().color = new Color(red,green, blue);
        }
	}

    //日付でシリアルナンバーを作成するメソッド
    string MakeDate(DateTime tm)
    {
        return tm.Year.ToString() + (((int)tm.Month < 10) ? "0" : "")
               + tm.Month.ToString() + (((int)tm.Day < 10) ? "0" : "") + tm.Day.ToString();
    }

    //シリアルが今月のカレンダーに入ってるかを判断する
    public bool isInRange(string date, DateTime head, DateTime tail)
    {
        int headDate = Int32.Parse(MakeDate(head));    //カレンダーの日付の最初のシリアル
        int tailDate = Int32.Parse(MakeDate(tail));  //カレンダーの日付の最後のシリアル
        int readDate = Int32.Parse(date); //調べるカレンダーの日付のシリアル
        return (readDate >= headDate && readDate <= tailDate) ? true : false;  //レンジに入っていればtrueを返す
    }

    //来月のカレンダーに移る
	public void goNext(){
        current = current.AddMonths(1); //月を進める
        setCalendar();                  //カレンダーの更新
	}

    //先月のカレンダーに移る
	public void goPrev(){
        current = current.AddMonths(-1);//月を戻す
        setCalendar();                  //カレンダーの更新
	}
}