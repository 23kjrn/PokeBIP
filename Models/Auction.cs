using System;
using System.Collections.Generic;
using System.Linq;

namespace PokeBID.Models
{
    /// <summary>
    /// Représente une enchère sur une carte Pokémon.
    /// </summary>
    public class Auction
    {
        // ── Statuts possibles ─────────────────────────────────────────────────
        public enum AuctionStatus { Open, Closed }

        // ── Propriétés ────────────────────────────────────────────────────────
        public int Id { get; set; }
        public Card Card { get; set; }
        public decimal StartPrice { get; set; }
        public DateTime StartTime { get; set; }
        public int DurationSec { get; set; }   // durée en secondes
        public AuctionStatus Status { get; set; }

        /// <summary>Liste de toutes les mises placées sur cette enchère.</summary>
        public List<Bid> Bids { get; private set; } = new List<Bid>();

        // ── Propriétés calculées ──────────────────────────────────────────────

        /// <summary>Meilleure offre actuelle (ou prix de départ s'il n'y a aucune mise).</summary>
        public decimal CurrentBid
            => Bids.Count > 0 ? Bids.Max(b => b.Amount) : StartPrice;

        /// <summary>Mise la plus haute (ou null si aucune).</summary>
        public Bid HighestBid
            => Bids.Count > 0 ? Bids.OrderByDescending(b => b.Amount).First() : null;

        /// <summary>Temps restant avant la fin.</summary>
        public TimeSpan TimeRemaining
        {
            get
            {
                TimeSpan elapsed = DateTime.Now - StartTime;
                TimeSpan total = TimeSpan.FromSeconds(DurationSec);
                return elapsed > total ? TimeSpan.Zero : total - elapsed;
            }
        }

        /// <summary>True si l'enchère est encore en cours.</summary>
        public bool IsActive
            => Status == AuctionStatus.Open && TimeRemaining > TimeSpan.Zero;

        // ── Constructeur ──────────────────────────────────────────────────────
        public Auction() { }

        public Auction(int id, Card card, decimal startPrice,
                       DateTime startTime, int durationSec)
        {
            Id = id;
            Card = card;
            StartPrice = startPrice;
            StartTime = startTime;
            DurationSec = durationSec;
            Status = AuctionStatus.Open;
        }

        // ── Méthodes ──────────────────────────────────────────────────────────

        /// <summary>
        /// Ajoute une mise si elle est valide (supérieure à la meilleure offre actuelle).
        /// Retourne vrai si la mise a été acceptée.
        /// </summary>
        public bool PlaceBid(Bid bid)
        {
            if (!IsActive) return false;
            if (bid.Amount <= CurrentBid) return false;

            Bids.Add(bid);
            return true;
        }

        /// <summary>Ferme l'enchère.</summary>
        public void Close() => Status = AuctionStatus.Closed;

        public override string ToString()
            => $"Enchère #{Id} – {Card?.Name} | Meilleure offre : {CurrentBid:C}";
    }
}