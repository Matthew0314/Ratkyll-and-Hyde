using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraIntroManager : MonoBehaviour
{
    public CinemachineVirtualCamera panCam1;
    public CinemachineVirtualCamera panCam2;
    public CinemachineVirtualCamera panCam3;

    public Transform cam1TargetPosition;
    public Transform cam2TargetPosition;
    public Transform cam3TargetPosition;

    public GameObject playerCam1;
    public GameObject playerCam2;

    public float moveDuration = 3f;

    void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        yield return MoveCameraTo(panCam1, cam1TargetPosition.position);
        yield return new WaitForSeconds(1f);

        yield return MoveCameraTo(panCam2, cam2TargetPosition.position);
        yield return new WaitForSeconds(1f);

        yield return MoveCameraTo(panCam3, cam3TargetPosition.position);
        yield return new WaitForSeconds(1f);

        // StartSplitScreen();
    }

    IEnumerator MoveCameraTo(CinemachineVirtualCamera vcam, Vector3 targetPos)
    {
        // Raise its priority
        panCam1.Priority = 0;
        panCam2.Priority = 0;
        panCam3.Priority = 0;
        vcam.Priority = 10;

        Transform camTransform = vcam.transform;
        Vector3 startPos = camTransform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            camTransform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        camTransform.position = targetPos;
    }
}
