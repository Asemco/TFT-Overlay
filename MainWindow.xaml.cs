﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public MainWindow()
        {
            scraper.checkVersionAsync();
            InitializeComponent();
            MouseLeftButtonDown += new MouseButtonEventHandler(MainWindow_MouseLeftButtonDown);
        }

        public void editGithubChecked(bool isChecked)
        {
            githubChecked = isChecked;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_Click_About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TFT Information Overlay " + APP_VERSION + " by J2GKaze/Jinsoku#4019.\n\nDM me on Discord if you have any questions.\n\nBig thanks to Chaoticoz for Lock Window, Always on Top, and Mouseover.\n\nAlso thanks to, Asemco/Asemco#7390 for adding Origins and Classes!\n\nLast Updated: July 2nd, 2019 @ 8:22PM PST");
        }

        private void MenuItem_Click_Lock(object sender, RoutedEventArgs e)
        {
            canDrag = !canDrag;
        }

        private void MenuItem_Click_OnTop(object sender, RoutedEventArgs e)
        {
            if (onTop)
            {
                this.Topmost = false;
                onTop = false;
            }
            else
            {
                this.Topmost = true;
                onTop = true;
            }

        }

        void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (canDrag)
            {
                this.DragMove();
            }
        }



    }


}


