// PLAYER
public delegate void OnPlayerHealthUpdate(float newHealth);
public delegate void OnPlayerVelocityUpdate(float newVelocity);
public delegate void OnPlayerDeath();
public delegate void OnPlayerElimination();

// OBJECTIVES
public delegate void OnObjectiveComplete(Objective objective);
public delegate void OnEnemyCountUpdate(int enemyCount);
public delegate void OnGetObjectives(ObjectiveType objectiveType);

// ENEMIES
public delegate void OnEnemyKilled(Enemy enemy);

public static class GameEvents
{
    #region PLAYER
    public static event OnPlayerHealthUpdate OnPlayerHealthUpdate;
    public static void PlayerHealthUpdate(float newHealth) => OnPlayerHealthUpdate?.Invoke(newHealth);

    public static event OnPlayerVelocityUpdate OnPlayerVelocityUpdate;
    public static void PlayerVelocityUpdate(float newVelocity) => OnPlayerVelocityUpdate?.Invoke(newVelocity);

    public static event OnPlayerDeath OnPlayerDeath;
    public static void PlayerDeath() => OnPlayerDeath?.Invoke();

    public static event OnPlayerElimination OnPlayerElimination;
    public static void PlayerElimination() => OnPlayerElimination?.Invoke();
    #endregion

    #region OBJECTIVES
    public static event OnObjectiveComplete OnObjectiveComplete;
    public static void ObjectiveComplete(Objective objective) => OnObjectiveComplete?.Invoke(objective);

    public static event OnGetObjectives OnGetObjectives;
    public static void GetObjectives(ObjectiveType objectiveType) => OnGetObjectives?.Invoke(objectiveType);

    // UPDATES THE ENEMY COUNT UI
    public static event OnEnemyCountUpdate OnEnemyCountUpdate;
    public static void EnemyCountUpdate(int enemyCount) => OnEnemyCountUpdate?.Invoke(enemyCount);
    #endregion
}