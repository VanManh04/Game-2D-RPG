using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private CharacterStats myStats => GetComponentInParent<CharacterStats>();
    private RectTransform myTransform => GetComponent<RectTransform>();
    private Slider slider;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();

        UpdateHealthUI();
        //Debug.Log("HealthBar_UI Call");
    }

    private void OnEnable()
    {
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void FlipUI() => myTransform.Rotate(0, 180, 0);

    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;
        if (entity != null)
            myStats.onHealthChanged -= UpdateHealthUI;
    }
}
