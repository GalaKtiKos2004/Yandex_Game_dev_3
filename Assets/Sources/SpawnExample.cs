using UnityEngine;
using Asteroids.Model;

public class SpawnExample : MonoBehaviour
{
    [SerializeField] private PresentersFactory _factory;
    [SerializeField] private Root _init;

    private int _index;
    private float _secondsPerIndex = 1f;

    private void Update()
    {
        int newIndex = (int)(Time.time / _secondsPerIndex);

        if (newIndex > _index)
        {
            _index = newIndex;
            OnTick();
        }
    }

    private void OnTick()
    {
        float chance = Random.Range(0, 100);

        if (chance < 20)
        {
            if (_init._bigNlo.Count == 0)
            {
                Nlo nlo = new Nlo(_init.Ship, GetRandomPositionOutsideScreen(), Config.NloSpeed);
                _factory.CreateNlo(nlo);
                _init.AddNlo(nlo, false);
            }
            else
            {
                Nlo nlo = new Nlo(_init._bigNlo[Random.Range(0, _init._bigNlo.Count - 1)],
                    GetRandomPositionOutsideScreen(), Config.NloSpeed);
                _factory.CreateNlo(nlo);
                _init.AddNlo(nlo, false);
            }
        }
        else if (chance < 40)
        {
            if (_init._nlo.Count == 0)
            {
                Nlo nlo = new Nlo(_init.Ship, GetRandomPositionOutsideScreen(), Config.NloSpeed);
                _factory.CreateBigNlo(nlo);
                _init.AddNlo(nlo, true);
            }
            else
            {
                Nlo nlo = new Nlo(_init._nlo[Random.Range(0, _init._nlo.Count - 1)], GetRandomPositionOutsideScreen(),
                    Config.NloSpeed);
                _factory.CreateBigNlo(nlo);
                _init.AddNlo(nlo, true);
            }
        }
        else
        {
            Vector2 position = GetRandomPositionOutsideScreen();
            Vector2 direction = GetDirectionThroughtScreen(position);

            _factory.CreateAsteroid(new Asteroid(position, direction, Config.AsteroidSpeed));
        }
    }

    private Vector2 GetRandomPositionOutsideScreen()
    {
        return Random.insideUnitCircle.normalized + new Vector2(0.5F, 0.5F);
    }

    private static Vector2 GetDirectionThroughtScreen(Vector2 postion)
    {
        return (new Vector2(Random.value, Random.value) - postion).normalized;
    }
}