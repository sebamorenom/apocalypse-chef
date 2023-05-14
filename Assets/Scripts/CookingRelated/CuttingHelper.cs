using UnityEngine;


public class CuttingHelper : MonoBehaviour
{
    [SerializeField] private float minCuttingSpeedThreshold;
    [SerializeField] [Range(0, 1f)] private float precisionForCut = 0.7f;
    private Ingredient foodToCut;

    private Rigidbody _rb;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody>();
    }

    /*private void OnTriggerEnter(Collider other)
    {
        Ingredient tryIng;

        if (other.TryGetComponent<Ingredient>(out tryIng) && tryIng != foodToCut)
        {
            foodToCut = other.GetComponent<Ingredient>();
            PrepareToCut(other);
            return;
        }

        if (foodToCut != null)
        {
            if (other.CompareTag("Blade") && -_transform.InverseTransformVector(other.attachedRigidbody.velocity).y >
                minCuttingSpeedThreshold)
            {
                foodToCut.cuttingHealth = Mathf.Max(foodToCut.cuttingHealth - 20f, 0f);
                if (foodToCut.cuttingHealth == 0)
                {
                    Instantiate(foodToCut.cutIngredient, transform.position, Quaternion.identity);
                    Destroy(foodToCut.gameObject);
                    foodToCut = null;
                }
            }
        }
    }

    private float GetVelocitySum(Rigidbody bladeRigidbody)
    {
        Rigidbody parentRigidbody = transform.parent.GetComponent<Rigidbody>();
        //Calculate the food velocity in the blade local space
        Vector3 bladeSpeedInCuttingLocal = _transform.InverseTransformVector(bladeRigidbody.velocity);
        //Calculate the CuttingHelper velocity in its own local space
        Vector3 cuttingSpeedLocal = _transform.InverseTransformVector(_rb.velocity);
        float bladeForce = bladeSpeedInCuttingLocal.y * bladeRigidbody.mass;
        float cuttingHelperForce = cuttingSpeedLocal.y * _rb.mass;
        float impactForce = Mathf.Abs(cuttingHelperForce - bladeForce);
        print(impactForce);
        return impactForce;
    }

    private void PrepareToCut(Collider other)
    {
        //other.transform.position = transform.position;
        other.attachedRigidbody.velocity = Vector3.zero;
        other.attachedRigidbody.isKinematic = true;
        //other.enabled = false;
        other.gameObject.layer = LayerMask.NameToLayer("FoodToCut");
    }

    private void TakeOut(Collider other)
    {
        other.attachedRigidbody.isKinematic = false;
        other.enabled = true;
        other.gameObject.layer = LayerMask.NameToLayer("Food");
        foodToCut = null;
    }

    private void OnTriggerStay(Collider other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
        Ingredient tryIng;
        if (other.TryGetComponent<Ingredient>(out tryIng))
        {
            if (foodToCut == tryIng)
            {
                TakeOut(other);
            }
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        Ingredient tryIng;
        if (collision.collider.TryGetComponent(out tryIng) && tryIng.canBeCut)
        {
            tryIng.ActivateCuttingMode();
        }
    }
}