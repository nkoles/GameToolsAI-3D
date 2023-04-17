using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextScroll : MonoBehaviour

{
    public float delay = 0.1f;

    public string myText;

    private string curText;

    public TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(showText());
    }

    IEnumerator showText()
    {
        for(int i =0; i < myText.Length; i++)
        {
            curText = myText.Substring(0,i+1);
            
            textMesh.text = curText;

            yield return new WaitForSeconds(delay);
        }
    }
}