using System;
using Script;
using UnityEngine;

public class SixLevel2 : MonoBehaviour
{
    private bool IsFirst = true;

    [SerializeField] private AudioClip[] _audioClips;

    [SerializeField] private Subtitle[] experimentSubtitles = new Subtitle[]
    {
        new Subtitle { title = "Umarım hala tek parçasındır. Sıradaki odaya geç" }, // Index 0
    };

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!IsFirst) return;
        IsFirst = false;
        VoiceManager.Instance.PlaySequence(_audioClips, experimentSubtitles);
    }
}