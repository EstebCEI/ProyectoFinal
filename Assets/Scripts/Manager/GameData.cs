using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    // Player
    public float playerHealth;
    public Vector3 playerPosition;
    public int playerStance;
    public bool isFinished;

    // Enemies
    public List<EnemyData> enemies;

    public GameData(float health, Vector3 position, int stance, bool finished)
    {
        playerHealth = health;
        playerPosition = position;
        playerStance = stance;
        isFinished = finished;

        enemies = new List<EnemyData>();
    }
}