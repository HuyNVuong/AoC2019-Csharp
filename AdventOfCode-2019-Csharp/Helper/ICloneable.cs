namespace AdventOfCode_2019_Csharp.Helper
{
    interface ICloneable<out T>
    {
        T Clone();
    }
}
