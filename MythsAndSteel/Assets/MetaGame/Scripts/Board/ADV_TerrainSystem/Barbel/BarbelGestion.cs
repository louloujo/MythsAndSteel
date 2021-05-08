using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbelGestion : MonoBehaviour
{


    public enum Direction
    {
        Nord, Sud, Est, Ouest, Unknown
    }
    [SerializeField] private int BarbelDamage = 2;

    [SerializeField] private Direction Direct;

    public Sprite Horizontal;
    public Sprite Vertical;
    public List<Barbel> BarbelActive;

    public void CreateBarbel(int tileId)
    {
        List<MYthsAndSteel_Enum.TerrainType> T = TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().TerrainEffectList;
        Direction D = Direction.Unknown;
        if (T.Contains(MYthsAndSteel_Enum.TerrainType.Barbelé_Est) || T.Contains(MYthsAndSteel_Enum.TerrainType.Barbelé_Nord) || T.Contains(MYthsAndSteel_Enum.TerrainType.Barbelé_Sud) || T.Contains(MYthsAndSteel_Enum.TerrainType.Barbelé_Ouest))
        {
            List<GameObject> G = TilesManager.Instance.TileList[tileId].GetComponent<TileScript>()._Child;
            foreach (GameObject Child in G)
            {
                if (Child.TryGetComponent<Barbel>(out Barbel B))
                {
                    if (B.Direc == Direct)
                    {
                        D = Direct;
                    }
                }
            }
            if (D != Direction.Unknown)
            {
                Delete(tileId, D);
            }
            BarbelCreationAfterVerification(tileId, Direct);
        }
        else
        {
            BarbelCreationAfterVerification(tileId, Direct);
        }
    }

    public void Delete(int tileId, Direction Direction)
    {
        TileScript TS = TilesManager.Instance.TileList[tileId].GetComponent<TileScript>();
        TileScript TSN = TilesManager.Instance.TileList[tileId + 9].GetComponent<TileScript>();
        TileScript TSS = TilesManager.Instance.TileList[tileId - 9].GetComponent<TileScript>();
        TileScript TSE = TilesManager.Instance.TileList[tileId + 1].GetComponent<TileScript>();
        TileScript TSO = TilesManager.Instance.TileList[tileId - 1].GetComponent<TileScript>();
        switch (Direction)
        {
            case Direction.Est: TS.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbelé_Est); TSE.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbelé_Ouest); break;
            case Direction.Nord: TS.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbelé_Nord); TSN.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbelé_Sud); break;
            case Direction.Sud: TS.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbelé_Sud); TSS.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbelé_Nord); break;
            case Direction.Ouest: TS.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbelé_Ouest); TSO.RemoveEffect(MYthsAndSteel_Enum.TerrainType.Barbelé_Est); break;
        }
    }

    MYthsAndSteel_Enum.TerrainType TY;
    protected void BarbelCreationAfterVerification(int tileId, Direction Direction)
    {
        foreach (TerrainType T in GameManager.Instance.Terrain.EffetDeTerrain)
        {
            foreach (MYthsAndSteel_Enum.TerrainType T1 in T._eventType)
            {
                if (T1 == MYthsAndSteel_Enum.TerrainType.Barbelé_Nord) // Aucune différence c'est le même enfant.
                {
                    GameObject Child = Instantiate(T.Child, transform.position, Quaternion.identity);
                    Child.transform.parent = TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().transform;
                    Child.transform.localScale = new Vector3(1, 1, 1);
                    TilesManager.Instance.TileList[tileId].GetComponent<TileScript>()._Child.Add(Child);
                    Child.GetComponentInChildren<Barbel>().BarbelG = this;
                    Child.GetComponentInChildren<Barbel>().Direc = Direction;
                    Child.GetComponentInChildren<Barbel>().TurnLeft = 2;
                    BarbelActive.Add(Child.GetComponentInChildren<Barbel>());
                    Child.transform.localPosition = Vector3.zero;

                    TileScript TS = TilesManager.Instance.TileList[tileId].GetComponent<TileScript>(); 
                    TileScript TSN = TilesManager.Instance.TileList[tileId + 9].GetComponent<TileScript>();
                    TileScript TSS = TilesManager.Instance.TileList[tileId - 9].GetComponent<TileScript>();
                    TileScript TSE = TilesManager.Instance.TileList[tileId + 1].GetComponent<TileScript>();
                    TileScript TSO = TilesManager.Instance.TileList[tileId - 1].GetComponent<TileScript>();

                    switch (Direction)
                    {
                        case Direction.Est: TY = MYthsAndSteel_Enum.TerrainType.Barbelé_Est; TS.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbelé_Est); TSE.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbelé_Ouest); break;
                        case Direction.Nord: TY = MYthsAndSteel_Enum.TerrainType.Barbelé_Nord; TS.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbelé_Nord); TSN.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbelé_Sud); break;
                        case Direction.Sud: TY = MYthsAndSteel_Enum.TerrainType.Barbelé_Sud; TS.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbelé_Sud); TSS.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbelé_Nord); break;
                        case Direction.Ouest: TY = MYthsAndSteel_Enum.TerrainType.Barbelé_Ouest; TS.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbelé_Ouest); TSO.TerrainEffectList.Add(MYthsAndSteel_Enum.TerrainType.Barbelé_Est); break;
                    }
                    Child.GetComponent<ChildEffect>().Type = TY;
                }
            }
        }
    }

    public void Turn()
    {
        foreach (Barbel b in BarbelActive)
        {
            b.TurnLeft--;
        }
    }
}
