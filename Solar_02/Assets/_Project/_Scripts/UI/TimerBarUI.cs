using UnityEngine;
using UnityEngine.UI;
public class TimerBarUI : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    private void Start()
    {
        _fillImage.fillAmount = 0f;
        Hide();
    }

    public void SetProgressBar(float progressVlaue)
    {
        _fillImage.fillAmount = progressVlaue;
        Show();
    }

    public void Hide() => gameObject.SetActive(false);
    public void Show() => gameObject.SetActive(true);
}
