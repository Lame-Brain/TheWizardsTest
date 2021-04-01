using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private static Tooltip instance;

    private Text toolTipText;
    private RectTransform backgroundRectTransform;
    private RectTransform rectTransform;

    [SerializeField] private RectTransform canvasRectTransform;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(transform.parent.gameObject);
            instance = this;
        }
        else { Destroy(this.gameObject); }
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        toolTipText = transform.Find("Text").GetComponent<Text>();
        rectTransform = transform.GetComponent<RectTransform>();
        HideToolTip();
    }

    private void Update()
    {
        Vector2 anchoredPosition = new Vector2(Input.mousePosition.x / canvasRectTransform.localScale.x, (Input.mousePosition.y / canvasRectTransform.localScale.y)+50);
        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width) anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height) anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void ShowToolTip(string text)
    {
        gameObject.SetActive(true);
        toolTipText.text = text;
        float textPaddingSize = 7f;
        Vector2 backgroundSize = new Vector2(toolTipText.preferredWidth + textPaddingSize * 2f, toolTipText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
        //Cursor.visible = false;
    }

    private void HideToolTip()
    {
        gameObject.SetActive(false);
        Cursor.visible = true;
    }

    public static void ShowToolTip_Static(string text)
    {
        instance.ShowToolTip(text);
    }

    public static void HideToolTip_Static()
    {
        instance.HideToolTip();
    }
}
