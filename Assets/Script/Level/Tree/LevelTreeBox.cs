using System.Collections;
using UnityEngine;

namespace Script.Level.LevelTree
{
    public class LevelTreeBox : MonoBehaviour
    {
        [SerializeField] private AudioClip[] initialAudioClips;
        [SerializeField] private AudioClip[] delayedAudioClips;

        [SerializeField] private Subtitle[] initialSubtitles = new Subtitle[]
        {
            new Subtitle { title = "Owwww bloklar. Sen de bunu yapacak bir beyin görmüyorum ama neyse bu sonuçta bir deney" },
            new Subtitle { title = "Acaba bu bloklar ne işe yarıyor bi düşün bakalım" }
        };

        [SerializeField] private Subtitle[] delayedSubtitles = new Subtitle[]
        {
            new Subtitle { title = "Hayır yanlış bildin bunlar dondurulabilir bloklar" }
        };

        private bool _isTriggered = false;

        private void Start()
        {
            // Initializing events only if necessary
            if (VoiceManager.Instance != null && delayedSubtitles.Length != 0)
            {
                // We'll subscribe when the trigger happens to avoid global interference
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isTriggered) return;
            if (!other.gameObject.CompareTag("Player")) return;
            if (VoiceManager.Instance == null) return;

            _isTriggered = true;
            VoiceManager.Instance.OnSequenceFinished += HandleInitialSequenceFinished;
            VoiceManager.Instance.PlaySequence(initialAudioClips, initialSubtitles);
        }

        private void HandleInitialSequenceFinished()
        {
            VoiceManager.Instance.OnSequenceFinished -= HandleInitialSequenceFinished;
            StartCoroutine(WaitAndPlayDelayedSequence());
        }

        private IEnumerator WaitAndPlayDelayedSequence()
        {
            yield return new WaitForSeconds(2f);
            
            if (VoiceManager.Instance != null)
            {
                VoiceManager.Instance.PlaySequence(delayedAudioClips, delayedSubtitles);
            }
        }
    }
}