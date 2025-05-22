using UnityEngine;
using System.Collections;

public class MainMenuRats : MonoBehaviour
{

    [SerializeField] GameObject rat1;
    [SerializeField] GameObject rat2;

    [SerializeField] GameObject spawn1;
    [SerializeField] GameObject spawn2;
    [SerializeField] GameObject spawn3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float minDelay = 7f;
    private float maxDelay = 10f;

    private int lastPlayedIndex = -1;

    void Start()
    {
        StartCoroutine(RandomRatLoop());
    }

    private IEnumerator RandomRatLoop()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            int newIndex;
            do
            {
                newIndex = Random.Range(0, 4);
            } while (newIndex == lastPlayedIndex);

            lastPlayedIndex = newIndex;

            switch (newIndex)
            {
                case 0: RatAni1(); break;
                case 1: RatAni2(); break;
                case 2: RatAni3(); break;
                case 3: RatAni4(); break;
            }
        }
    }

    private void RatAni1() {
        int newIndex = Random.Range(0, 2);
        GameObject newRat;
        if (newIndex == 1) {
            newRat = Instantiate(rat1);
        }
        else {
            newRat = Instantiate(rat2);
        }
        StartCoroutine(MoveOverTime(newRat, spawn1.transform.position, new Vector3(7, -9, -30), 4f));
    }

    private void RatAni2() {
        Quaternion yRotation = Quaternion.Euler(0f, 325f, 0f);
        GameObject newRat;
        int newIndex = Random.Range(0, 2);
        if (newIndex == 1) newRat = Instantiate(rat1, Vector3.zero, yRotation);
        else newRat = Instantiate(rat2, Vector3.zero, yRotation);
        // GameObject newRat2 = Instantiate(rat2, Vector3.zero, yRotation);
        StartCoroutine(MoveOverTime(newRat, spawn2.transform.position, new Vector3(20f, -9, -30f), 4f));
    }

    private void RatAni3() {
        Quaternion yRotation = Quaternion.Euler(0f, 180f, 0f);

        GameObject newRat;
        GameObject newRat2;
        int newIndex = Random.Range(0, 2);
        if (newIndex == 1) {
            newRat = Instantiate(rat1, Vector3.zero, yRotation);;
            newRat2 = Instantiate(rat2);
        } else {
            newRat = Instantiate(rat2, Vector3.zero, yRotation);;
            newRat2 = Instantiate(rat1);
        }

        StartCoroutine(MoveOverTime(newRat, spawn3.transform.position, new Vector3(7, -9, 15), 4f));

        StartCoroutine(MoveOverTime(newRat2, spawn1.transform.position, new Vector3(7, -9, -30), 4f));
    }

    private void RatAni4() {
        GameObject newRat;
        int newIndex = Random.Range(0, 2);
        if (newIndex == 1) newRat = Instantiate(rat1);
        else newRat = Instantiate(rat2);
        StartCoroutine(MoveOverTime(newRat, spawn1.transform.position,new Vector3(7, -9, -10), new Vector3(7, -9, -30), 2f, 2f));
    }

    private IEnumerator MoveOverTime(GameObject obj, Vector3 start, Vector3 end, float duration)
    {
        float elapsed = 0f;
        obj.transform.position = start;

        while (elapsed < duration)
        {
            obj.transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = end; 
        Destroy(obj);
    }

    private IEnumerator MoveOverTime(GameObject obj, Vector3 start, Vector3 mid, Vector3 end, float durationToMid, float durationToEnd)
    {
        float elapsed = 0f;
        obj.transform.position = start;

        // Step 1: Move from start to mid
        while (elapsed < durationToMid)
        {
            obj.transform.position = Vector3.Lerp(start, mid, elapsed / durationToMid);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = mid;

        // Step 2: Play looping animation for 5 seconds
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("Happy", true);
        }

        yield return new WaitForSeconds(3f);

        animator.SetBool("Happy", false);

        // Step 3: Move from mid to end
        elapsed = 0f;
        while (elapsed < durationToEnd)
        {
            obj.transform.position = Vector3.Lerp(mid, end, elapsed / durationToEnd);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = end;
        Destroy(obj);
    }

}
