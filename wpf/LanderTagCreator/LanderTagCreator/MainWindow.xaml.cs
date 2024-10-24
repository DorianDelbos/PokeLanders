using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LanderTagCreator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Récupérer les données des TextBoxes
                string tag = TagTextBox.Text.Replace(" ", ""); // Ex: "00 00 00 00"
                ushort id = ushort.Parse(IdTextBox.Text); // Ex: 1
                string name = NameTextBox.Text.PadRight(12).Substring(0, 12); // Ex: "Abcdefgh", 12 chars
                ushort hp = ushort.Parse(HpTextBox.Text); // Ex: 0
                int xp = int.Parse(XpTextBox.Text); // Ex: 0 (4 octets)
                ushort height = ushort.Parse(HeightTextBox.Text); // Ex: 0
                ushort weight = ushort.Parse(WeightTextBox.Text); // Ex: 0

                // Récupérer les ID d'attaque
                ushort attack1Id = ushort.Parse(Attack1IdTextBox.Text); // Ex: 0
                ushort attack2Id = ushort.Parse(Attack2IdTextBox.Text); // Ex: 0
                ushort attack3Id = ushort.Parse(Attack3IdTextBox.Text); // Ex: 0
                ushort attack4Id = ushort.Parse(Attack4IdTextBox.Text); // Ex: 0

                // Récupérer les IVs
                byte ivPv = byte.Parse(IvPvTextBox.Text); // Ex: 0
                byte ivAtk = byte.Parse(IvAtkTextBox.Text); // Ex: 0
                byte ivDef = byte.Parse(IvDefTextBox.Text); // Ex: 0
                byte ivAtkSpe = byte.Parse(IvAtkSpeTextBox.Text); // Ex: 0
                byte ivDefSpe = byte.Parse(IvDefSpeTextBox.Text); // Ex: 0
                byte ivSpeed = byte.Parse(IvSpeedTextBox.Text); // Ex: 0

                // Récupérer les EVs
                byte evPv = byte.Parse(EvPvTextBox.Text); // Ex: 0
                byte evAtk = byte.Parse(EvAtkTextBox.Text); // Ex: 0
                byte evDef = byte.Parse(EvDefTextBox.Text); // Ex: 0
                byte evAtkSpe = byte.Parse(EvAtkSpeTextBox.Text); // Ex: 0
                byte evDefSpe = byte.Parse(EvDefSpeTextBox.Text); // Ex: 0
                byte evSpeed = byte.Parse(EvSpeedTextBox.Text); // Ex: 0

                // Convertir les valeurs en une séquence de bytes
                StringBuilder sb = new StringBuilder();

                // Block 1
                sb.Append(tag); // 4 octets
                foreach (char ch in name) // 12 octets
                {
                    sb.Append(((int)ch).ToString("X2"));
                }

                // Block 2
                sb.Append(id.ToString("X4")); // 2 octets (ID)
                sb.Append(hp.ToString("X4")); // 2 octets (HP)
                sb.Append(((uint)xp).ToString("X8")); // 4 octets (XP)
                sb.Append(height.ToString("X4")); // 2 octets (Height)
                sb.Append(weight.ToString("X4")); // 2 octets (Weight)

                // Block 3 - Attack IDs
                sb.Append(attack1Id.ToString("X4")); // 2 octets
                sb.Append(attack2Id.ToString("X4")); // 2 octets
                sb.Append(attack3Id.ToString("X4")); // 2 octets
                sb.Append(attack4Id.ToString("X4")); // 2 octets

                // Block 4 - IVs
                sb.Append(ivPv.ToString("X2")); // 1 octet
                sb.Append(ivAtk.ToString("X2")); // 1 octet
                sb.Append(ivDef.ToString("X2")); // 1 octet
                sb.Append(ivAtkSpe.ToString("X2")); // 1 octet
                sb.Append(ivDefSpe.ToString("X2")); // 1 octet
                sb.Append(ivSpeed.ToString("X2")); // 1 octet

                // Block 4 - EVs
                sb.Append(evPv.ToString("X2")); // 1 octet
                sb.Append(evAtk.ToString("X2")); // 1 octet
                sb.Append(evDef.ToString("X2")); // 1 octet
                sb.Append(evAtkSpe.ToString("X2")); // 1 octet
                sb.Append(evDefSpe.ToString("X2")); // 1 octet
                sb.Append(evSpeed.ToString("X2")); // 1 octet

                // Afficher le résultat dans le TextBox de résultat
                ResultTextBox.Text = FormatHexOutput(sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        // Formater la chaîne hexadécimale avec des espaces pour une meilleure lisibilité
        private string FormatHexOutput(string hex)
        {
            StringBuilder formatted = new StringBuilder();
            for (int i = 0; i < hex.Length; i += 2)
            {
                formatted.Append(hex.Substring(i, 2) + " ");
            }
            return formatted.ToString().Trim().ToUpper();
        }
    }
}