using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

[ExecuteAlways]
public class LevelLoader : MonoBehaviour
{
    public static LevelDefinition NextLevel;

    public LevelDefinition WorkingLevel;

    public bool SaveLevel = false;
    public bool LoadLevel = false;
    public bool ClearLevel = false;

    public Transform ScrapHolder;
    public Transform BombHolder;
    public Transform FuelHolder;

    public PlayerController PlayerPrefab;
    public Scrap ScrapPrefab;
    public Bomb BombPrefab;
    public Fuel FuelPrefab;

    private void Update()
    {
        if (Application.isPlaying)
        {
            return;
        }

        if (SaveLevel)
        {
            _saveLevel();
            SaveLevel = false;
        }
        else if (LoadLevel)
        {
            _loadLevel();
            LoadLevel = false;
        }
        else if (ClearLevel)
        {
            _clearLevel();
            ClearLevel = false;
        }
    }

    private void _saveLevel()
    {
        if (Application.isPlaying)
            return;
        if(WorkingLevel == null)
        {
            Debug.LogError("please create a level and put it in the working level slot");
            return;
        }

        var spawnedScrapList = new List<PositionVelocity>();
        foreach(Transform scrapObject in ScrapHolder)
        {
            var orbit = scrapObject.GetComponent<OrbitHandler>();
            spawnedScrapList.Add(new PositionVelocity { Position = scrapObject.position, Velocity = orbit.StartingVelocity, Rotation = orbit.StartingRotation });
        }
        WorkingLevel.Scraps = spawnedScrapList;

        if(spawnedScrapList == null || spawnedScrapList.Count == 0)
        {
            Debug.LogError("please place at least one scrap");
            return;
        }

        var spawnedBombList = new List<PositionVelocity>();
        foreach (Transform bombObject in BombHolder)
        {
            var orbit = bombObject.GetComponent<OrbitHandler>();
            spawnedBombList.Add(new PositionVelocity { Position = bombObject.position, Velocity = orbit.StartingVelocity, Rotation = orbit.StartingRotation });
        }
        WorkingLevel.Bombs = spawnedBombList;

        var spawnedFuelList = new List<PositionVelocity>();
        foreach (Transform fuelObject in FuelHolder)
        {
            var orbit = fuelObject.GetComponent<OrbitHandler>();
            spawnedFuelList.Add(new PositionVelocity { Position = fuelObject.position, Velocity = orbit.StartingVelocity, Rotation = orbit.StartingRotation });
        }
        WorkingLevel.Fuels = spawnedFuelList;

        var player = Transform.FindObjectOfType<PlayerController>();

        if(player == null)
        {
            Debug.LogError("Please place a player");
            return;
        }

        var playerOrbitHandler = player.gameObject.GetComponent<OrbitHandler>();
        WorkingLevel.PlayerStart = new PositionVelocity { Position = playerOrbitHandler.transform.position, Velocity = playerOrbitHandler.StartingVelocity };

        WorkingLevel.LevelScene = (GameSceneLoader.GameScene)SceneManager.GetActiveScene().buildIndex;

        EditorUtility.SetDirty(WorkingLevel);
    }

    public void BeginLevelLoad() 
    {
        _loadLevel();
    }

    private void _clearLevel()
    {
        var player = Transform.FindObjectOfType<PlayerController>();
        if (player != null)
        {
            _destroy(player.gameObject);
        }

        foreach (Transform scrap in ScrapHolder)
        {
            _destroy(scrap.gameObject);
        }
        foreach (Transform bomb in BombHolder)
        {
            _destroy(bomb.gameObject);
        }
        foreach (Transform fuel in FuelHolder)
        {
            _destroy(fuel.gameObject);
        }
    }

    private void _loadLevel()
    {
        _clearLevel();

        if(NextLevel != null)
        {
            WorkingLevel = NextLevel;
            NextLevel = null;
        }

        if (WorkingLevel == null)
        {
            Debug.LogError("please create a level and put it in the working level slot");
            return;
        }

        var player = Instantiate(PlayerPrefab, transform.parent);

        player.transform.position = WorkingLevel.PlayerStart.Position;

        var playerOrbitHandler = player.GetComponent<OrbitHandler>();
        playerOrbitHandler.StartingVelocity = WorkingLevel.PlayerStart.Velocity;
        playerOrbitHandler.ResetVelocity();

        foreach(var scrapPlacement in WorkingLevel.Scraps)
        {
            var newScrap = Instantiate(ScrapPrefab, ScrapHolder);
            var newScrapOrbit = newScrap.GetComponent<OrbitHandler>();
            newScrap.transform.position = scrapPlacement.Position;
            newScrapOrbit.StartingVelocity = scrapPlacement.Velocity;
            newScrapOrbit.StartingRotation = scrapPlacement.Rotation;
            newScrapOrbit.ResetVelocity();
        }

        foreach (var bombPlacement in WorkingLevel.Bombs)
        {
            var newBomb = Instantiate(BombPrefab, BombHolder);
            var newBombOrbit = newBomb.GetComponent<OrbitHandler>();
            newBomb.transform.position = bombPlacement.Position;
            newBombOrbit.StartingVelocity = bombPlacement.Velocity;
            newBombOrbit.StartingRotation = bombPlacement.Rotation;
            newBombOrbit.ResetVelocity();
        }

        foreach (var fuelPlacement in WorkingLevel.Fuels)
        {
            var newFuel = Instantiate(FuelPrefab, FuelHolder);
            var newFuelOrbit = newFuel.GetComponent<OrbitHandler>();
            newFuel.transform.position = fuelPlacement.Position;
            newFuelOrbit.StartingVelocity = fuelPlacement.Velocity;
            newFuelOrbit.StartingRotation = fuelPlacement.Rotation;
            newFuelOrbit.ResetVelocity();
        }

        if(Application.isPlaying)
            MusicController.PlayTrack(WorkingLevel.Music);
    }

    private void _destroy(GameObject go)
    {
        if (!Application.isPlaying)
        {
            DestroyImmediate(go);
        }
        else
        {
            Destroy(go);
        }
    }
}
