using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void
        LineFromPoints(List<double> P, List<double> Q,
                       ref double a, ref double b, ref double c)
    {
        a = Q[1] - P[1];
        b = P[0] - Q[0];
        c = a * (P[0]) + b * (P[1]);
    }

    // Function which converts the input line to its
    // perpendicular bisector. It also inputs the points
    // whose mid-point lies on the bisector
    public static void PerpendicularBisectorFromLine(
        List<double> P, List<double> Q, ref double a,
        ref double b, ref double c)
    {
        List<double> mid_point = new List<double>();
        mid_point.Add((P[0] + Q[0]) / 2);

        mid_point.Add((P[1] + Q[1]) / 2);

        // c = -bx + ay
        c = -b * (mid_point[0]) + a * (mid_point[1]);

        double temp = a;
        a = -b;
        b = temp;
    }

    // Returns the intersection point of two lines
    public static List<double>
    LineLineIntersection(double a1, double b1, double c1,
                         double a2, double b2, double c2)
    {
        List<double> ans = new List<double>();
        double determinant = a1 * b2 - a2 * b1;
        if (determinant == 0)
        {
            // The lines are parallel. This is simplified
            // by returning a pair of FLT_MAX
            ans.Add(double.MaxValue);
            ans.Add(double.MaxValue);
        }

        else
        {
            double x = (b2 * c1 - b1 * c2) / determinant;
            double y = (a1 * c2 - a2 * c1) / determinant;
            ans.Add(x);
            ans.Add(y);
        }
        return ans;
    }


    public static Vector2 FindCircumCenter(Vector2 P,
                                        Vector2 Q,
                                        Vector2 R)
    {
        List<double> p = new List<double>() { P.x, P.y };
        List<double> q = new List<double>() { Q.x, Q.y };
        List<double> r = new List<double>() { R.x, R.y };

        return FindCircumCenter(p, q, r);
    }

    public static Vector2 FindCircumCenter(List<double> P,
                                        List<double> Q,
                                        List<double> R)
    {
        // Line PQ is represented as ax + by = c
        double a = 0;
        double b = 0;
        double c = 0;
        LineFromPoints(P, Q, ref a, ref b, ref c);

        // Line QR is represented as ex + fy = g
        double e = 0;
        double f = 0;
        double g = 0;
        LineFromPoints(Q, R, ref e, ref f, ref g);

        // Converting lines PQ and QR to perpendicular
        // vbisectors. After this, L = ax + by = c
        // M = ex + fy = g
        PerpendicularBisectorFromLine(P, Q, ref a, ref b,
                                      ref c);
        PerpendicularBisectorFromLine(Q, R, ref e, ref f,
                                      ref g);

        // The point of intersection of L and M gives
        // the circumcenter
        List<double> circumcenter
            = LineLineIntersection(a, b, c, e, f, g);

        return new Vector2((float)circumcenter[0], (float)circumcenter[1]);
    }

    public static Color GetColorWithAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}
