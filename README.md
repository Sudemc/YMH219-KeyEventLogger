# YMH219 - Key Event Logger (Eğitsel Keylogger)

Bu proje, **Nesne Tabanlı Programlama (YMH 219)** dersi kapsamında Windows Forms platformunda klavye olaylarını yakalama ve işleme mekanizmalarını öğrenmek amacıyla geliştirilmiştir.

## Öğrenci Bilgileri
* **Adı Soyadı:** Sudem CÜCEMEN
* **Öğrenci Numarası:** 235541021
* **Öğretim / Şube:** İkinci Öğretim (İ.Ö.) - A Şubesi (Alttan Alan Öğrenci)

---

## Güvenlik ve Kapsam Bildirimi
> **ÖNEMLİ:** Bu uygulama gerçek/zararlı bir sistem keyloggerı DEĞİLDİR. Yalnızca eğitim amaçlıdır. Global keyboard hook, arka planda gizli çalışma, parola/veri çalma veya ağ üzerinden veri gönderme gibi hiçbir zararlı davranış içermez. Sadece kendi formu odaktayken (`KeyPreview = true`) basılan tuşları yakalar.

---

## Proje Açıklaması
Kullanıcı uygulama penceresi üzerindeyken meydana gelen `KeyDown` ve `KeyUp` olayları yakalanır; zaman damgası ve olay türüyle birlikte `TusKaydi` nesnesi olarak listelenir. Kayıt durumu kontrol edilebilir, liste temizlenebilir ve kayıtlar yerel `.txt` dosyasına aktarılabilir.

---

## Kullanılan Teknolojiler
* **Programlama Dili:** C# (.NET 9 / Windows Forms)
* **Geliştirme Ortamı:** Visual Studio 2022 / 2026
* **Dosya İşlemleri:** `System.IO.StreamWriter`, `SaveFileDialog`

---

## Uygulamanın Temel Özellikleri
1. **Olay Yakalama:** Form odaklıyken basılan ve bırakılan tuşlar anlık olarak yakalanır.
2. **Kayıt Yönetimi:** "Başlat" ve "Durdur" butonları ile dinleme açılıp kapatılabilir.
3. **Görsel Durum Bildirimi:** Arayüzde kaydın aktif veya durdurulmuş olduğunu gösteren renkli durum etiketi bulunur.
4. **Dosyaya Aktarma:** Listelenen tuş kayıtları kullanıcı istediğinde `.txt` dosyasına kaydedilebilir.
5. **İstatistik Takibi (Ekstra):** Toplam yakalanan olay sayısı ve en sık basılan tuş dinamik olarak hesaplanır.

---

## Kurulum ve Çalıştırma Adımları
1. Projeyi klonlayın veya indirin:
   ```bash
   git clone https://github.com/Sudemc/YMH219-KeyEventLogger.git
   ```
2. `YMH219-KeyEventLogger.csproj` dosyasını Visual Studio ile açın.
3. Çözümü derleyin ve çalıştırın (`F5` veya terminalden `dotnet run`).
4. "Kaydı Başlat" butonuna tıklayıp form üzerinde tuşlara basın.

---

## Ekran Görüntüleri
*(Uygulama çalışırken alınan en az 2 ekran görüntüsü)*

### 1. Aktif Kayıt ve Tuş Yakalama Ekranı
![Aktif Kayıt](screenshots/logger_active.png)

### 2. Dosyaya Kaydetme ve İstatistik Görünümü
![Dosya Kayıt](screenshots/logger_save.png)

---

## Bilinen Sınırlamalar
* Güvenlik ilkeleri gereği form minimize edildiğinde veya arka plana atıldığında tuşları kaydetmez (ödev yönergesine uygundur).
