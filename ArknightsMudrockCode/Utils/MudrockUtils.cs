namespace ArknightsMudrock.ArknightsMudrockCode.Utils;

public static class MudrockUtils
{
    public static int ClampMin(int value, int min)
    {
        return value <= min ? min : value;
    }
}