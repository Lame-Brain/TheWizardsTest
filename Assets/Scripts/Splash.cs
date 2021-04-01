using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
    private Text MessageText;
    private RectTransform backgroundRectTransform;
    private RectTransform rectTransform;
    private Image backgroundImage;
    private GameObject target;

    //[SerializeField] private RectTransform canvasRectTransform;

    private void Awake()
    {
        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        backgroundImage = transform.Find("Background").GetComponent<Image>();
        MessageText = transform.Find("Text").GetComponent<Text>();
        rectTransform = transform.GetComponent<RectTransform>();
    }

    public void Show(string text, Color bgColor, Color txtColor)
    {
        backgroundImage.color = bgColor;
        MessageText.text = text;
        MessageText.color = txtColor;
        rectTransform.position = transform.parent.transform.position;

        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(MessageText.preferredWidth + textPaddingSize * 2f, MessageText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
        StartCoroutine(MessageDelay(GameManager.RULES.messageDelay));
    }

    IEnumerator MessageDelay(float n)
    {
        yield return new WaitForSecondsRealtime(n);

        Hide();
    }

    private void Hide()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
