using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
Ce Script fonctionne pour les deux zones d'orgones à condition de bien assigner les variables propres à ces dernières. Il va gérer les conditions qui permettent le déplacement de la zone, son déplacement en lui-même,
puis la validation de son déplacement avec le fait de attribuer et de retirer les effets de terrains RedOrgone et BlueOrgone.
 */
public class ZoneOrgone : MonoBehaviour
{
    #region Variables
    //Variables communes au deux zones
    [Range(15, 40)]
    [SerializeField] private float _speedTiles = 0f;
    [SerializeField]
    GameObject[] casesinterdites;
    List<GameObject> CasesInTheRange = new List<GameObject>();
    bool ZoneOrgoneActivé = false;
    int indexZoneOrgoneCaseCentrale;
    bool Replacement = false;

    bool DéplacementZoneEffectué = false;
    bool ValidationPanel = true;
    MYthsAndSteel_Enum.PhaseDeJeu PhaseOrgoneJ1 = MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ1;
    MYthsAndSteel_Enum.PhaseDeJeu PhaseOrgoneJ2 = MYthsAndSteel_Enum.PhaseDeJeu.OrgoneJ2;


    //Variables propres à chaque Zone d'orgone à Assigner dans l'Inspecteur à mettre dans le Player Class
    [SerializeField]
    List<GameObject> CasesZoneOrgone = new List<GameObject>();
    [SerializeField]
    GameObject ZoneOrgoneCaseCentrale;
    [SerializeField]
    bool OwnerZoneOrgone;
    [SerializeField]
    MYthsAndSteel_Enum.TerrainType orgoneterrain;

    #endregion


    void Update()
    {
        //Vérifie que le joueur se trouve dans une phase d'orgone
        if (GameManager.Instance.ActualTurnPhase == PhaseOrgoneJ1 | GameManager.Instance.ActualTurnPhase == PhaseOrgoneJ2)
        {
            //Vérifie qu'il se trouve dans la bonne phase d'orgone pour utiliser sa zone d'orgone
            if(GameManager.Instance.IsPlayerRedTurn == OwnerZoneOrgone)
            {
            // Vérifie que le joueur n'a pas déjà déplacé sa zone.
            if (!DéplacementZoneEffectué)
            {
                //Replace la zone d'orgone si besoin
                if (Replacement)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(ZoneOrgoneCaseCentrale.transform.position.x, ZoneOrgoneCaseCentrale.transform.position.y, transform.position.z),
                        _speedTiles * Vector2.Distance(transform.position, ZoneOrgoneCaseCentrale.transform.position) * Time.deltaTime);
                    if (Vector2.Distance(transform.position, ZoneOrgoneCaseCentrale.transform.position) < 0.3)
                    {
                        Replacement = false;

                    }
                }
                //S'il y a pas de case détecter par le curseur alors replace la zone d'orgone
                if (RaycastManager.Instance.Tile == null)
                {
                    Replacement = true;
                    ZoneOrgoneActivé = false;
                  
                }

                if (RaycastManager.Instance.Tile != null && Replacement == false)
                {
                    //Si le joueur clique sur une case ayant l'effet de terrain Orgone alors on change le layer de certaines cases pour éviter la sortie de la zone d'orgone.
                    //Puis, en fonction de la position de la zone d'orgone, on met dans une liste l'ensemble des cases qui sont à portée du déplacement de la zone d'orgone.
                    if (Input.GetMouseButtonDown(1) && RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Contains(orgoneterrain))

                    {
                        foreach (GameObject elements in casesinterdites)
                        {
                            elements.layer = 2;
                        }
                        CasesInTheRange.Clear();
                        CasesZoneOrgone.Remove(ZoneOrgoneCaseCentrale);
                        CasesInTheRange.AddRange(CasesZoneOrgone);

                        if (indexZoneOrgoneCaseCentrale + 18 < 80)
                        {
                            CasesInTheRange.Add(TilesManager.Instance.TileList[indexZoneOrgoneCaseCentrale + 18]);

                        }
                        if (indexZoneOrgoneCaseCentrale - 18 > 0)
                        {
                            CasesInTheRange.Add(TilesManager.Instance.TileList[indexZoneOrgoneCaseCentrale - 18]);

                        }
                        OrgoneCheckSpécifiquesTiles(17, 9);
                        OrgoneCheckSpécifiquesTiles(26, 18);
                        OrgoneCheckSpécifiquesTiles(35, 27);
                        OrgoneCheckSpécifiquesTiles(44, 36);
                        OrgoneCheckSpécifiquesTiles(53, 45);
                        OrgoneCheckSpécifiquesTiles(62, 54);
                        OrgoneCheckSpécifiquesTiles(71, 63);

                        ZoneOrgoneActivé = true;
                    }
                }
                //On déplace la zone vers la case visée par le curseur du joueur
                if (Input.GetMouseButton(1) && ZoneOrgoneActivé && Replacement == false)
                {



                    transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(RaycastManager.Instance.Tile.transform.position.x, RaycastManager.Instance.Tile.transform.position.y, transform.position.z),
                    _speedTiles * Vector2.Distance(transform.position, RaycastManager.Instance.Tile.transform.position) * Time.deltaTime);



                }
                //lorsque le joueur relâche son clic droit, on valide la nouvelle position de la zone d'orgone on supprime les effet de terrain d'orgone des cases de l'ancienne positition.
                // La case visée devient la nouvelle case centrale et on ajoute l'effet de terrain d'orgone à cette case et les cases autour de cette dernière.
                if (Input.GetMouseButtonUp(1) && Replacement == false)
                {

                    GameObject caseCiblé = RaycastManager.Instance.Tile.gameObject;
                    if (CasesInTheRange.Contains(caseCiblé))
                    {
                            //panneau de validation à modifier certainement lorsqu'il sera fait
                            if (ValidationPanel)
                        {
                            CasesZoneOrgone.Add(ZoneOrgoneCaseCentrale);
                            foreach (GameObject elements in CasesZoneOrgone)
                            {
                                elements.GetComponent<TileScript>().TerrainEffectList.Remove(orgoneterrain);
                            }
                            ZoneOrgoneCaseCentrale = caseCiblé;
                          
                            OrgoneCheckAdjacentesTiles();
                            CasesZoneOrgone.Add(ZoneOrgoneCaseCentrale);
                          
                            foreach (GameObject elements in CasesZoneOrgone)
                            {
                                elements.GetComponent<TileScript>().TerrainEffectList.Add(orgoneterrain);
                            }
                            foreach (GameObject elements in casesinterdites)
                            {
                                elements.layer = 3;
                            }
                            //feedback
                            Debug.Log("tu as déplacé ta zone");
                            ZoneOrgoneActivé = false;
                            DéplacementZoneEffectué = true;
                        }
                        else if (!ValidationPanel)
                        {
                            Replacement = true;
                            ZoneOrgoneActivé= false;
                        }
                    }
                    else if (!CasesInTheRange.Contains(RaycastManager.Instance.Tile.gameObject))
                    {
                        Debug.Log("Ce Placement est hors portée");
                        Replacement = true;
                        ZoneOrgoneActivé = false;
                    }




                    

                }
            }
        
        else if (DéplacementZoneEffectué && Input.GetMouseButtonDown(1) && RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Contains(orgoneterrain))
        {
            //feedback
            Debug.Log("Tu as déjà déplacé ta zone pendant ce tour");
        }
            }
            else if (GameManager.Instance.IsPlayerRedTurn != OwnerZoneOrgone && Input.GetMouseButtonDown(1)
            && RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Contains(orgoneterrain))
            
            {
                //feedback
                Debug.Log("Pour déplacer la zone il faut que tu sois dans ta phase d'orgone");
            }

        }
        else if (GameManager.Instance.ActualTurnPhase != PhaseOrgoneJ1 && Input.GetMouseButtonDown(1) 
            && RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Contains(orgoneterrain) 
               | GameManager.Instance.ActualTurnPhase != PhaseOrgoneJ2
            && Input.GetMouseButtonDown(1) && RaycastManager.Instance.Tile.GetComponent<TileScript>().TerrainEffectList.Contains(orgoneterrain))
        {
            //feedback
            Debug.Log("Pour déplacer la zone il faut que tu sois dans ta phase d'orgone");
        }

    }
    void OrgoneCheckSpécifiquesTiles(int borneMax, int borneMin)
    {
        if (indexZoneOrgoneCaseCentrale < borneMax && indexZoneOrgoneCaseCentrale > borneMin)
        {
            if (indexZoneOrgoneCaseCentrale + 2 < borneMax && indexZoneOrgoneCaseCentrale + 2 > borneMin)
            {
                CasesInTheRange.Add(TilesManager.Instance.TileList[indexZoneOrgoneCaseCentrale + 2]);
            }
            if (indexZoneOrgoneCaseCentrale - 2 < borneMax && indexZoneOrgoneCaseCentrale - 2 > borneMin)
            {
                CasesInTheRange.Add(TilesManager.Instance.TileList[indexZoneOrgoneCaseCentrale - 2]);
            }
        }
    }
    /// <summary>
    /// Permet de vider la liste des case de la zone d'orgone, et de la remplir en fonction de la nouvelle case centrale de la zone déterminé par le curseur du joueur
    /// </summary>
    void OrgoneCheckAdjacentesTiles()
    {
        CasesZoneOrgone.Clear();
        indexZoneOrgoneCaseCentrale = TilesManager.Instance.TileList.IndexOf(ZoneOrgoneCaseCentrale);

        CasesZoneOrgone.Add(TilesManager.Instance.TileList[indexZoneOrgoneCaseCentrale - 1]);
        CasesZoneOrgone.Add(TilesManager.Instance.TileList[indexZoneOrgoneCaseCentrale + 1]);
        CasesZoneOrgone.Add(TilesManager.Instance.TileList[indexZoneOrgoneCaseCentrale + 9]);
        CasesZoneOrgone.Add(TilesManager.Instance.TileList[indexZoneOrgoneCaseCentrale - 9]);
        CasesZoneOrgone.Add(TilesManager.Instance.TileList[indexZoneOrgoneCaseCentrale - 8]);
        CasesZoneOrgone.Add(TilesManager.Instance.TileList[indexZoneOrgoneCaseCentrale - 10]);
        CasesZoneOrgone.Add(TilesManager.Instance.TileList[indexZoneOrgoneCaseCentrale + 8]);
        CasesZoneOrgone.Add(TilesManager.Instance.TileList[indexZoneOrgoneCaseCentrale + 10]);
       
    }
    /// <summary>
    /// Reset la bool de déplacement de la zone, à mettre a chaque fois qu'on entre dans une phase d'orgone
    /// </summary>
    void refreshbool()
    {
        DéplacementZoneEffectué = false;
    }

    
        }

