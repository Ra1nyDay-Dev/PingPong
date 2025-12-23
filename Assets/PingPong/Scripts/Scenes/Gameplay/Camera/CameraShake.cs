using System.Collections;
using UnityEngine;

namespace PingPong.Scripts.Scenes.Gameplay.Camera
{
    public class CameraShake : MonoBehaviour
    {
        public void Shake(float duration = 0.5f, float magnitude = 0.1f)
        {
            StartCoroutine(ShakeCoroutine(duration, magnitude));
        }

        private IEnumerator ShakeCoroutine(float duration, float magnitude)
        {
            Vector3 originalPos = transform.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float damper = 1f - (elapsed / duration);
            
                float x = Random.Range(-1f, 1f) * magnitude * damper;
                float y = Random.Range(-1f, 1f) * magnitude * damper;

                transform.localPosition = new Vector3(x, y, originalPos.z);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalPos;
        }
    }
}
