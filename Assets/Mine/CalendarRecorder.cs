using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarRecorder : MonoBehaviour
{
    enum csvData { date, pushUp, crunch };  //enumによるインデックスの宣言(記録用)
    int csvSize = Enum.GetNames(typeof(csvData)).Length;    //enumの大きさ
    private string directory;   //データを保存する場所
    DateTime current = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

    //csvに回数を記録する
    //type -> トレーニングの種類：type=1->pushUp, type=2->crunch
    public void CallRecorder(int count, int type)
    {
        //読み込み用のストリームの宣言
        StreamReader sr = new StreamReader(@"saveData.csv", Encoding.GetEncoding("Shift_JIS"));
        string readLine;    //読み込む文字列
        string writeLine = "";  //書き込む文字列
        string[] tmpLine = new string[csvSize]; //成果があった場合に一時的に保存する

        bool isExist = false;//同じ日が書き込まれているかのフラグ
        //csvの読み込み
        while ((readLine = sr.ReadLine()) != null)
        {
            string[] splitedLine = readLine.Split(',');
            //今日の成果でなければ残す
            if (splitedLine[(int)csvData.date] != MakeDate(current))
                writeLine += string.Join(",", splitedLine) + "\r";
            else {
                //今日の成果であれば、一時的に配列を保存する
                tmpLine = splitedLine;
                isExist = true;
            }
        }
        sr.Close();

        //書き込み用ストリームの宣言
        StreamWriter sw = new StreamWriter(@"saveData.csv", false, Encoding.GetEncoding("Shift_JIS"));
        //いじらないデータを一旦書き込む
        sw.Write(writeLine);
        //今日の成果を書き込む
        writeLine = MakeDate(current);
        for (int i = 1; i < csvSize; i++){
            //すでに書き込みがあれば更新する
            //トレーニングのタイプで加算する数字とそうでない数字を分ける
            if (isExist)
                writeLine += (type == i) ? ',' + (Int32.Parse(tmpLine[i]) + count).ToString() : ',' + tmpLine[i];
            //存在しなければ新たに文字列を追加する
            else
                writeLine += (type == i) ? ',' + count.ToString() : ",0";
        }
        writeLine += '\r';
        sw.Write(writeLine);
        sw.Close();
    }

    //日付でシリアルナンバーを作成するメソッド
    string MakeDate(DateTime tm)
    {
        return tm.Year.ToString() + (((int)tm.Month < 10) ? "0" : "")
               + tm.Month.ToString() + (((int)tm.Day < 10) ? "0" : "") + tm.Day.ToString();
    }


}