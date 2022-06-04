using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    [SerializeField] private GameObject mainCharacter;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float height;
    [SerializeField] private float reareDistance;
    private Vector3 cameraOffset;
    private Vector3 currentVector;
    private void Start()
    {
        transform.position = new Vector3(mainCharacter.transform.position.x, mainCharacter.transform.position.y + height, mainCharacter.transform.position.z - reareDistance);
        transform.rotation = Quaternion.LookRotation(mainCharacter.transform.position - transform.position);
    }

    private void SetOffset(Vector3 offset)
    {
        if (offset.z < 0) cameraOffset = offset * 10;
        else if (offset.z > 0) cameraOffset = offset * 3;
        else cameraOffset = offset * 8;
    }

    private void CameraMovement()
    {
        currentVector = new Vector3(mainCharacter.transform.position.x + cameraOffset.x, mainCharacter.transform.position.y + height, (mainCharacter.transform.position.z - reareDistance) + cameraOffset.z);
        transform.position = Vector3.Lerp(transform.position, currentVector, returnSpeed * Time.deltaTime);
    }

    private void Update()
    {
        CameraMovement();
    }
}
