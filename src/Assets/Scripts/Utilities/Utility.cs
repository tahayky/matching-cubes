using System.Collections;
using System.Collections.Generic;
using System;
using Array2DEditor;
public enum CubeColor { Blue, Purple, Orange };
public enum Part { Path, Gap,Finish };
public enum GateType { Random, Order };
public enum SortingType { Random,  Color};
public static class Methods
{
    public static int GetColumnCount<T>(this Array2D<T> arr,int x,Predicate<T> predicate)
    {
        int count = 0;
        for(int y = 0; y < arr.GridSize.y; y++)
        {
            T cell = arr.GetCell(x, y);
            if (predicate.Invoke(cell))
                count++;
            
        }
        return count;
    }
    public static void Shuffle<T>(this List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
public class Constants
{
    public const float path_prefab_lenght = 50f;
    public const float cube_size = 1.7f;
    public const float player_height = 3.5f;
    public const float default_fow = 60f;
    public const float turbo_fow = 70f;
    public const float touch_movement_sensivity = 40f;
}
