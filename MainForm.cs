using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PokeBID.Database;
using PokeBID.Models;
using PokeBID.Forms;

namespace PokeBID
{
    /// <summary>
    /// Formulaire principal de PokeBID.
    /// Contient trois onglets : Cartes | Enchères | Historique.
    /// </summary>
    public partial class MainForm : Form
    {
        // ═════════════════════════════════════════════════════════════════════
        //  CHAMPS PRIVÉS – Infrastructure
        // ═════════════════════════════════════════════════════════════════════
        private readonly DatabaseManager _db = new DatabaseManager();
        private List<Card> _cards = new List<Card>();
        private List<Bot> _bots = new List<Bot>();

        // ── Enchère active ────────────────────────────────────────────────────
        private Auction _currentAuction;
        private Timer _auctionTimer;   // compte à rebours
        private Timer _botTimer;       // déclencheur des bots

        // ── Nom du joueur ─────────────────────────────────────────────────────
        private const string PlayerName = "Joueur";

        // ═════════════════════════════════════════════════════════════════════
        //  CONTRÔLES – TabControl
        // ═════════════════════════════════════════════════════════════════════
        private TabControl tabMain;
        private TabPage tabCards, tabAuction, tabHistory;

        // ── Onglet Cartes ─────────────────────────────────────────────────────
        private ListView lvCards;
        private Button btnAddCard, btnEditCard, btnDeleteCard;
        private PictureBox pbCardPreview;

        // ── Onglet Enchères ───────────────────────────────────────────────────
        private Panel pnlCardInfo;
        private PictureBox pbAuctionCard;
        private Label lblCardName, lblCardRarity, lblCardHP,
                           lblCardWeakness, lblCardAttacks;
        private Label lblCurrentBid, lblTimeLeft, lblWinner;
        private ListBox lbBids;
        private Button btnStartAuction, btnBid5, btnBid10, btnBid25,
                           btnBid50, btnBid100, btnBidCustom;
        private ComboBox cboAuctionCard;
        private NumericUpDown nudDuration;
        private Label lblSelectCard, lblDuration;

        // ── Onglet Historique ─────────────────────────────────────────────────
        private DataGridView dgHistory;
        private Button btnRefreshHistory;

        // ═════════════════════════════════════════════════════════════════════
        //  CONSTRUCTEUR
        // ═════════════════════════════════════════════════════════════════════
        public MainForm()
        {
            Text = "PokeBID – Enchères de cartes Pokémon";
            Size = new Size(1000, 680);
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;

            BuildUI();
            WireEvents();
            LoadData();
        }

        // ═════════════════════════════════════════════════════════════════════
        //  CONSTRUCTION DE L'INTERFACE
        // ═════════════════════════════════════════════════════════════════════
        private void BuildUI()
        {
            tabMain = new TabControl { Dock = DockStyle.Fill };
            tabCards = new TabPage("🃏 Cartes");
            tabAuction = new TabPage("🔨 Enchères");
            tabHistory = new TabPage("📜 Historique");
            tabMain.TabPages.AddRange(new[] { tabCards, tabAuction, tabHistory });
            Controls.Add(tabMain);

            BuildCardsTab();
            BuildAuctionTab();
            BuildHistoryTab();
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Onglet CARTES
        // ─────────────────────────────────────────────────────────────────────
        private void BuildCardsTab()
        {
            // ListView
            lvCards = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Left = 8,
                Top = 8,
                Width = 580,
                Height = 580,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom
            };
            lvCards.Columns.Add("Id", 45);
            lvCards.Columns.Add("Nom", 160);
            lvCards.Columns.Add("Rareté", 100);
            lvCards.Columns.Add("HP", 55);
            lvCards.Columns.Add("Faiblesse", 100);
            lvCards.Columns.Add("Attaques", 120);

            // Boutons CRUD
            btnAddCard = MakeButton("➕ Ajouter", 600, 10, 120);
            btnEditCard = MakeButton("✏️ Modifier", 600, 46, 120);
            btnDeleteCard = MakeButton("🗑️ Supprimer", 600, 82, 120);

            // Prévisualisation image
            pbCardPreview = new PictureBox
            {
                Left = 600,
                Top = 130,
                Width = 320,
                Height = 260,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            tabCards.Controls.AddRange(new Control[]
            {
                lvCards, btnAddCard, btnEditCard, btnDeleteCard, pbCardPreview
            });
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Onglet ENCHÈRES
        // ─────────────────────────────────────────────────────────────────────
        private void BuildAuctionTab()
        {
            // ── Sélection de la carte ─────────────────────────────────────────
            lblSelectCard = new Label { Text = "Carte :", Left = 10, Top = 12, Width = 60 };
            cboAuctionCard = new ComboBox
            {
                Left = 75,
                Top = 8,
                Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            lblDuration = new Label { Text = "Durée (s) :", Left = 310, Top = 12, Width = 75 };
            nudDuration = new NumericUpDown
            {
                Left = 390,
                Top = 8,
                Width = 70,
                Minimum = 15,
                Maximum = 300,
                Value = 60
            };
            btnStartAuction = MakeButton("▶ Démarrer enchère", 475, 6, 160);
            btnStartAuction.BackColor = Color.ForestGreen;
            btnStartAuction.ForeColor = Color.White;

            // ── Info carte ────────────────────────────────────────────────────
            pbAuctionCard = new PictureBox
            {
                Left = 10,
                Top = 45,
                Width = 180,
                Height = 160,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };
            lblCardName = MakeLabelBold("", 200, 50, 300, 22);
            lblCardRarity = MakeLabel2("", 200, 75, 300);
            lblCardHP = MakeLabel2("", 200, 95, 300);
            lblCardWeakness = MakeLabel2("", 200, 115, 300);
            lblCardAttacks = MakeLabel2("", 200, 135, 300);

            // ── Enchère en cours ──────────────────────────────────────────────
            var sepLine = new Label
            {
                Left = 10,
                Top = 215,
                Width = 620,
                Height = 2,
                BorderStyle = BorderStyle.Fixed3D
            };
            lblCurrentBid = new Label
            {
                Left = 10,
                Top = 225,
                Width = 400,
                Height = 30,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Text = "Meilleure offre : —"
            };
            lblTimeLeft = new Label
            {
                Left = 420,
                Top = 225,
                Width = 220,
                Height = 30,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                Text = "Temps : —",
                TextAlign = ContentAlignment.MiddleRight
            };
            lblWinner = new Label
            {
                Left = 10,
                Top = 260,
                Width = 620,
                Height = 24,
                Font = new Font("Segoe UI", 11, FontStyle.Bold | FontStyle.Italic),
                ForeColor = Color.Goldenrod,
                Text = ""
            };

            // ── Boutons de mise ───────────────────────────────────────────────
            int bx = 10, by = 290;
            btnBid5 = MakeBidButton("+5", bx, by);
            btnBid10 = MakeBidButton("+10", bx + 80, by);
            btnBid25 = MakeBidButton("+25", bx + 160, by);
            btnBid50 = MakeBidButton("+50", bx + 240, by);
            btnBid100 = MakeBidButton("+100", bx + 320, by);
            btnBidCustom = MakeBidButton("Montant libre…", bx + 400, by, 130);
            btnBidCustom.BackColor = Color.SteelBlue;

            // ── Liste des mises ───────────────────────────────────────────────
            lbBids = new ListBox
            {
                Left = 650,
                Top = 8,
                Width = 320,
                Height = 560,
                Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font("Consolas", 9),
                HorizontalScrollbar = true
            };
            var lblBidsTitle = new Label
            {
                Text = "Mises en temps réel",
                Left = 650,
                Top = 570,
                Width = 320,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            tabAuction.Controls.AddRange(new Control[]
            {
                lblSelectCard, cboAuctionCard, lblDuration, nudDuration, btnStartAuction,
                pbAuctionCard, lblCardName, lblCardRarity, lblCardHP, lblCardWeakness, lblCardAttacks,
                sepLine, lblCurrentBid, lblTimeLeft, lblWinner,
                btnBid5, btnBid10, btnBid25, btnBid50, btnBid100, btnBidCustom,
                lbBids, lblBidsTitle
            });
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Onglet HISTORIQUE
        // ─────────────────────────────────────────────────────────────────────
        private void BuildHistoryTab()
        {
            dgHistory = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White
            };
            btnRefreshHistory = MakeButton("🔄 Rafraîchir", 10, 10, 140);

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 44 };
            pnlTop.Controls.Add(btnRefreshHistory);

            tabHistory.Controls.Add(dgHistory);
            tabHistory.Controls.Add(pnlTop);
        }

        // ═════════════════════════════════════════════════════════════════════
        //  CÂBLAGE DES ÉVÉNEMENTS
        // ═════════════════════════════════════════════════════════════════════
        private void WireEvents()
        {
            // Cartes
            btnAddCard.Click += BtnAddCard_Click;
            btnEditCard.Click += BtnEditCard_Click;
            btnDeleteCard.Click += BtnDeleteCard_Click;
            lvCards.SelectedIndexChanged += LvCards_SelectedIndexChanged;

            // Enchères
            btnStartAuction.Click += BtnStartAuction_Click;
            btnBid5.Click += (s, e) => PlacePlayerBid(5);
            btnBid10.Click += (s, e) => PlacePlayerBid(10);
            btnBid25.Click += (s, e) => PlacePlayerBid(25);
            btnBid50.Click += (s, e) => PlacePlayerBid(50);
            btnBid100.Click += (s, e) => PlacePlayerBid(100);
            btnBidCustom.Click += BtnBidCustom_Click;

            // Historique
            btnRefreshHistory.Click += (s, e) => LoadHistory();
            tabMain.SelectedIndexChanged += (s, e) =>
            {
                if (tabMain.SelectedTab == tabHistory) LoadHistory();
            };
        }

        // ═════════════════════════════════════════════════════════════════════
        //  CHARGEMENT DES DONNÉES
        // ═════════════════════════════════════════════════════════════════════
        private void LoadData()
        {
            try
            {
                _cards = _db.GetAllCards();
                _bots = _db.GetAllBots();
                RefreshCardsList();
                RefreshAuctionCardCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Impossible de se connecter à la base de données.\n\n" +
                    "Vérifiez votre connexion MySQL et la chaîne de connexion dans DatabaseManager.cs.\n\n" +
                    $"Détail : {ex.Message}",
                    "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  ONGLET CARTES – Logique
        // ═════════════════════════════════════════════════════════════════════
        private void RefreshCardsList()
        {
            lvCards.Items.Clear();
            foreach (var c in _cards)
            {
                var item = new ListViewItem(c.Id.ToString());
                item.SubItems.Add(c.Name);
                item.SubItems.Add(c.Rarity);
                item.SubItems.Add(c.HP.ToString());
                item.SubItems.Add(c.Weakness);
                item.SubItems.Add(c.Attacks);
                item.Tag = c;

                // Couleur selon rareté
                switch (c.Rarity)
                {
                    case "Ultra Rare": item.BackColor = Color.Gold; break;
                    case "Rare": item.BackColor = Color.LightSkyBlue; break;
                    case "Uncommon": item.BackColor = Color.LightGreen; break;
                }
                lvCards.Items.Add(item);
            }
        }

        private void RefreshAuctionCardCombo()
        {
            cboAuctionCard.Items.Clear();
            foreach (var c in _cards)
                cboAuctionCard.Items.Add(c);
            if (cboAuctionCard.Items.Count > 0)
                cboAuctionCard.SelectedIndex = 0;
        }

        private void LvCards_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCards.SelectedItems.Count == 0) return;
            var card = (Card)lvCards.SelectedItems[0].Tag;
            LoadCardPreview(card);
        }

        private void LoadCardPreview(Card card)
        {
            pbCardPreview.Image = null;
            if (!string.IsNullOrEmpty(card.ImagePath) && File.Exists(card.ImagePath))
            {
                try { pbCardPreview.Image = Image.FromFile(card.ImagePath); }
                catch { /* image non chargeable */ }
            }
        }

        private void BtnAddCard_Click(object sender, EventArgs e)
        {
            using (var dlg = new CardForm())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                try
                {
                    dlg.ResultCard.Id = _db.InsertCard(dlg.ResultCard);
                    _cards.Add(dlg.ResultCard);
                    RefreshCardsList();
                    RefreshAuctionCardCombo();
                }
                catch (Exception ex) { ShowDbError(ex); }
            }
        }

        private void BtnEditCard_Click(object sender, EventArgs e)
        {
            if (lvCards.SelectedItems.Count == 0)
            {
                MessageBox.Show("Sélectionnez une carte à modifier.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var card = (Card)lvCards.SelectedItems[0].Tag;
            using (var dlg = new CardForm(card))
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                try
                {
                    _db.UpdateCard(dlg.ResultCard);
                    // Mettre à jour la liste locale
                    int idx = _cards.FindIndex(c => c.Id == card.Id);
                    if (idx >= 0) _cards[idx] = dlg.ResultCard;
                    RefreshCardsList();
                    RefreshAuctionCardCombo();
                }
                catch (Exception ex) { ShowDbError(ex); }
            }
        }

        private void BtnDeleteCard_Click(object sender, EventArgs e)
        {
            if (lvCards.SelectedItems.Count == 0)
            {
                MessageBox.Show("Sélectionnez une carte à supprimer.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var card = (Card)lvCards.SelectedItems[0].Tag;
            var confirm = MessageBox.Show(
                $"Supprimer la carte \"{card.Name}\" ?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _db.DeleteCard(card.Id);
                _cards.Remove(card);
                RefreshCardsList();
                RefreshAuctionCardCombo();
                pbCardPreview.Image = null;
            }
            catch (Exception ex) { ShowDbError(ex); }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  ONGLET ENCHÈRES – Logique
        // ═════════════════════════════════════════════════════════════════════
        private void BtnStartAuction_Click(object sender, EventArgs e)
        {
            if (cboAuctionCard.SelectedItem == null)
            {
                MessageBox.Show("Sélectionnez une carte pour l'enchère.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_currentAuction != null && _currentAuction.IsActive)
            {
                MessageBox.Show("Une enchère est déjà en cours.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var card = (Card)cboAuctionCard.SelectedItem;
            int durationSec = (int)nudDuration.Value;

            // Prix de départ basé sur la rareté
            decimal startPrice = 20m;
            
            switch(card.Rarity)
            {
                case "Ultra Rare":
                    startPrice = 200m;
                    break;

                case "Rare":
                    startPrice = 100m;
                    break;
                case "Uncommon":
                    startPrice = 50m;
                    break;
            };

            _currentAuction = new Auction(0, card, startPrice, DateTime.Now, durationSec);

            try
            {
                _currentAuction.Id = _db.InsertAuction(_currentAuction);
            }
            catch (Exception ex) { ShowDbError(ex); }

            // UI
            UpdateAuctionCardPanel(card);
            lbBids.Items.Clear();
            lblWinner.Text = "";
            SetBidButtonsEnabled(true);
            btnStartAuction.Enabled = false;

            AddBidLine($"--- Enchère démarrée | Prix départ : {startPrice:C} | Durée : {durationSec}s ---");

            // Timers
            StartAuctionTimers();
        }

        private void StartAuctionTimers()
        {
            // Timer principal (1 seconde)
            _auctionTimer = new Timer { Interval = 1000 };
            _auctionTimer.Tick += AuctionTimer_Tick;
            _auctionTimer.Start();

            // Timer bots (déclenche la logique bots toutes les 500 ms)
            _botTimer = new Timer { Interval = 500 };
            _botTimer.Tick += BotTimer_Tick;
            _botTimer.Start();
        }

        private void AuctionTimer_Tick(object sender, EventArgs e)
        {
            if (_currentAuction == null) return;

            TimeSpan remaining = _currentAuction.TimeRemaining;
            lblTimeLeft.Text = $"⏱ {remaining:mm\\:ss}";

            // Couleur urgence
            lblTimeLeft.ForeColor = remaining.TotalSeconds <= 10
                ? Color.Red : Color.DarkRed;

            if (!_currentAuction.IsActive)
            {
                EndAuction();
            }
        }

        private async void BotTimer_Tick(object sender, EventArgs e)
        {
            if (_currentAuction == null || !_currentAuction.IsActive || _bots.Count == 0)
                return;

            // Chaque bot a une chance d'enchérir (traitement asynchrone)
            foreach (var bot in _bots)
            {
                if (!_currentAuction.IsActive) break;
                if (!bot.WantsToBid(_currentAuction.CurrentBid)) continue;

                decimal amount = bot.ComputeBid(_currentAuction.CurrentBid);
                if (amount <= _currentAuction.CurrentBid) continue;

                // Délai de réaction du bot
                int delay = bot.GetReactionDelayMs();
                await System.Threading.Tasks.Task.Delay(delay);

                // Vérification après le délai
                if (!_currentAuction.IsActive) break;

                var bid = new Bid(_currentAuction.Id, amount, bot.Name, false);
                if (_currentAuction.PlaceBid(bid))
                {
                    try { _db.InsertBid(bid); } catch { /* ignore en cas d'erreur */ }
                    // Mise à jour UI (thread-safe)
                    if (InvokeRequired)
                        Invoke(new Action(() => OnNewBid(bid)));
                    else
                        OnNewBid(bid);
                }
            }
        }

        private void PlacePlayerBid(decimal increment)
        {
            if (_currentAuction == null || !_currentAuction.IsActive)
            {
                MessageBox.Show("Aucune enchère en cours.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            decimal amount = _currentAuction.CurrentBid + increment;
            SubmitPlayerBid(amount);
        }

        private void BtnBidCustom_Click(object sender, EventArgs e)
        {
            if (_currentAuction == null || !_currentAuction.IsActive)
            {
                MessageBox.Show("Aucune enchère en cours.", "Info",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                $"Mise minimale : {_currentAuction.CurrentBid + 1:C}\nEntrez votre montant :",
                "Mise personnalisée", (_currentAuction.CurrentBid + 10).ToString("F0"));

            if (decimal.TryParse(input, out decimal amount))
                SubmitPlayerBid(amount);
        }

        private void SubmitPlayerBid(decimal amount)
        {
            if (amount <= _currentAuction.CurrentBid)
            {
                MessageBox.Show(
                    $"Votre mise ({amount:C}) doit être supérieure à la mise actuelle ({_currentAuction.CurrentBid:C}).",
                    "Mise invalide", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var bid = new Bid(_currentAuction.Id, amount, PlayerName, true);
            if (_currentAuction.PlaceBid(bid))
            {
                try { _db.InsertBid(bid); } catch { /* ignore */ }
                OnNewBid(bid);
            }
        }

        private void OnNewBid(Bid bid)
        {
            lblCurrentBid.Text = $"💰 Meilleure offre : {_currentAuction.CurrentBid:C}  ({_currentAuction.HighestBid?.BidderName})";
            lblCurrentBid.ForeColor = bid.IsPlayer ? Color.DarkGreen : Color.DarkBlue;
            AddBidLine(bid.ToString());
        }

        private void EndAuction()
        {
            _auctionTimer?.Stop();
            _botTimer?.Stop();

            _currentAuction.Close();
            try { _db.CloseAuction(_currentAuction.Id); } catch { /* ignore */ }

            SetBidButtonsEnabled(false);
            btnStartAuction.Enabled = true;
            lblTimeLeft.Text = "⏱ 00:00";

            // Résultat
            var winner = _currentAuction.HighestBid;
            if (winner == null)
            {
                lblWinner.Text = "🏳️ Aucune mise – enchère annulée.";
                lblWinner.ForeColor = Color.Gray;
                AddBidLine("--- Enchère terminée : aucune mise ---");
            }
            else
            {
                bool playerWon = winner.IsPlayer;
                lblWinner.Text = playerWon
                    ? $"🏆 VOUS AVEZ GAGNÉ ! {_currentAuction.Card.Name} pour {winner.Amount:C}"
                    : $"😞 {winner.BidderName} a remporté la carte pour {winner.Amount:C}";
                lblWinner.ForeColor = playerWon ? Color.DarkGreen : Color.DarkRed;
                AddBidLine($"--- Enchère terminée | Gagnant : {winner.BidderName} | {winner.Amount:C} ---");

                MessageBox.Show(lblWinner.Text,
                    playerWon ? "Félicitations !" : "Enchère terminée",
                    MessageBoxButtons.OK,
                    playerWon ? MessageBoxIcon.Information : MessageBoxIcon.None);
            }
        }

        private void UpdateAuctionCardPanel(Card card)
        {
            lblCardName.Text = $"✦ {card.Name}";
            lblCardRarity.Text = $"Rareté : {card.Rarity}";
            lblCardHP.Text = $"HP : {card.HP}";
            lblCardWeakness.Text = $"Faiblesse : {card.Weakness}";
            lblCardAttacks.Text = $"Attaques : {card.Attacks}";

            pbAuctionCard.Image = null;
            if (!string.IsNullOrEmpty(card.ImagePath) && File.Exists(card.ImagePath))
            {
                try { pbAuctionCard.Image = Image.FromFile(card.ImagePath); }
                catch { /* pas d'image */ }
            }

            lblCurrentBid.Text = $"💰 Meilleure offre : {_currentAuction.StartPrice:C}  (départ)";
            lblTimeLeft.Text = $"⏱ {TimeSpan.FromSeconds(_currentAuction.DurationSec):mm\\:ss}";
        }

        private void AddBidLine(string line)
        {
            lbBids.Items.Insert(0, line);   // plus récent en haut
            if (lbBids.Items.Count > 200)
                lbBids.Items.RemoveAt(lbBids.Items.Count - 1);
        }

        private void SetBidButtonsEnabled(bool enabled)
        {
            btnBid5.Enabled = btnBid10.Enabled = btnBid25.Enabled =
            btnBid50.Enabled = btnBid100.Enabled = btnBidCustom.Enabled = enabled;
        }

        // ═════════════════════════════════════════════════════════════════════
        //  ONGLET HISTORIQUE – Logique
        // ═════════════════════════════════════════════════════════════════════
        private void LoadHistory()
        {
            try
            {
                dgHistory.DataSource = _db.GetAuctionHistory();
            }
            catch (Exception ex) { ShowDbError(ex); }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  HELPERS UI
        // ═════════════════════════════════════════════════════════════════════
        private static Button MakeButton(string text, int left, int top, int width = 110)
            => new Button
            {
                Text = text,
                Left = left,
                Top = top,
                Width = width,
                Height = 30
            };

        private static Button MakeBidButton(string text, int left, int top, int width = 70)
        {
            var btn = new Button
            {
                Text = text,
                Left = left,
                Top = top,
                Width = width,
                Height = 34,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.OrangeRed,
                ForeColor = Color.White
            };
            return btn;
        }

        private static Label MakeLabelBold(string text, int left, int top, int width, int height)
            => new Label
            {
                Text = text,
                Left = left,
                Top = top,
                Width = width,
                Height = height,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.DarkSlateBlue
            };

        private static Label MakeLabel2(string text, int left, int top, int width)
            => new Label
            {
                Text = text,
                Left = left,
                Top = top,
                Width = width,
                Height = 18,
                Font = new Font("Segoe UI", 9)
            };

        private static void ShowDbError(Exception ex)
            => MessageBox.Show($"Erreur base de données :\n{ex.Message}",
                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}