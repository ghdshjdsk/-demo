using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance{get; private set;}
    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float volume = 1;

    private const string PLAYER_PREFS_SOUND_VOLUME = "SoundVolume";


    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_VOLUME);
    }

    void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnTryCut += CuttingCounter_OnTryCut;
        Player.Instance.OnObjectUp += Player_OnObjectUp;
        BaseCounter.OnAnyObjectPlacedHere += Player_OnAnyObjectPlacedHere;
        TrashCounter.OnTrash += Player_OnTrash;
    }

    private void Player_OnTrash(object sender,System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.objectPickup,trashCounter.transform.position);
    }

    private void Player_OnObjectUp(object sender,System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup,Player.Instance.transform.position);
    }

    private void Player_OnAnyObjectPlacedHere(object sender,System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectPickup,baseCounter.transform.position);
    }

    private void CuttingCounter_OnTryCut(object sender,System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop,cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender,System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess,DeliveryManager.Instance.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender,System.EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail,DeliveryManager.Instance.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray,Vector3 position,float volume = 1)
    {
        PlaySound(audioClipArray[Random.Range(0,audioClipArray.Length)],position,volume);
    }

    private void PlaySound(AudioClip audioClip,Vector3 position,float volumeMultiplier = 1)
    {
        AudioSource.PlayClipAtPoint(audioClip,position,volume * volumeMultiplier);
    }

    public void PlayerFootstepsSound(Vector3 position,float volume = 1)
    {
        PlaySound(audioClipRefsSO.footstep,position,volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefsSO.warning,Vector3.zero);
    }

    public void ChangeVolume(float value)
    {
        volume = value;

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_VOLUME,volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
