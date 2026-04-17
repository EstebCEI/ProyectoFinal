using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    // Player
    public float playerHealth;
    public Vector3 playerPosition;
    public int playerStance;

    // Mission
    public bool hasHackedComputer;
    public bool missionCompleted;

    // Enemies
    public List<EnemyData> enemies;

    public GameData()
    {
        enemies = new List<EnemyData>();
    }

    public GameData(
        float health,
        Vector3 position,
        int stance,
        bool hasHackedComputer,
        bool missionCompleted,
        List<EnemyData> enemies)
    {
        this.playerHealth = health;
        this.playerPosition = position;
        this.playerStance = stance;
        this.hasHackedComputer = hasHackedComputer;
        this.missionCompleted = missionCompleted;

        this.enemies = enemies ?? new List<EnemyData>();
    }
}