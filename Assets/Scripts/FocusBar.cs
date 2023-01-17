using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class FocusBar : MonoBehaviour
{
    public float maximum = 100f;
    public float current;
    public float fillRate;
    public Image mask;
    [SerializeField] Player _focusMode;

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

         //Minimum Value
        if (current < 0)
        {
           current = 0;
        }

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
            current = current + 0.55f * Time.deltaTime * 10f * fillRate;
        }
    }

    public void EmptyRefillBar()
    {
        current = current + 0.25f * Time.deltaTime * 10f * fillRate;
    }

    public void AdjustFocusBar()
    {
        current = current - 1f * Time.deltaTime *10f * fillRate;
    }
}
