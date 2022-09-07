using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class systemErrorTextAnimatioon : MonoBehaviour
{
    private TMP_Text text;
    bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponentInParent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning) StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        isRunning = true;
        text.text = "SYS BOOT";
        yield return new WaitForSeconds(3f);
        text.text = text.text + "\n \nWARNING: SYSTEM ERROR";
        yield return new WaitForSeconds(0.1f);
        text.text = text.text + "\n \nERROR: SYSTEM CRASH";
        yield return new WaitForSeconds(0.4f);
        text.text = text.text + "\n \nREBOOT REQUIRED";
        yield return new WaitForSeconds(2f);
        text.text = text.text + "\n \nUSER: *********";
        yield return new WaitForSeconds(2f);
        text.text = text.text + "\n \nPASSWORD: *********";
        yield return new WaitForSeconds(0.2f);
        text.text = text.text + "\n \nAUTO REBOOT IN: 3";
        yield return new WaitForSeconds(1f);
        text.text = text.text + "\n \nAUTO REBOOT IN: 2";
        yield return new WaitForSeconds(1f);
        text.text = text.text + "\n \nAUTO REBOOT IN: 1";
        yield return new WaitForSeconds(1f);
        text.text = "--\ninjecting\nC# WELCOME\n--\nclass novice { \n    public void main() { \n        System.Console.WriteLine(\"Hello World\"); \n    } \n}\n \nfunc main() {\n    System.Console.WriteLine(\"Hello World\"); \n    Print(\"This code is ass\")\n}";
        yield return new WaitForSeconds(4f);
        isRunning = false;
    }
}
