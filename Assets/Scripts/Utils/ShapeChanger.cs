using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeChanger : MonoBehaviour
{
    [SerializeField] public bool changeProgressively;
    [SerializeField] private float timeToChangeShape;
    [SerializeField] private bool changeDamage;
    [SerializeField] private float damageModifier;
    private SkinnedMeshRenderer _skMeshRenderer;
    private Mesh _mesh;
    private int _blendShapeCount;


    // Start is called before the first frame update
    void Start()
    {
        _skMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        _mesh = _skMeshRenderer.sharedMesh;
        _blendShapeCount = _mesh.blendShapeCount;
    }

    public void ChangeShape(int index)
    {
        if (index < _blendShapeCount && index >= 0)
        {
            if (changeDamage)
                GetComponent<Weapon>().ChangeDamage(damageModifier);
            if (!changeProgressively)
                _skMeshRenderer.SetBlendShapeWeight(index, 1f);
            else
                StartCoroutine(StartChangingShape(index));
        }
    }


    private IEnumerator StartChangingShape(int index)
    {
        float currentTime = Time.unscaledTime;
        float targetTime = currentTime + timeToChangeShape;
        float blendShapeWeight = 0f;
        while (currentTime < targetTime)
        {
            blendShapeWeight += 1 / Time.fixedDeltaTime;
            _skMeshRenderer.SetBlendShapeWeight(index, blendShapeWeight);
            currentTime += Time.fixedDeltaTime;
            yield return null;
        }
    }
}