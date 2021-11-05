using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterPanel : MonoBehaviour
{
    public static Action CharacterAnim;

    public void OnCharacterAnimation()
    {
        CharacterAnim?.Invoke();
    }
}
