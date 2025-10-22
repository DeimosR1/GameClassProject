using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWidget : MonoBehaviour
{
    Button mButton;
    [SerializeField] TextMeshProUGUI mAbilityNameText;
    private void Awake()
    {
        mButton = GetComponent<Button>();
        mButton.onClick.AddListener(ActivateAbility);
        mAbilityNameText = mButton.GetComponentInChildren<TextMeshProUGUI>();
    }
    Ability mAbility;
    public void SetAbility(Ability ability)
    {
        mAbility = ability;
        mAbilityNameText.text = $"{mAbility.mAbilityName}";
    }

    void ActivateAbility()
    {
        mAbility.ActivateAbility();
    }
}
