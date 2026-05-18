using System;

namespace PokeBID.Models
{
    /// <summary>
    /// Représente une carte Pokémon avec ses caractéristiques.
    /// </summary>
    public class Card
    {
        // ── Propriétés ────────────────────────────────────────────────────────
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rarity { get; set; }   // Common / Uncommon / Rare / Ultra Rare
        public int HP { get; set; }
        public string Weakness { get; set; }   // ex: "Feu", "Eau"…
        public string Attacks { get; set; }   // liste séparée par virgule
        public string ImagePath { get; set; }   // chemin local ou URL

        // ── Constructeur ──────────────────────────────────────────────────────
        public Card() { }

        public Card(int id, string name, string rarity, int hp,
                    string weakness, string attacks, string imagePath = "")
        {
            Id = id;
            Name = name;
            Rarity = rarity;
            HP = hp;
            Weakness = weakness;
            Attacks = attacks;
            ImagePath = imagePath;
        }

        // ── Affichage ─────────────────────────────────────────────────────────
        public override string ToString() => $"{Name} ({Rarity}) – {HP} HP";
    }
}