using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterBoard : MonoBehaviour
{
    public void IncreaseOne()
    {
        Hub.PlayerStatus.AddCurrentHP(0.5);
    }

    public void DecreaseOne()
    {
        Hub.PlayerStatus.AddCurrentHP(-0.5);
    }

}
