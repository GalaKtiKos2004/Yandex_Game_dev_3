using UnityEngine;

public class ShipPresenter : Presenter
{
    private Root _init;

    public void Init(Root init)
    {
        _init = init;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Nlo") ||
            collision.gameObject.CompareTag("BigNlo"))
        {
            _init.DisableShip();
        }
    }
}