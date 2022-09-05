using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMotion : MonoBehaviour {
  [Header("Configuration")]
  public float maxHorizontalSpeed = 88;
  public float acceleration = 176;
  public float decceleration = 264;
  public float duckingAcceleration;
  public float jumpVelocity = 100;

  [Header("Information")]
  public float currentSpeed = 0;
  public Vector3 Position { get => transform.position; set => transform.position = value; }
  public bool SameDirectionCommanded {
    get => (Mathf.Abs(Controls.WASD.x) < 0.1f && IsDucking) ||
      (Mathf.Sign(Controls.WASD.x) == Mathf.Sign(Controls.WASD.x) &&
       (currentSpeed == 0? Mathf.Abs(Controls.WASD.x) < 0.1f: Mathf.Abs(Controls.WASD.x) > 0.1f));
  }
  public float CurrentAcceleration {
    get => SameDirectionCommanded? (IsDucking? duckingAcceleration: acceleration): decceleration; }
  public bool IsDucking { get => Controls.WASD.y < -0.1f; }
  public bool IsGrounded { get => upAngle < 54 && upAngle >= 0; }
  public bool isHorizontallyBlocked;
  public float timestampLastJump = -100;
  public Vector3 velocity;
  public float upAngle;
  public float debug;

  [Header("Initialization")]
  public Rigidbody2D body;
  public Transform stateMachine;

  void Update () {
    currentSpeed = isHorizontallyBlocked? 0:
      Mathf.MoveTowards(currentSpeed, IsDucking? 0: (Controls.WASD.x * maxHorizontalSpeed),
                        CurrentAcceleration * Time.deltaTime);
    velocity = body.velocity;
    velocity.x = currentSpeed;
    stateMachine.Find(IsDucking? "duck": "stand").gameObject.SetActive(true);
    if (IsGrounded && Controls.Jump) { timestampLastJump = Time.time; velocity.y = jumpVelocity; }
    body.velocity = velocity;
  }

  void OnCollisionStay2D (Collision2D c) {
    upAngle = Vector3.Angle(c.GetContact(0).normal, Vector3.up);
    isHorizontallyBlocked = upAngle > 54 && Mathf.Abs(c.relativeVelocity.x) >= 2;
    debug = c.relativeVelocity.x;
  }
  void OnCollisionExit2D (Collision2D c) { upAngle = -1; }
}
