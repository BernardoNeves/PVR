using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Healthbar : MonoBehaviour {

    Slider _slider;
    TMP_Text _Text;
    private float _slideSpeed = 400;
    private float _target = 100;

    private void Start() {
        _Text = GetComponentInChildren<TMP_Text>();
        _slider = GetComponent<Slider>();
    }

    public void SetMax(float max) {
        if(_slider != null)
        _slider.maxValue = max;
    }

    public void SetCurrent(float current) {
        _target = current;
    }

    private void FixedUpdate()
    {
        _slider.value = Mathf.MoveTowards(_slider.value, _target, _slideSpeed * Time.deltaTime);
        if (_Text != null)
        {
            _Text.text = Mathf.RoundToInt(_slider.value).ToString() + "/" + _slider.maxValue.ToString();
        }
    }
}   