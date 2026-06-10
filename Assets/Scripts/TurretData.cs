using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Scriptable Objects/TurretData")]
public class TurretData : ScriptableObject
{
    public string TurretName;
    public float BulletDamage;
    public float BulletSpeed;
    public float Cadencia;
    public float TurrentRange;

}
