using System;

namespace PokeBID.Models
{
    /// <summary>
    /// Représente un bot participant aux enchères de manière automatique.
    /// </summary>
    public class Bot
    {
        // ── Propriétés ────────────────────────────────────────────────────────
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal MaxBudget { get; set; }   // plafond maximum que le bot accepte de miser
        public decimal Aggression { get; set; }  // 0.0 (passif) à 1.0 (agressif)

        private static readonly Random _rng = new Random();

        // ── Constructeur ──────────────────────────────────────────────────────
        public Bot() { }

        public Bot(int id, string name, decimal maxBudget, decimal aggression)
        {
            Id = id;
            Name = name;
            MaxBudget = maxBudget;
            Aggression = aggression;
        }

        // ── Logique de mise ───────────────────────────────────────────────────

        /// <summary>
        /// Retourne vrai si le bot décide d'enchérir sur la mise actuelle.
        /// La probabilité est proportionnelle à son niveau d'agressivité.
        /// </summary>
        public bool WantsToBid(decimal currentBid)
        {
            if (currentBid >= MaxBudget) return false;

            // Probabilité d'enchérir basée sur l'agressivité
            double roll = _rng.NextDouble();
            return roll < (double)Aggression;
        }

        /// <summary>
        /// Calcule le montant de la prochaine mise du bot.
        /// Surenchère de 1 à 20 % au-dessus de la mise courante, dans son budget.
        /// </summary>
        public decimal ComputeBid(decimal currentBid)
        {
            // Surenchère aléatoire entre 1 % et 20 %
            double pct = 0.01 + _rng.NextDouble() * 0.19;
            decimal bid = currentBid + Math.Round(currentBid * (decimal)pct, 0);

            // On s'assure de ne pas dépasser le budget
            return Math.Min(bid, MaxBudget);
        }

        /// <summary>
        /// Délai aléatoire (en ms) avant que le bot réagisse.
        /// </summary>
        public int GetReactionDelayMs()
        {
            // Entre 1 et 5 secondes selon l'agressivité (bot agressif = réaction rapide)
            double minMs = 1000;
            double maxMs = 5000 - (double)Aggression * 3000;   // agressif → 2 000 ms max
            return _rng.Next((int)minMs, (int)Math.Max(minMs + 500, maxMs));
        }

        public override string ToString() => Name;
    }
}