using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace YMH219_KeyEventLogger
{
    public partial class Form1 : Form
    {
        private bool kayitAktif = false;
        private List<TusKaydi> kayitListesi = new List<TusKaydi>();
        private Dictionary<string, int> tusIstatistigi = new Dictionary<string, int>();

        // Kontroller
        private Button btnBaslat;
        private Button btnDurdur;
        private Button btnTemizle;
        private Button btnKaydet;
        private Label lblDurum;
        private Label lblIstatistik;
        private ListBox lstKayitlar;
        private TextBox txtYaziAlani;

        public Form1()
        {
            InitializeComponent();
            ArayuzOlustur();

            // Formun tuş olaylarını yakalayabilmesi için KeyPreview aktif edilir
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
        }

        private void ArayuzOlustur()
        {
            this.Text = "YMH219 - Eğitsel Key Event Logger (Sudem Cücemen - 235541021)";
            this.Size = new Size(620, 560);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // 1. Üst Kontrol Butonları Paneli
            btnBaslat = new Button
            {
                Text = "▶ Kaydı Başlat",
                Location = new Point(20, 15),
                Size = new Size(120, 35),
                BackColor = Color.LightGreen,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnBaslat.Click += BtnBaslat_Click;

            btnDurdur = new Button
            {
                Text = "⏹ Kaydı Durdur",
                Location = new Point(150, 15),
                Size = new Size(120, 35),
                BackColor = Color.LightCoral,
                Enabled = false,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnDurdur.Click += BtnDurdur_Click;

            btnTemizle = new Button
            {
                Text = "🗑 Temizle",
                Location = new Point(280, 15),
                Size = new Size(100, 35),
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            btnTemizle.Click += BtnTemizle_Click;

            btnKaydet = new Button
            {
                Text = "💾 Dosyaya Kaydet (.txt)",
                Location = new Point(390, 15),
                Size = new Size(190, 35),
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            btnKaydet.Click += BtnKaydet_Click;

            // 2. Durum Göstergesi
            lblDurum = new Label
            {
                Text = "DURUM: Kayıt Durduruldu (Tuşlar dinlenmiyor)",
                Location = new Point(20, 60),
                Size = new Size(560, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.DarkRed
            };

            // 3. Kullanıcının Tuş Girişi Yapabileceği Deneme Kutusu
            Label lblKutuAciklama = new Label
            {
                Text = "Tuş girişi yapmak için bu kutuyu tıklayabilir ya da pencerede tuşlara basabilirsiniz:",
                Location = new Point(20, 95),
                Size = new Size(560, 20)
            };

            txtYaziAlani = new TextBox
            {
                Location = new Point(20, 120),
                Size = new Size(560, 30),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                PlaceholderText = "Buraya yazı yazmayı deneyebilirsiniz..."
            };

            // 4. Kayıt Listesi (ListBox)
            lstKayitlar = new ListBox
            {
                Location = new Point(20, 165),
                Size = new Size(560, 280),
                Font = new Font("Consolas", 9, FontStyle.Regular)
            };

            // 5. İstatistik Göstergesi (Alt Panel)
            lblIstatistik = new Label
            {
                Text = "Toplam Yakalanan Olay: 0  |  En Sık Basılan Tuş: Yok",
                Location = new Point(20, 465),
                Size = new Size(560, 30),
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.DarkSlateGray
            };

            // Kontrolleri Forma Ekle
            this.Controls.Add(btnBaslat);
            this.Controls.Add(btnDurdur);
            this.Controls.Add(btnTemizle);
            this.Controls.Add(btnKaydet);
            this.Controls.Add(lblDurum);
            this.Controls.Add(lblKutuAciklama);
            this.Controls.Add(txtYaziAlani);
            this.Controls.Add(lstKayitlar);
            this.Controls.Add(lblIstatistik);
        }

        private void BtnBaslat_Click(object sender, EventArgs e)
        {
            kayitAktif = true;
            btnBaslat.Enabled = false;
            btnDurdur.Enabled = true;
            lblDurum.Text = "DURUM: 🟢 Kayıt Aktif (Basılan tuşlar kaydediliyor)";
            lblDurum.ForeColor = Color.DarkGreen;
        }

        private void BtnDurdur_Click(object sender, EventArgs e)
        {
            kayitAktif = false;
            btnBaslat.Enabled = true;
            btnDurdur.Enabled = false;
            lblDurum.Text = "DURUM: 🔴 Kayıt Durduruldu";
            lblDurum.ForeColor = Color.DarkRed;
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            kayitListesi.Clear();
            tusIstatistigi.Clear();
            lstKayitlar.Items.Clear();
            txtYaziAlani.Clear();
            IstatistigiGuncelle();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (kayitListesi.Count == 0)
            {
                MessageBox.Show("Kaydedilecek herhangi bir tuş kaydı bulunmamaktadır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Metin Dosyası (*.txt)|*.txt";
                sfd.FileName = "TusKayitlari_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName))
                        {
                            sw.WriteLine("=== YMH219 - KEY EVENT LOGGER RAPORU ===");
                            sw.WriteLine($"Öğrenci: Sudem CÜCEMEN (235541021)");
                            sw.WriteLine($"Tarih: {DateTime.Now}");
                            sw.WriteLine($"Toplam Kayıt Sayısı: {kayitListesi.Count}");
                            sw.WriteLine("---------------------------------------------");
                            foreach (var kayit in kayitListesi)
                            {
                                sw.WriteLine(kayit.ToString());
                            }
                            sw.WriteLine("---------------------------------------------");
                        }
                        MessageBox.Show("Kayıtlar başarıyla dosyaya kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Dosya kaydedilirken hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!kayitAktif) return;

            string tus = e.KeyCode.ToString();
            OlayKaydet(tus, "KeyDown");
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (!kayitAktif) return;

            string tus = e.KeyCode.ToString();
            OlayKaydet(tus, "KeyUp");
        }

        private void OlayKaydet(string tusAdi, string olayTuru)
        {
            TusKaydi kayit = new TusKaydi(tusAdi, olayTuru);
            kayitListesi.Add(kayit);

            // Listbox'a ekle ve en sona kaydır
            lstKayitlar.Items.Add(kayit.ToString());
            lstKayitlar.TopIndex = lstKayitlar.Items.Count - 1;

            // İstatistik güncelle
            if (olayTuru == "KeyDown")
            {
                if (tusIstatistigi.ContainsKey(tusAdi))
                    tusIstatistigi[tusAdi]++;
                else
                    tusIstatistigi[tusAdi] = 1;
            }

            IstatistigiGuncelle();
        }

        private void IstatistigiGuncelle()
        {
            string enSikTus = "Yok";
            int maxSayi = 0;

            foreach (var kvp in tusIstatistigi)
            {
                if (kvp.Value > maxSayi)
                {
                    maxSayi = kvp.Value;
                    enSikTus = $"{kvp.Key} ({kvp.Value} kez)";
                }
            }

            lblIstatistik.Text = $"Toplam Yakalanan Olay: {kayitListesi.Count}  |  En Sık Basılan Tuş: {enSikTus}";
        }
    }
}
