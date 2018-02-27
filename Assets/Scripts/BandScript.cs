using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BandScript : MonoBehaviour {

    public abstract void GiveLife();

    public abstract void IncreaseLifetime();

    public abstract void Toggle();
}
