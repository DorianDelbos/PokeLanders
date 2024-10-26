using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LanderJsonCreator
{
    public partial class MainWindow : Window
    {
        private StatsManager statsManager;
        private TypesManager typesManager;

        public MainWindow()
        {
            InitializeComponent();
            statsManager = new StatsManager(DataGridStats);
            typesManager = new TypesManager(DataGridTypes);
        }

        #region
        private void AddType_Click(object sender, RoutedEventArgs e)
        {
            typesManager.AddStat($"New Stat {typesManager.TypesList.Count}");
        }

        private void RemoveType_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Type typeToRemove)
            {
                typesManager.RemoveStat(typeToRemove);
                DataGridTypes.Items.Refresh();
            }
        }
        #endregion

        #region STATS
        private void AddStat_Click(object sender, RoutedEventArgs e)
        {
            statsManager.AddStat($"New Stat {statsManager.StatsList.Count}");
        }

        private void RemoveStat_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Stat statToRemove)
            {
                statsManager.RemoveStat(statToRemove);
                DataGridStats.Items.Refresh();
            }
        }
        #endregion

        // Méthodes pour l'ajout

        private void AddMove_Click(object sender, RoutedEventArgs e)
        {
            // Ajouter un nouveau mouvement
            MessageBox.Show("Ajouter un mouvement");
        }

        private void AddCreature_Click(object sender, RoutedEventArgs e)
        {
            // Ajouter une nouvelle créature
            MessageBox.Show("Ajouter une créature");
        }

        private void AddEvolutionChain_Click(object sender, RoutedEventArgs e)
        {
            // Ajouter une chaîne d'évolution
            MessageBox.Show("Ajouter une chaîne d'évolution");
        }

        private void AddStatusEffect_Click(object sender, RoutedEventArgs e)
        {
            // Ajouter un effet de statut
            MessageBox.Show("Ajouter un effet de statut");
        }

        // Méthodes pour la sauvegarde
        private void SaveTypes_Click(object sender, RoutedEventArgs e)
        {
            // Sauvegarder les types
            MessageBox.Show("Sauvegarder les types");
        }

        private void SaveStats_Click(object sender, RoutedEventArgs e)
        {
            // Sauvegarder les stats
            MessageBox.Show("Sauvegarder les stats");
        }

        private void SaveMoves_Click(object sender, RoutedEventArgs e)
        {
            // Sauvegarder les mouvements
            MessageBox.Show("Sauvegarder les mouvements");
        }

        private void SaveCreatures_Click(object sender, RoutedEventArgs e)
        {
            // Sauvegarder les créatures
            MessageBox.Show("Sauvegarder les créatures");
        }

        private void SaveEvolutionChains_Click(object sender, RoutedEventArgs e)
        {
            // Sauvegarder les chaînes d'évolution
            MessageBox.Show("Sauvegarder les chaînes d'évolution");
        }

        private void SaveStatusEffects_Click(object sender, RoutedEventArgs e)
        {
            // Sauvegarder les effets de statut
            MessageBox.Show("Sauvegarder les effets de statut");
        }
    }
}
