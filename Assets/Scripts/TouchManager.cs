using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveTime;
    [SerializeField] private float _tryToMoveSpeed;
    [SerializeField] private float _tryToMoveTime;

    private List<CarPlacer.CarData> _cars = new List<CarPlacer.CarData>();

    private void Start()
    {
        var carPlacer = GetComponent<CarPlacer>();
        _cars = carPlacer.Cars;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Car"))
                {
                    if (CanMove(hit.transform))
                    {
                        _cars.Remove(_cars.FirstOrDefault(car => car.obj == hit.transform.gameObject));
                        StartCoroutine(Move(hit.transform, _moveTime));
                    }
                    else
                        StartCoroutine(TryToMove(hit.transform));
                }
            }
        }
    }

    private bool CanMove(Transform target)
    {
        Vector3 pos1 = target.position;
        Vector3 targetDirection = target.forward;
        float targetLength = 50;

        foreach (var car in _cars.ToList())
        {
            if (car.obj == target.gameObject)
                continue;
            float x = car.x;
            float y = car.y;
            if (car.direction == Direction.Right || car.direction == Direction.Left)
                x = car.x - 0.5f;
            if (car.direction == Direction.Up || car.direction == Direction.Down)
                y = car.y - 0.5f;
            Vector3 pos2 = new Vector3(x * 10, 0, y * 10);
            Vector3 carDirection = GetDirectionVector(car.direction);
            float carLength = car.length * 10;

            Vector3 end1 = pos1 + targetDirection * targetLength;
            Vector3 end2 = pos2 + carDirection * carLength;
            if (targetDirection == Vector3.forward && UpRight(pos1, end1, pos2, end2))
            {
                return false;
            }
            if (targetDirection == Vector3.right && UpRight(pos1, end1, pos2, end2))
            {
                return false;
            }
            if (targetDirection == Vector3.back && RightDown(pos1, end1, pos2, end2))
            {
                return false;
            }
            if (targetDirection == Vector3.left && LeftUp(pos1, end1, pos2, end2))
            {
                return false;
            }
        }
        return true;
    }

    private Vector3 GetDirectionVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Vector3.forward;
            case Direction.Right:
                return Vector3.right;
            case Direction.Down:
                return Vector3.forward;
            case Direction.Left:
                return Vector3.right;
            default:
                return Vector3.zero;
        }
    }

    private bool UpRight(Vector3 pos1, Vector3 end1, Vector3 pos2, Vector3 end2)
    {
        return (pos1.x < end2.x || Math.Abs(pos1.x - end2.x) < 0.01f)
            && (end1.x > pos2.x || Math.Abs(end1.x - pos2.x) < 0.01f)
            && (pos1.z < end2.z || Math.Abs(pos1.z - end2.z) < 0.01f)
            && (end1.z > pos2.z || Math.Abs(end1.z - pos2.z) < 0.01f);
    }

    private bool RightDown(Vector3 pos1, Vector3 end1, Vector3 pos2, Vector3 end2)
    {
        return (pos1.x < end2.x || Math.Abs(pos1.x - end2.x) < 0.01f)
            && (end1.x > pos2.x || Math.Abs(end1.x - pos2.x) < 0.01f)
            && (pos1.z > end2.z || Math.Abs(pos1.z - end2.z) < 0.01f)
            && (end1.z < pos2.z || Math.Abs(end1.z - pos2.z) < 0.01f);
    }

    private bool DownLeft(Vector3 pos1, Vector3 end1, Vector3 pos2, Vector3 end2)
    {
        return (pos1.x > end2.x || Math.Abs(pos1.x - end2.x) < 0.01f)
            && (end1.x < pos2.x || Math.Abs(end1.x - pos2.x) < 0.01f)
            && (pos1.z > end2.z || Math.Abs(pos1.z - end2.z) < 0.01f)
            && (end1.z < pos2.z || Math.Abs(end1.z - pos2.z) < 0.01f);
    }

    private bool LeftUp(Vector3 pos1, Vector3 end1, Vector3 pos2, Vector3 end2)
    {
        return (pos1.x > end2.x || Math.Abs(pos1.x - end2.x) < 0.01f)
            && (end1.x < pos2.x || Math.Abs(end1.x - pos2.x) < 0.01f)
            && (pos1.z < end2.z || Math.Abs(pos1.z - end2.z) < 0.01f)
            && (end1.z > pos2.z || Math.Abs(end1.z - pos2.z) < 0.01f);
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

    private IEnumerator TryToMove(Transform target)
    {
        float forwardTime = 0f;
        float backTime = 0f;

        while (forwardTime < _tryToMoveTime/2)
        {
            target.Translate(Vector3.forward * _tryToMoveSpeed * Time.deltaTime);
            forwardTime += Time.deltaTime;
            yield return null;
        }
        while (backTime < _tryToMoveTime/2)
        {
            target.Translate(Vector3.back * _tryToMoveSpeed * Time.deltaTime);
            backTime += Time.deltaTime;
            yield return null;
        }
    }
}
