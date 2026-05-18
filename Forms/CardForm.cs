using System;
using System.Windows.Forms;
using PokeBID.Models;

namespace PokeBID.Forms
{
    /// <summary>
    /// Formulaire d'ajout / modification d'une carte Pokémon.
    /// </summary>
    public class CardForm : Form
    {
        // ── Contrôles ─────────────────────────────────────────────────────────
        private Label lblName, lblRarity, lblHP, lblWeakness, lblAttacks, lblImage;
        private TextBox txtName, txtWeakness, txtAttacks, txtImage;
        private NumericUpDown nudHP;
        private ComboBox cboRarity;
        private Button btnSave, btnCancel, btnBrowse;

        // ── Donnée résultante ─────────────────────────────────────────────────
        public Card ResultCard { get; private set; }

        // ── Constructeur ──────────────────────────────────────────────────────
        public CardForm(Card cardToEdit = null)
        {
            BuildUI();

            if (cardToEdit != null)
            {
                Text = "Modifier la carte";
                ResultCard = cardToEdit;
                txtName.Text = cardToEdit.Name;
                cboRarity.Text = cardToEdit.Rarity;
                nudHP.Value = cardToEdit.HP;
                txtWeakness.Text = cardToEdit.Weakness;
                txtAttacks.Text = cardToEdit.Attacks;
                txtImage.Text = cardToEdit.ImagePath;
            }
            else
            {
                Text = "Ajouter une carte";
                ResultCard = new Card();
            }
        }

        // ── Construction de l'interface ───────────────────────────────────────
        private void BuildUI()
        {
            Size = new System.Drawing.Size(420, 340);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            // Labels
            lblName = MakeLabel("Nom :", 12);
            lblRarity = MakeLabel("Rareté :", 44);
            lblHP = MakeLabel("HP :", 76);
            lblWeakness = MakeLabel("Faiblesse :", 108);
            lblAttacks = MakeLabel("Attaques :", 140);
            lblImage = MakeLabel("Image :", 172);

            // Inputs
            txtName = MakeTextBox(120, 12, 260);
            cboRarity = new ComboBox { Left = 120, Top = 44, Width = 160 };
            cboRarity.Items.AddRange(new[] { "Common", "Uncommon", "Rare", "Ultra Rare" });
            cboRarity.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRarity.SelectedIndex = 0;

            nudHP = new NumericUpDown
            {
                Left = 120,
                Top = 76,
                Width = 100,
                Minimum = 10,
                Maximum = 300,
                Value = 60
            };
            txtWeakness = MakeTextBox(120, 108, 260);
            txtAttacks = MakeTextBox(120, 140, 260);
            txtImage = MakeTextBox(120, 172, 200);

            btnBrowse = new Button { Text = "…", Left = 328, Top = 171, Width = 50, Height = 23 };
            btnBrowse.Click += BtnBrowse_Click;

            // Boutons
            btnSave = new Button
            {
                Text = "Enregistrer",
                Left = 120,
                Top = 270,
                Width = 110,
                Height = 28,
                DialogResult = DialogResult.OK
            };
            btnCancel = new Button
            {
                Text = "Annuler",
                Left = 245,
                Top = 270,
                Width = 110,
                Height = 28,
                DialogResult = DialogResult.Cancel
            };
            btnSave.Click += BtnSave_Click;
            AcceptButton = btnSave;
            CancelButton = btnCancel;

            Controls.AddRange(new Control[]
            {
                lblName, lblRarity, lblHP, lblWeakness, lblAttacks, lblImage,
                txtName, cboRarity, nudHP, txtWeakness, txtAttacks, txtImage,
                btnBrowse, btnSave, btnCancel
            });
        }

        // ── Événements ────────────────────────────────────────────────────────

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Images|*.png;*.jpg;*.jpeg;*.gif;*.bmp";
                if (dlg.ShowDialog() == DialogResult.OK)
                    txtImage.Text = dlg.FileName;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Le nom de la carte est obligatoire.", "Validation",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            ResultCard.Name = txtName.Text.Trim();
            ResultCard.Rarity = cboRarity.Text;
            ResultCard.HP = (int)nudHP.Value;
            ResultCard.Weakness = txtWeakness.Text.Trim();
            ResultCard.Attacks = txtAttacks.Text.Trim();
            ResultCard.ImagePath = txtImage.Text.Trim();
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private static Label MakeLabel(string text, int top)
            => new Label
            {
                Text = text,
                Left = 12,
                Top = top + 3,
                Width = 100,
                AutoSize = false
            };

        private static TextBox MakeTextBox(int left, int top, int width)
            => new TextBox { Left = left, Top = top, Width = width };
    }
}