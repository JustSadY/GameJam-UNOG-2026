using System;
using UnityEngine;

namespace Script
{
    public class LastLevel : MonoBehaviour
    {
        [Header("Audio Settings")] [SerializeField]
        private AudioClip[] levelAudioClips;

        [SerializeField] private Subtitle[] experimentSubtitles = new Subtitle[]
        {
            new Subtitle { title = "Yes, we have reached the end of everything." }, // Index 0
            new Subtitle
            {
                title = "You have one final task. I gave you a password earlier, if you recall — in the mouse room."
            }, // Index 1
            new Subtitle
            {
                title = " Enter it at the door, and that will conclude the experiment. Quite simple, isn’t it?"
            }, // Index 2
        };

        private void Start()
        {
            VoiceManager.Instance.PlaySequence(levelAudioClips, experimentSubtitles);
        }
    }
}