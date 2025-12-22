using UnityEngine;

namespace SupplyChain.Model
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        public void PlayBGM(AudioClip clip)
        {
            if (_audioSource.clip == clip) return;

            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        public void StopBGM()
        {
            _audioSource.Stop();
            _audioSource.clip = null;
        }

        public void PlaySFX(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }
    }
}