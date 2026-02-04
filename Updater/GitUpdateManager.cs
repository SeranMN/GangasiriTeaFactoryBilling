using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
        private const string GitHubToken = ""; // Paste your Fine-grained Token here

        public static async Task CheckForUpdates()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    ConfigureHttpClient(client);

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
                                string tagName = tagElement.GetString();
                                string versionStr = tagName.TrimStart('v');
                                
                                if (Version.TryParse(versionStr, out Version latestVersion))
                                {
                                    Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                                    if (currentVersion == null) currentVersion = new Version(1, 0, 0, 0);

                                    if (latestVersion > currentVersion)
                                    {
                                        DialogResult result = MessageBox.Show(
                                            $"A new update is available!\n\nCurrent Version: {currentVersion}\nNew Version: {latestVersion}\n\n" +
                                            "The application will close, download the update, and restart automatically.\n\n" +
                                            "Do you want to update now?",
                                            "Update Available",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Information);

                                        if (result == DialogResult.Yes)
                                        {
                                            string assetUrl = GetAssetUrl(root);
                                            if (!string.IsNullOrEmpty(assetUrl))
                                            {
                                                await PerformUpdate(assetUrl);
                                            }
                                            else
                                            {
                                                MessageBox.Show("Could not find a valid update asset (zip file).", "Update Error");
                                            }
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
                MessageBox.Show($"Update Check Failed: {ex.Message}", "Error");
            }
        }

        private static void ConfigureHttpClient(HttpClient client)
        {
            client.Timeout = TimeSpan.FromSeconds(300); // 5 mins for download
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GangasiriTeaFactoryApp", "1.0"));
            if (!string.IsNullOrEmpty(GitHubToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GitHubToken);
            }
        }

        private static string GetAssetUrl(JsonElement root)
        {
            if (root.TryGetProperty("assets", out JsonElement assets) && assets.GetArrayLength() > 0)
            {
                // Prefer 'url' (API) for private repos with token, otherwise 'browser_download_url'
                if (!string.IsNullOrEmpty(GitHubToken))
                {
                    return assets[0].GetProperty("url").GetString();
                }
                return assets[0].GetProperty("browser_download_url").GetString();
            }
            return null;
        }

        private static async Task PerformUpdate(string assetUrl)
        {
            string tempPath = Path.GetTempPath();
            string zipPath = Path.Combine(tempPath, "update.zip");
            string extractPath = Path.Combine(tempPath, "GangasiriUpdate_" + DateTime.Now.Ticks);

            try
            {
                // 1. Download
                using (HttpClient client = new HttpClient())
                {
                    ConfigureHttpClient(client);
                    if (!string.IsNullOrEmpty(GitHubToken))
                    {
                         // For API asset download
                         client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
                    }

                    // Show a simple loading cursor or form (blocking for now is acceptable for MVP)
                    Cursor.Current = Cursors.WaitCursor;
                    var data = await client.GetByteArrayAsync(assetUrl);
                    await File.WriteAllBytesAsync(zipPath, data);
                    Cursor.Current = Cursors.Default;
                }

                // 2. Extract
                if (Directory.Exists(extractPath)) Directory.Delete(extractPath, true);
                ZipFile.ExtractToDirectory(zipPath, extractPath);

                // 3. Prepare Updater Script
                string currentExe = Process.GetCurrentProcess().MainModule.FileName;
                string currentDir = Path.GetDirectoryName(currentExe);
                string appName = Path.GetFileName(currentExe);

                string scriptPath = Path.Combine(tempPath, "update.ps1");
                string script = $@"
                    param($pidToWait, $sourceDir, $destDir, $exeName)
                    Write-Host 'Waiting for application to exit...'
                    try {{
                        Wait-Process -Id $pidToWait -ErrorAction Stop -Timeout 30
                    }} catch {{
                        Write-Warning 'Process did not exit or was not found.'
                    }}
                    
                    Start-Sleep -Seconds 2
                    Write-Host 'Copying new files...'
                    Copy-Item -Path ""$sourceDir\*"" -Destination ""$destDir"" -Recurse -Force -ErrorAction Stop
                    
                    Write-Host 'Restarting application...'
                    Start-Process ""$destDir\$exeName""
                ";

                await File.WriteAllTextAsync(scriptPath, script);

                // 4. Run Script and Exit
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\" -pidToWait {Process.GetCurrentProcess().Id} -sourceDir \"{extractPath}\" -destDir \"{currentDir}\" -exeName \"{appName}\"",
                    UseShellExecute = true, // To show the window
                    WindowStyle = ProcessWindowStyle.Normal
                };

                Process.Start(psi);
                Environment.Exit(0);

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show($"Failed to apply update: {ex.Message}", "Update Error");
            }
        }
    }
}

