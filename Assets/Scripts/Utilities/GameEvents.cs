using UnityEngine;

// PLAYER
public delegate void OnPlayerHealthUpdate(float newHealth);
public delegate void OnPlayerLivesUpdate(int numOfLives);
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

// ROOMS MANAGEMENT
public delegate void OnRoomProgression(RoomCollection roomCollection);
public delegate void OnRoomReset();
public delegate void OnRoomLoad();

// ENEMIES
public delegate void OnEnemyKilled(Enemy enemy);

// ITEMS
public delegate void OnExplosion(Vector3 explosionPosition, float explosionPower, float explosionRadius, float explosionUpwardForce, float explosionDamage);

// PICKUPS
public delegate void OnBombLauncherPickup();
public delegate void OnForwardFacingShieldPickup();
public delegate void OnMineDropperPickup();

// Countdown
public delegate void OnCountdownUpdate(int newCountdown);
   
public static class GameEvents
{
    #region PLAYER
    public static event OnPlayerHealthUpdate OnPlayerHealthUpdate;
    public static void PlayerHealthUpdate(float newHealth) => OnPlayerHealthUpdate?.Invoke(newHealth);
    
    public static event OnPlayerLivesUpdate OnPlayerLivesUpdate;
    public static void PlayerLivesUpdate(int numOfLives) => OnPlayerLivesUpdate?.Invoke(numOfLives);

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

    #region ROOM MANAGEMENT
    public static event OnRoomProgression OnRoomProgression;
    public static void RoomProgression(RoomCollection roomCollection) => OnRoomProgression?.Invoke(roomCollection);

    public static event OnRoomReset OnRoomReset;
    public static void RoomReset() => OnRoomReset?.Invoke();
    
    public static event OnRoomLoad OnRoomLoad;
    public static void RoomLoad() => OnRoomLoad?.Invoke();
    #endregion

    #region ITEM
    public static event OnExplosion OnExplosion;
    public static void Explosion(Vector3 explosionPosition, float explosionPower, float explosionRadius, float explosionUpwardForce, float explosionDamage) => OnExplosion?.Invoke(explosionPosition, explosionPower, explosionRadius, explosionUpwardForce, explosionDamage);
    #endregion

    #region PICKUP
    public static event OnBombLauncherPickup OnBombLauncherPickup;
    public static void BombLauncherPickup() => OnBombLauncherPickup?.Invoke();

    public static event OnForwardFacingShieldPickup OnForwardFacingShieldPickup;
    public static void ForwardFacingShieldPickup() => OnForwardFacingShieldPickup?.Invoke();
    
    public static event OnMineDropperPickup OnMineDropperPickup;
    public static void MineDropperPickup() => OnMineDropperPickup?.Invoke();
    #endregion

    #region UI UPDATES
    // UPDATES THE ENEMY COUNT UI
    public static event OnEnemyCountUpdate OnEnemyCountUpdate;
    public static void EnemyCountUpdate(int enemyCount) => OnEnemyCountUpdate?.Invoke(enemyCount);
    
    public static event OnCountdownUpdate OnCountdownUpdate;
    public static void CountdownUpdate(int newCountdown) => OnCountdownUpdate?.Invoke(newCountdown);
    #endregion
}