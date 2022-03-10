using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbbyPlayer : Player
{
    void Awake()
    {
        _instance = this;
    }
    void OnDestroy()
    {
        _instance = null;
    }

    void Update()
    {

        if (GameManager.instance.gameIsOver)
        {
            return;
        }

        // Update our aim direction
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = (mousePosition - (Vector2)transform.position).normalized;
        aimDirection = toMouse;

        // Update our invincibility frame counter.
        if (_iFrameTimer > 0)
        {
            _iFrameTimer -= Time.deltaTime;
            _sprite.enabled = !_sprite.enabled;
            if (_iFrameTimer <= 0)
            {
                _sprite.enabled = true;
            }
        }

        // If we press space, we're attempting to either pickup, drop, or switch items.
        //Abby addition: or we are giving stuff to a town.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool pickedUpOrDroppedItem = false;


            // First, drop the item we're holding
            if (tileWereHolding != null)
            {
                // Keep track of the fact that we just dropped this item so we don't pick it up again.
                _lastTileWeHeld = tileWereHolding;
                // Put it at out feet
                tileWereHolding.dropped(this);

                // If we're no longer holding an item, we successfully dropped it.
                if (tileWereHolding == null)
                {
                    pickedUpOrDroppedItem = true;
                }
            }


            // If we successully dropped the item
            if (tileWereHolding == null)
            {
                // Check to see if we're on top of an item that can be held
                int numObjectsFound = _body.Cast(Vector2.zero, _maybeRaycastResults);
                for (int i = 0; i < numObjectsFound && i < _maybeRaycastResults.Length; i++)
                {
                    RaycastHit2D result = _maybeRaycastResults[i];

                    Tile tileHit = result.transform.GetComponent<Tile>();
                    // Ignore the tile we just dropped
                    if (tileHit == null || tileHit == _lastTileWeHeld)
                    {
                        continue;
                    }
                    if (tileHit.hasTag(TileTags.CanBeHeld))
                    {
                        tileHit.pickUp(this);
                        if (tileWereHolding != null)
                        {
                            pickedUpOrDroppedItem = true;
                            break;
                        }
                    }
                }
            }

            if (pickedUpOrDroppedItem)
            {
                AudioManager.playAudio(pickupDropSound);
            }

            // Finally, clear the last tile we held so we can pick it up again next frame if we want to
            _lastTileWeHeld = null;
        }

        // If we click the mouse, we try to use whatever item we're holding.
        if (Input.GetMouseButtonDown(0))
        {
            if (tileWereHolding != null)
            {
                tileWereHolding.useAsItem(this);
            }
        }

        updateSpriteSorting();
    }

    void FixedUpdate()
    {
        if (GameManager.instance.gameIsOver)
        {
            return;
        }

        // Update our aim direction
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = (mousePosition - (Vector2)transform.position).normalized;
        aimDirection = toMouse;

        // Update our invincibility frame counter.
        if (_iFrameTimer > 0)
        {
            _iFrameTimer -= Time.deltaTime;
            _sprite.enabled = !_sprite.enabled;
            if (_iFrameTimer <= 0)
            {
                _sprite.enabled = true;
            }
        }

        // If we press space, we're attempting to either pickup, drop, or switch items.
        //Abby addition: or we are giving stuff to a town.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool pickedUpOrDroppedItem = false;

            //town check
            int numObjectsFound = _body.Cast(Vector2.zero, _maybeRaycastResults);
            for (int i = 0; i < numObjectsFound && i < _maybeRaycastResults.Length; i++)
            {
                RaycastHit2D result = _maybeRaycastResults[i];
                if (result.transform.GetComponent<AbbyTile>().Equals(null))
                    return;
                AbbyTile tileHit = result.transform.GetComponent<AbbyTile>();
                // Ignore the tile we just dropped
                if (tileHit == null || tileHit == tileWereHolding)
                {
                    continue;
                }
                if (tileHit.hasTag(TileTags.Merchant))
                {
                    tileHit.interact(this);
                    return;
                }
            }

            // First, drop the item we're holding
            if (tileWereHolding != null)
            {
                // Keep track of the fact that we just dropped this item so we don't pick it up again.
                _lastTileWeHeld = tileWereHolding;
                // Put it at out feet
                tileWereHolding.dropped(this);

                // If we're no longer holding an item, we successfully dropped it.
                if (tileWereHolding == null)
                {
                    pickedUpOrDroppedItem = true;
                }
            }


            // If we successully dropped the item
            if (tileWereHolding == null)
            {
                // Check to see if we're on top of an item that can be held
                numObjectsFound = _body.Cast(Vector2.zero, _maybeRaycastResults);
                for (int i = 0; i < numObjectsFound && i < _maybeRaycastResults.Length; i++)
                {
                    RaycastHit2D result = _maybeRaycastResults[i];
                    Tile tileHit = result.transform.GetComponent<Tile>();
                    // Ignore the tile we just dropped
                    if (tileHit == null || tileHit == _lastTileWeHeld)
                    {
                        continue;
                    }
                    if (tileHit.hasTag(TileTags.CanBeHeld))
                    {
                        tileHit.pickUp(this);
                        if (tileWereHolding != null)
                        {
                            pickedUpOrDroppedItem = true;
                            break;
                        }
                    }
                }
            }

            if (pickedUpOrDroppedItem)
            {
                AudioManager.playAudio(pickupDropSound);
            }

            // Finally, clear the last tile we held so we can pick it up again next frame if we want to
            _lastTileWeHeld = null;
        }

        // If we click the mouse, we try to use whatever item we're holding.
        if (Input.GetMouseButtonDown(0))
        {
            if (tileWereHolding != null)
            {
                tileWereHolding.useAsItem(this);
            }
        }

        updateSpriteSorting();
    }

    // The only trigger collision the player cares about is the exit.
    void OnTriggerEnter2D(Collider2D other)
    {
        Tile otherTile = other.transform.GetComponent<Tile>();
        if (otherTile != null)
        {
            if (otherTile.hasTag(TileTags.Exit))
            {
                AudioManager.playAudio(exitSound);
                GameManager.instance.playerJustWon();
            }
        }
    }

}
