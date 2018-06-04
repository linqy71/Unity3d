using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bulletin : MonoBehaviour
{

    private Button yourButton;
    public Text text;
    private int frame = 20;

    // Use this for initialization  
    void Start()
    {
        Button btn = this.gameObject.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        text.gameObject.SetActive(false);
    }

    IEnumerator rotateIn()
    {
        float rx = 0;
        float xy = 40;
        for (int i = 0; i < frame; i++)
        {
            rx -= 90f / frame;
            xy -= 40f / frame;
            text.transform.rotation = Quaternion.Euler(rx, 0, 0);
            //text.transform.position += new Vector3(0,xy, 0);
            text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, xy);
            if (i == frame - 1)
            {
                text.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    IEnumerator rotateOut()
    {
        float rx = -90;
        float xy = 0;
        for (int i = 0; i < frame; i++)
        {
            rx += 90f / frame;
            xy += 40f / frame;
            text.transform.rotation = Quaternion.Euler(rx, 0, 0);
            //text.transform.position += new Vector3(0, xy, 0);
            text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, xy);
            if (i == 0)
            {
                text.gameObject.SetActive(true);
            }
            yield return null;
        }
    }


    void TaskOnClick()
    {
        if (text.gameObject.activeSelf)
        {
            StartCoroutine(rotateIn());
        }
        else
        {
            StartCoroutine(rotateOut());
        }

    }
}