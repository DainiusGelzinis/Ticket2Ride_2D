using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayerMovementPlayModeTests
{
    GameObject         _go;
    PlayerController   _pc;
    Rigidbody2D        _rb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // 1) Create a clean GameObject
        _go = new GameObject("Player");
        _rb = _go.AddComponent<Rigidbody2D>();
        _go.AddComponent<BoxCollider2D>();

        // 2) Add your PlayerController
        _pc = _go.AddComponent<PlayerController>();

        // Ensure default inertia is zero
        _pc.SetInertiaForce(0f);

        // Wait a frame so Awake() runs
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(_go);
        // wait a frame so Unity cleans it up
        yield return null;
    }

    [UnityTest]
    public IEnumerator NoInput_NoHorizontalVelocity()
    {
        // No keys pressed, default inertia=0 => should stay still
        yield return new WaitForSeconds(0.1f);
        Assert.AreEqual(0f, _rb.linearVelocity.x, 1e-3f,
            "With zero input & zero inertia, player should not drift.");
    }

    [UnityTest]
    public IEnumerator InertiaForce_DrivesConstantDrift()
    {
        // Apply a small constant inertia
        _pc.SetInertiaForce(2f);

        // Wait two frames to let Update() apply it
        yield return null;
        yield return null;

        // Expect velocity.x == -inertia (movement to the left)
        Assert.AreEqual(-2f, _rb.linearVelocity.x, 0.1f,
            "InertiaForce should produce a steady drift to the left.");
    }
}
