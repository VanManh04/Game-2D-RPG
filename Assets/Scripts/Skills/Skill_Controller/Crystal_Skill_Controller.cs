using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystalExistTimer;

    public void SetupCrystal(float _currentDuration)
    {
        crystalExistTimer =_currentDuration;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
            SelfDestroy();
    }

    private void SelfDestroy() => Destroy(this.gameObject);
}
