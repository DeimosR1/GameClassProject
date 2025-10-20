using System;
using UnityEngine;
using TMPro;

public class CharacterControlWidget : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mCharacterNameText;
    internal void SetBattleCharacter(BattleCharacter battleCharacter)
    {
        mCharacterNameText.SetText(battleCharacter.gameObject.name);
    }
}
