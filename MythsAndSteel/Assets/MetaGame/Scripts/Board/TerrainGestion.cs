using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGestion : MonoSingleton<TerrainGestion>
{
    public void UnitModification(TerrainParent pt)
    {
        pt.OnUnityAdd();
    }
}
