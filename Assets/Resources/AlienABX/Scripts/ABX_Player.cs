using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABX_Player : Player
{
	public bool hasGasMask = false;
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

		if (Input.GetKeyDown(KeyCode.Space))
		{
			// Check to see if we're on top of an item that can be held
			int numObjectsFound = _body.Cast(Vector2.zero, _maybeRaycastResults);
			int canPickUpItmes = 0;
			for (int i = 0; i < numObjectsFound && i < _maybeRaycastResults.Length; i++)
			{
				RaycastHit2D result = _maybeRaycastResults[i];
				ABX_Tile tileHit = result.transform.GetComponent<ABX_Tile>();
				if (tileHit.hasTag(TileTags.CanBeHeld))
					canPickUpItmes++;
				if (tileHit.hasTag(TileTags.CanBeHeld))
				{
					tileHit.pickUp(this);
					AudioManager.playAudio(pickupDropSound);
					break;
				}
			}
			if (canPickUpItmes == 0)
            {
				if (tileWereHolding != null)
                {
					tileWereHolding.dropped(this);
					AudioManager.playAudio(pickupDropSound);
				}
			}
		}
			/*// If we successully dropped the item
			if (tileWereHolding == null)
			{
				// Check to see if we're on top of an item that can be held
				int numObjectsFound = _body.Cast(Vector2.zero, _maybeRaycastResults);
				for (int i = 0; i < numObjectsFound && i < _maybeRaycastResults.Length; i++)
				{
					
					RaycastHit2D result = _maybeRaycastResults[i];
					Tile tileHit = result.transform.GetComponent<Tile>();
					*//*
					// Ignore the tile we just dropped
					if (tileHit == null || tileHit == _lastTileWeHeld)
					{
						continue;
					}
					*//*
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

		if (Input.GetKeyDown(KeyCode.Mouse1))
        {
			bool pickedUpOrDroppedItem = false;

			// First, drop the item we're holding
			if (tileWereHolding != null)
			{
				// Keep track of the fact that we just dropped this item so we don't pick it up again.
				//_lastTileWeHeld = tileWereHolding;
				// Put it at out feet
				tileWereHolding.dropped(this);

				// If we're no longer holding an item, we successfully dropped it.
				if (tileWereHolding == null)
				{
					pickedUpOrDroppedItem = true;
				}

				if (pickedUpOrDroppedItem)
				{
					AudioManager.playAudio(pickupDropSound);
				}
			}
		}*/

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

    public override void takeDamage(Tile tileDamagingUs, int amount, DamageType damageType)
    {
		if (damageType == DamageType.Explosive)
        {
			if (hasGasMask)
				return;
        }
		base.takeDamage(tileDamagingUs, amount, damageType);
	}
}
