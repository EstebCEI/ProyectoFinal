using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public string enemyID;
    public Vector3 position;
    public float health;
    public bool isDead;

    public EnemyData(string enemyID, Vector3 position, float health, bool isDead)
    {
        this.enemyID = enemyID;
        this.position = position;
        this.health = health;
        this.isDead = isDead;
    }
}