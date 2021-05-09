using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HauteColline : TerrainParent
{
    public override int AttackRangeValue(int i = 0)
    {
        i = 1;
        return base.AttackRangeValue(i);
    }

    public override void CibledByAttack(UnitScript AttackerUnit, TileScript AttackerUnitCase)
    {
        Debug.Log("Cibled");
        if(!AttackerUnitCase.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Haute_colline) && !AttackerUnitCase.TerrainEffectList.Contains(MYthsAndSteel_Enum.TerrainType.Colline))
        {
            Attaque.Instance._JaugeAttack.SynchAttackBorne(AttackerUnit, -2);
        }
        base.CibledByAttack(AttackerUnit, AttackerUnitCase);
    }

    public override void UnCibledByAttack(UnitScript Unit)
    {
        Debug.Log("Uncibled");
        Attaque.Instance._JaugeAttack.SynchAttackBorne(Unit, 0);
        base.UnCibledByAttack(Unit);
    }
}
