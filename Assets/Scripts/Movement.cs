using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Rigidbody PlayerRigidbody;
    [Space]
    public float PlanarSpeed = 30f;
    public float ForwardSpeed = 50f;
    [Space]
    public float Acceleration = 0.3f;
    public float Deceleration = 0.2f;

    private Vector2 _inputVector;
    private Vector2 _currentInputVector;
    private Vector2 _smoothInputVelocity;

    private Vector2 _planarMovement;

    private float _forwardMovement;
    private float _smoothForwardVelocity;

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
    }

    /*************************************************************************************************************/

    private void Update()
    {
        // Calculate acceleration stuff;
        ProcessPlanarAcceleration();
        ProcessZAcceleration();
    }

    private void FixedUpdate()
    {
        // Move the player;
        HandleMovement();
    }

    /*************************************************************************************************************/

    private void ProcessPlanarAcceleration()
    {
        // Rate at which to gain/lose speed;
        float velocityChangeRate = _inputVector != Vector2.zero ?
                Acceleration :
                Deceleration;

        _currentInputVector = Vector2.SmoothDamp(
            current: _currentInputVector,
            target: new Vector2(
                x: _inputVector.x * PlanarSpeed,
                y: _inputVector.y * PlanarSpeed * 0.9f),
            currentVelocity: ref _smoothInputVelocity,
            smoothTime: velocityChangeRate);

        _planarMovement.Set(
            newX: _currentInputVector.x,
            newY: _currentInputVector.y);
    }

    private void ProcessZAcceleration()
    {
        _forwardMovement = Mathf.SmoothDamp(
            current: _forwardMovement,
            target: ForwardSpeed,
            currentVelocity: ref _smoothForwardVelocity,
            smoothTime: 0.5f);
    }

    private void HandleMovement()
    {
        PlayerRigidbody.MovePosition(Time.fixedDeltaTime *
            new Vector3(
                x: _planarMovement.x,
                y: _planarMovement.y,
                z: _forwardMovement)
            + this.transform.position);
    }
}
