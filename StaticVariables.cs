public static class StaticVariables
{
    public static int StuckToWin { get; } = 10;
    public static int MaxInitStuck { get; } = 3;
    public static float KunaiMargin { get; } = 0.5f;
    public static string LogObjectName { get; } = "Log";
    public static string GetKunaiName(int id) => $"Kunai{id}";
}
