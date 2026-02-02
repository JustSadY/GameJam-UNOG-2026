using Script;
using UnityEngine;

public class LevelOne : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] levelAudioClips;
    
    [Header("Door References")]
    [SerializeField] private Door entryDoor;
    [SerializeField] private Door exitDoor;
    [SerializeField] private int whichIndex;
    [SerializeField] private int whichIndex2;

    public int currentSubtitleIndex = 0;

    [SerializeField] private Subtitle[] experimentSubtitles = new Subtitle[]
    {
        new Subtitle { title = "Ses deneme ses bir iki üç. Sesim geliyor mu?" }, // Index 0
        new Subtitle { title = "Bu mikrofonu değiştirelim böyle olmaz" }, // Index 1
        new Subtitle { title = "Galiba şimdi geliyor. Deney 5351'e hoşgeldin" }, // Index 2
        new Subtitle { title = "Seninle küçük ama çok küçük bir deney yapacağız" }, // Index 3
        new Subtitle { title = "Bazı odalarda bazı şeyler yapman gerekecek" }, // Index 4
        new Subtitle { title = "Oluşabilecek her türlü rahatsızlıktan dolayı üzgün olduğumuzu.. neyse buraları geçelim" }, // Index 5
        new Subtitle { title = "Seni ilk odaya alalım", extraDelay = 1.5f } // Index 6
    };

    private void Start()
    {
        StartLevelSequence();
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks or null reference errors
        if (VoiceManager.Instance != null)
        {
            VoiceManager.Instance.OnClipFinished -= HandleClipFinished;
        }
    }

    private void StartLevelSequence()
    {
        if (VoiceManager.Instance == null)
        {
            Debug.LogError("LevelOne: VoiceManager Instance not found!");
            return;
        }

        if (levelAudioClips.Length != experimentSubtitles.Length)
        {
            Debug.LogError($"LevelOne: Array length mismatch! Audio: {levelAudioClips.Length}, Subs: {experimentSubtitles.Length}");
            return;
        }

        // Reset index before starting
        currentSubtitleIndex = 0;
        
        VoiceManager.Instance.OnClipFinished += HandleClipFinished;
        VoiceManager.Instance.PlaySequence(levelAudioClips, experimentSubtitles);
    }

    private void HandleClipFinished()
    {
        // Index 5: Opening the entry door after the 6th clip (index 5) finishes
        if (currentSubtitleIndex == whichIndex)
        {
            if (entryDoor != null)
            {
                entryDoor.OpenTheDoor();
            }
        }

        // Logic Note: Your array has 7 elements (0 to 6). 
        // Index 11 is unreachable here. If you have more clips later, 
        // ensure the array size matches.
        if (currentSubtitleIndex == whichIndex2)
        {
            if (exitDoor != null)
            {
                exitDoor.OpenTheDoor();
            }
        }

        currentSubtitleIndex++;
    }
}