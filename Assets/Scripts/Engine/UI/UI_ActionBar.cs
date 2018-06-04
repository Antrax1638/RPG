using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ActionBar : UI_Base
{
    [Header("General Properties:")]
    public bool CustomPercentages = false;
    public bool ClampValues = false;

    [Header("Health Properties:")]
    public float Health;
    public float MaxHealth;
    public Image HealthFill;
    public Color HealthTint = Color.white;
    [Range(0,1)] public float HealthPercentage = 1.0f;
    public Text HealthText;

    [Header("Energy Properties:")]
    public float Energy;
    public float MaxEnergy;
    public Image EnergyFill;
    public Color EnergyTint = Color.white;
    [Range(0, 1)] public float EnergyPercentage = 1.0f;
    public Text EnergyText;

    [Header("Experience Properties:")]
    public float Experience;
    public float MaxExperience;
    public Image ExperienceFill;
    public Color ExperienceTint = Color.white;
    [Range(0, 1)] public float ExperiencePercentage = 1.0f;
    public Text ExperienceText;

    protected string HealthString, EnergyString,ExperienceString;
    protected float HealthRatio, EnergyRatio,ExperienceRatio;
    //protected List<GameObject> ToolTipComponents = new List<GameObject>(); 

    protected virtual void Awake()
    {
        if (!HealthFill) Debug.LogWarning("UI_ActionBar: Health fill component is null");
        if (!HealthText) Debug.LogWarning("UI_ActionBar: Health text component is null");

        if (!EnergyFill) Debug.LogWarning("UI_ActionBar: Energy fill component is null");
        if (!EnergyText) Debug.LogWarning("UI_ActionBar: Energy text component is null");

        if (!ExperienceFill) Debug.LogWarning("UI_ActionBar: Experience fill component is null");
        if (!ExperienceText) Debug.LogWarning("UI_ActionBar: Experience text component is null");
    }

    protected virtual void Update ()
    {
        if(ClampValues)
        {
            Health = Mathf.Clamp(Health, 0.0f, MaxHealth);
            Experience = Mathf.Clamp(Experience, 0.0f, MaxExperience);
        }

        if (HealthFill)
        {
            MaxHealth = (MaxHealth == 0) ? 1 : MaxHealth;
            HealthRatio = (CustomPercentages) ? HealthPercentage : Health / MaxHealth;
            HealthFill.fillAmount = HealthRatio;
            HealthFill.material.SetColor("_Color", HealthTint);
        }

        if (EnergyFill)
        {
            MaxEnergy = (MaxEnergy == 0) ? 1 : MaxEnergy;
            EnergyRatio = (CustomPercentages) ? EnergyPercentage : Energy / MaxEnergy;
            EnergyFill.fillAmount = EnergyRatio;
            EnergyFill.material.SetColor("_Color", EnergyTint);
        }

        if (ExperienceFill)
        {
            MaxExperience = (MaxExperience == 0) ? 1 : MaxExperience;
            ExperienceRatio = (CustomPercentages) ? ExperiencePercentage : Experience / MaxExperience;
            ExperienceFill.fillAmount = ExperienceRatio;
            ExperienceFill.material.SetColor("_Color", ExperienceTint);
        }

        if(HealthText)
        {
            HealthString = Health + "/" + MaxHealth;
            HealthText.text = HealthString;
        }

        if (EnergyText)
        {
            EnergyString = Energy + "/" + MaxEnergy;
            EnergyText.text = EnergyString;
        }

        if(ExperienceText)
        {
            ExperienceString = Experience + "/" + MaxExperience;
            ExperienceText.text = ExperienceString;
        }
	}
}
