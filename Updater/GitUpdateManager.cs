using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GangasiriTeaFactoryBilling.Updater
{
    public static class GitUpdateManager
    {
        private const string RepoOwner = "SeranMN";
        private const string RepoName = "GangasiriTeaFactoryBilling";
        
        // Check for updates
        public static async Task CheckForUpdates()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GangasiriTeaFactoryApp", "1.0"));

                    string url = $"https://api.github.com/repos/{RepoOwner}/{RepoName}/releases/latest";
                    var response = await client.GetAsync(url);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        using (JsonDocument doc = JsonDocument.Parse(json))
                        {
                            JsonElement root = doc.RootElement;
                            if (root.TryGetProperty("tag_name", out JsonElement tagElement))
                            {
                                string tagName = tagElement.GetString(); // e.g., "v1.0.42"
                                
                                // Parse version from tag (remove 'v' prefix if present)
                                string versionStr = tagName.TrimStart('v');
                                
                                if (Version.TryParse(versionStr, out Version latestVersion))
                                {
                                    Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                                    // Use a simpler comparison or default to 1.0.0.0 if assembly version is not set properly
                                    if (currentVersion == null) currentVersion = new Version(1, 0, 0, 0);

                                    // Check if latest version is greater
                                    if (latestVersion > currentVersion)
                                    {
                                        DialogResult result = MessageBox.Show(
                                            $"A new update is available!\n\nCurrent Version: {currentVersion}\nNew Version: {latestVersion}\n\nDo you want to update now?",
                                            "Update Available",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Information);

                                        if (result == DialogResult.Yes)
                                        {
                                            // Get browser download URL for the asset (zip)
                                            string downloadUrl = "";
                                            if (root.TryGetProperty("assets", out JsonElement assets) && assets.GetArrayLength() > 0)
                                            {
                                                 // Assuming the first asset is the zip
                                                 downloadUrl = assets[0].GetProperty("browser_download_url").GetString();
                                            }
                                            else
                                            {
                                                downloadUrl = root.GetProperty("html_url").GetString(); // Fallback to release page
                                            }

                                            // Open download link
                                            Process.Start(new ProcessStartInfo
                                            {
                                                FileName = downloadUrl,
                                                UseShellExecute = true
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Auto-Update Check Failed: {ex.Message}");
            }
        }
    }
}

