using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarPlacer : MonoBehaviour
{
    [SerializeField] private GameObject _carPrefab;
    [SerializeField] private List<Material> _carMaterials;
    [SerializeField] private GameObject _busPrefab;
    [SerializeField] private List<Material> _busMaterials;
    [SerializeField] private Transform _carsParent;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _carCount;

    public List<CarData> Cars => _cars;
    
    private List<CarData> _cars = new List<CarData>();
    public int Width {get => _width; set => _width = value;}
    public int Height { get => _height; set => _height = value; }
    public int CarCount { get => _carCount; set => _carCount = value; }
    private int[,] _grid;

    private void Start()
    {
        _grid = new int[Width, Height];
        PlaceCars();
    }

    private void PlaceCars()
    {
        System.Random rand = new System.Random();

        for (int i = 0; i < CarCount; i++)
        {
            bool placed = false;
            while (!placed)
            {
                int x = rand.Next(0, Width);
                int y = rand.Next(0, Height);
                Direction dir = (Direction)rand.Next(0, 4);
                int length = rand.Next(2, 4);

                if (CanPlaceCar(x, y, dir, length))
                {
                    PlaceCar(x, y, dir, length);
                    placed = true;
                }
            }
        }
    }

    private bool CanPlaceCar(int x, int y, Direction dir, int length)
    {
        if (dir == Direction.Right || dir == Direction.Left)
        {
            if (x + length > Width)
                return false;
            for (int i = 0; i < length; i++)
                if (_grid[x + i, y] == 1)
                    return false;
        }
        else
        {
            if (y + length > Height)
                return false;
            for (int i = 0; i < length; i++)
                if (_grid[x, y + i] == 1)
                    return false;
        }

        if (dir == Direction.Right && _cars.Any(c => c.direction == Direction.Left && c.y == y))
            return false;
        if (dir == Direction.Left && _cars.Any(c => c.direction == Direction.Right && c.y == y))
            return false;
        if (dir == Direction.Up && _cars.Any(c => c.direction == Direction.Down && c.x == x))
            return false;
        if (dir == Direction.Down && _cars.Any(c => c.direction == Direction.Up && c.x == x))
            return false;

        return true;
    }

    private void PlaceCar(int x, int y, Direction dir, int length)
    {
        if (dir == Direction.Right || dir == Direction.Left)
            for (int i = 0; i < length; i++)
                _grid[x + i, y] = 1;
        else
            for (int i = 0; i < length; i++)
                _grid[x, y + i] = 1;

        InstantiateCar(x, y, dir, length);
    }

    private void InstantiateCar(int carX, int carY, Direction carDirection, int carLength)
    {
        float x = carX;
        float y = carY;
        if (carDirection == Direction.Right || carDirection == Direction.Left)
            x = carX + ((float)carLength / 2) - 0.5f;
        else
            y = carY + ((float)carLength / 2) - 0.5f;
        Vector3 position = new Vector3(x * 10, 0, y * 10);
        Quaternion rotation = Quaternion.Euler(0, (int)carDirection * 90, 0);
        var newCar = Instantiate(carLength == 2 ? _carPrefab : _busPrefab, position, rotation, _carsParent);
        System.Random rand = new System.Random();
        if (carLength == 2)
            newCar.GetComponentInChildren<Renderer>().material = _carMaterials[rand.Next(0, _carMaterials.Count)];
        else
        {
            var busMaterial = newCar.GetComponentInChildren<Renderer>().materials;
            busMaterial[1] =  _busMaterials[rand.Next(0, _busMaterials.Count)];
            newCar.GetComponentInChildren<Renderer>().materials = busMaterial;
        }
        CarData car = new CarData(carX, carY, carDirection, carLength, newCar);
        _cars.Add(car);
    }

    public class CarData
    {
        public int x, y, length;
        public Direction direction;
        public GameObject obj;

        public CarData(int x, int y, Direction direction, int length, GameObject obj)
        {
            this.x = x;
            this.y = y;
            this.direction = direction;
            this.length = length;
            this.obj = obj;
        }
    }
}

public enum Direction
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}
