using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public static class SaveLoadModule
{
    public static SaveSlot[] save_slot = new SaveSlot[5];
    public static int ActiveSceneIndex;

    public static void InitSave(int _n)
    {
        save_slot[_n] = new SaveSlot();
        save_slot[_n].InitialSave();
    }

    public static void SaveGame(int _n)
    {
        Debug.Log("saving");
        save_slot[_n].SaveData(SceneManager.GetActiveScene().buildIndex);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveGame0"+_n+".sg");
        bf.Serialize(file, SaveLoadModule.save_slot[_n]);
        file.Close();
    }

    public static void LoadGame(int _n)
    {
        Debug.Log("reached loadgame");
        if(File.Exists(Application.persistentDataPath + "/saveGame0" + _n + ".sg"))
        {
            Debug.Log("found save");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveGame0"+_n+".sg", FileMode.Open);
            SaveLoadModule.save_slot[_n] = (SaveSlot)bf.Deserialize(file);
            file.Close();

            Debug.Log("checking scene");

            //If not in the correct level, load the correct level
            if (SceneManager.GetActiveScene().buildIndex != save_slot[_n].CurrentScene) GameManager.GAME.LoadLevelandWaitUntilDone(save_slot[_n].CurrentScene, "Here");
            if (SceneManager.GetActiveScene().buildIndex == save_slot[_n].CurrentScene) FinishLoadingGame(_n);
        }
    }

    public static void FinishLoadingGame(int _n)
    {
        Debug.Log("finishing load");

        GameManager.GAME.ToggleUI(true);

        //Load data to current
        save_slot[_n].LoadData(save_slot[_n]);

        //Draw the UI
        GameManager.EXPLORE.DrawExplorerUI();

        //Trigger Dynamic Props
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
        for (int i = 0; i < nodes.Length; i++) nodes[i].GetComponent<GridNode>().DynamicProps();       
    }

}