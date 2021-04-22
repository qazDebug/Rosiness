using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rosiness.Utility;

public class MonoUtility : MonoBehaviour
{
    #region Button

    public static Button FindButtonAndSetOnClick(Transform transform, string strPath = null, System.Action onClick = null, bool repeat = true)
    {
        if (transform == null) return null;
        Button button = null;
        button = string.IsNullOrEmpty(strPath) ? transform.GetComponent<Button>() : GOUtility.FindObject<Button>(transform, strPath);
        
        if (button != null) button.onClick.AddListener(() =>
        {
            if (!repeat)
            {
                if(IsButtonRepeatClick(button)) return;
            }

            onClick?.Invoke();
        });
        return button;
    }

    private static bool IsButtonRepeatClick(Button btnCheck)
    {
        if (btnCheck == null) return true;
        if (!btnCheck.enabled) return true;
        btnCheck.enabled = false;
        return false;
    }
    #endregion

    public static GameObject RemoveCanvas(GameObject myObject)
    {
        if (myObject == null) return myObject;

        Canvas addCanvas = myObject.GetComponent<Canvas>();
        if (addCanvas != null)
        {
            GraphicRaycaster graphicRaycaster = myObject.GetComponent<GraphicRaycaster>();
            if(graphicRaycaster != null) Destroy(graphicRaycaster);
            Destroy(addCanvas);
        }
        return myObject;
    }
}
