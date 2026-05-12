using UnityEngine;
using System.Collections;

public class Absorbable : MonoBehaviour
{
    private bool isBeingAbsorbed = false;

    public void StartAbsorbing(Transform pullCenter, float duration)
    {
        if (isBeingAbsorbed) return;
        
        isBeingAbsorbed = true;

        if (GetComponent<Collider>()) GetComponent<Collider>().enabled = false;

        StartCoroutine(AbsorbRoutine(pullCenter, duration));
    }

    private IEnumerator AbsorbRoutine(Transform pullCenter, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;
        
        Vector3 finalScale = Vector3.zero; 

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (pullCenter == null) break;

            elapsedTime += Time.deltaTime;
            
            float normalizedTime = elapsedTime / duration; 
            transform.position = Vector3.Lerp(startPosition, pullCenter.position, normalizedTime);

            transform.localScale = Vector3.Lerp(startScale, finalScale, normalizedTime);

            yield return null;
        }

        Destroy(gameObject);
    }
}