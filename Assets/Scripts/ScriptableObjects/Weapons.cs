using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewClass", menuName = "Class Weapon/FireGuns")]
public class WeaponsClass : ScriptableObject
{
    public string idClass;

    [Header("Movement")]
    public float fieldOfView;
    public int BulletAmount;
    public float RateOfFire;
    public Sprite crosshair;

    public GameObject FireGunModel;
}