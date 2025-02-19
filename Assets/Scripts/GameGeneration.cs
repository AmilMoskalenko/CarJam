using UnityEngine;
using UnityEngine.Pool;

public class GameGeneration : MonoBehaviour
{
    [SerializeField] private GameObject _carPrefab;
    public int _width = 5;
    public int _height = 5;



    public ObjectPool<Car> ObjectPool { get; private set; }

    void Start()
    {
        ObjectPool = new ObjectPool<Car>(CreateCar, OnGetCar, OnReleaseCar, OnDestroyCar, true, 10, 20);
        System.Random rnd = new System.Random();
        int x = rnd.Next(0, _width);
        int y = rnd.Next(0, _height);
        for (int i = 0; i < 10; i++)
        {
            //Car car = ObjectPool.Get();
            //car.transform.position = new Vector3(i * 10, 2.3f, 0);
        }
    }

    private Car CreateCar()
    {
        GameObject carObject = Instantiate(_carPrefab);
        return carObject.GetComponent<Car>();
    }

    private void OnGetCar(Car car)
    {
        car.gameObject.SetActive(true);
    }

    private void OnReleaseCar(Car car)
    {
        car.gameObject.SetActive(false);
    }

    private void OnDestroyCar(Car car)
    {
        Destroy(car.gameObject);
    }
}
