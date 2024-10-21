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
				string tag = TagTextBox.Text; // Ex: "00 00 00 00"
				ushort id = ushort.Parse(IdTextBox.Text); // Ex: 1
				string name = NameTextBox.Text; // Ex: "Abcdefgh"
				ushort hp = ushort.Parse(HpTextBox.Text); // Ex: 0
				ushort level = ushort.Parse(LevelTextBox.Text); // Ex: 0
				ushort xp = ushort.Parse(XpTextBox.Text); // Ex: 0
				ushort height = ushort.Parse(HeightTextBox.Text); // Ex: 0
				ushort weight = ushort.Parse(WeightTextBox.Text); // Ex: 0

				// Convertir les valeurs en une séquence de bytes
				StringBuilder sb = new StringBuilder();

				// Tag (Bytes déjà formatés comme une chaîne)
				sb.Append(tag.Replace(" ", ""));

				// Id (1 byte)
				sb.Append(id.ToString("X2"));

				// Name (chaîne en hexadécimal)
				for (int i = 0; i < 8; i++)
					sb.Append(((int)name.ElementAtOrDefault(i)).ToString("X2"));

				// Hp (1 byte)
				sb.Append(hp.ToString("X2"));

				// Level (1 byte)
				sb.Append(level.ToString("X2"));

				// Xp (1 byte)
				sb.Append(xp.ToString("X2"));

				// Height (1 byte)
				sb.Append(height.ToString("X2"));

				// Weight (1 byte)
				sb.Append(weight.ToString("X2"));

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