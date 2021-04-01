using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : MonoBehaviour
{
    private static MessageWindow instance;

    private Text MessageText;
    private RectTransform backgroundRectTransform;
    private RectTransform rectTransform;

    [SerializeField] private RectTransform canvasRectTransform;

    private void Awake()
    {
        instance = this;
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        MessageText = transform.Find("Text").GetComponent<Text>();
        rectTransform = transform.GetComponent<RectTransform>();
        HideMessageWindow();
    }

    private void ShowMessage(string text)
    {
        GameManager.GAME.PageSound.Play();
        gameObject.SetActive(true);
        MessageText.text = text;
        float textPaddingSize = 10f;
        Vector2 backgroundSize = new Vector2(MessageText.preferredWidth + textPaddingSize * 2f, MessageText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
        StartCoroutine(MessageDelay(GameManager.RULES.messageDelay));
    }

    IEnumerator MessageDelay(float n)
    {
        yield return new WaitForSecondsRealtime(n);

        HideMessageWindow();
    }

    private void HideMessageWindow()
    {
        gameObject.SetActive(false);
    }

    public static void ShowMessage_Static(string text)
    {
        instance.ShowMessage(text);
    }

    public static void HideMessageWindow_Static()
    {
        instance.HideMessageWindow();
    }
}
