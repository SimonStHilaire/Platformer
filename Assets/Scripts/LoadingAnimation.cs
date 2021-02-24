using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LoadingAnimation : MonoBehaviour
{
    private Image TargetImage;

    void Start()
    {
        TargetImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(TargetImage.fillClockwise)
        {
            TargetImage.fillAmount += Time.deltaTime;

            if(TargetImage.fillAmount >= 1f)
            {
                TargetImage.fillClockwise = !TargetImage.fillClockwise;
            }
        }
        else
        {
            TargetImage.fillAmount -= Time.deltaTime;

            if (TargetImage.fillAmount <= 0f)
            {
                TargetImage.fillClockwise = !TargetImage.fillClockwise;
            }
        }
    }
}
