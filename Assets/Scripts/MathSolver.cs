using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathSolver {

    static int MAX = 99999;
	

    public static bool CanSolve(Operation op, int a, int b = -1) {
        if (op == Operation.SUM) return a + b <= MAX;
        if (op == Operation.DIFFERENCE) return true;
        if (op == Operation.PRODUCT) return a == 0 || b == 0 || a + 1 < MAX / b;
        if (op == Operation.DIVISION) return a == 0 || b == 0 || (a % b == 0) || (b % a == 0);
        if (op == Operation.POWER) return PowerChecker(a, b);
        if (op == Operation.CONCAT) return a != 44 || b != 44;
        if(op == Operation.SQRT) return a != 1 && Mathf.Sqrt(a) % 1 == 0;
        if (op == Operation.FACTORIAL) return 2 < a && a <= 8;
        return 1 < a && a <= 446;
    }

    public static int Solve(Operation op, int a, int b = -1) {
        if (op == Operation.SUM) return a + b;
        if (op == Operation.DIFFERENCE) return Mathf.Abs(a - b);
        if (op == Operation.PRODUCT) return a * b;
        if (op == Operation.DIVISION) return (a == 0 || b == 0) ? 0 : Mathf.Max(a / b, b / a);
        if (op == Operation.POWER) return (int) Mathf.Pow(a, b);
        if (op == Operation.CONCAT) return a == 4 ? (b == 4 ? 44 : 444) : 444;
        if (op == Operation.SQRT) return (int) Mathf.Sqrt(a);
        if (op == Operation.FACTORIAL) return fact[a];
        return (a * (a + 1)) >> 1;
    }

    static int[] fact = { 1, 1, 2, 6, 24, 120, 720, 5040, 40320 };

    static bool PowerChecker(int a, int b) {
        if (a < 2) return true;
        if (a == 2) return b <= 16;
        if (a == 3) return b <= 10;
        if (a == 4) return b <= 8;
        if (a == 5) return b <= 7;
        if (a == 6) return b <= 6;
        if (a < 10) return b <= 5;
        if (a < 18) return b <= 4;
        if (a < 47) return b <= 3;
        if (a < 317) return b <= 2;
        return b <= 1;
    }
}

public enum Operation {
    SUM = 0,
    DIFFERENCE = 1,
    PRODUCT = 2,
    DIVISION = 3,
    POWER = 4,
    CONCAT = 5,
    SQRT = 6,
    FACTORIAL = 7,
    TERMIAL = 8
}
