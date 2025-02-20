using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _time;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Car"))
                {
                    StartCoroutine(Move(hit.transform, _time));
                }
            }
        }
    }

    private IEnumerator Move(Transform target, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            target.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
