using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Util
{
    private static Dictionary<float, WaitForSeconds> _delayDic = new Dictionary<float, WaitForSeconds>();
    private static Dictionary<float, WaitForSecondsRealtime> _realDelayDic = new Dictionary<float, WaitForSecondsRealtime>();

    private static StringBuilder _sb = new StringBuilder();


    public static WaitForSeconds GetDelay(this float delay)
    {
        if (_delayDic.ContainsKey(delay) == false)
        {
            _delayDic.Add(delay, new WaitForSeconds(delay));
        }
        return _delayDic[delay];
    }


    public static WaitForSecondsRealtime GetRealDelay(this float delay)
    {
        if (_realDelayDic.ContainsKey(delay) == false)
        {
            _realDelayDic.Add(delay, new WaitForSecondsRealtime(delay));
        }
        return _realDelayDic[delay];
    }

    public static StringBuilder GetSB(this string text)
    {
        _sb.Clear();
        _sb.Append(text);
        return _sb;
    }
}