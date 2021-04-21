using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rosiness.Utility;

public class MonoUtility : MonoBehaviour
{
    #region Button

    public static Button FindButtonAndSetOnClick(Transform transform, string strPath = null,
        System.Action onClick = null, bool repeat = true)
    {
        if (transform == null) return null;
        Button button = null;
        if (string.IsNullOrEmpty(strPath)) button = transform.GetComponent<Button>();
        else button = GOUtility.FindObject<Button>(transform, strPath);
        
        if (button != null) button.onClick.AddListener(() =>
        {
            

            onClick?.Invoke();
        });
        return null;
    }
    #endregion
}
