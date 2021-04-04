using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeHive))]
public class NodePlacement : Editor
{  
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Release the Nodes"))
        {
            //BIND THE TORCHES! BIND THEM TO THE WALLS!
            GameObject[] AllTheTORCHES = GameObject.FindGameObjectsWithTag("Torch"); GameObject[] AllTheWalls = GameObject.FindGameObjectsWithTag("MapWall");
            foreach(GameObject torch in AllTheTORCHES)
            {
                //find the closest mapwall
                GameObject closest_wall = AllTheWalls[0];
                for (int _i = 0; _i < AllTheWalls.Length; _i++)
                {
                    if (Vector3.Distance(torch.transform.position, AllTheWalls[_i].transform.position) < Vector3.Distance(torch.transform.position, closest_wall.transform.position)) closest_wall = AllTheWalls[_i];
                }
                torch.transform.SetParent(closest_wall.transform);
            }

            //Init gn (GridNodes) array with plenty of extra space to prevent overflows
            GameObject[,] gn = new GameObject[32,32];

            //Find the NodeHive and load up the Node prefab (and the rules)
            GameObject hive = GameObject.FindGameObjectWithTag("NodeHive");
            GameObject nodePF = hive.GetComponent<NodeHive>().nodePF;
            RULES rules = GameObject.FindGameObjectWithTag("GameRules").GetComponent<RULES>();

            //Clear the field of existing Nodes
            GameObject[] oldNodes = GameObject.FindGameObjectsWithTag("Node");
            foreach (GameObject oldNode in oldNodes) DestroyImmediate(oldNode);

            //Find all the floors
            GameObject[] floors = GameObject.FindGameObjectsWithTag("MapFloor");
            foreach(GameObject o in floors) 
            {
                //Adjust the X,Z position of the floors, so they are in the array with comfortable buffer on all sides
                int x = (int)(o.transform.position.x+7)/6; 
                int y = (int)(o.transform.position.z+7)/6;                
                //Instantiate my nodes, childed under the Hive
                gn[x,y] = Instantiate(nodePF, hive.transform);
                //Position them to the floor position, set rotation to 0, and load the nodeX and nodeY publics
                gn[x, y].transform.position = new Vector3(o.transform.position.x, o.transform.position.y + 1, o.transform.position.z);
                gn[x, y].transform.rotation = Quaternion.identity; //rotate to 0,0,0
                gn[x, y].GetComponent<GridNode>().SetNodePosition(x, y); //establish node in grid
                //Set door links to null, until I can refine them with raycasting.
                gn[x, y].GetComponent<GridNode>().northDoor = null;
                gn[x, y].GetComponent<GridNode>().eastDoor = null;
                gn[x, y].GetComponent<GridNode>().southDoor = null;
                gn[x, y].GetComponent<GridNode>().westDoor = null;
                //Set Points of Interest
                //if (o.transform.Find("NorthChest") != null) { gn[x, y].transform.Find("North").transform.localPosition = new Vector3(0, -3, -6); gn[x, y].GetComponent<GridNode>().northChest = true; }
                //if (o.transform.Find("EastChest") != null) { gn[x, y].transform.Find("East").transform.localPosition = new Vector3(-6, -3, 0); gn[x, y].GetComponent<GridNode>().eastChest = true; }
                //if (o.transform.Find("SouthChest") != null) { gn[x, y].transform.Find("South").transform.localPosition = new Vector3(0, -3, 6); gn[x, y].GetComponent<GridNode>().southChest = true; }
                //if (o.transform.Find("WestChest") != null) { gn[x, y].transform.Find("West").transform.localPosition = new Vector3(6, -3, 0); gn[x, y].GetComponent<GridNode>().westChest = true; }
            }

            //Establish Direction Links
            for (int y = 0; y < 18; y++) //<----- Why 0 to 18?
            {
                for (int x = 0; x < 18; x++)
                {
                    if (gn[x, y] != null)
                    {
                        if (gn[x, y - 1] != null) gn[x, y].GetComponent<GridNode>().northLink = gn[x, y - 1];
                        if (gn[x - 1, y] != null) gn[x, y].GetComponent<GridNode>().eastLink = gn[x - 1, y];
                        if (gn[x, y + 1] != null) gn[x, y].GetComponent<GridNode>().southLink = gn[x, y + 1];
                        if (gn[x + 1, y] != null) gn[x, y].GetComponent<GridNode>().westLink = gn[x + 1, y];
                    }
                }
            }
            //Refine Direction Links with raycasting
            float d = rules.TileSize;
            for (int y = 0; y < 18; y++)
            {
                for (int x = 0; x < 18; x++)
                {
                    if (gn[x, y] != null)
                    {
                        RaycastHit nHit, eHit, sHit, wHit;
                        if (Physics.Raycast(gn[x, y].transform.position, Vector3.back, out nHit, d) && nHit.transform.tag != "MapDoor") gn[x, y].GetComponent<GridNode>().northLink = null;
                        if (Physics.Raycast(gn[x, y].transform.position, Vector3.back, out nHit, d) && nHit.transform.tag == "MapDoor") gn[x, y].GetComponent<GridNode>().northDoor = nHit.transform.gameObject;
                        if (Physics.Raycast(gn[x, y].transform.position, Vector3.back, out nHit, d) && (nHit.transform.childCount > 0))
                            for (int i = 0; i < nHit.transform.childCount; i++) if (nHit.transform.GetChild(i).tag == "Torch") gn[x, y].GetComponent<GridNode>().northTorch = true;

                        if (Physics.Raycast(gn[x, y].transform.position, Vector3.left, out eHit, d) && eHit.transform.tag != "MapDoor") gn[x, y].GetComponent<GridNode>().eastLink = null;
                        if (Physics.Raycast(gn[x, y].transform.position, Vector3.left, out eHit, d) && eHit.transform.tag == "MapDoor") gn[x, y].GetComponent<GridNode>().eastDoor = eHit.transform.gameObject;
                        if (Physics.Raycast(gn[x, y].transform.position, Vector3.left, out eHit, d) && (eHit.transform.childCount > 0))
                            for (int i = 0; i < eHit.transform.childCount; i++) if (eHit.transform.GetChild(i).tag == "Torch") gn[x, y].GetComponent<GridNode>().eastTorch = true;

                        if (Physics.Raycast(gn[x, y].transform.position, Vector3.forward, out sHit, d) && sHit.transform.tag != "MapDoor") gn[x, y].GetComponent<GridNode>().southLink = null;
                        if (Physics.Raycast(gn[x, y].transform.position, Vector3.forward, out sHit, d) && sHit.transform.tag == "MapDoor") gn[x, y].GetComponent<GridNode>().southDoor = sHit.transform.gameObject;
                        if (Physics.Raycast(gn[x, y].transform.position, Vector3.forward, out sHit, d) && (sHit.transform.childCount > 0))
                            for (int i = 0; i < sHit.transform.childCount; i++) if (sHit.transform.GetChild(i).tag == "Torch") gn[x, y].GetComponent<GridNode>().southTorch = true;                        

                        if (Physics.Raycast(gn[x, y].transform.position, Vector3.right, out wHit, d) && wHit.transform.tag != "MapDoor") gn[x, y].GetComponent<GridNode>().westLink = null;
                        if (Physics.Raycast(gn[x, y].transform.position, Vector3.right, out wHit, d) && wHit.transform.tag == "MapDoor") gn[x, y].GetComponent<GridNode>().westDoor = wHit.transform.gameObject;
                        if (Physics.Raycast(gn[x, y].transform.position, Vector3.right, out wHit, d) && (wHit.transform.childCount > 0))
                            for (int i = 0; i < wHit.transform.childCount; i++) if (wHit.transform.GetChild(i).tag == "Torch") gn[x, y].GetComponent<GridNode>().westTorch = true;
                    }
                }
            }
        }
    }
}
