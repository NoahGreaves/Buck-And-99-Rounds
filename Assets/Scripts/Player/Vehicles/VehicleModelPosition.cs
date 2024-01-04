using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleModelPosition : MonoBehaviour
{
    [SerializeField] private GameObject _playerCar;
    [SerializeField] private float _turnSpeed = 30f;

    private Vector2 _rotationInput;

    private float _groundCheckDistance = 1f;

    private void Start()
    {
        //transform.parent = null;
    }

    private void Update()
    {
        SetRotation();
        var newPos = new Vector3(_playerCar.transform.position.x, _playerCar.transform.position.y - 0.75f, _playerCar.transform.position.z);
        transform.position = newPos;
    }

    public void OnRotation(InputAction.CallbackContext context)
    {
        _rotationInput = context.ReadValue<Vector2>();
    }

    private void SetRotation()
    {
        RaycastHit hit;
        bool rayHitGround = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down) * _groundCheckDistance, out hit, 10);

        if (rayHitGround)
        {
            //var target = new Vector3(hit.normal.x, transform.rotation.y, hit.normal.z);
            //transform.up = Vector3.Lerp(transform.up, target, Time.deltaTime * 10f);
            //transform.Rotate(0, transform.eulerAngles.y, 0);

            //var rotation = _grounded ? hit.normal : _model.transform.rotation.eulerAngles;
            //var rotation = hit.normal;
            //_model.transform.rotation = Quaternion.Euler(rotation.x, _model.transform.rotation.y, rotation.z);
        }

        // Set Vehicle Rotation only while the player is moving
        float newRot = Player.IsMoving ? (_rotationInput.x * _turnSpeed) * Time.deltaTime : 0f;
        transform.Rotate(0, newRot, 0, Space.World);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color= Color.red;
    //    Gizmos.DrawLine(transform.position, Player.GroundNormal);
    //}
}
