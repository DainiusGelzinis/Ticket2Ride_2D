namespace Game.Tests
{
    // Simplified copy of the Health behaviour without any Unity dependencies
    public class HealthLogic
    {
        private readonly float _startingHealth;
        public float CurrentHealth { get; private set; }

        public HealthLogic(float startingHealth = 10f)
        {
            _startingHealth = startingHealth;
            CurrentHealth = startingHealth;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth = System.Math.Max(CurrentHealth - damage, 0f);
        }

        public void RestoreFullHealth()
        {
            CurrentHealth = _startingHealth;
        }
    }
}
