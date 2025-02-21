using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomTextFade : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float duration = 5f; // Durée totale de l'effet
    private float[] charAlphas; // Opacité des lettres
    private int[] randomOrder; // Ordre aléatoire des lettres

    void Start()
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshProUGUI>();

        // Désactive immédiatement le texte
        textMesh.ForceMeshUpdate();
        int length = textMesh.textInfo.characterCount;
        charAlphas = new float[length];
        randomOrder = new int[length];

        // Initialise toutes les lettres à invisible et crée un ordre aléatoire
        for (int i = 0; i < length; i++)
        {
            charAlphas[i] = 0f;
            randomOrder[i] = i;
        }

        // Mélange l'ordre d'apparition des lettres
        ShuffleArray(randomOrder);

        // Démarre l'effet
        StartCoroutine(FadeInLetters());
    }

    IEnumerator FadeInLetters()
    {
        float interval = duration / textMesh.textInfo.characterCount;

        for (int i = 0; i < randomOrder.Length; i++)
        {
            int index = randomOrder[i];
            StartCoroutine(FadeLetter(index, interval));
            yield return new WaitForSeconds(interval / 2); // Petit décalage pour un effet progressif
        }
    }

    IEnumerator FadeLetter(int index, float time)
    {
        float elapsed = 0;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            charAlphas[index] = Mathf.Lerp(0, 1, elapsed / time);
            UpdateTextAlpha();
            yield return null;
        }
        charAlphas[index] = 1;
        UpdateTextAlpha();
    }

    void UpdateTextAlpha()
    {
        textMesh.ForceMeshUpdate();
        TMP_TextInfo textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (textInfo.characterInfo[i].isVisible)
            {
                int meshIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                Color32[] vertexColors = textInfo.meshInfo[meshIndex].colors32;

                for (int j = 0; j < 4; j++)
                {
                    vertexColors[vertexIndex + j].a = (byte)(charAlphas[i] * 255);
                }
            }
        }

        textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    void ShuffleArray(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
