// PLAYER
public delegate void OnPlayerHealthUpdate(float newHealth);
public delegate void OnPlayerVelocityUpdate(float newVelocity);
public delegate void OnPlayerFuelUpdate(float newFuelAmount);
public delegate void OnPlayerDeath();
public delegate void OnPlayerElimination();
public delegate void OnPlayerGivenFuel(float fuelAmount);

// OBJECTIVES
public delegate void OnObjectiveComplete();
public delegate void OnAlertMainObjective(ObjectiveType objectiveType);
public delegate void OnEnemyCountUpdate(int enemyCount);
public delegate void OnGetObjectives(ObjectiveType objectiveType);
public delegate void OnRoomObjectivesComplete();
public delegate void OnAlertRoomObjectiveComplete();

// PROGRESSION
public delegate void OnRoomProgression();

// ENEMIES
public delegate void OnEnemyKilled(Enemy enemy);

public static class GameEvents
{
    #region PLAYER
    public static event OnPlayerHealthUpdate OnPlayerHealthUpdate;
    public static void PlayerHealthUpdate(float newHealth) => OnPlayerHealthUpdate?.Invoke(newHealth);

    public static event OnPlayerVelocityUpdate OnPlayerVelocityUpdate;
    public static void PlayerVelocityUpdate(float newVelocity) => OnPlayerVelocityUpdate?.Invoke(newVelocity);

    public static event OnPlayerFuelUpdate OnPlayerFuelUpdate;
    public static void PlayerFuelUpdate(float newFuelAmount) => OnPlayerFuelUpdate?.Invoke(newFuelAmount);

    public static event OnPlayerDeath OnPlayerDeath;
    public static void PlayerDeath() => OnPlayerDeath?.Invoke();

    public static event OnPlayerElimination OnPlayerElimination;
    public static void PlayerElimination() => OnPlayerElimination?.Invoke();

    public static event OnPlayerGivenFuel OnPlayerGivenFuel;
    public static void AddPlayerFuel(float fuelAmount) => OnPlayerGivenFuel?.Invoke(fuelAmount);
    #endregion

    #region OBJECTIVES
    public static event OnObjectiveComplete OnObjectiveComplete;
    public static void ObjectiveComplete() => OnObjectiveComplete?.Invoke();

    public static event OnAlertMainObjective OnAlertMainObjective;
    public static void AlertMainObjective(ObjectiveType objectiveType) => OnAlertMainObjective?.Invoke(objectiveType);

    public static event OnGetObjectives OnGetObjectives;
    public static void GetObjectives(ObjectiveType objectiveType) => OnGetObjectives?.Invoke(objectiveType);

    public static event OnRoomObjectivesComplete OnRoomObjectivesComplete;
    public static void RoomObjectivesComplete() => OnRoomObjectivesComplete.Invoke();

    public static event OnAlertRoomObjectiveComplete OnAlertRoomObjectiveComplete;
    public static void AlertRoomObjectiveComplete() => OnAlertRoomObjectiveComplete.Invoke();
    #endregion

    #region PROGRESSION

    public static event OnRoomProgression OnRoomProgression;
    public static void RoomProgression() => OnRoomProgression.Invoke();

    #endregion


    #region UI UPDATES
    // UPDATES THE ENEMY COUNT UI
    public static event OnEnemyCountUpdate OnEnemyCountUpdate;
    public static void EnemyCountUpdate(int enemyCount) => OnEnemyCountUpdate?.Invoke(enemyCount);
    #endregion
}