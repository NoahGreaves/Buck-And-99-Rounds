using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowTarget : MonoBehaviour
{
    // Camera
    [Header("Camera")]
    [SerializeField] private Transform _cameraFollowTarget;
    [SerializeField] private float _cameraSensitivity = 0.25f;

    // MAKE A VEHCILE SCRIPT
    [SerializeField] private GameObject _currentVehicle;

    // GET GUN PROGRAMMATICALLY
    [SerializeField] private GameObject _currentWeapon;

    private Vector2 _lookInput;
    public void OnLook(InputAction.CallbackContext context) => _lookInput = context.ReadValue<Vector2>();

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        CameraRotation();
        UpdatePosition();
    }

    private void UpdatePosition() 
    {
        transform.position = _currentVehicle.transform.position;
    }

    private void CameraRotation()
    {
        var lookAngle = Quaternion.AngleAxis(_lookInput.x * _cameraSensitivity, Vector3.up);
        transform.rotation *= lookAngle;
        _currentWeapon.transform.rotation *= lookAngle;
    }
}
