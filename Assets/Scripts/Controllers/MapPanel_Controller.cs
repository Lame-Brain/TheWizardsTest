using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel_Controller : MonoBehaviour
{
    public GameObject ref_mapPanel;
    public GameObject ref_mapTileEmptyPF, ref_tileImage, ref_darkImage;
    public Sprite ref_emptyTile, ref_drawnTile, ref_damageTile, ref_darkTile, ref_chestTile, ref_inWall, ref_exWall, ref_doorWall, ref_torchWall, ref_player, ref_arrowUp, ref_arrowDown, ref_arrowOver;

    private GameObject[,] maptile;
    private int n = 5;
    private int deltaX = 0, deltaY = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Start by drawing the map tiles surrounding the player
        maptile = new GameObject[n*2+1, n*2+1];
        for (int y=0; y < n*2+1; y++)
            for(int x = 0; x < n*2+1; x++)
            {
                maptile[x, y] = Instantiate(ref_mapTileEmptyPF, ref_mapPanel.transform);
                maptile[x, y].transform.localPosition = new Vector2((x - n) * 25, (y - n) * 25);
            }
        //int playerX = (int)((GameManager.PARTY.x_coor + (GameManager.RULES.TileSize / 2)) / GameManager.RULES.TileSize), playerY = (int)((GameManager.PARTY.y_coor + (GameManager.RULES.TileSize / 2)) / GameManager.RULES.TileSize), _px = playerX, _py = playerY;
        DrawMap();
    }

    private void DrawMap()
    {
        //Check if it is dark
        if (GameManager.PARTY.light > 0) ref_darkImage.SetActive(false);
        if (GameManager.PARTY.light <= 0) ref_darkImage.SetActive(true);

        //First, Destroy old map tiles
        GameObject[] _go = GameObject.FindGameObjectsWithTag("MiniMapTile");
        foreach (GameObject _t in _go) Destroy(_t);

        //now draw:
        GameObject _tgo;

        int playerX = (int)((GameManager.PARTY.x_coor + (GameManager.RULES.TileSize / 2)) / GameManager.RULES.TileSize), playerY = (int)((GameManager.PARTY.y_coor + (GameManager.RULES.TileSize / 2)) / GameManager.RULES.TileSize), _px = playerX+deltaX, _py = playerY+deltaY;
        for (int y = -n; y <= n; y++)
            for (int x = -n; x <= n; x++)
            {
                if(_px + x >= 0 && _px + x < 18 && _py + y >= 0 && _py + y < 18) //Checking array bounds of Party.map
                {
                    //Debug.Log("wot? " + (_px + x) + ", " + (_py + y) + " NDoor? " + GameManager.PARTY.mapND[_px + x, _py + y]);
                    if (GameManager.PARTY.map[_px + x, _py + y] == 0) Instantiate(ref_tileImage, maptile[x + n, y + n].transform); //draw empty tiles                    
                    if (GameManager.PARTY.map[_px + x, _py + y] == 1 && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw filled tiles
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.GetComponent<Image>().sprite = ref_drawnTile;
                    }
                    if (GameManager.PARTY.map[_px + x, _py + y] == 2 && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw damage tiles
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.GetComponent<Image>().sprite = ref_damageTile;
                    }
                    if (GameManager.PARTY.map[_px + x, _py + y] == 3 && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw dark tiles
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.GetComponent<Image>().sprite = ref_darkTile;
                    }
                    if (GameManager.PARTY.mapC[_px + x, _py + y] && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw chest
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "CHEST";
                        _tgo.GetComponent<Image>().sprite = ref_chestTile;
                    }
                    if (GameManager.PARTY.mapN[_px + x, _py + y] > 0 && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw north wall
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "North Wall";
                        if (GameManager.PARTY.mapN[_px + x, _py + y] == 1) _tgo.GetComponent<Image>().sprite = ref_exWall;
                        if (GameManager.PARTY.mapN[_px + x, _py + y] == 2) _tgo.GetComponent<Image>().sprite = ref_inWall;
                        _tgo.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    if (GameManager.PARTY.mapND[_px + x, _py + y] && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw north door
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "North Door";
                        _tgo.GetComponent<Image>().sprite = ref_doorWall;
                        _tgo.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    if (GameManager.PARTY.mapNT[_px + x, _py + y] && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw north torch
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "North Torch";
                        _tgo.GetComponent<Image>().sprite = ref_torchWall;
                        _tgo.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }

                    if (GameManager.PARTY.mapE[_px + x, _py + y] > 0 && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw east wall
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "East Wall";
                        if (GameManager.PARTY.mapE[_px + x, _py + y] == 1) _tgo.GetComponent<Image>().sprite = ref_exWall;
                        if (GameManager.PARTY.mapE[_px + x, _py + y] == 2) _tgo.GetComponent<Image>().sprite = ref_inWall;
                        _tgo.transform.rotation = Quaternion.Euler(0, 0, 270);
                    }
                    if (GameManager.PARTY.mapED[_px + x, _py + y] && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw east door
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "East Door";
                        _tgo.GetComponent<Image>().sprite = ref_doorWall;
                        _tgo.transform.rotation = Quaternion.Euler(0, 0, 270);
                    }
                    if (GameManager.PARTY.mapET[_px + x, _py + y] && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw east torch
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "East Torch";
                        _tgo.GetComponent<Image>().sprite = ref_torchWall;
                        _tgo.transform.rotation = Quaternion.Euler(0, 0, 270);
                    }

                    if (GameManager.PARTY.mapS[_px + x, _py + y] > 0 && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw south wall
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "South Wall";
                        if (GameManager.PARTY.mapS[_px + x, _py + y] == 1) _tgo.GetComponent<Image>().sprite = ref_exWall;
                        if (GameManager.PARTY.mapS[_px + x, _py + y] == 2) _tgo.GetComponent<Image>().sprite = ref_inWall;
                        _tgo.transform.rotation = Quaternion.Euler(0, 0, 180);
                    }
                    if (GameManager.PARTY.mapSD[_px + x, _py + y] && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw south door
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "South Door";
                        _tgo.GetComponent<Image>().sprite = ref_doorWall;
                        _tgo.transform.rotation = Quaternion.Euler(0, 0, 180);
                    }
                    if (GameManager.PARTY.mapST[_px + x, _py + y] && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw south torch
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "South Torch";
                        _tgo.GetComponent<Image>().sprite = ref_torchWall;
                        _tgo.transform.rotation = Quaternion.Euler(0, 0, 180);
                    }

                    if (GameManager.PARTY.mapW[_px + x, _py + y] > 0 && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw west wall
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "West Wall";
                        if (GameManager.PARTY.mapW[_px + x, _py + y] == 1) _tgo.GetComponent<Image>().sprite = ref_exWall;
                        if (GameManager.PARTY.mapW[_px + x, _py + y] == 2) _tgo.GetComponent<Image>().sprite = ref_inWall;
                        _tgo.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                    if (GameManager.PARTY.mapWD[_px + x, _py + y] && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw west door
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "West Door";
                        _tgo.GetComponent<Image>().sprite = ref_doorWall;
                        _tgo.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                    if (GameManager.PARTY.mapWT[_px + x, _py + y] && GameManager.PARTY.showMapTile[_px + x, _py + y]) //draw west torch
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "West Torch";
                        _tgo.GetComponent<Image>().sprite = ref_torchWall;
                        _tgo.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                    //Draw map stairs arrow up
                    if (GameManager.PARTY.ladder[_px + x, _py + y] == 1)
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "Go Up";
                        _tgo.GetComponent<Image>().sprite = ref_arrowUp;
                    }
                    //Draw map stairs arrow Down
                    if (GameManager.PARTY.ladder[_px + x, _py + y] == 2)
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "Go Up";
                        _tgo.GetComponent<Image>().sprite = ref_arrowDown;
                    }
                    //Draw map stairs arrow Over
                    if (GameManager.PARTY.ladder[_px + x, _py + y] == 3)
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x + n, y + n].transform);
                        _tgo.name = "Go Up";
                        _tgo.GetComponent<Image>().sprite = ref_arrowOver;
                    }

                    if (_px+x == playerX && _py+y == playerY)
                    {
                        _tgo = Instantiate(ref_tileImage, maptile[x+n, y+n].transform);
                        _tgo.GetComponent<Image>().sprite = ref_player;
                        _tgo.name = "PLAYER MARK";
                        if (GameManager.PARTY.face == 0) _tgo.transform.rotation = Quaternion.Euler(0, 0, 0); //North
                        if (GameManager.PARTY.face == 1) _tgo.transform.rotation = Quaternion.Euler(0, 0, 270); //East
                        if (GameManager.PARTY.face == 2) _tgo.transform.rotation = Quaternion.Euler(0, 0, 180); //South
                        if (GameManager.PARTY.face == 3) _tgo.transform.rotation = Quaternion.Euler(0, 0, 90); //West
                    }
                }
            }
    }





    public void CloseMapScreen()
    {
        GetComponentInParent<ExploreController>().ClearAllScreens();
    }
    public void Navigate_Left()
    {
        int _c = GameManager.EXPLORE.selected_Character;
        GetComponentInParent<ExploreController>().ClearAllScreens();
        GetComponentInParent<ExploreController>().OpenSpellCompendium(_c);
    }
    public void Navigate_Right()
    {
        int _c = GameManager.EXPLORE.selected_Character;
        GetComponentInParent<ExploreController>().ClearAllScreens();
        GetComponentInParent<ExploreController>().OpenInventoryScreen(_c);
    }
    public void ShowCloseTooltip()
    {
        Tooltip.ShowToolTip_Static("Close the Map");
    }
    public void ShowLeftArrowToolTip()
    {
        Tooltip.ShowToolTip_Static("Go to Spell Compendium");
    }
    public void ShowRightArrowToolTip()
    {
        Tooltip.ShowToolTip_Static("Go to Inventory");
    }
    public void HideLeftArrowToolTip()
    {
        Tooltip.HideToolTip_Static();
    }
    public void MovePY(int value)
    {
        deltaY += value;
        DrawMap();
    }
    public void MovePX(int value)
    {
        deltaX += value;
        DrawMap();
    }
}
