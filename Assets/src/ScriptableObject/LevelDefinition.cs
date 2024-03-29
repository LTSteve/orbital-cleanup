using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

[Serializable]
[CreateAssetMenu(fileName = "0", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelDefinition : ScriptableObject
{
    public GameSceneLoader.GameScene LevelScene;

    public PositionVelocity PlayerStart;

    public List<PositionVelocity> Scraps;
    public List<PositionVelocity> Bombs;
    public List<PositionVelocity> Fuels;
    public List<PositionVelocity> Aliens;

    public AudioClip Music;

    public string Name;
    public int Time = -1;
    [Range(0f, 1f)]
    public float StartingFuel = 1f;
    public int StartingHealth = 3;

    public float MaxCameraRange = 9f;

    public DialogueDefinition Dialogue;
}
