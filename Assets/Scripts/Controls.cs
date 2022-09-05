using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour {
  static TheFuckingInput _thefuckingasset;
  public static TheFuckingInput thefuckingasset {
    get {
      if (_thefuckingasset == null) {
        _thefuckingasset = new TheFuckingInput();
        _thefuckingasset.TheInput.Enable();
      }
      return _thefuckingasset;
    }
  }

  public static bool releasedWASDY = false;
  public static Vector2 WASD { get => thefuckingasset.TheInput.Motion.ReadValue<Vector2>(); }
  public static bool _jump;
  public static bool Jump { get => _jump; }

  void Update () {
    _jump = WASD.y > 0.5f && releasedWASDY;
    releasedWASDY = WASD.y <= 0.5f;
  }
}
