using System;
using System.IO;
using UnityEngine;

public class Logger
{
    private static readonly string fileName;
    private static readonly string logPath;

    static Logger()
    {
        // 현재 날짜와 시간으로 파일 이름 생성 (프로그램 시작 시 1회만 호출)
        fileName = $"{DateTime.Now.ToString("yyMMdd_HHmmss")}.log";
        logPath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
    }

    public static void Log(params object[] _text)
    {
        // 메시지를 문자열로 결합
        string logText = string.Join(" ", _text);

        // log 파일에 log 추가
        File.AppendAllText(logPath, $"{GetDateTimeString()} | {logText}\n");

        // Unity 콘솔에 로그 출력
        Debug.Log(logText);
    }

    private static string GetDateTimeString()
    {
        return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
    }
}