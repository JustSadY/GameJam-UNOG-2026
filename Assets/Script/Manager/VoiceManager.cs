using System;
using UnityEngine;
using System.Collections;
using TMPro;

namespace Script
{
    [Serializable]
    public struct Subtitle
    {
        public string title;
        public float extraDelay;
        [HideInInspector] public float duration;
    }

    [RequireComponent(typeof(AudioSource))]
    public class VoiceManager : MonoBehaviour
    {
        public event Action OnClipFinished;
        public event Action OnSequenceFinished;

        public static VoiceManager Instance { get; private set; }

        [Header("UI Components")] [SerializeField]
        private TextMeshProUGUI subtitleText;

        [SerializeField] private GameObject subtitlePanel;

        private AudioSource _audioSource;
        private Coroutine _playbackCoroutine;

        public AudioSource GetAudioSource => _audioSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _audioSource = GetComponent<AudioSource>();
            InitializeUI();
        }

        private void InitializeUI()
        {
            if (subtitlePanel == null)
            {
                subtitlePanel = GameObject.Find("SubtitlePanel");
            }

            if (subtitlePanel != null && subtitleText == null)
            {
                subtitleText = subtitlePanel.GetComponentInChildren<TextMeshProUGUI>();
            }

            if (subtitlePanel != null)
            {
                subtitlePanel.SetActive(false);
            }
        }

        public void PlaySequence(AudioClip[] clips, Subtitle[] subtitles)
        {
            if (clips == null || subtitles == null || clips.Length != subtitles.Length)
            {
                Debug.LogError($"VoiceManager: Count mismatch! Clips: {clips?.Length}, Subtitles: {subtitles?.Length}");
                return;
            }

            if (_playbackCoroutine != null) StopCoroutine(_playbackCoroutine);
            _playbackCoroutine = StartCoroutine(PlaybackSequenceRoutine(clips, subtitles));
        }

        private IEnumerator PlaybackSequenceRoutine(AudioClip[] clips, Subtitle[] subtitles)
        {
            if (subtitlePanel != null) subtitlePanel.SetActive(true);

            for (int i = 0; i < clips.Length; i++)
            {
                AudioClip currentClip = clips[i];
                Subtitle currentSubtitle = subtitles[i];

                if (subtitleText != null) subtitleText.text = currentSubtitle.title;

                if (currentClip != null)
                {
                    _audioSource.clip = currentClip;
                    _audioSource.Play();
                    yield return new WaitForSeconds(currentClip.length);
                }

                if (currentSubtitle.extraDelay > 0f)
                {
                    yield return new WaitForSeconds(currentSubtitle.extraDelay);
                }

                OnClipFinished?.Invoke();
            }

            FinishPlayback();
        }

        private void FinishPlayback()
        {
            ClearSubtitles();
            _playbackCoroutine = null;
            OnSequenceFinished?.Invoke();
        }

        public void ClearSubtitles()
        {
            if (subtitleText != null) subtitleText.text = string.Empty;
            if (subtitlePanel != null) subtitlePanel.SetActive(false);
        }

        public void StopAll()
        {
            if (_audioSource != null) _audioSource.Stop();
            if (_playbackCoroutine != null) StopCoroutine(_playbackCoroutine);
            ClearSubtitles();
        }
    }
}