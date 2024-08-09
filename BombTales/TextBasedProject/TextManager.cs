namespace TextBasedProject;

internal class TextManager
{
    private static TextManager _singleton = null!;
    
    public TextManager() {
        _singleton = Helper.GetSingleton(_singleton, this);
    }
    public void Welcoming() {
        if (!Helper.IsSingleton(_singleton, this)) {
            return;
        }
        Console.WriteLine(CommonData.GameName + " Oyununa Hoş geldin!" );
    }
    
    public void GameOver() {
        if (!Helper.IsSingleton(_singleton, this)) {
            return;
        }
        Console.WriteLine("Womp Womp... Oyunu Kaybettin!");
    }
    
    public void Introduction() {
        if (!Helper.IsSingleton(_singleton, this)) {
            return;
        }
        Console.WriteLine("Oyunun amacı bombayı etkisiz hale getirmek.");
        Console.WriteLine("Ama bunu yaparken zamana dikkat etmelisin çünkü sadece " + CommonData.GameTime + " saniyen var!");
        Console.WriteLine("ÖNEMLİ!!! Hangi fonksiyonları kullanabileceğini görmek için 'GetAvailableFunctions()' fonksiyonunu kullanabilirsin.");
        Console.WriteLine("Oyunun herhangi bir evresinde 'ExitGame()' fonksiyonunu yazarak oyunu bitirebilirsin.");
        Console.WriteLine("Oyun içinde ne kadar zamanın kaldığını görmek için 'TimeLeft()' fonksiyonunu kullanabilirsin");
        Console.WriteLine("Tutorial'ı yapmak için StartTutorial Fonksiyonunu Kullanabilirsin.");
        Console.WriteLine("İyi şanslar!");
    }
    
    public void Exit() {
        if (!Helper.IsSingleton(_singleton, this)) {
            return;
        }
        Console.WriteLine("Oyundan Çıktın.");
        Console.WriteLine("Çıkmak zorunda kaldığın için üzgünüm :(");
    }
    
}