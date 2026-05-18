using System;

namespace PokeBID.Models
{
    public class Bid
    {
        // ── Propriétés ────────────────────────────────────────────────────────
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PlacedAt { get; set; }

        /// <summary>Nom affiché du poseur de l'offre (joueur ou nom du bot).</summary>
        public string BidderName { get; set; }

        /// <summary>True = offre placée par le joueur humain.</summary>
        public bool IsPlayer { get; set; }

        // ── Constructeur ──────────────────────────────────────────────────────
        public Bid() { }

        public Bid(int auctionId, decimal amount, string bidderName, bool isPlayer)
        {
            AuctionId = auctionId;
            Amount = amount;
            BidderName = bidderName;
            IsPlayer = isPlayer;
            PlacedAt = DateTime.Now;
        }

        // ── Affichage ─────────────────────────────────────────────────────────
        public override string ToString()
            => $"{PlacedAt:HH:mm:ss} | {BidderName,-15} | {Amount:C}";
    }
}