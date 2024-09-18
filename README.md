# CodifyTube

CodifyTube, YouTube videolarını hızlı ve kolay bir şekilde indirmek ve dönüştürmek için geliştirilmiş bir C# WinForms uygulamasıdır. Uygulama, videoların MP4 ve MP3 formatında indirilmesini sağlar. Ayrıca, videoların thumbnail'lerini görüntüleme, video kalitesini seçme ve ses kalitesini seçme gibi özellikler sunar.

## Özellikler

- YouTube video URL'sinden video bilgilerini alma
- Thumbnail görüntüleme
- Video ve ses akışlarını seçme
- Video ve ses akışlarını birleştirerek MP4 formatında indirme
- İndirme işlemini durdurma
- İndirme ilerlemesini gösteren bir progress bar

## Kullanım

1. [YouTube Video URL'sini](https://www.youtube.com/watch?v=PA0Jhpvz6L4) uygulama penceresine yapıştırın.
2. "Get Info" butonuna tıklayarak video bilgilerini ve thumbnail'i yükleyin.
3. Video kalitesini seçin.
4. "Download" butonuna tıklayarak videoyu indirin.

## Gereksinimler

- [.NET Framework 4.7.2 veya üstü](https://dotnet.microsoft.com/download/dotnet-framework)
- [FFmpeg](https://ffmpeg.org/) - Video dönüştürme işlemleri için gerekli. `ffmpeg.exe` uygulama klasörüne yerleştirilmelidir.

## Kurulum

1. Projeyi klonlayın veya ZIP dosyasını indirin.
2. `ffmpeg.exe` dosyasını proje klasörüne ekleyin (Zaten projede var çalışacaktır.)
3. `CodifyTube.sln` dosyasını Visual Studio ile açın.
4. Uygulamayı derleyin ve çalıştırın.

## Videolu Tanıtım

Uygulamanın nasıl kullanılacağına dair tanıtım videosunu izleyebilirsiniz: [Tanıtım Videosu](https://www.youtube.com/watch?v=PA0Jhpvz6L4)

## Katkıda Bulunma

Her türlü katkı kabul edilir! İssues veya pull request'ler ile katkıda bulunabilirsiniz.

## Lisans

Bu proje [MIT Lisansı](LICENSE) altında lisanslanmıştır.
