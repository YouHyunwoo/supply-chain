using UnityEngine;

public class ResourceCargo : Cargo
{
    public float Value = 0.0f;
    public float Amount = 0.0f;
    public bool isTransported = false;
    private Transform _carrier;

    public int Price => (int)(Value * Amount);
    public bool IsBeingCarried => _carrier != null;

    public void Dissolve()
    {
        _carrier = null;
        Destroy(gameObject);
    }

    public void Follow(Transform target) => _carrier = target;

    public void Unfollow() => _carrier = null;

    public override void StartTransport() => isTransported = true;

    public override void CompleteTransport() => isTransported = false;

    void Update()
    {
        if (isTransported) return;
        if (_carrier != null)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                _carrier.position,
                Time.deltaTime * 10.0f
            );
        }
    }
}