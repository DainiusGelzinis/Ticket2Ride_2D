using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Game.Tests
{
    [TestClass]
    public class HealthTests
    {
        [TestMethod]
        public void Health_Starts_With_StartingValue()
        {
            var health = new HealthLogic();
            Assert.AreEqual(10f, health.CurrentHealth);
        }

        [TestMethod]
        public void TakeDamage_Reduces_Health()
        {
            var health = new HealthLogic();
            health.TakeDamage(3f);
            Assert.AreEqual(7f, health.CurrentHealth);
        }

        [TestMethod]
        public void TakeDamage_Clamps_To_Zero()
        {
            var health = new HealthLogic();
            health.TakeDamage(15f);
            Assert.AreEqual(0f, health.CurrentHealth);
        }

        [TestMethod]
        public void RestoreFullHealth_Resets_To_Starting()
        {
            var health = new HealthLogic();
            health.TakeDamage(5f);
            health.RestoreFullHealth();
            Assert.AreEqual(10f, health.CurrentHealth);
        }
    }
}
