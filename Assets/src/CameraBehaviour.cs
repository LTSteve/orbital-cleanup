using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private float lead = 1f;
    [SerializeField]
    private float transition = 0.2f;

    private Vector2 leadLocation = Vector2.zero;
    private Vector2 leadTargetLocation = Vector2.zero;

    [SerializeField]
    private Vector2 center = Vector2.zero;
    [SerializeField]
    public Vector2 MaxBounds;

    [SerializeField]
    private float minZoom = 4f;

    private new Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();

        var listeners = Transform.FindObjectsOfType<AudioListener>();
        if(listeners.Length == 1)
        {
            GetComponent<AudioListener>().enabled = true;
        }
    }

    public void OnRightStick(InputAction.CallbackContext value)
    {
        leadTargetLocation = value.ReadValue<Vector2>() * lead;
    }

    private void Update()
    {
        var target = PlayerController.Instance.transform;

        leadLocation = Vector2.Lerp(leadLocation, leadTargetLocation, transition * Time.deltaTime);
        var leadPos = target.position + (Vector3)leadLocation;
        transform.position = new Vector3(
            Mathf.Clamp(leadPos.x, -MaxBounds.x, MaxBounds.x), 
            Mathf.Clamp(leadPos.y, -MaxBounds.y, MaxBounds.y),
            transform.position.z);

        camera.orthographicSize = Mathf.Clamp(((Vector2)transform.position - center).magnitude, minZoom, 100f);
    }
}
