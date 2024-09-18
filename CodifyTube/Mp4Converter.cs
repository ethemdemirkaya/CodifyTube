using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Converter;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Text.RegularExpressions;

namespace CodifyTube
{
    public partial class Mp4Converter : Form
    {
        private YoutubeClient youtube;
        private StreamManifest streamManifest;
        private string videoTitle;
        private List<IAudioStreamInfo> audioStreams;
        private string videoId;
        private CancellationTokenSource cts;

        public Mp4Converter()
        {
            InitializeComponent();
            youtube = new YoutubeClient();
            string ffmpegPath = Path.Combine(Application.StartupPath, "ffmpeg.exe");
            if (!File.Exists(ffmpegPath))
            {
                MessageBox.Show("FFmpeg bulunamadı. Lütfen ffmpeg.exe dosyasını uygulama klasörüne kopyalayın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private string GetVideoIdFromUrl(string videoUrl)
        {
            try
            {
                if (Uri.TryCreate(videoUrl, UriKind.Absolute, out Uri uri))
                {
                    if (uri.Host.Contains("youtube.com") || uri.Host.Contains("youtu.be"))
                    {
                        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                        if (!string.IsNullOrEmpty(query["v"]))
                        {
                            return query["v"];
                        }
                        else if (uri.Host.Contains("youtu.be"))
                        {
                            return uri.Segments.LastOrDefault();
                        }
                    }
                }
            }
            catch
            {
            }

            return null;
        }
        private async Task LoadThumbnail(string videoId)
        {
            string thumbnailUrl = $"https://img.youtube.com/vi/{videoId}/maxresdefault.jpg";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var imageBytes = await httpClient.GetByteArrayAsync(thumbnailUrl);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        pictureBoxThumbnail.Image = System.Drawing.Image.FromStream(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Thumbnail yüklenirken hata oluştu: {ex.Message}");
            }
        }




        private async void btnGetInfo_Click_1(object sender, EventArgs e)
        {
            string videoUrl = txtVideoUrl.Text;
            if (string.IsNullOrWhiteSpace(videoUrl))
            {
                MessageBox.Show("Lütfen bir YouTube video URL'si girin.");
                return;
            }

            try
            {
                progressBar1.Visible = true;
                progressBar1.Style = ProgressBarStyle.Marquee;

                videoId = GetVideoIdFromUrl(videoUrl);

                if (string.IsNullOrEmpty(videoId))
                {
                    MessageBox.Show("Geçersiz YouTube video URL'si.");
                    return;
                }

                var video = await youtube.Videos.GetAsync(videoId);
                videoTitle = video.Title;
                lblVideoTitle.Text = videoTitle;

                await LoadThumbnail(videoId);
                streamManifest = await youtube.Videos.Streams.GetManifestAsync(videoId);
                audioStreams = streamManifest.GetAudioStreams()
                    .OrderByDescending(s => s.Bitrate)
                    .ToList();

                listBoxQualities.Items.Clear();
                foreach (var stream in audioStreams)
                {
                    string quality = $"{stream.Bitrate.KiloBitsPerSecond:F0} kbps - {stream.Container}";
                    listBoxQualities.Items.Add(quality);
                }

                if (listBoxQualities.Items.Count > 0)
                    listBoxQualities.SelectedIndex = 0;

                btnDownload.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
            finally
            {
                progressBar1.Visible = false;
                lblProgress.Visible = false;
                progressBar1.Style = ProgressBarStyle.Continuous;
            }
        }

        private async void btnDownload_Click_1(object sender, EventArgs e)
        {
            if (listBoxQualities.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen bir ses kalitesi seçin.");
                return;
            }

            var selectedAudioStream = audioStreams[listBoxQualities.SelectedIndex];
            string sanitizedFileName = SanitizeFileName(videoTitle);

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = $"{sanitizedFileName}.mp3",
                Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    progressBar1.Visible = true;
                    lblProgress.Visible = true;
                    btnDownload.Enabled = false;
                    btnDurdur.Enabled = true;

                    cts = new CancellationTokenSource();

                    var progress = new Progress<double>(p =>
                    {
                        progressBar1.Value = (int)(p * 100);
                        lblProgress.Text = $"{p:P0}";
                    });

                    await youtube.Videos.DownloadAsync(
                        videoId,
                        new ConversionRequestBuilder(saveFileDialog.FileName)
                            .SetContainer("mp3")
                            .SetPreset(ConversionPreset.UltraFast)
                            .Build(),
                        progress,
                        cts.Token
                    );

                    MessageBox.Show("İndirme tamamlandı!");
                }
                catch (OperationCanceledException)
                {
                    MessageBox.Show("İndirme iptal edildi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"İndirme sırasında hata oluştu: {ex.Message}");
                }
                finally
                {
                    progressBar1.Visible = false;
                    lblProgress.Visible = false;
                    btnDownload.Enabled = true;
                    btnDurdur.Enabled = false;
                    cts?.Dispose();
                }
            }
        }
        private string SanitizeFileName(string fileName)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegEx = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            fileName = Regex.Replace(fileName, invalidRegEx, "_");

            if (fileName.Length > 245) 
            {
                fileName = fileName.Substring(0, 245);
            }

            return fileName.Trim();
        }

        private void btnDurdur_Click_1(object sender, EventArgs e)
        {
            cts?.Cancel();
            btnDurdur.Enabled = false;
        }
    }
}