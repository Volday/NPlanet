using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath
{


    /// <summary>
    /// Точка пересечения двух отрезков
    /// </summary>
    // Возвращает вектор с минимальными значениями, если отрезки паралельны, накладываются друг на друга или не пересекаются
    static public Vector2 IntersectionPointOfTwoLines(Vector2 firstLineFirstPoint, Vector2 firstLineSecondPoint, Vector2 secondLineFirstPoint, Vector2 secondLineSecondPoint)
    {
        float d = (firstLineFirstPoint.x - firstLineSecondPoint.x) * (secondLineSecondPoint.y - secondLineFirstPoint.y)
            - (firstLineFirstPoint.y - firstLineSecondPoint.y) * (secondLineSecondPoint.x - secondLineFirstPoint.x);
        if (d != 0)
        {
            float da = (firstLineFirstPoint.x - secondLineFirstPoint.x) * (secondLineSecondPoint.y - secondLineFirstPoint.y)
                - (firstLineFirstPoint.y - secondLineFirstPoint.y) * (secondLineSecondPoint.x - secondLineFirstPoint.x);
            float db = (firstLineFirstPoint.x - firstLineSecondPoint.x) * (firstLineFirstPoint.y - secondLineFirstPoint.y)
                - (firstLineFirstPoint.y - firstLineSecondPoint.y) * (firstLineFirstPoint.x - secondLineFirstPoint.x);

            float ta = da / d;
            float tb = db / d;

            if (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1)
            {
                float dx = firstLineFirstPoint.x + ta * (firstLineSecondPoint.x - firstLineFirstPoint.x);
                float dy = firstLineFirstPoint.y + ta * (firstLineSecondPoint.y - firstLineFirstPoint.y);

                return new Vector2(dx, dy);
            }
        }
        return new Vector2(float.MinValue, float.MinValue);
    }

    /// <summary>
    /// Возвращает положительное значение если точка находится справа от прямой, ноль если пересекается и отрицательное если слево от прямой
    /// </summary>
    public static float PointToRightOfLine(Vector2 point, Vector2 finishPoint, Vector2 startPoint)
    {
        //D = (х3 - х1) * (у2 - у1) - (у3 - у1) * (х2 - х1)
        return (point.x - startPoint.x) * (finishPoint.y - startPoint.y) - (point.y - startPoint.y) * (finishPoint.x - startPoint.x);
    }

    /// <summary>
    /// Дистанция между точками в квадрате
    /// </summary>
    public static float sqrDistanceFromPointToPoint(Vector3 point1, Vector3 point2)
    {
        Vector3 differenceVector = point1 - point2;
        float distance = (differenceVector.x * differenceVector.x) + (differenceVector.y * differenceVector.y) + (differenceVector.z * differenceVector.z);
        return distance;
    }

    /// <summary>
    /// Дистанция между точками в квадрате
    /// </summary>
    public static float sqrDistanceFromPointToPoint(Vector2 point1, Vector2 point2)
    {
        Vector2 differenceVector = point1 - point2;
        float distance = (differenceVector.x * differenceVector.x) + (differenceVector.y * differenceVector.y);
        return distance;
    }

    /// <summary>
    /// Угол между векторами отсчитывая против часовой стрелке
    /// </summary>
    public static float Angle360BetweenСounterСlockwiseVector2(Vector2 point, Vector2 finishSection, Vector2 startSection)
    {
        float D = (point.x - startSection.x) * (finishSection.y - startSection.y) - (point.y - startSection.y) * (finishSection.x - startSection.x);
        finishSection.x -= startSection.x;
        finishSection.y -= startSection.y;
        point.x -= startSection.x;
        point.y -= startSection.y;
        if (D <= 0)
        {
            return Vector2.Angle(finishSection, point);
        }
        else
        {
            return 360 - Vector2.Angle(finishSection, point);
        }
    }

    /// <summary>
    /// Угол между векторами отсчитывая по часовой стрелке
    /// </summary>
    public static float Angle360BetweenСlockwiseVector2(Vector2 point, Vector2 finishSection, Vector2 startSection)
    {
        float D = (point.x - startSection.x) * (finishSection.y - startSection.y) - (point.y - startSection.y) * (finishSection.x - startSection.x);
        finishSection.x -= startSection.x;
        finishSection.y -= startSection.y;
        point.x -= startSection.x;
        point.y -= startSection.y;
        if (D <= 0)
        {
            return 360 - Vector2.Angle(finishSection, point);
        }
        else
        {
            return Vector2.Angle(finishSection, point);
        }
    }

    /// <summary>
    /// Дистанция между точкой и отрезком в квадрате
    /// </summary>
    public static float sqrDistanceFromPointToSection(Vector2 point, Vector2 finishSection, Vector2 startSection)
    {
        finishSection.x -= startSection.x;
        finishSection.y -= startSection.y;
        point.x -= startSection.x;
        point.y -= startSection.y;
        return sqrDistanceFromPointToSection(point, finishSection);
    }

    /// <summary>
    /// Дистанция между точкой и отрезком, который начинается из начала координат, в квадрате
    /// </summary>
    public static float sqrDistanceFromPointToSection(Vector2 point, Vector2 finishSection)
    {
        float sqrSideA = (point.x - finishSection.x) * (point.x - finishSection.x) + (point.y - finishSection.y) * (point.y - finishSection.y);
        float sqrSideB = point.x * point.x + point.y * point.y;
        float sqrSideC = finishSection.x * finishSection.x + finishSection.y * finishSection.y;
        if (sqrSideA + sqrSideC - sqrSideB < 0)
        {
            return sqrSideA;
        }
        else if (sqrSideB + sqrSideC - sqrSideA < 0)
        {
            return sqrSideB;
        }
        else
        {
            return Mathf.Pow(Mathf.Abs(finishSection.y * point.x - finishSection.x * point.y) / Mathf.Sqrt(sqrSideC), 2);
        }
    }

    /// <summary>
    /// Дистанция между точкой и лучом в квадрате
    /// </summary>
    public static float sqrDistanceFromPointToRay(Vector2 point, Vector2 directionPoint, Vector2 startPoint)
    {
        directionPoint.x -= startPoint.x;
        directionPoint.y -= startPoint.y;
        point.x -= startPoint.x;
        point.y -= startPoint.y;
        return sqrDistanceFromPointToRay(point, directionPoint);
    }

    /// <summary>
    /// Дистанция между точкой и лучём, который начинается из начала координат, в квадрате
    /// </summary>
    public static float sqrDistanceFromPointToRay(Vector2 point, Vector2 directionPoint)
    {
        float sqrSideA = (point.x - directionPoint.x) * (point.x - directionPoint.x) + (point.y - directionPoint.y) * (point.y - directionPoint.y);
        float sqrSideB = point.x * point.x + point.y * point.y;
        float sqrSideC = directionPoint.x * directionPoint.x + directionPoint.y * directionPoint.y;
        if (sqrSideB + sqrSideC - sqrSideA < 0)
        {
            return sqrSideB;
        }
        else
        {
            return Mathf.Pow(Mathf.Abs(directionPoint.y * point.x - directionPoint.x * point.y) / Mathf.Sqrt(sqrSideC), 2);
        }
    }

    /// <summary>
    /// Поворот вектора на заданный угл в градусах
    /// </summary>
    public static Vector2 Rotate(Vector2 point, float angle)
    {
        angle = (angle / 180.0f) * Mathf.PI;
        Vector2 rotated_point;
        rotated_point.x = point.x * Mathf.Cos(angle) - point.y * Mathf.Sin(angle);
        rotated_point.y = point.x * Mathf.Sin(angle) + point.y * Mathf.Cos(angle);
        return rotated_point;
    }

    /// <summary>
    /// Перемешать случайным образом массив векторов
    /// </summary>
    public static Vector2[] MixVector2(Vector2[] vector, int seed)
    {
        System.Random prng = new System.Random(seed);

        for (int i = 0; i < vector.Length; i++)
        {
            int temp = prng.Next(0, vector.Length);
            Vector2 temporaryVector = vector[temp];
            vector[temp] = vector[i];
            vector[i] = temporaryVector;
        }

        return vector;
    }

    /// <summary>
    /// Перемешать случайным образом список векторов
    /// </summary>
    public static List<Vector2> MixVector2(List<Vector2> vector, int seed)
    {
        System.Random prng = new System.Random(seed);

        for (int i = 0; i < vector.Count; i++)
        {
            int temp = prng.Next(0, vector.Count);
            Vector2 temporaryVector = vector[temp];
            vector[temp] = vector[i];
            vector[i] = temporaryVector;
        }

        return vector;
    }

    //public static bool PointBelongsQuadrilateral(Vector2 point, Vector2 quadrilateralPoint1, Vector2 quadrilateralPoint2, Vector2 quadrilateralPoint3, Vector2 quadrilateralPoint4) {
    //    if (PointBelongsTriangle(point, quadrilateralPoint1, quadrilateralPoint2, quadrilateralPoint3) ||
    //        PointBelongsTriangle(point, quadrilateralPoint3, quadrilateralPoint4, quadrilateralPoint1) ||
    //        PointBelongsTriangle(point, quadrilateralPoint3, quadrilateralPoint4, quadrilateralPoint2) ||
    //        PointBelongsTriangle(point, quadrilateralPoint4, quadrilateralPoint2, quadrilateralPoint1)
    //        )
    //    {
    //        return true;
    //    }
    //    else {
    //        return false;
    //    }
    //}

    /// <summary>
    /// Определить находиться ли точка внутри треугольника
    /// </summary>
    public static bool PointBelongsTriangle(Vector2 point, Vector2 trianglePoint1, Vector2 trianglePoint2, Vector2 trianglePoint3)
    {
        float a = (trianglePoint1.x - point.x) * (trianglePoint2.y - trianglePoint1.y) - (trianglePoint2.x - trianglePoint1.x) * (trianglePoint1.y - point.y);
        float b = (trianglePoint2.x - point.x) * (trianglePoint3.y - trianglePoint2.y) - (trianglePoint3.x - trianglePoint2.x) * (trianglePoint2.y - point.y);
        float c = (trianglePoint3.x - point.x) * (trianglePoint1.y - trianglePoint3.y) - (trianglePoint1.x - trianglePoint3.x) * (trianglePoint3.y - point.y);

        if ((a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Определить находится ли точка внутри круга
    /// </summary>
    public static bool PointBelongsCircle(Vector2 point, Vector2 centor, float radius)
    {
        point = new Vector2(point.x - centor.x, point.y - centor.y);

        if ((point.x * point.x + point.y * point.y) <= radius * radius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}