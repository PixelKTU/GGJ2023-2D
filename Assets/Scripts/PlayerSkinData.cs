using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkinData", menuName = "ScriptableObjects/PlayerSkinData", order = 1)]
public class PlayerSkinData : ScriptableObject
{
    public Sprite playerHeadSprite;
    public int maxHealth;
    public RuntimeAnimatorController animations;
}
