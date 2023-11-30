using UnityEngine;

public class BombLauncherPosition : MonoBehaviour
{
    [SerializeField] private BombLauncher _launcher;

    private void LateUpdate()
    {
        _launcher.transform.position = transform.position;
        _launcher.transform.rotation = transform.rotation;
    }
}
