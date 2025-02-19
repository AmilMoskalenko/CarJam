using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarPlacer : MonoBehaviour
{
    [SerializeField] private GameObject _carPrefab;
    [SerializeField] private GameObject _busPrefab;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _carCount;

    private int[,] grid;
    private List<CarData> cars = new List<CarData>();

    private enum Direction { Up = 0, Right = 1, Down = 2, Left = 3 }

    private void Start()
    {
        grid = new int[_width, _height];
        PlaceCars();
    }

    private void PlaceCars()
    {
        System.Random rand = new System.Random();

        for (int i = 0; i < _carCount; i++)
        {
            bool placed = false;
            while (!placed)
            {
                int x = rand.Next(0, _width);
                int y = rand.Next(0, _height);
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
            if (x + length > _width)
                return false;
            for (int i = 0; i < length; i++)
                if (grid[x + i, y] == 1)
                    return false;
        }
        else
        {
            if (y + length > _height)
                return false;
            for (int i = 0; i < length; i++)
                if (grid[x, y + i] == 1)
                    return false;
        }

        if (dir == Direction.Right && cars.Any(c => c.direction == Direction.Left && c.y == y))
            return false;
        if (dir == Direction.Left && cars.Any(c => c.direction == Direction.Right && c.y == y))
            return false;
        if (dir == Direction.Up && cars.Any(c => c.direction == Direction.Down && c.x == x))
            return false;
        if (dir == Direction.Down && cars.Any(c => c.direction == Direction.Up && c.x == x))
            return false;

        return true;
    }

    private void PlaceCar(int x, int y, Direction dir, int length)
    {
        CarData car = new CarData(x, y, dir, length);
        cars.Add(car);
        
        if (dir == Direction.Right || dir == Direction.Left)
            for (int i = 0; i < length; i++)
                grid[x + i, y] = 1;
        else
            for (int i = 0; i < length; i++)
                grid[x, y + i] = 1;

        InstantiateCar(car);
    }

    private void InstantiateCar(CarData car)
    {
        float x = car.x;
        float y = car.y;
        if (car.direction == Direction.Right || car.direction == Direction.Left)
            x = car.x + ((float)car.length / 2) - 0.5f;
        else
            y = car.y + ((float)car.length / 2) - 0.5f;
        Vector3 position = new Vector3(x * 10, 0, y * 10);
        Quaternion rotation = Quaternion.Euler(0, (int)car.direction * 90, 0);
        Instantiate(car.length == 2 ? _carPrefab : _busPrefab, position, rotation);
    }

    private class CarData
    {
        public int x, y, length;
        public Direction direction;

        public CarData(int x, int y, Direction direction, int length)
        {
            this.x = x;
            this.y = y;
            this.direction = direction;
            this.length = length;
        }
    }
}
