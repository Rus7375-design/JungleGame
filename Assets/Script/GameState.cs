public static class GameState
{
    public static int coins;
    public static int available;
    public static int score;
    public static int hp = -1;   

    static int savedCoins, savedAvailable, savedScore;
    static string lastScene = "";

    public static int LevelCoins => coins - savedCoins;   

    public static void BeginLevel(string scene, int maxHp)
    {
        if (scene != lastScene)
        {
            savedCoins = coins;
            savedAvailable = available;
            savedScore = score;
            lastScene = scene;
        }
        else
        {
            coins = savedCoins;
            available = savedAvailable;
            score = savedScore;
            hp = maxHp;
        }
    }

    public static void Reset()
    {
        coins = 0; available = 0; score = 0; hp = -1; lastScene = "";
    }
}