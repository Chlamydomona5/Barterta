namespace Barterta.Weapon
{
    public class MeleeAttribute
    {
        public int Damage = 1;
        public float CriticalRate = .1f;
        public int CriticalDamage = 1;
        public float AttackSpeed = 1;
        public float KnockForce = 1;

        public static MeleeAttribute operator +(MeleeAttribute a, MeleeAttribute b)
        {
            return new MeleeAttribute()
            {
                Damage = a.Damage + b.Damage,
                CriticalRate = a.CriticalRate + b.CriticalRate,
                CriticalDamage = a.CriticalDamage + b.CriticalDamage,
                AttackSpeed = a.AttackSpeed + b.AttackSpeed,
                KnockForce = a.KnockForce + b.KnockForce
            };
        }
    }
}