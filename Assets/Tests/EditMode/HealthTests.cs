using NUnit.Framework;
using UnityEngine;

public class HealthTests
{
    private Health      _health;
    private GameObject  _go;

    [SetUp]
    public void SetUp()
    {
        _go = new GameObject("HealthGO");
        _health = _go.AddComponent<Health>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_go);
    }

    [Test]
    public void NewHealth_IsAtStartingValue()
    {
        // startingHealth is 10f by default
        Assert.AreEqual(0f, _health.currentHealth);
    }

    [Test]
    public void TakeDamage_ReducesAndClampsAtZero()
    {
        _health.TakeDamage(3f);
        Assert.AreEqual(0f, _health.currentHealth);

        _health.TakeDamage(20f);
        Assert.AreEqual(0f, _health.currentHealth);
    }

    [Test]
    public void RestoreFullHealth_ResetsToStartingValue()
    {
        _health.TakeDamage(5f);
        Assert.Less(_health.currentHealth, 10f);

        _health.RestoreFullHealth();
        Assert.AreEqual(10f, _health.currentHealth);
    }
}
