using System;
using Script;
using UnityEngine;
using Script.InteractionSystem.Interface;
using Script.InteractionSystem.Struct;

public class MaskObject : MonoBehaviour, IInteractable
{
    public InteractionData Data { get; }
    private LevelOne _levelOne;


    [SerializeField] private AudioClip[] _audioClip;

    [SerializeField] private Subtitle[] experimentSubtitles = new Subtitle[]
    {
        new Subtitle { title = "Oooooo baya da yakıştı haaa" }, // Index 0
        new Subtitle
        {
            title =
                "Tamamdır şimdi maskeni de takdığına göre devam edebiliriz. Kapıdan ilerleyelim daha bir çok işimiz var"
        }, // Index 1
    };

    [Header("Settings")] [SerializeField] private GameObject prefabToAttach;

    private void Awake()
    {
        _levelOne = FindObjectOfType<LevelOne>();
    }

    public void Interact(GameObject interactor)
    {
        if (interactor == null) return;

        bool isSequenceActive = _levelOne != null && _levelOne.currentSubtitleIndex < 7;
        bool isAudioPlaying = VoiceManager.Instance != null &&
                              VoiceManager.Instance.GetAudioSource != null &&
                              VoiceManager.Instance.GetAudioSource.isPlaying;

        if (isSequenceActive || isAudioPlaying)
        {
            Debug.Log("Interaction locked: Voice sequence or subtitles are still in progress.");
            return;
        }

        VoiceManager.Instance.PlaySequence(_audioClip, experimentSubtitles);
        AttachObjectToInteractor(interactor);
        gameObject.SetActive(false);
    }

    private void AttachObjectToInteractor(GameObject interactor)
    {
        if (prefabToAttach == null)
        {
            return;
        }

        GameObject attachedObj = Instantiate(prefabToAttach, interactor.transform);

        attachedObj.transform.localPosition = new Vector3(0, 0.567f, 2.689f);
        attachedObj.transform.localRotation = Quaternion.identity;
        Mask mask = interactor.AddComponent<Mask>();
        mask.SetMask(attachedObj);
        mask.EquipMask();
    }
}