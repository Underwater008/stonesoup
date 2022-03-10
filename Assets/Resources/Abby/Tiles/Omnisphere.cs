using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omnisphere : Tile
{
    Material _mat;
    Transform _transform;
    bool _held;
    public override void init()
    {
        _sprite = GetComponentInChildren<SpriteRenderer>();
        updateSpriteSorting();
        _anim = GetComponentInChildren<Animator>();
        _mat = _sprite.material;
        _transform = _sprite.gameObject.GetComponent<Transform>();
        if (hasTag(TileTags.Creature) && GetComponent<Rigidbody2D>() == null)
        {
            _body = gameObject.AddComponent<Rigidbody2D>();
        }
        else
        {
            _body = GetComponent<Rigidbody2D>();
        }
        _mat.SetFloat("_bandsOn", 0);
        _collider = GetComponent<Collider2D>();
        _startHealth = health;
    }

    public override void pickUp(Tile tilePickingUsUp)
    {
        if (!hasTag(TileTags.CanBeHeld))
        {
            return;
        }
        if (_body != null)
        {
            _body.velocity = Vector2.zero;
            _body.bodyType = RigidbodyType2D.Kinematic;
        }
        transform.parent = tilePickingUsUp.transform;
        transform.localPosition = new Vector3(heldOffset.x, heldOffset.y, 0f);
        transform.localRotation = Quaternion.Euler(0, 0, heldAngle);
        removeTag(TileTags.CanBeHeld);
        tilePickingUsUp.tileWereHolding = this;
        _tileHoldingUs = tilePickingUsUp;
        // updateSpriteSorting();
        _held = true;
        _mat.SetFloat("_bandsOn", 1);
        StartCoroutine(animateMaterial());
        StartCoroutine(increaseScale());
    }

    protected IEnumerator animateMaterial()
    {
        while (_held)
        {
            _transform.localScale += new Vector3(.1f, .1f, .1f);
            yield return new WaitForSeconds(1);
            yield return null;
        }
        yield return null;
    }
    public IEnumerator increaseScale()
    {
        if (_held)
        {

            yield return null;
        }
        yield return null;
    }

    public override void dropped(Tile tileDroppingUs)
    {
        if (_tileHoldingUs != tileDroppingUs)
        {
            return;
        }
        if (onTransitionArea())
        {
            return; // Don't allow items to drop on the transition area.
        }

        if (_body != null)
        {
            _body.bodyType = RigidbodyType2D.Dynamic;
        }
        // We move ourselves to the current room when we're dropped
        transform.localRotation = Quaternion.identity;
        transform.parent = tileDroppingUs.transform.parent;
        addTag(TileTags.Wall);
        _tileHoldingUs.tileWereHolding = null;
        _tileHoldingUs = null;
        _collider.isTrigger = false;
        _held = false;
    }


}
