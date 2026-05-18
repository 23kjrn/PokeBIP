using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using PokeBID.Models;

namespace PokeBID.Database
{
    /// <summary>
    /// Gère toutes les interactions avec la base de données MySQL/MariaDB.
    /// Adaptez la chaîne de connexion dans App.config ou ici directement.
    /// </summary>
    public class DatabaseManager
    {
        // ── Chaîne de connexion ───────────────────────────────────────────────
        // Modifiez ces valeurs selon votre environnement.
        private const string ConnectionString =
            "Server=localhost;Port=3306;Database=pokebid;Uid=root;Pwd=;CharSet=utf8;";

        // ── Helpers internes ──────────────────────────────────────────────────

        /// <summary>Ouvre et retourne une connexion MySQL.</summary>
        private MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        // ═════════════════════════════════════════════════════════════════════
        //  CARDS
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>Récupère toutes les cartes de la base.</summary>
        public List<Card> GetAllCards()
        {
            var cards = new List<Card>();
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand("SELECT * FROM cards ORDER BY name", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    cards.Add(MapCard(rdr));
                }
            }
            return cards;
        }

        /// <summary>Insère une nouvelle carte et retourne son Id généré.</summary>
        public int InsertCard(Card card)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(
                "INSERT INTO cards (name, rarity, hp, weakness, attacks, image_path) " +
                "VALUES (@name,@rarity,@hp,@weakness,@attacks,@img); SELECT LAST_INSERT_ID();", conn))
            {
                BindCardParams(cmd, card);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>Met à jour une carte existante.</summary>
        public void UpdateCard(Card card)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(
                "UPDATE cards SET name=@name, rarity=@rarity, hp=@hp, " +
                "weakness=@weakness, attacks=@attacks, image_path=@img WHERE id=@id", conn))
            {
                BindCardParams(cmd, card);
                cmd.Parameters.AddWithValue("@id", card.Id);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Supprime une carte par son Id.</summary>
        public void DeleteCard(int cardId)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand("DELETE FROM cards WHERE id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", cardId);
                cmd.ExecuteNonQuery();
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  BOTS
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>Récupère tous les bots.</summary>
        public List<Bot> GetAllBots()
        {
            var bots = new List<Bot>();
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand("SELECT * FROM bots", conn))
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    bots.Add(new Bot(
                        rdr.GetInt32(rdr.GetOrdinal("id")),
                        rdr.GetString(rdr.GetOrdinal("name")),
                        rdr.GetDecimal(rdr.GetOrdinal("max_budget")),
                        rdr.GetDecimal(rdr.GetOrdinal("aggression"))));
                }
            }
            return bots;
        }

        // ═════════════════════════════════════════════════════════════════════
        //  AUCTIONS
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>Insère une nouvelle enchère et retourne son Id.</summary>
        public int InsertAuction(Auction auction)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(
                "INSERT INTO auctions (card_id, start_price, start_time, duration_sec, status) " +
                "VALUES (@cid,@sp,@st,@dur,@status); SELECT LAST_INSERT_ID();", conn))
            {
                cmd.Parameters.AddWithValue("@cid", auction.Card.Id);
                cmd.Parameters.AddWithValue("@sp", auction.StartPrice);
                cmd.Parameters.AddWithValue("@st", auction.StartTime);
                cmd.Parameters.AddWithValue("@dur", auction.DurationSec);
                cmd.Parameters.AddWithValue("@status", auction.Status.ToString());
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>Clôture une enchère en base (met le statut à Closed).</summary>
        public void CloseAuction(int auctionId)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(
                "UPDATE auctions SET status='Closed' WHERE id=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", auctionId);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Récupère l'historique des enchères (fermées) avec leur résultat.</summary>
        public DataTable GetAuctionHistory()
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(
                "SELECT a.id AS '#', c.name AS 'Carte', a.start_price AS 'Prix départ', " +
                "       MAX(b.amount) AS 'Prix final', " +
                "       COALESCE((SELECT bidder_name FROM bids WHERE auction_id=a.id " +
                "                 ORDER BY amount DESC LIMIT 1),'Aucune mise') AS 'Gagnant', " +
                "       a.start_time AS 'Date', a.status AS 'Statut' " +
                "FROM auctions a " +
                "JOIN cards c ON c.id = a.card_id " +
                "LEFT JOIN bids b ON b.auction_id = a.id " +
                "GROUP BY a.id " +
                "ORDER BY a.start_time DESC", conn))
            {
                var da = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  BIDS
        // ═════════════════════════════════════════════════════════════════════

        /// <summary>Insère une mise en base.</summary>
        public void InsertBid(Bid bid)
        {
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(
                "INSERT INTO bids (auction_id, amount, bidder_name, is_player, placed_at) " +
                "VALUES (@aid,@amt,@name,@isp,@pat)", conn))
            {
                cmd.Parameters.AddWithValue("@aid", bid.AuctionId);
                cmd.Parameters.AddWithValue("@amt", bid.Amount);
                cmd.Parameters.AddWithValue("@name", bid.BidderName);
                cmd.Parameters.AddWithValue("@isp", bid.IsPlayer ? 1 : 0);
                cmd.Parameters.AddWithValue("@pat", bid.PlacedAt);
                cmd.ExecuteNonQuery();
            }
        }

        // ── Mappers privés ────────────────────────────────────────────────────

        private static Card MapCard(IDataReader rdr)
        {
            int colId = rdr.GetOrdinal("id");
            int colName = rdr.GetOrdinal("name");
            int colRarity = rdr.GetOrdinal("rarity");
            int colHp = rdr.GetOrdinal("hp");
            int colWeakness = rdr.GetOrdinal("weakness");
            int colAttacks = rdr.GetOrdinal("attacks");
            int colImagePath = rdr.GetOrdinal("image_path");

            return new Card(
                rdr.GetInt32(colId),
                rdr.GetString(colName),
                rdr.GetString(colRarity),
                rdr.GetInt32(colHp),
                rdr.GetString(colWeakness),
                rdr.GetString(colAttacks),
                rdr.IsDBNull(colImagePath) ? "" : rdr.GetString(colImagePath));
        }

        private static void BindCardParams(MySqlCommand cmd, Card card)
        {
            cmd.Parameters.AddWithValue("@name", card.Name);
            cmd.Parameters.AddWithValue("@rarity", card.Rarity);
            cmd.Parameters.AddWithValue("@hp", card.HP);
            cmd.Parameters.AddWithValue("@weakness", card.Weakness);
            cmd.Parameters.AddWithValue("@attacks", card.Attacks);
            cmd.Parameters.AddWithValue("@img", card.ImagePath ?? "");
        }
    }
}