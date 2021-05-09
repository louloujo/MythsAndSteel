using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plage : TerrainParent
{
    public override void CibledByAttack(UnitScript AttackerUnit, TileScript AttackerUnitCase)
    {
        Attaque.Instance._JaugeAttack.SynchAttackBorne(AttackerUnit, -1);
        base.CibledByAttack(AttackerUnit, AttackerUnitCase);
    }

    public override void UnCibledByAttack(UnitScript Unit)
    {
        Attaque.Instance._JaugeAttack.SynchAttackBorne(Unit, 0);
        base.UnCibledByAttack(Unit);
    }
}
