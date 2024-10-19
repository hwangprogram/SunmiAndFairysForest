using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class a_Dialog : MonoBehaviour
{

    public Text textLocation;
    [TextArea]
    public string[] dialog;

    private void Update()
    {
        if (Hub.InputManager.isAOnce) NextDialog();
        if (Hub.InputManager.isCOnce) SkipDialog();
    }

    private void NextDialog()
    {
        Hub.UIManager.currentDialog += 1;
        if (Hub.UIManager.currentDialog >= dialog.Length)
        {
            SkipDialog();
        }
        else StartCoroutine(NextDialogCor());

    }

    IEnumerator NextDialogCor()
    {

        textLocation.text = dialog[Hub.UIManager.currentDialog];
        try { Hub.CVManager.cv[Hub.UIManager.currentDialog - 1].Stop(); } catch { }
        Hub.CVManager.cv[Hub.UIManager.currentDialog].Play();
        yield return null;
    }


    private void SkipDialog()
    {
        for (int i = 0; i < Hub.CVManager.cv.Length; i++) Hub.CVManager.cv[i].Stop();
        Hub.UIManager.currentDialog = 0;
        // 여기서 dialog 리셋
        textLocation.text = dialog[0];
        Hub.GameManager.DialogClose();
        Hub.UIManager.dialogUI.SetActive(false);       
    }           
}
