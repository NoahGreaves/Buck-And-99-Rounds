
public class EliminationObjective : Objective
{
    private int _countToEliminate = 0;

    private void Awake()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameEvents.OnGetEnemies += CountNumberOfEnemies;   
    }

    private void UnsubscribeToEvents()
    {
        GameEvents.OnGetEnemies -= CountNumberOfEnemies;   
    }

    private void CountNumberOfEnemies(bool isAlive) 
    {
        // Count the Enemies on Spawn and when they are Eliminated. If the count is going to go below 
        // 0, dont do anything, else -1 from the count
        _countToEliminate += isAlive ? 1 : -1;
        if (_countToEliminate - 1 <= 0)
        {
            _countToEliminate = 0;
            IsCompleted();
        }

        GameEvents.EnemyCountUpdate(_countToEliminate);
    }
}
