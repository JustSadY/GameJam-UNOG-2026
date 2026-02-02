using System;
using Script;
using UnityEngine;

public class FirstDoor : MonoBehaviour
{
    private bool IsFirst = true;
    [SerializeField] private AudioClip[] _audioClip;

    [SerializeField] private Subtitle[] experimentSubtitles = new Subtitle[]
    {
        new Subtitle { title = "Pekala ilk odamıza geldik ve karşımızda bir maske var" }, // Index 0
        new Subtitle { title = "Görmesi çok zor olmalı değil mi" }, // Index 1
        new Subtitle
            { title = "Kim tasarladı abi bu odayı dsafdsgfdsag neyse çok sorgulamayalım tak şunu " }, // Index 2
    };

    private void OnTriggerEnter(Collider other)
    {
        if (!IsFirst) return;
        if (!other.gameObject.CompareTag("Player")) return;
        IsFirst = false;
        VoiceManager.Instance.PlaySequence(_audioClip, experimentSubtitles);
    }
}