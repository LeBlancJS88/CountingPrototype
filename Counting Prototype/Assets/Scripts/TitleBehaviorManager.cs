using System.Collections;
using UnityEngine;

public class TitleBehaviorManager : MonoBehaviour
{
    public GameObject[] objects;  // Array of objects to activate/deactivate
    public Light[] lights;  // Array of lights to scale intensity
    public float activationDelay = 9f;  // Delay before activating the next object
    public float deactivationDelay = 10f;  // Delay before deactivating the current object
    public float maxIntensity = 2f;  // Maximum intensity value

    private int currentIndex = 0;  // Index of the currently activated object

    private void Start()
    {
        // Start the activation coroutine
        StartCoroutine(ActivateObjectsCoroutine());
    }

    private IEnumerator ActivateObjectsCoroutine()
    {
        while (true)
        {
            // Activate the current object
            objects[currentIndex].SetActive(true);
            Debug.Log("Object " + (currentIndex + 1) + " activated");

            float activationEndTime = Time.time + deactivationDelay;

            while (Time.time < activationEndTime)
            {
                // Scale light intensity from 2 to max value in the first half of the activation time
                float elapsedTime = Time.time - (activationEndTime - deactivationDelay);
                if (elapsedTime < deactivationDelay / 2f)
                {
                    float t = Mathf.Clamp01(elapsedTime / (deactivationDelay / 2f));
                    lights[currentIndex].intensity = Mathf.Lerp(2f, maxIntensity, t);
                }
                // Scale light intensity from max value to 2 in the second half of the activation time
                else
                {
                    float t = Mathf.Clamp01((elapsedTime - deactivationDelay / 2f) / (deactivationDelay / 2f));
                    lights[currentIndex].intensity = Mathf.Lerp(maxIntensity, 2f, t);
                }

                yield return null;
            }

            // Reset the light intensity to 2
            lights[currentIndex].intensity = 2f;

            // Deactivate the current object
            objects[currentIndex].SetActive(false);
            Debug.Log("Object " + (currentIndex + 1) + " deactivated");

            // Calculate the index of the next object to activate
            int nextIndex = (currentIndex + 1) % objects.Length;

            // Wait for the activation delay before activating the next object
            yield return new WaitForSeconds(activationDelay - deactivationDelay);

            // Move to the next object
            currentIndex = nextIndex;
        }
    }
}