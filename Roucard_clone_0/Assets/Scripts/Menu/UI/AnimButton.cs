using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimButton : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    public float initialSpeed = 1.0f;
    public float[] startCordX;
    public float[] stopCordX;


    public float[] slowDownDistances;
    public float[] slowDownFactors;

    private RectTransform[] rectTransforms;
    private bool[] isMoving;
    private float[] currentSpeeds;

    void Start()
    {
        rectTransforms = new RectTransform[buttons.Length];
        isMoving = new bool[buttons.Length];
        currentSpeeds = new float[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            rectTransforms[i] = buttons[i].GetComponent<RectTransform>();
            rectTransforms[i].anchoredPosition = new Vector2(startCordX[i], rectTransforms[i].anchoredPosition.y);
            isMoving[i] = true;
            currentSpeeds[i] = initialSpeed;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (isMoving[i])
            {

                Vector3 newPosition = rectTransforms[i].anchoredPosition;
                float distance = Mathf.Abs(rectTransforms[i].anchoredPosition.x - stopCordX[i]);
                float step = currentSpeeds[i] * Time.deltaTime;


                for (int j = 0; j < slowDownDistances.Length; j++)
                {
                    if (distance <= slowDownDistances[j])
                    {
                        step *= slowDownFactors[j];
                        break;
                    }
                }


                currentSpeeds[i] -= 4.0f;
                currentSpeeds[i] = Mathf.Max(currentSpeeds[i], 0f);

                newPosition.x = Mathf.MoveTowards(rectTransforms[i].anchoredPosition.x, stopCordX[i], step);
                rectTransforms[i].anchoredPosition = newPosition;


                if (Mathf.Approximately(newPosition.x, stopCordX[i]))
                {
                    isMoving[i] = false;
                }
            }
        }
    }
}