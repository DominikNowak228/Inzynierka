using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private TMP_Text barText;

    int _lastManaValue = -1;

    public void UpdateManaBar(float currentMana, float maxManaAmount)
    {
        barImage.fillAmount = Mathf.Clamp01(currentMana / maxManaAmount);
        var currentValueRounded = Mathf.FloorToInt(currentMana);
        if (barText != null && _lastManaValue != currentValueRounded)
        {
            barText.text = $"{currentValueRounded}/{maxManaAmount}";
            _lastManaValue = currentValueRounded;
        }
    }
}
