﻿using UnityEngine;

public class ShadowCombat : CharacterCombat
{
    public GameObject BulletPrefab;
    
    public override void Die()
    {
        throw new System.NotImplementedException();
        //TODO: Change music
        //TODO: Particle Effect
        //TODO: Animation
        //TODO: End game event + listener on player to add shadow
    }
    
    //TODO: 3 attack sequences on repeat
}