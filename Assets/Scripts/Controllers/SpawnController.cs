using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject ref_mobPF;
    public GameObject[] ref_AssignedWaypointCluster;
    public int minTimeToSpawn, maxTimeToSpawn, MaxNumberOfSpawns, startingNumberOfSpawns;
    public float offsetRadius;
    public int wanderingMonster_MinPackSize, wanderingMonster_MaxPackSize;
    public float WanderingMonster_Frequency;

    private int timer, alarm;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SetAlarm();
        for (int _i = 0; _i < startingNumberOfSpawns; _i++) SpawnMob();        
    }

    private void SpawnMob()
    {
        //if((transform.childCount - 1) < MaxNumberOfSpawns && (Vector3.Distance(transform.position, player.transform.position) < 6f)) //Wont Spawn if the player is too close, or there are too many spawns already (this didn't work out, I will investigate later)
        if ((transform.childCount - 1) < MaxNumberOfSpawns) //Wont Spawn if there are too many spawns already
            Instantiate(ref_mobPF, new Vector3(transform.position.x + Random.Range(0f, offsetRadius), 1f, transform.position.z + Random.Range(0f, offsetRadius)), Quaternion.identity, transform);
    }
    private void SetAlarm()
    {
        timer = 0;
        alarm = Random.Range(minTimeToSpawn, maxTimeToSpawn);
    }

    public void TurnPasses()
    {
        if(minTimeToSpawn < 2147483647) timer++;
        if(timer > alarm)
        {
            SetAlarm();
            SpawnMob();
        }
    }

    public void RestoreChildren(SaveSlot.MonsterData[] mob)
    {
        //Clear existing mobs
        GameObject[] _allMobs = GameObject.FindGameObjectsWithTag("Mob");
        foreach (GameObject _mob in _allMobs) if(_mob.transform.IsChildOf(transform))Destroy(_mob);

        GameObject _thismob;
        GameObject[] _allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        for(int _i = 0; _i < mob.Length; _i++)
        {
            _thismob = Instantiate(ref_mobPF, transform);
            _thismob.transform.position = new Vector3(mob[_i].XCoor, 1.4f, mob[_i].Ycoor);
            _thismob.GetComponent<MonsterLogic>().wounds = mob[_i].wounds;
            _thismob.GetComponent<MonsterLogic>().monsterState = mob[_i].MonsterState;
            _thismob.GetComponent<MonsterLogic>().orders = mob[_i].Orders;
            _thismob.GetComponent<MonsterLogic>().wayPoint = _thismob;
            foreach (GameObject _wp in _allWaypoints) if (mob[_i].waypoint.Xcoor == (int)_wp.transform.position.x && mob[_i].waypoint.Ycoor == (int)_wp.transform.position.z && mob[_i].waypoint.UID == _wp.GetComponent<WaypointController>().UID) _thismob.GetComponent<MonsterLogic>().wayPoint = _wp;
        }
    }
}
