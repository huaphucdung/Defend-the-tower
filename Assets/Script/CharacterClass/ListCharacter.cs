using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListCharacter : MonoBehaviour
{
    [SerializeField] CharacterClass[] characters;

    public CharacterClass GetCharacter(int index) {
        return characters[index];
    }
}
