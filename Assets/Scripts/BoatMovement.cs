using UnityEngine;

public sealed class BoatMovement : MonoBehaviour
{
    [SerializeField] private SailController _sailController;
    [SerializeField] private Vector3 _windVelocity = new Vector3(0f, 0f, 2f);
    [SerializeField, Min(0f)] private float _sailForceCoefficient = 0.02f;
    [SerializeField, Min(0f)] private float _waterDrag = 0.2f;
    [SerializeField, Min(0f)] private float _keelDrag = 2f;

    private Vector3 _velocity;

    private void Start()
    {
        if (_sailController == null)
        {
            Debug.LogError("Assign the SailController reference in the Inspector.", this);
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        UpdateVelocityFromSail();
        ApplyWaterDrag();
        ApplyKeelDrag();
        MoveBoat();
    }

    private void UpdateVelocityFromSail()
    {
        Vector3 sailNormal = _sailController.MastPivot.forward;

        sailNormal.Normalize();

        float windSpeedAcrossSail = Vector3.Dot(_windVelocity, sailNormal);

        Vector3 sailAcceleration = sailNormal * windSpeedAcrossSail * Mathf.Abs(windSpeedAcrossSail) * _sailController.SailOpenness * _sailForceCoefficient;

        _velocity += sailAcceleration * Time.deltaTime;
    }

    private void ApplyWaterDrag()
    {
        float waterDamping = Mathf.Exp(-_waterDrag * Time.deltaTime);
        _velocity *= waterDamping;
    }

    private void ApplyKeelDrag()
    {
        Vector3 sidewaysVelocity = Vector3.Project(_velocity, transform.right);
        float keelDamping = Mathf.Exp(-_keelDrag * Time.deltaTime);

        _velocity -= sidewaysVelocity * (1f - keelDamping);
    }

    private void MoveBoat()
    {
        transform.position += _velocity * Time.deltaTime;
    }
}
