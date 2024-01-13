using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Manager _stat;
    [SerializeField] private Image _fillimage;

    private void OnEnable() => _stat.OnChange += UpdateBar;

    private void OnDisable() => _stat.OnChange -= UpdateBar;

    private void UpdateBar() => _fillimage.fillAmount = _stat.Ratio;
}
