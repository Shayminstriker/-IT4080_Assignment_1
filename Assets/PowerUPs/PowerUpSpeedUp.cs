using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeedUp : BasePowerUp
{
    protected override bool ApplyToPlayer(Player thePickerUpper)
    {

        //This powerup increases the players speed. Didn't know what to make the prefab so I just made it an upside down ! mark.

        thePickerUpper.movementSpeed = 100f;
        return true;


    }
}
