namespace AG.Gameplay.Characters
{
	// TODO: Rename IDamageable to IHealthBarSource
	public interface IDamageable
	{
		float CurrentHealth { get; }
		float MaxHealth { get; }
		float HealthRatio => CurrentHealth / MaxHealth;
	}
}
