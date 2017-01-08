using System;

[Serializable]
public class PlayerEmitterVO
{
    public string name;
    public FireRateVO[] fireRates;
    public string bullet;
    public float lifeTime = 1.0f;
}