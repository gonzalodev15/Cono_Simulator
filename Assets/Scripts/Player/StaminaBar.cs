using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider stamina_bar;

    private float max_stamina = 100f;
    private float current_stamina;

    public bool isRegenerating = false;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public Color fillColorNormal = new Color(250/255.0f, 250 / 255.0f, 100 / 255.0f, 255 / 255.0f);
    public Color backgroundColorNormal = new Color(185 / 255.0f, 185 / 255.0f, 1 / 255.0f, 255 / 255.0f);

    public Color fillColorRegen = Color.red;
    public Color backgroundColorRegen = Color.grey;

    public static StaminaBar instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        current_stamina = max_stamina;
        stamina_bar.maxValue = max_stamina;
        stamina_bar.value = max_stamina;
    }

    // Update is called once per frame
    void Update()
    {
        Image fillImage = stamina_bar.fillRect.GetComponent<Image>();
        Image backgroundImage = stamina_bar.GetComponentsInChildren<Image>()[0];
        if (isRegenerating) {
            fillImage.color = fillColorRegen;
            backgroundImage.color = backgroundColorRegen;
        } else
        {
            fillImage.color = fillColorNormal;
            backgroundImage.color = backgroundColorNormal;
        }
    }

    public void UseStamina(float amount)
    {
        if (!isRegenerating && current_stamina - amount >= 0)
        {
            current_stamina -= amount;
            stamina_bar.value = current_stamina;

            if (regen != null)
                StopCoroutine(regen);

            regen = StartCoroutine(RegenStamina());

            if(current_stamina == 0)
            {
                isRegenerating = true;
            }

        } else
        {
            Debug.Log("Not enough stamina");
        }
    }

    public bool StaminaBarDepleted()
    {
        if (current_stamina <= 0)
            return true;

        return false;
    }

    public bool StaminaBarFull()
    {
        if (current_stamina == max_stamina)
            return true;

        return false;
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(2);

        while (current_stamina < max_stamina)
        {
            current_stamina += max_stamina / 50;
            stamina_bar.value = current_stamina;
            if(current_stamina == max_stamina)
            {
                isRegenerating = false;
            }
            yield return regenTick;
        }
        regen = null;
    }
}
