using UnityEngine;

public class Trainer : MonoBehaviour, ISaveable
{
    [SerializeField] private PokemonPartyManager _pokemonPartyManager;

    public bool CanBattle { get; set; }

    public PokemonPartyManager PokemonPartyManager { get => _pokemonPartyManager; }

    private void Awake()
    {
        CanBattle = true;
    }

    public void LoadData(GameData data)
    {
        string uid = GetComponent<GuidHolder>().UniqueId;
        string sceneName = gameObject.scene.name;
        
        if (!data.ScenesData.ContainsKey(sceneName))
        {
            return;
        }
        
        if (data.ScenesData[sceneName].TrainersSaveData.ContainsKey(uid))
        {
            CanBattle = data.ScenesData[sceneName].TrainersSaveData[uid].CanBattle;
        }
    }

    public void SaveData(ref GameData data)
    {
        string uid = GetComponent<GuidHolder>().UniqueId;
        string sceneName = gameObject.scene.name;

        if (!data.ScenesData.ContainsKey(sceneName))
        {
            data.ScenesData.Add(sceneName, new SceneSaveData(null));
        }

        if (data.ScenesData[sceneName].TrainersSaveData.ContainsKey(uid))
        {
            data.ScenesData[sceneName].TrainersSaveData.Remove(uid);
        }
        data.ScenesData[sceneName].TrainersSaveData[uid] = new TrainerSaveData(CanBattle);
    }
}
