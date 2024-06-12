using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static float playerItemWeightLimit;

    public float _playerItemWeightLimit;

    private void Start()
    {
        playerItemWeightLimit = _playerItemWeightLimit;
    }
}
