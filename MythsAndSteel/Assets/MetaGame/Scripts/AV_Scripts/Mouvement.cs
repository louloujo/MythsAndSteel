using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mouvement : MonoSingleton<Mouvement> // Script AV.
{
    [SerializeField] private int[] neighbourValue; // +1 +9 +10...

    [SerializeField] private List<int> newNeighbourId = new List<int>(); // Voisins atteignables avec le range de l'unité.
    public List<int> _selectedTileId => selectedTileId;

    [SerializeField] private List<int> selectedTileId = new List<int>(); // Cases selectionnées par le joueur.
    public List<int> _newNeighbourId => newNeighbourId;

    [SerializeField] private float speed = 1; // Speed de déplacement de l'unité 

    [SerializeField] private GameObject mStart; // mT Start. 
    [SerializeField] private GameObject mEnd; // mT End.
    [SerializeField] private GameObject mUnit; // mT Unité.

    [SerializeField] private List<GameObject> outTileLeft; 
    [SerializeField] private List<GameObject> outTileRight;

    private int LocalRange = 0; // permet de gérer la range d'une unité en local. 
    private List<int> temp = new List<int>(); //

    private void Update()
    {
        // Permet d'effectuer le moveTowards de l'unité à sa prochaine case.
        UpdatingMove(mUnit, mStart, mEnd); 
    }

    /// <summary>
    /// Cette fonction "highlight" les cases atteignables par l'unité sur la case sélectionnée.
    /// </summary>
    /// <param name="tileId">Tile centrale</param>
    /// <param name="Range">Range de l'unité</param>
    public void Highlight(int tileId, int Range)
    {
        if (Range > 0)
        {
            foreach (int ID in PlayerStatic.GetNeighbourDiag(tileId, TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false))
            {
                if (!newNeighbourId.Contains(ID))
                {
                    TilesManager.Instance.TileList[ID].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("selectedtile");
                    newNeighbourId.Add(ID);
                } 
                Highlight(ID, Range - 1);
            }
        }
    }

    /// <summary>
    /// Lance le mouvement d'une unité avec une range défini.
    /// </summary>
    /// <param name="tileId">Tile de l'unité</param>
    /// <param name="Range">Mvmt de l'unité</param>
    public void StartMvmtForSelectedUnit()
    {
        if (TilesManager.Instance._actualTileSelected != null)
        {
            StartMouvement(TilesManager.Instance.TileList.IndexOf(TilesManager.Instance._actualTileSelected), TilesManager.Instance._actualTileSelected.GetComponent<TileScript>().Unit.GetComponent<UnitScript>().MoveSpeed);
        }
    }

    public void StartMouvement(int tileId, int Range)
    {
        TilesManager.Instance._Mouvement = true;
        LocalRange = Range;
        selectedTileId.Add(tileId);
        List<int> ID = new List<int>();
        ID.Add(tileId);
        Highlight(tileId, Range); // Lance l'highlight des cases dans la range.
    }

    /// <summary>
    /// Arête le Mouvement pour l'unité selectionnée (menu, cases highlights...)
    /// </summary>
    public void StopMouvement()
    {        
        foreach (int Neighbour in newNeighbourId) // Supprime toutes les tiles.
        {
            TilesManager.Instance.TileList[Neighbour].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("empty"); // Assigne un sprite empty à toutes les anciennes cases "neighbour".
        }
        if(TilesManager.Instance._actualTileSelected != null) // Si une case était séléctionnée.
        {
            //Tiles.Instance._actualTileSelected.GetComponent<TileScript>().Unit.GetComponent<UnitScript>().DemandMenu.enabled = false;
            //Tiles.Instance._actualTileSelected.GetComponent<TileScript>().Unit.GetComponent<UnitScript>().Menu.enabled = false;
        }
        foreach (int NeighbourSelect in selectedTileId) // Si un path de mvmt était séléctionné.
        {
            TilesManager.Instance.TileList[NeighbourSelect].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("empty");
            TilesManager.Instance.TileList[NeighbourSelect].GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        }
        // Clear de toutes les listes et stats.
        selectedTileId.Clear(); 
        newNeighbourId.Clear();
        mStart = null;
        mEnd = null;
        mUnit = null;
        LocalRange = 0;
        TilesManager.Instance._Mouvement = false;
        TilesManager.Instance._Selected = false;
        TilesManager.Instance._actualTileSelected = null;
    }

    /// <summary>
    /// Ajoute la tile à TileSelected. Pour le mvmt du joueur => Check egalement toutes les conditions de déplacement.
    /// </summary>
    /// <param name="tileId">Tile</param>
    public void AddMouvement(int tileId) 
    {
        if (TilesManager.Instance._Mouvement)
        {
            if (newNeighbourId.Contains(tileId)) // Si cette case est dans la range de l'unité.
            {
                if (selectedTileId.Contains(tileId)) // Si cette case est déjà selectionnée.
                {
                    if (tileId == selectedTileId[0]) // la première case est selectionnée = Le joueur a cliqué sur l'unité.
                    {
                        Debug.Log("stop");
                        StopMouvement(); // Désactive le mvmt.
                    } 
                    else // Sinon suppréssion de toutes les cases séléctionnées après celle-ci.
                    {
                        for (int i = selectedTileId.IndexOf(tileId); i < selectedTileId.Count; i++) // Supprime toutes les cases sélectionnées à partir de l'ID tileId.
                        {
                            Debug.Log("REMOVE");
                            LocalRange++; // Redistribution du Range à chaque suppression de case.
                            temp.Add(selectedTileId[i]);
                            TilesManager.Instance.TileList[selectedTileId[i]].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("selectedtile"); // Repasse les sprites en apparence "séléctionnable".
                        }
                        foreach (int i in temp)
                        {
                            selectedTileId.Remove(i);
                        }
                        temp.Clear();
                    }
                }
                else if (PlayerStatic.IsNeighbour(tileId, selectedTileId[selectedTileId.Count - 1], TilesManager.Instance.TileList[tileId].GetComponent<TileScript>().Line, false)) // Sinon, si cette case est bien voisine de l'ancienne selection. 
                {
                    if(LocalRange > 0) // et qu'il reste du mvmt, on assigne la nouvelle case selectionnée à la liste SelectedTile.
                    {
                        LocalRange--; // sup 1 mvmt.
                        selectedTileId.Add(tileId); 
                        TilesManager.Instance.TileList[tileId].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("selecttile");
                        //Tiles.Instance._actualTileSelected.GetComponent<TileScript>().Unit.GetComponent<UnitScript>()._DemandMenu.enabled = true;
                    }
                }
                else // Sinon cette case est trop loin de l'ancienne seletion.
                {
                    Debug.Log("La tile d'ID : " + tileId + " est trop loin de la tile d'ID: " + selectedTileId[selectedTileId.Count - 1]);
                }
            }
            else // Sinon cette case est hors de la range de l'unité.
            {
                Debug.Log("La tile d'ID : " + tileId + " est trop loin de la tile d'ID: " + selectedTileId[selectedTileId.Count - 1]);
            }
        }
    }

    /// <summary>
    /// Lance mvmt.
    /// </summary>
    int MvmtIndex = 1; // Numéro du mvmt actuel dans la liste selectedTileId;
    [SerializeField] bool Launch = false; // Evite les répétitions dans updatingmove();
    /// <summary>
    /// Assigne le prochain mouvement demandé à l'unité. Change les stats de l'ancienne et de la nouvelle case. Actualise les informations de position de l'unité.
    /// </summary>
    public void ApplyMouvement()
    {
        mUnit = TilesManager.Instance._actualTileSelected.GetComponent<TileScript>().Unit; // Assignation de l'unité.
        mStart = TilesManager.Instance._actualTileSelected; // Assignation du nouveau départ.
        mEnd = TilesManager.Instance.TileList[selectedTileId[MvmtIndex]];  // Assignation du nouvel arrirée.
        //mUnit.GetComponent<UnitScript>()._DemandMenu.enabled = false; // Désactive le menu (Se déplacer, annuler)
        //mUnit.GetComponent<UnitScript>()._Menu.enabled = false; // Désactive le menu global de l'unité.
        foreach (int Neighbour in newNeighbourId) // Désactive toutes les cases selectionnées par la fonction Highlight.
        {
            if (!selectedTileId.Contains(Neighbour))
            {
                TilesManager.Instance.TileList[Neighbour].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("empty"); // Assigne un sprite empty à toutes les anciennes cases "neighbour"
            }
        }
        Debug.Log("Actual tile target: " + TilesManager.Instance.TileList[selectedTileId[MvmtIndex]]);
    }
    /// <summary>
    /// Coroutine d'attente entre chaque case. Probablement pendant ce temps que l'on devra appliquer les effets de case.
    /// </summary>
    /// <returns>Temps à définir</returns>
    IEnumerator MvmtEnd()
    {
        mEnd.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("empty"); // La case dépassée redevient une "empty"
        mEnd.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255); // La case reprend sa couleur d'origine.
        mUnit.GetComponent<UnitScript>().MoveLeft--;  // Suppression d'un point de mvmt à l'unité.
        mEnd.GetComponent<TileScript>().AddUnitToTile(mStart.GetComponent<TileScript>().Unit); // L'unité de la case d'arrivée devient celle de la case de départ.
        mStart.GetComponent<TileScript>().RemoveUnitFromTile(); // L'ancienne case n'a plus d'unité.
        mUnit = mEnd.GetComponent<TileScript>().Unit;
        mUnit.GetComponent<UnitScript>().ActualTiledId = TilesManager.Instance.TileList.IndexOf(mEnd); 
        TilesManager.Instance._actualTileSelected = mEnd; 
        mStart = mEnd;
        mEnd = null;
        yield return new WaitForSeconds(1); // Temps d'attente.
        if (MvmtIndex < selectedTileId.Count - 1) // Si il reste des mvmts à effectuer dans la liste SelectedTile.
        {
            MvmtIndex++;
            ApplyMouvement();
        }
        else // Si il ne reste aucun mvmt dans la liste SelectedTile.
        {
            Debug.Log("STOP");
            MvmtIndex = 1;
            StopMouvement(); // Arête le mvmt de l'unité.
        }
        Launch = false; // Reset de la bool Launch
    }

    /// <summary>
    /// Cette fonction lance l'animation de translation de l'unité entre les cases.
    /// </summary>
    /// <param name="Unit">The unit gameobject.</param>
    /// <param name="StartPos">start position tile</param>
    /// <param name="EndPos">end position tile</param>
    float speed1;
    void UpdatingMove(GameObject Unit, GameObject StartPos, GameObject EndPos)
    {
        if (Unit != null && StartPos != null && EndPos != null)
        {
            Unit.transform.position = Vector2.MoveTowards(Unit.transform.position, EndPos.transform.position, speed1); // Application du mvmt.
            speed1 = Mathf.Abs((Vector2.Distance(mUnit.transform.position, mEnd.transform.position) * speed)); // Régulation de la vitesse. (effet de ralentissement) 
            if (Vector2.Distance(mUnit.transform.position, mEnd.transform.position) <= 0.05f && Launch == false) // Si l'unité est arrivée.
            {
                Launch = true;
                StartCoroutine(MvmtEnd()); // Lancer le prochain mvmt avec délai. 
            }
            else // Sinon appliqué l'opacité à la case d'arrivée en fonction de la distance unité - arrivée.
            {
                mEnd.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Vector2.Distance(mUnit.transform.position, mEnd.transform.position));
            }
        }
    }
}


