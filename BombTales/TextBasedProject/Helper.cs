internal class Helper
{
    public static bool CheckIntParameter(string parameter, int min, int max)
    {
        if (int.TryParse(parameter, out int result))
        {
            if (result < min || result > max) {
                Console.WriteLine("Girdiği sayıyı kontrol et. Lütfen geçerli bir sayı gir.");
                return false;
            }
            return true;
        }

        Console.WriteLine("Oyunu Bozmayı Bırakıp Lütfen Oyuna Odaklanır mısın?!!");
        return false;
    }
    public static bool IsSingleton<T>(T instance, T reference) where T : class
    {
        if (instance == reference) {
            return true;
        }
        Console.WriteLine("O kodu öyle kullanamazsın!");
        return false;
    }
    public static T GetSingleton<T>(T instance, T reference) where T : class
    {
        if (instance == null)
        {
            return reference;
        }
        Console.WriteLine("Ne yaptığını bilmiyorum mu zannediyorsun?");
        return instance;
    }
}
