using System;

namespace YMH219_KeyEventLogger
{
    // Tek bir tuş hareketini temsil eden nesne sınıfı
    public class TusKaydi
    {
        public string TusAdi { get; set; }
        public DateTime Zaman { get; set; }
        public string OlayTuru { get; set; } // KeyDown, KeyPress, KeyUp

        public TusKaydi(string tusAdi, string olayTuru)
        {
            TusAdi = tusAdi;
            OlayTuru = olayTuru;
            Zaman = DateTime.Now;
        }

        // Listede ve dosyada gösterilecek format
        public override string ToString()
        {
            return $"[{Zaman:HH:mm:ss.fff}] - Olay: {OlayTuru} - Tuş: {TusAdi}";
        }
    }
}
