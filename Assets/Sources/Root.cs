using System.Collections.Generic;
using Asteroids.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Root : MonoBehaviour
{
    [SerializeField] private ShipPresenter _shipPresenter;
    [SerializeField] private Camera _camera;
    [SerializeField] private PresentersFactory _factory;
    [SerializeField] private EndGameWindowPresenter _endGameWindow;

    private Ship _shipModel;
    private ShipInputRouter _shipInputRouter;
    private DefaultGun _baseGun;
    private LaserGun _laserGun;
    private LaserGunRollback _laserGunRollback;

    public Ship Ship => _shipModel;
    public LaserGun LaserGun => _laserGun;
    public LaserGunRollback LaserGunRollback => _laserGunRollback;

    public List<Nlo> _nlo = new List<Nlo>();
    public List<Nlo> _bigNlo = new List<Nlo>();

    public void DisableShip()
    {
        _shipModel.GettingHurt();
        if (_shipModel._health == 0)
        {
            OnDisable();
        }
    }

    public void AddNlo(Nlo nlo, bool big)
    {
        if (big)
            _bigNlo.Add(nlo);
        else
            _nlo.Add(nlo);
    }

    public void RemoveNlo(int index, bool big)
    {
        if (big)
            _bigNlo.RemoveAt(index);
        else
            _nlo.RemoveAt(index);
    }

    private void Awake()
    {
        _shipModel = new Ship(new Vector2(0.5f, 0.5f), 0);

        _baseGun = new DefaultGun(_shipModel);
        _laserGun = new LaserGun(_shipModel, 10);

        _shipInputRouter = new ShipInputRouter(_shipModel)
            .BindGunToFirstSlot(_baseGun)
            .BindGunToSecondSlot(_laserGun);

        _shipPresenter.Init(_shipModel, _camera);
        _shipPresenter.Init(this);

        _laserGunRollback = new LaserGunRollback(_laserGun, Config.LaserCooldown);
    }

    private void OnEnable()
    {
        _shipInputRouter.OnEnable();

        _baseGun.Shot += OnShot;
        _laserGun.Shot += OnShot;
        _shipModel.Destroying += OnShipDestroying;
    }

    private void OnDisable()
    {
        _shipInputRouter.OnDisable();

        _baseGun.Shot -= OnShot;
        _laserGun.Shot -= OnShot;
        _shipModel.Destroying -= OnShipDestroying;
        
        OnShipDestroying();
    }

    private void Update()
    {
        _shipInputRouter.Update();
        _laserGunRollback.Tick(Time.deltaTime);
    }

    private void OnShot(Bullet bullet)
    {
        _factory.CreateBullet(bullet);
    }

    private void OnShipDestroying()
    {
        _endGameWindow.Show(0, () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }
}
