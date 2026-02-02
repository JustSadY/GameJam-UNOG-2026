using System.Collections.Generic;
using Script.InteractionSystem.Struct;
using UnityEngine;

namespace Script
{
    [RequireComponent(typeof(AudioSource))]
    public class PasswdDoor : MonoBehaviour
    {
        public InteractionData Data { get; }

        [SerializeField] private string _originalKey = "MKMK";
        [SerializeField] private GameObject _floor;

        private AudioSource _audioSource;
        [SerializeField] private AudioClip _errorAudioClip;

        private TextMesh _textMesh;
        private string _currentInputKey = "";
        private string _displayedFakeKey = "";

        private List<string> _pressedColors = new List<string>();


        [Header("Audio Settings")] [SerializeField]
        private AudioClip[] WronglevelAudioClips;   

        [SerializeField] private Subtitle[] wrongExperimentSubtitles = new Subtitle[]
        {
            new Subtitle
                { title = "Ahh, another subject, the same ending again. Are all of you the same?" }, // Index 0
            new Subtitle
            {
                title = "It really should not be this difficult. Just once, let someone succeed in this experiment."
            }, // Index 1
            new Subtitle { title = "And after all the help I gave… anyway, a hopeless case." }, // Index 2
            new Subtitle { title = "Next. Let’s move a little faster, come on" }, // Index 3
        };

        [SerializeField] private AudioClip[] levelAudioClips;

        [SerializeField] private Subtitle[] ExperimentSubtitles = new Subtitle[]
        {
            new Subtitle
                { title = "One moment… No way. Seriously? You remembered? Alright, alright, stay calm. Yes, this subject… um… appears to be successful" }, // Index 0
            new Subtitle
            {
                title = "Congratulations, Subject 5351. You have succeeded."
            }, // Index 1
        };
        private void Awake()
        {
            _textMesh = GetComponentInChildren<TextMesh>();
            if (_textMesh != null)
            {
                _textMesh.richText = true;
            }

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogWarning("AudioSource component is missing on " + gameObject.name);
            }
        }

        public void AddKey(char key, char fakeKeyChar, string color)
        {
            if (_currentInputKey.Length >= _originalKey.Length) return;

            _currentInputKey += key;
            _displayedFakeKey += fakeKeyChar;
            _pressedColors.Add(color);

            UpdateVisualText();

            if (_currentInputKey.Length == _originalKey.Length)
            {
                CheckKey();
            }
        }

        private void UpdateVisualText()
        {
            if (string.IsNullOrEmpty(_displayedFakeKey) || _textMesh == null)
            {
                _textMesh.text = "";
                return;
            }

            string formattedText = "";

            for (int i = 0; i < _displayedFakeKey.Length; i++)
            {
                string colorHex = (i < _pressedColors.Count) ? _pressedColors[i] : "#FFFFFF";
                formattedText += $"<color={colorHex}>{_displayedFakeKey[i]}</color>";
            }

            _textMesh.text = formattedText;
        }

        private bool CheckKey()
        {
            if (_originalKey == _currentInputKey)
            {
                OpenDoor();
                return true;
            }

            HandleWrongPassword();
            Invoke(nameof(ResetInputs), 0.5f);
            return false;
        }

        private void ResetInputs()
        {
            _currentInputKey = "";
            _displayedFakeKey = "";
            _pressedColors.Clear();
            if (_textMesh != null) _textMesh.text = "";
        }

        private void OpenDoor()
        {
            VoiceManager.Instance.PlaySequence(levelAudioClips, ExperimentSubtitles);

        }

        private void HandleWrongPassword()
        {
            if (_floor != null) _floor.SetActive(false);

            PlayErrorSound();
            VoiceManager.Instance.PlaySequence(WronglevelAudioClips, wrongExperimentSubtitles);
        }

        private void PlayErrorSound()
        {
            if (_audioSource != null && _errorAudioClip != null)
            {
                _audioSource.PlayOneShot(_errorAudioClip);
            }
            else
            {
                Debug.LogError("AudioSource or Error AudioClip is missing!");
            }
        }
    }
}