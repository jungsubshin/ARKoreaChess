using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlinkText : MonoBehaviour {

    Image image;

    // Use this for initialization
    void Start() {
        image = GetComponent<Image>();
        StartCoroutine(blink());
    }

    public IEnumerator blink() {
        while (true) {
            image.enabled = false;
            yield return new WaitForSeconds(.5f);
            image.enabled = true;
            yield return new WaitForSeconds(.5f);
        }
    }
}