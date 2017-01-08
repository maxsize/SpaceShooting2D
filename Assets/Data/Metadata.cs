using UnityEngine;
using System.IO;

public class Metadata
{
    private static Metadata _instance;

    [SerializeField]
    public PlayerEmitterVO playerEmitter;
    [SerializeField]
    public PulseEmitterVO[] pulseEmitters;
    [SerializeField]
    public EnemyVO[] enemies;
    [SerializeField]
    public PlayerVO[] players;

    public static Metadata Instance
    {
        get { return _instance; }
    }

    public static void Initialize()
    {
        var str = File.ReadAllText(Path.Combine(ComponentAppender.JSON_PATH, "metadata.json"));
        _instance = JsonUtility.FromJson<Metadata>(str);
        str = null;
    }
}
