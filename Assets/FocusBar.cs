using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class FocusBar : MonoBehaviour
{
    public float maximum = 100f;
    public float current;
    public Image mask;

    private void Start()
    {
        current = maximum;
    }

    void Update()
    {
        // Maximum Value
        if (current > maximum)
        {
            current = maximum;
        }
        // Minimum Value
        if (current < 0)
        {
            current = 0;
        }
        // Refill Bar

        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        float fillAmount = current / maximum;
        mask.fillAmount = fillAmount;
    }

    public void RefillBar()
    {
        if (current < maximum)
        {
            current = current + 1.75f;
        }
    }

    public void AdjustFocusBar()
    {
        current = current - 1f;
    }
}
