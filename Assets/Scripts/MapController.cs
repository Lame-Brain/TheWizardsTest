using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private Material[] colorMats;
    public Color levelColor;

    // Start is called before the first frame update
    void Start()
    {
        colorMats = GameManager.GAME.DungeonColorTextures;
        foreach (Material cm in colorMats)
            cm.color = levelColor;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("EditorOnly")) go.SetActive(false);

        StartCoroutine(LateStart(.15f));

    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Load map from saved file        
        Debug.Log("Load Mini Map Info for Save Slot #"+ GameManager.GAME.SelectedSaveSlot+", map " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        SaveLoadModule.save_slot[GameManager.GAME.SelectedSaveSlot].GetMiniMap(SaveLoadModule.save_slot[GameManager.GAME.SelectedSaveSlot], UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        GameManager.PARTY.BuildMapVisibility();
    }
}
