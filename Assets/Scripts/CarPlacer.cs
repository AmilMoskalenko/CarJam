using System.Collections.Generic;
using UnityEngine;

public class CarPlacer : MonoBehaviour
{
    public int width = 6;  // Ширина парковки
    public int height = 6; // Высота парковки
    public int carCount = 10; // Количество машин
    public GameObject carPrefab; // Префаб машины
    public GameObject longCarPrefab;

    private int[,] grid; // 0 - пусто, 1 - занято
    private List<CarData> cars = new List<CarData>();

    private enum Direction { Horizontal, Vertical }

    private void Start()
    {
        grid = new int[width, height];
        PlaceCars();
    }

    private void PlaceCars()
    {
        System.Random rand = new System.Random();

        for (int i = 0; i < carCount; i++)
        {
            bool placed = false;
            while (!placed)
            {
                int x = rand.Next(0, width);
                int y = rand.Next(0, height);
                Direction dir = (Direction)rand.Next(0, 2);
                int length = rand.Next(2, 4); // Длина машины: 2 или 3 клетки

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
        if (dir == Direction.Horizontal)
        {
            if (x + length > width)
            {
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                if (grid[x + i, y] == 1)
                {
                    return false;
                }
            }
        }
        else
        {
            if (y + length > height)
            {
                return false;
            }
            for (int i = 0; i < length; i++)
            {
                if (grid[x, y + i] == 1)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void PlaceCar(int x, int y, Direction dir, int length)
    {
        CarData car = new CarData(x, y, dir, length);
        cars.Add(car);
        
        if (dir == Direction.Horizontal)
        {
            for (int i = 0; i < length; i++)
            {
                grid[x + i, y] = 1;
            }   
        }
        else
        {
            for (int i = 0; i < length; i++)
            {
                grid[x, y + i] = 1;
            }
        }
        InstantiateCar(car);
    }

    private void InstantiateCar(CarData car)
    {
        float x = car.x;
        float y = car.y;
        if (car.direction == Direction.Horizontal)
        {
            x = car.x + ((float)car.length / 2) - 0.5f;
        }
        else
        {
            y = car.y + ((float)car.length / 2) - 0.5f;
        }
        Vector3 position = new Vector3(x * 10, 2.3f, y * 10);
        Quaternion rotation = car.direction != Direction.Horizontal ? Quaternion.identity : Quaternion.Euler(0, 90, 0);
        Instantiate(car.length == 2 ? carPrefab : longCarPrefab, position, rotation);
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
