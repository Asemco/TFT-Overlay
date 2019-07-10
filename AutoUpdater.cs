using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace TFT_Overlay
{
    class AutoUpdater : WebScraper
    {
        const string OVERLAY_PULSE_URL = "https://github.com/Just2good/TFT-Overlay/pulse";
        private string version = MainWindow.APP_VERSION;

        public async void checkVersionAsync()
        {
            this.ObeyRobotsDotTxt = false;
            await this.StartAsync();
        }

        public void compareVersion(string newVersion)
        {
            bool currentVersionIsNewestVersion = version == newVersion;
            if (!currentVersionIsNewestVersion)
            {
                System.Windows.MessageBox.Show("The newest version is " + newVersion + ". Your version is " + version + ": " + currentVersionIsNewestVersion);
            }
        }

        public override void Init()
        {
            this.LoggingLevel = WebScraper.LogLevel.All;
            this.Request(OVERLAY_PULSE_URL, Parse);
        }

        public override void Parse(Response response)
        {
            // All EVEN indexes are \n.  All ODD indexes contain actual content.
            // This is factual as of 2019-07-04, and should be until Github changes their page.
            // There is no reliable css to attach to, so we grab the highest element with an ID
            // and head down until we can isolate the version number in the format: V{Number}.
            try
            {
                HtmlNode releasesSection = response.GetElementById("releases");
                HtmlNode releases = releasesSection.ChildNodes[3];
                HtmlNode latestRelease = releases.ChildNodes[releases.ChildNodes.Length - 2];
                string latestVersion = latestRelease.ChildNodes[3].TextContentClean;
                Scrape(new ScrapedData() { { "Version", latestVersion } });
                compareVersion(latestVersion);
            }
            catch (Exception excep)
            {
                // If it failed for any reason, inform the user to check the github and download it from there.
                // If the Github's no longer working, yikes.
                //System.Windows.MessageBox.Show("Please check the github directly for a new update!  The URL is: https://github.com/Just2good/TFT-Overlay");
            }
        }
    }
}
        