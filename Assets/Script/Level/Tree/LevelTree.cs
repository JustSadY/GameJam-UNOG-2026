using UnityEngine;

namespace Script.Level.LevelTree
{
    public class LevelTree : MonoBehaviour
    {
        public int CurrentSubtitleIndex = 0;

        [SerializeField] private AudioClip[] audioClips;

        public Subtitle[] experimentSubtitles = new Subtitle[]
        {
            new Subtitle { title = "Bir şey söyleyeyim mi gerekten de bok gibi bir merdiven oldu." }, // Index 0
        };
    }
}