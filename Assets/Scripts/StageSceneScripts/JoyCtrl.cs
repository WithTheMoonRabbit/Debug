using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class JoyCtrl : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image imgJoyStick;
    private Image imgLever;
    private Vector2 posInput;
    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgJoyStick.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out posInput))
        {
            posInput.x = posInput.x / (imgJoyStick.rectTransform.sizeDelta.x);
            posInput.y = posInput.y / (imgJoyStick.rectTransform.sizeDelta.y);

            if (posInput.magnitude > 1.0f)
            {
                posInput = posInput.normalized;
            }

            imgLever.rectTransform.anchoredPosition = new Vector2(
                posInput.x * (imgJoyStick.rectTransform.sizeDelta.x / 2),
                posInput.y * (imgJoyStick.rectTransform.sizeDelta.y / 2));
        }
    }

    public Vector2 Inputjoy()
    {
        return posInput;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        posInput = Vector2.zero;
        imgLever.rectTransform.anchoredPosition = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        imgJoyStick = GetComponent<Image>();
        imgLever = transform.GetChild(0).GetComponent<Image>();
    }
}
