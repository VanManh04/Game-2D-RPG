using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBingerTriggers : Enemy_AnimationTriggers
{
    private Enemy_DeathBinger enemyDeathBinger => GetComponentInParent<Enemy_DeathBinger>();

    private void Relocate() => enemyDeathBinger.FindPosition();

    private void MakeInvisibleTrue()=>enemyDeathBinger.fx.Maketransprent(true);
    private void MakeVisibleFalse()=>enemyDeathBinger.fx.Maketransprent(false);
}
