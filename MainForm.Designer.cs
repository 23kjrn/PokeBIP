using System.Windows.Forms;

namespace PokeBID
{
    public partial class PokeBID : MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        private void InitializeComponent()
        {
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabCards = new System.Windows.Forms.TabPage();
            this.btnDeleteCard = new System.Windows.Forms.Button();
            this.btnEditCard = new System.Windows.Forms.Button();
            this.btnAddCard = new System.Windows.Forms.Button();
            this.dgvCards = new System.Windows.Forms.DataGridView();
            this.tabAuctions = new System.Windows.Forms.TabPage();
            this.lblTimeLeft = new System.Windows.Forms.Label();
            this.lstBidHistory = new System.Windows.Forms.ListBox();
            this.btnPlaceBid = new System.Windows.Forms.Button();
            this.lblCurrentBid = new System.Windows.Forms.Label();
            this.lblCardDetails = new System.Windows.Forms.Label();
            this.lblCardName = new System.Windows.Forms.Label();
            this.btnStartAuction = new System.Windows.Forms.Button();
            this.dgvActiveAuctions = new System.Windows.Forms.DataGridView();
            this.tabHistory = new System.Windows.Forms.TabPage();
            this.dgvHistory = new System.Windows.Forms.DataGridView();

            this.tabControlMain.SuspendLayout();
            this.tabCards.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCards)).BeginInit();
            this.tabAuctions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvActiveAuctions)).BeginInit();
            this.tabHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabCards);
            this.tabControlMain.Controls.Add(this.tabAuctions);
            this.tabControlMain.Controls.Add(this.tabHistory);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(984, 561);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabCards
            // 
            this.tabCards.Controls.Add(this.btnDeleteCard);
            this.tabCards.Controls.Add(this.btnEditCard);
            this.tabCards.Controls.Add(this.btnAddCard);
            this.tabCards.Controls.Add(this.dgvCards);
            this.tabCards.Location = new System.Drawing.Point(4, 22);
            this.tabCards.Name = "tabCards";
            this.tabCards.Padding = new System.Windows.Forms.Padding(3);
            this.tabCards.Size = new System.Drawing.Size(976, 535);
            this.tabCards.TabIndex = 0;
            this.tabCards.Text = "Gestion des Cartes";
            this.tabCards.UseVisualStyleBackColor = true;
            // 
            // btnDeleteCard
            // 
            this.btnDeleteCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteCard.Location = new System.Drawing.Point(220, 490);
            this.btnDeleteCard.Name = "btnDeleteCard";
            this.btnDeleteCard.Size = new System.Drawing.Size(100, 30);
            this.btnDeleteCard.TabIndex = 3;
            this.btnDeleteCard.Text = "Supprimer";
            this.btnDeleteCard.UseVisualStyleBackColor = true;
            // 
            // btnEditCard
            // 
            this.btnEditCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditCard.Location = new System.Drawing.Point(114, 490);
            this.btnEditCard.Name = "btnEditCard";
            this.btnEditCard.Size = new System.Drawing.Size(100, 30);
            this.btnEditCard.TabIndex = 2;
            this.btnEditCard.Text = "Modifier...";
            this.btnEditCard.UseVisualStyleBackColor = true;
            // 
            // btnAddCard
            // 
            this.btnAddCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddCard.Location = new System.Drawing.Point(8, 490);
            this.btnAddCard.Name = "btnAddCard";
            this.btnAddCard.Size = new System.Drawing.Size(100, 30);
            this.btnAddCard.TabIndex = 1;
            this.btnAddCard.Text = "Ajouter...";
            this.btnAddCard.UseVisualStyleBackColor = true;
            // 
            // dgvCards
            // 
            this.dgvCards.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCards.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCards.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCards.Location = new System.Drawing.Point(8, 6);
            this.dgvCards.MultiSelect = false;
            this.dgvCards.Name = "dgvCards";
            this.dgvCards.ReadOnly = true;
            this.dgvCards.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCards.Size = new System.Drawing.Size(960, 465);
            this.dgvCards.TabIndex = 0;
            // 
            // tabAuctions
            // 
            this.tabAuctions.Controls.Add(this.lblTimeLeft);
            this.tabAuctions.Controls.Add(this.lstBidHistory);
            this.tabAuctions.Controls.Add(this.btnPlaceBid);
            this.tabAuctions.Controls.Add(this.lblCurrentBid);
            this.tabAuctions.Controls.Add(this.lblCardDetails);
            this.tabAuctions.Controls.Add(this.lblCardName);
            this.tabAuctions.Controls.Add(this.btnStartAuction);
            this.tabAuctions.Controls.Add(this.dgvActiveAuctions);
            this.tabAuctions.Location = new System.Drawing.Point(4, 22);
            this.tabAuctions.Name = "tabAuctions";
            this.tabAuctions.Padding = new System.Windows.Forms.Padding(3);
            this.tabAuctions.Size = new System.Drawing.Size(976, 535);
            this.tabAuctions.TabIndex = 1;
            this.tabAuctions.Text = "Enchères Live";
            this.tabAuctions.UseVisualStyleBackColor = true;
            // 
            // lblTimeLeft
            // 
            this.lblTimeLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeLeft.AutoSize = true;
            this.lblTimeLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblTimeLeft.ForeColor = System.Drawing.Color.Red;
            this.lblTimeLeft.Location = new System.Drawing.Point(520, 150);
            this.lblTimeLeft.Name = "lblTimeLeft";
            this.lblTimeLeft.Size = new System.Drawing.Size(193, 24);
            this.lblTimeLeft.TabIndex = 7;
            this.lblTimeLeft.Text = "Temps restant : 00s";
            // 
            // lstBidHistory
            // 
            this.lstBidHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstBidHistory.Font = new System.Drawing.Font("Consolas", 10F);
            this.lstBidHistory.FormattingEnabled = true;
            this.lstBidHistory.ItemHeight = 15;
            this.lstBidHistory.Location = new System.Drawing.Point(524, 240);
            this.lstBidHistory.Name = "lstBidHistory";
            this.lstBidHistory.Size = new System.Drawing.Size(444, 274);
            this.lstBidHistory.TabIndex = 6;
            // 
            // btnPlaceBid
            // 
            this.btnPlaceBid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlaceBid.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnPlaceBid.Location = new System.Drawing.Point(524, 190);
            this.btnPlaceBid.Name = "btnPlaceBid";
            this.btnPlaceBid.Size = new System.Drawing.Size(180, 40);
            this.btnPlaceBid.TabIndex = 5;
            this.btnPlaceBid.Text = "Enchérir (+10$)";
            this.btnPlaceBid.UseVisualStyleBackColor = true;
            // 
            // lblCurrentBid
            // 
            this.lblCurrentBid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentBid.AutoSize = true;
            this.lblCurrentBid.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblCurrentBid.Location = new System.Drawing.Point(520, 110);
            this.lblCurrentBid.Name = "lblCurrentBid";
            this.lblCurrentBid.Size = new System.Drawing.Size(175, 20);
            this.lblCurrentBid.TabIndex = 4;
            this.lblCurrentBid.Text = "Offre actuelle : 0,00$";
            // 
            // lblCardDetails
            // 
            this.lblCardDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCardDetails.AutoSize = true;
            this.lblCardDetails.Location = new System.Drawing.Point(521, 75);
            this.lblCardDetails.Name = "lblCardDetails";
            this.lblCardDetails.Size = new System.Drawing.Size(117, 13);
            this.lblCardDetails.TabIndex = 3;
            this.lblCardDetails.Text = "PV, Attaques, Rareté...";
            // 
            // lblCardName
            // 
            this.lblCardName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCardName.AutoSize = true;
            this.lblCardName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblCardName.Location = new System.Drawing.Point(519, 40);
            this.lblCardName.Name = "lblCardName";
            this.lblCardName.Size = new System.Drawing.Size(211, 26);
            this.lblCardName.TabIndex = 2;
            this.lblCardName.Text = "Sélectionnez un lot";
            // 
            // btnStartAuction
            // 
            this.btnStartAuction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartAuction.Location = new System.Drawing.Point(524, 6);
            this.btnStartAuction.Name = "btnStartAuction";
            this.btnStartAuction.Size = new System.Drawing.Size(150, 25);
            this.btnStartAuction.TabIndex = 1;
            this.btnStartAuction.Text = "Lancer l'enchère";
            this.btnStartAuction.UseVisualStyleBackColor = true;
            // 
            // dgvActiveAuctions
            // 
            this.dgvActiveAuctions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvActiveAuctions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvActiveAuctions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvActiveAuctions.Location = new System.Drawing.Point(8, 6);
            this.dgvActiveAuctions.MultiSelect = false;
            this.dgvActiveAuctions.Name = "dgvActiveAuctions";
            this.dgvActiveAuctions.ReadOnly = true;
            this.dgvActiveAuctions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvActiveAuctions.Size = new System.Drawing.Size(496, 521);
            this.dgvActiveAuctions.TabIndex = 0;
            // 
            // tabHistory
            // 
            this.tabHistory.Controls.Add(this.dgvHistory);
            this.tabHistory.Location = new System.Drawing.Point(4, 22);
            this.tabHistory.Name = "tabHistory";
            this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabHistory.Size = new System.Drawing.Size(976, 535);
            this.tabHistory.TabIndex = 2;
            this.tabHistory.Text = "Historique";
            this.tabHistory.UseVisualStyleBackColor = true;
            // 
            // dgvHistory
            // 
            this.dgvHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistory.Location = new System.Drawing.Point(3, 3);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.Size = new System.Drawing.Size(970, 529);
            this.dgvHistory.TabIndex = 0;
            // 
            // PokeBID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.tabControlMain);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "PokeBID";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PokeBID - Enchères de Cartes Pokémon";
            this.tabControlMain.ResumeLayout(false);
            this.tabCards.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCards)).EndInit();
            this.tabAuctions.ResumeLayout(false);
            this.tabAuctions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvActiveAuctions)).EndInit();
            this.tabHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabCards;
        private System.Windows.Forms.TabPage tabAuctions;
        private System.Windows.Forms.TabPage tabHistory;

        // Onglet Cartes
        private System.Windows.Forms.DataGridView dgvCards;
        private System.Windows.Forms.Button btnAddCard;
        private System.Windows.Forms.Button btnEditCard;
        private System.Windows.Forms.Button btnDeleteCard;

        // Onglet Enchères
        private System.Windows.Forms.DataGridView dgvActiveAuctions;
        private System.Windows.Forms.Button btnStartAuction;
        private System.Windows.Forms.Label lblCardName;
        private System.Windows.Forms.Label lblCardDetails;
        private System.Windows.Forms.Label lblCurrentBid;
        private System.Windows.Forms.Button btnPlaceBid;
        private System.Windows.Forms.ListBox lstBidHistory;
        private System.Windows.Forms.Label lblTimeLeft;

        // Onglet Historique
        private System.Windows.Forms.DataGridView dgvHistory;
    }
}