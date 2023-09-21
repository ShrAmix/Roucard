using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimButton : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    public float speed = 1.0f;
    public float[] startCordX;
    public float[] stopCordX;
    public bool[] moveBoolX;

    private RectTransform[] rectTransforms;

    void Start()
    {
        rectTransforms = new RectTransform[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            rectTransforms[i] = buttons[i].GetComponent<RectTransform>();
            rectTransforms[i].anchoredPosition = new Vector2(startCordX[i], rectTransforms[i].anchoredPosition.y);
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (moveBoolX[i])
            {
                // Рухаємо кнопку до позиції для зупинки
                Vector3 newPosition = rectTransforms[i].anchoredPosition;
                newPosition.x = Mathf.MoveTowards(newPosition.x, stopCordX[i], speed * Time.deltaTime);
                rectTransforms[i].anchoredPosition = newPosition;

                // Перевіряємо, чи кнопка досягла позиції для зупинки
                if (Mathf.Approximately(rectTransforms[i].anchoredPosition.x, stopCordX[i]))
                {
                    moveBoolX[i] = false;
                }
            }
        }
    }
}
