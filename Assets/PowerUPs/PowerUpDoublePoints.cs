using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDoublePoints : BasePowerUp
{
    

    //This doubles the players current amount of points upon contact. The prefab is what it is because I couldn't figure how how to make an x and not make it look exactly like Firerate when spawned.

        protected override bool ApplyToPlayer(Player thePickerUpper)
    {
        thePickerUpper.ScoreNetVar.Value *= 2;
        return true;
    }

}
