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

    public GameObject mainCam;

    public GameObject playerCam1;
    public GameObject playerCam2;

    [SerializeField] PotGameManager potGameManager;

    public float moveDuration = 3f;

    void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        yield return MoveCameraTo(panCam1, cam1TargetPosition.position, cam1TargetPosition.rotation);
        yield return new WaitForSeconds(0.1f);

        yield return MoveCameraTo(panCam2, cam2TargetPosition.position, cam2TargetPosition.rotation);
        yield return new WaitForSeconds(0.1f);

        yield return MoveCameraTo(panCam3, cam3TargetPosition.position, cam3TargetPosition.rotation);
        yield return new WaitForSeconds(0.1f);

        mainCam.SetActive(false);
        panCam1.gameObject.SetActive(false);
        panCam2.gameObject.SetActive(false);
        panCam3.gameObject.SetActive(false);

        potGameManager.StartGame();
    }


    IEnumerator MoveCameraTo(CinemachineVirtualCamera vcam, Vector3 targetPos, Quaternion targetRot)
    {
        Debug.LogError("MOVING CAMERA WITH ROTATION");

        // Reset all priorities
        panCam1.Priority = 0;
        panCam2.Priority = 0;
        panCam3.Priority = 0;
        vcam.Priority = 10;

        Transform camTransform = vcam.transform;
        Vector3 startPos = camTransform.position;
        Quaternion startRot = camTransform.rotation;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            camTransform.position = Vector3.Lerp(startPos, targetPos, t);
            camTransform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        camTransform.position = targetPos;
        camTransform.rotation = targetRot;
    }

}
