/****************************************************
	文件：PageTest.cs
	作者：世界和平
	日期：2021/2/7 12:20:36
	功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rosiness;

public class PageTest : MonoBehaviour
{
    RSPageScrollRect pageScrollRect;

    [SerializeField]
    Button button;
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        pageScrollRect = GetComponent<RSPageScrollRect>();
        index = 0;

    }

    // Update is called once per frame
    public void Next()
    {
        index++;
        RosinessLog.Log("" + index);
        index = index > 2 ? 0 : index;
        pageScrollRect.doScrollAnimToPage(index);
    }
}
