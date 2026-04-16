using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public Vector3 position;
    public bool isDead;

    public EnemyData(Vector3 pos, bool dead)
    {
        position = pos;
        isDead = dead;
    }
}
