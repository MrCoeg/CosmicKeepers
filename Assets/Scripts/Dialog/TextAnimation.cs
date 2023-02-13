using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextAnimation : MonoBehaviour
{
    public delegate void textAnimationCallback();
    public IEnumerator TextTyping(TextMeshProUGUI textMesh, string text, float duration, textAnimationCallback callback)
    {
        int length = text.Length;
        float textShiftTime = duration / length;
        int counter = 0;
        textMesh.text = "";
        while(counter < length)
        {
            textMesh.text += text[counter];
            yield return new WaitForSeconds(textShiftTime);
            counter++;
        }
        callback();

        yield return null;
    }

    public IEnumerator ImageFadeIn(Image image, float fadeTime)
    {
        byte alpha = 0;
        float alphaShiftSpeed = fadeTime / 255;
        while (alpha < 255)
        {
            image.color = new Color32(0, 0, 0, alpha++);
            yield return new WaitForSeconds(alphaShiftSpeed);
        }
        yield return null;
    }

    public IEnumerator ImageFadeOut(Image image, float fadeTime)
    {
        byte alpha = 255;
        float alphaShiftSpeed = fadeTime / 255;
        while (alpha > 0)
        {
            image.color = new Color32(0, 0, 0, alpha--);
            yield return new WaitForSeconds(alphaShiftSpeed);
        }
    }
}
