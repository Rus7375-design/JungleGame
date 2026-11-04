public static class GameState
{
    public static int coins;       
    public static int available;   
    public static int hp = 3;

    static int savedCoins, savedAvailable;
    static string lastScene = "";

    public static void BeginLevel(string scene, int maxHp)
    {
        if (scene != lastScene)   
        {
            savedCoins = coins;
            savedAvailable = available;
            lastScene = scene;
        }
        else                         
        {
            coins = savedCoins;
            available = savedAvailable;
            hp = maxHp;
        }
    }

    public static void Reset()         
    {
        coins = 0; available = 0; hp = 3; lastScene = "";
    }
}