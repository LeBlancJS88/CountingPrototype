using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject[] gameObjects; // Array of game objects to activate
    public KeyCode[] inputKeys; // Array of input keys corresponding to the colors
    public float activationTime = 1.0f; // Base time to activate each game object
    public float flashDuration = 0.5f; // Duration of the flash effect
    public float sameIndexGap = 0.5f; // Additional time gap for the same index

    public SoundManager soundManager; // Reference to the SoundManager component
    public UIManager uiManager; // Reference to the UIManager component

    private List<int> savedPattern; // List to store the saved pattern
    private int currentIndex = 0; // Index to keep track of the current element in the pattern
    private int patternLength = 2; // Initial length of the pattern

    private void Start()
    {
        savedPattern = new List<int>();
        GeneratePattern(patternLength);
        StartCoroutine(ActivatePattern());
    }

    private void GeneratePattern(int length)
    {
        savedPattern.Clear();
        for (int i = 0; i < length; i++)
        {
            savedPattern.Add(Random.Range(0, gameObjects.Length));
        }
    }

    private IEnumerator ActivatePattern()
    {
        for (int i = 0; i < savedPattern.Count; i++)
        {
            yield return new WaitForSeconds(activationTime);

            // Deactivate previous game object
            if (i > 0)
            {
                gameObjects[savedPattern[i - 1]].SetActive(false);
            }

            // Check if the current index matches the previous index
            if (i > 0 && savedPattern[i] == savedPattern[i - 1])
            {
                // Additional time gap for the same index
                yield return new WaitForSeconds(sameIndexGap);
            }

            // Activate current game object
            int gameObjectIndex = savedPattern[i];
            gameObjects[gameObjectIndex].SetActive(true);

            // Play the associated audio clip
            soundManager.PlayAudioClip(gameObjectIndex);

            // Deactivate the last game object
            if (i == savedPattern.Count - 1)
            {
                yield return new WaitForSeconds(activationTime);
                gameObjects[savedPattern[i]].SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            KeyCode pressedKey = GetPressedKey();
            if (pressedKey != KeyCode.None)
            {
                OnInput(pressedKey);
            }
        }
    }

    private KeyCode GetPressedKey()
    {
        foreach (KeyCode key in inputKeys)
        {
            if (Input.GetKeyDown(key))
            {
                return key;
            }
        }
        return KeyCode.None;
    }

    public void OnInput(KeyCode inputKey)
    {
        int expectedIndex = -1;

        switch (inputKey)
        {
            case KeyCode.W:
                expectedIndex = 0; // Blue
                break;
            case KeyCode.A:
                expectedIndex = 1; // Yellow
                break;
            case KeyCode.S:
                expectedIndex = 3; // Red
                break;
            case KeyCode.D:
                expectedIndex = 2; // Green
                break;
        }

        if (expectedIndex == savedPattern[currentIndex])
        {
            // Flash the corresponding game object
            StartCoroutine(FlashGameObject(expectedIndex));

            // Play the associated audio clip
            soundManager.PlayAudioClip(expectedIndex);

            currentIndex++;

            // Check if the pattern is completed
            if (currentIndex >= savedPattern.Count)
            {
                Debug.Log("Pattern matched!");

                // Increase level and score in UIManager
                uiManager.IncreaseLevel();
                uiManager.IncreaseScore();

                // Increase pattern length
                patternLength++;
                currentIndex = 0;
                GeneratePattern(patternLength);
                StartCoroutine(ActivatePattern());

                // You can perform any desired action when the pattern is successfully matched
                // You can also reset the currentIndex and generate a new pattern here
            }
        }
        else
        {
            Debug.Log("Pattern mismatch!");

            // Deduct score in UIManager
            uiManager.DeductScore();

            // Restart the pattern
            currentIndex = 0;
            StartCoroutine(ActivatePattern());

            // You can perform any desired action when the pattern is mismatched
            // You can also reset the currentIndex and start a new round here
        }
    }


    private IEnumerator FlashGameObject(int index)
    {
        GameObject gameObject = gameObjects[index];
        gameObject.SetActive(true);

        yield return new WaitForSeconds(flashDuration);

        gameObject.SetActive(false);
    }
}