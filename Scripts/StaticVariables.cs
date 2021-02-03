public static class StaticVariables
{
    public static int StuckToWin { get; } = 10;
    public static int MaxInitStuck { get; } = 3;
    public static float KunaiMargin { get; } = 0.5f;
    public static float LogRotationSpeed { get; } = 0.5f;
    public static string LogObjectName { get; } = "Log";
    public static string LevelObjectName { get; } = "Level";
    public static string AppleObjectName { get; } = "Apple";
    public static string PlayerPrefStage { get; } = "Stage";
    public static string PlayerPrefApple { get; } = "Apple";
    public static string GetKunaiName(int id) => $"Kunai{id}";
}
