﻿using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TFT_Overlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string APP_VERSION = "V1.7.2";
        public bool githubChecked = false;
        bool canDrag = true;
        bool onTop = true;
        AutoUpdater scraper = new AutoUpdater();

        private readonly Cursor LoLNormal = CustomCursor.FromByteArray(Properties.Resources.LoLNormal);
        private readonly Cursor LoLPointer = CustomCursor.FromByteArray(Properties.Resources.LoLPointer);
        private readonly Cursor LoLHover = CustomCursor.FromByteArray(Properties.Resources.LoLHover);

        private bool OnTop { get; set; } = true;
        private bool CanDrag { get; set; }
        private string CurrentVersion { get; } = Version.version;

        public MainWindow()
        {
            scraper.checkVersionAsync();
            InitializeComponent();
            LoadStringResource("en-US");
            this.Cursor = LoLNormal;
            CanDrag = !Properties.Settings.Default.Lock;

        public void editGithubChecked(bool isChecked)
        {
            githubChecked = isChecked;
        }
            if (Properties.Settings.Default.AutoDim == true)
            {
                Thread t = new Thread(new ThreadStart(AutoDim))
                {
                    IsBackground = true
                };

                t.Start();
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Click_About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TFT Information Overlay V" + CurrentVersion + " by J2GKaze/Jinsoku#4019\n\nDM me on Discord if you have any questions\n\nLast Updated: July 10th, 2019 @ 12:39AM PST", "About");
        }

        private void MenuItem_Click_Credits(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Big thanks to:\nChaoticoz: Lock Window, Always on Top, and Mouseover\nAsemco/Asemco#7390: Adding Origins and Classes\nAthenyx#9406: Designs\nTenebris: Auto-Updater\nOBJECT#3031: Items/Origins/Classes Strings Base\nJpgdev: Readme format\nKbphan\nEerilai\nꙅꙅɘᴎTqAbɘbᴎɘld#1175: Window Position/Size Saving, CPU Threading Fix\n\nShoutout to:\nAlexander321#7153 for the Discord Nitro Gift!\nAnonymous for Reddit Gold\nu/test01011 for Reddit Gold\n\nmac#0001 & bNatural#0001(Feel free to bug these 2 on Discord) ;)\nShamish#4895 (Make sure you bug this guy a lot)\nDekinosai#7053 (Buy this man tons of drinks)", "Credits");
        }

        private void MenuItem_Click_Lock(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Lock = !Properties.Settings.Default.Lock;
            Properties.Settings.Default.Save();
            CanDrag = !CanDrag;
        }

        private void MenuItem_AutoUpdate(object sender, RoutedEventArgs e)
        {
            string state = Properties.Settings.Default.AutoUpdate == true ? "OFF" : "ON";

            MessageBoxResult result = MessageBox.Show($"Would you like to turn {state} Auto-Update? This will restart the program.", "Auto-Updater", MessageBoxButton.OKCancel);

            if (result != MessageBoxResult.OK)
            {
                return;
            }

            Properties.Settings.Default.AutoUpdate = !Properties.Settings.Default.AutoUpdate;
            Properties.Settings.Default.Save();

            System.Windows.Forms.Application.Restart();
            Application.Current.Shutdown();
        }

        private void MenuItem_Click_OnTop(object sender, RoutedEventArgs e)
        {
            if (OnTop)
            {
                this.Topmost = false;
                OnTop = false;
            }
            else
            {
                this.Topmost = true;
                OnTop = true;
            }
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Control)sender).Cursor = LoLPointer;
            if (CanDrag)
            {
                this.DragMove();
            }
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Control)sender).Cursor = LoLNormal;
        }

        private void MainWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((Control)sender).Cursor = LoLHover;
        }

        private void MainWindow_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Control)sender).Cursor = LoLNormal;
        }

        private void AutoDim_Click(object sender, RoutedEventArgs e)
        {
            string state = Properties.Settings.Default.AutoDim == true ? "OFF" : "ON";

            MessageBoxResult result = MessageBox.Show($"Would you like to turn {state} Auto-Dim? This will restart the program.", "Auto-Dim", MessageBoxButton.OKCancel);

            if (result != MessageBoxResult.OK)
            {
                return;
            }

            Properties.Settings.Default.AutoDim = !Properties.Settings.Default.AutoDim;
            Properties.Settings.Default.Save();

            System.Windows.Forms.Application.Restart();
            Application.Current.Shutdown();
        }

        private void AutoDim()
        {
            while (true)
            {
                if (IsLeagueOrOverlayActive())
                {
                    Dispatcher.BeginInvoke(new ThreadStart(() => App.Current.MainWindow.Opacity = 1));
                }
                else if (!IsLeagueOrOverlayActive())
                {
                    Dispatcher.BeginInvoke(new ThreadStart(() => App.Current.MainWindow.Opacity = .2));
                }
                Thread.Sleep(100);
            }
        }

        private bool IsLeagueOrOverlayActive()
        {
            string currentActiveProcessName = ProcessHelper.GetActiveProcessName();
            return currentActiveProcessName.Contains("League of Legends") || currentActiveProcessName.Contains("TFT Overlay");
        }

        //
        // Localization
        //
        private void LoadStringResource(string locale)
        {
            var resources = new ResourceDictionary();

            resources.Source = new Uri("pack://application:,,,/Resource/Localization/ItemStrings_" + locale + ".xaml", UriKind.Absolute);

            var current = Application.Current.Resources.MergedDictionaries.FirstOrDefault(
                             m => m.Source.OriginalString.EndsWith("ItemStrings_" + locale + ".xaml"));

            if (current != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(current);
            }

            Application.Current.Resources.MergedDictionaries.Add(resources);
        }
        private void Localization_Credits(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("es-AR: Oscarinom\nes-MX: Jukai#3434\nfr-FR: Darkneight\nit-IT: BlackTYWhite#0943\nJA: つかぽん＠PKMotion#8731\nPL: Czapson#9774\npt-BR: Bigg#4019\nRU: Jeremy Buttson#2586\nvi-VN: GodV759\nzh-CN: nevex#4441\nzh-TW: noheart#6977\n", "Localization Credits");
        }

        private void US_Click(object sender, RoutedEventArgs e)
        {
            LoadStringResource("en-US");
        }

        private void AR_Click(object sender, RoutedEventArgs e)
        {
            LoadStringResource("es-AR");
        }

        private void MX_Click(object sender, RoutedEventArgs e)
        {
            LoadStringResource("es-MX");
        }

        private void FR_Click(object sender, RoutedEventArgs e)
        {
            LoadStringResource("fr-FR");
        }

        private void IT_Click(object sender, RoutedEventArgs e)
        {
            LoadStringResource("it-IT");
        }

        private void JA_Click(object sender, RoutedEventArgs e)
        {
            LoadStringResource("JA");
        }

        private void PL_Click(object sender, RoutedEventArgs e)
        {
            LoadStringResource("PL");
        }

        private void BR_Click(object sender, RoutedEventArgs e)
        {
            LoadStringResource("pt-BR");
        }

        private void RU_Click(object sender, RoutedEventArgs e)
        {
            LoadStringResource("RU");
        }

        private void VN_Click(object sender, RoutedEventArgs e)
        {
            LoadStringResource("vi-VN");
        }

        private void CN_Click(object sender, RoutedEventArgs e)
        {
            LoadStringResource("zh-CN");
        }

        private void TW_Click(object sender, RoutedEventArgs e)
        {
            LoadStringResource("zh-TW");
        }
        //
        // Localization
        //

    }
}
